using IssueCreator.Logging;
using IssueCreator.Models;
using Microsoft.Extensions.Caching.Memory;
using Octokit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZenHub;
using ZenHub.Models;

namespace IssueCreator
{
    public class IssueManager
    {
        private ZenHubClient _zenHubClient;
        private GitHubClient _githubClient;
        private FileLogger _fileLogger;
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private readonly string _cacheFolder;

        public static IssueManager Create(Settings settings, string cacheFolder, FileLogger fileLogger)
        {
            using IDisposable scope = fileLogger.CreateScope($"Creating issue manager with cache folder: {cacheFolder}");
            IssueManager manager = new IssueManager(cacheFolder, fileLogger);
            if (!manager.RefreshGitHubToken(settings.GitHubToken))
            {
                return null;
            }

            manager.RefreshZenHubToken(settings.ZenHubToken);

            return manager;
        }

        private IssueManager(string cache, FileLogger logger)
        {
            _cacheFolder = cache;
            _fileLogger = logger;
        }

        public void RefreshZenHubToken(string newToken)
        {
            using IDisposable scope = _fileLogger.CreateScope("Refreshing ZenHub token");
            _zenHubClient = new ZenHubClient(newToken);
        }

        public bool RefreshGitHubToken(string newToken)
        {
            using IDisposable scope = _fileLogger.CreateScope("Refreshing GitHub token");
            try
            {
                _githubClient = new GitHubClient(new ProductHeaderValue("IssueCreator"))
                {
                    Credentials = new Credentials(newToken)
                };

                //make request to ensure valid token
                User user = _githubClient.User.Current().GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                _fileLogger.Log(ex.ToString());
                _githubClient = null;
                return false;
            }
        }

        public async Task<IEnumerable<IssueMilestone>> GetMilestonesAsync(string owner, string repo)
        {
            using IDisposable scope = _fileLogger.CreateScope("Retrieving milestones");
            IEnumerable<IssueMilestone> milestones = await GetValueFromCache(StringTemplate.Milestones(owner, repo), async () => IssueMilestone.FromMilestoneList(await _githubClient.Issue.Milestone.GetAllForRepository(owner, repo)));

            return milestones;
        }

        public async Task<List<RepoLabel>> GetLabelsAsync(string owner, string repo)
        {
            using IDisposable scope = _fileLogger.CreateScope("Retrieving labels");
            List<RepoLabel> labels = await GetValueFromCache(StringTemplate.Labels(owner, repo), async () => RepoLabel.FromLabelList(await _githubClient.Issue.Labels.GetAllForRepository(owner, repo)));

            return labels;
        }

        public async Task<List<GitHubContributor>> GetContributorsAsync(string owner, string repo)
        {
            using IDisposable scope = _fileLogger.CreateScope("Retrieving contributors");

            List<GitHubContributor> contributors = await GetValueFromCache(StringTemplate.Contributors(owner, repo), async () => GitHubContributor.FromContributorsList(await _githubClient.Repository.GetAllContributors(owner, repo)));
            return contributors;
        }

        public async Task<bool> AddIssueToEpicAsync(long repoId, int epicNumber, long repoIdIssue, int issueNumber)
        {
            using IDisposable scope = _fileLogger.CreateScope("Adding issue to epic");
            try
            {
                //create an issue with just the number and repository set
                // The only 2 parameters that need to be specified are the issueNumber and the repository.
                Issue issue = new Issue(default, default, default, default,
                    number: issueNumber, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default,
                    repository: new Repository(repoIdIssue), default);
                await _zenHubClient.GetEpicClient(repoId, epicNumber).AddIssuesAsync(new[] { issue });
                return true;
            }
            catch (Exception ex)
            {
                _fileLogger.Log(ex.ToString());
                return false;
            }
        }

        public async Task<bool> RemoveIssueFromEpicAsync(long repoId, int epicNumber, long repoIdIssue, int issueNumber)
        {
            using IDisposable scope = _fileLogger.CreateScope("Removing issue from epic");
            try
            {
                //create an issue with just the number and repository set
                // The only 2 parameters that need to be specified are the issueNumber and the repository.
                Issue issue = new Issue(default, default, default, default,
                    number: issueNumber, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default,
                    repository: new Repository(repoIdIssue), default);
                await _zenHubClient.GetEpicClient(repoId, epicNumber).RemoveIssuesAsync(new[] { issue });
                return true;
            }
            catch (Exception ex)
            {
                _fileLogger.Log(ex.ToString());
                return false;
            }
        }


        public async Task<bool> SetIssueEstimateAsync(long repoId, int issueNumber, double estimate)
        {
            using IDisposable scope = _fileLogger.CreateScope($"Setting issue estimate to {estimate}");

            try
            {
                if (estimate > 0)
                {
                    await _zenHubClient.GetIssueClient(repoId, issueNumber).SetEstimateAsync(estimate);
                }
                return true;
            }
            catch (Exception ex)
            {
                _fileLogger.Log(ex.ToString());
                return false;
            }
        }

        public async Task<bool> ConvertToEpicAsync(long repoId, int issueNumber)
        {
            using IDisposable scope = _fileLogger.CreateScope("Convert issue to epic");

            try
            {
                await _zenHubClient.GetIssueClient(repoId, issueNumber).ConvertToEpicAsync(Enumerable.Empty<Issue>());
                return true;
            }
            catch (Exception ex)
            {
                _fileLogger.Log(ex.ToString());
                return false;
            }
        }

        private async Task<Issue> CreateIssueAsync(string owner, string repo, string title, string description, string assignedTo, List<string> labels, int? milestone = null)
        {
            NewIssue issue = new NewIssue(title);

            foreach (string item in labels)
            {
                issue.Labels.Add(item.ToString());
            }

            issue.Body = description;
            issue.Milestone = milestone;

            if (!string.IsNullOrEmpty(assignedTo))
            {
                issue.Assignees.Add(assignedTo);
            }

            return await _githubClient.Issue.Create(owner, repo, issue);
        }

        public async Task<RepositoryInfo> GetRepositoryAsync(string owner, string repo)
        {
            return await GetValueFromCache(StringTemplate.Repo(owner, repo), async () => new RepositoryInfo(await _githubClient.Repository.Get(owner, repo)));
        }

        internal async Task<(bool, string)> TryCreateNewIssueAsync(IssueToCreate issueToCreate)
        {
            using IDisposable scope = _fileLogger.CreateScope("Creating issue");

            bool validEstimate = false;
            double estimate = 0;
            if (string.IsNullOrEmpty(issueToCreate.Estimate))
            {
                validEstimate = true;
            }
            else if (double.TryParse(issueToCreate.Estimate, out estimate) && estimate > 0)
            {
                validEstimate = true;
            }

            if (!validEstimate)
            {
                return (false, "Estimate needs to be a positive number greater than zero.");
            }

            Issue createdIssue;
            try
            {
                // Create the issue on GitHub
                createdIssue = await CreateIssueAsync(
                    issueToCreate.Organization,
                    issueToCreate.Repository,
                    issueToCreate.Title,
                    issueToCreate.Description,
                    issueToCreate.AssignedTo?.ToString(),
                    issueToCreate.Labels,
                    issueToCreate.Milestone?.Number);
            }
            catch (Exception ex)
            {
                _fileLogger.Log(ex.ToString());
                return (false, "Could not create issue.");
            }

            try
            {
                RepositoryInfo repoFromGH = await GetRepositoryAsync(issueToCreate.Organization, issueToCreate.Repository);
                // assign the epic, if one is selected

                if (issueToCreate.Epic != null)
                {
                    bool result = await AddIssueToEpicAsync(issueToCreate.Epic.Repo.Id, issueToCreate.Epic.Issue.Number, repoFromGH.Id, createdIssue.Number);
                    if (!result)
                    {
                        MessageBox.Show("Cannot associate issue to epic");
                    }
                }

                // set the estimate
                if (estimate > 0)
                {
                    bool result = await SetIssueEstimateAsync(repoFromGH.Id, createdIssue.Number, estimate);
                    if (!result)
                    {
                        MessageBox.Show("Cannot set issue estimate");
                    }
                }

                // Convert to Epic
                if (issueToCreate.CreateAsEpic)
                {
                    bool result = await ConvertToEpicAsync(repoFromGH.Id, createdIssue.Number);
                    if (!result)
                    {
                        MessageBox.Show("Cannot convert issue to epic");
                    }
                }
            }
            catch (Exception ex)
            {
                _fileLogger.Log(ex.ToString());
                return (false, "Failed to create the epic. Please check the issue on the website.");
            }

            Process.Start(createdIssue.HtmlUrl);

            return (true, string.Empty);
        }

        public async Task<IssueObject> GetIssueAsync(long repoId, int issueNumber)
        {
            IssueObject issue = await GetValueFromCache(StringTemplate.Issue(repoId, issueNumber), async () => new IssueObject(await _githubClient.Issue.Get(repoId, issueNumber)));
            return issue;
        }

        public async Task<List<IssueDescription>> GetEpicsAsync(List<string> repositoriesToUse)
        {
            ConcurrentBag<IssueDescription> results = new ConcurrentBag<IssueDescription>();

            // defensive copy in case the list of repos changes.
            List<string> repositories = new List<string>(repositoriesToUse);
            ConcurrentBag<IssueDescription> result = new ConcurrentBag<IssueDescription>();

            Task[] tasks = new Task[repositories.Count];

            int count = 0;
            foreach (string ownerAndRepoName in repositories)
            {
                // for each repo, get the epics.
                (string owner, string repo) = GetOwnerAndRepoFromString(ownerAndRepoName);

                tasks[count++] = Task.Run(async () =>
                {
                    RepositoryInfo gitHubRepoObj = await GetRepositoryAsync(owner, repo);

                    _fileLogger.Log($"Getting epics for repo {owner}\\{repo}");

                    EpicList epicList = await GetValueFromCache(StringTemplate.Epic(owner, repo), async () => (await _zenHubClient.GetRepositoryClient(gitHubRepoObj.Id).GetEpicsAsync()).Value, DateTimeOffset.Now.AddHours(1));

                    foreach (EpicInfo epic in epicList.Epics)
                    {
                        long repoId = epic.RepositoryId;
                        int issueNumber = epic.IssueNumber;
                        // from the issue link, get the cached issue from the repo
                        IssueObject issue = await GetIssueAsync(repoId, issueNumber);

                        result.Add(new IssueDescription() { Issue = issue, Repo = gitHubRepoObj });
                    }
                });
            }

            await Task.WhenAll(tasks);

            return result.ToList();
        }

        private (string, string) GetOwnerAndRepoFromString(string input)
        {
            string[] parts = input.Split('\\');
            return (parts[0], parts[1]);
        }

        /// <summary>
        /// This is the code that calls into the cache.
        /// </summary>
        private async Task<TResult> GetValueFromCache<TResult>(string key, Func<Task<TResult>> getValue, DateTimeOffset cacheDuration = default)
        {
            TResult itemInCache = _cache.Get<TResult>(key);
            if (itemInCache != null)
            {
                // the value was found in the cache
                return itemInCache;
            }

            // add the value to the cache.
            if (cacheDuration == default)
            {
                cacheDuration = DateTimeOffset.Now.AddHours(8);
            }

            TResult valueToCache = await getValue();

            // save the object to disk
            await SerializeObjectToDiskAsync(key, valueToCache);

            _cache.Set(key, valueToCache, cacheDuration);

            return valueToCache;
        }

        private async Task SerializeObjectToDiskAsync<TValue>(string key, TValue value)
        {
            using IDisposable scope = _fileLogger.CreateScope($"Saving data to cache {key}");

            using (StreamWriter sw = new StreamWriter($"{Path.Combine(_cacheFolder, key)}.json"))
            {
                await sw.WriteAsync(JsonSerializer.Serialize(value));
            }
        }

        public bool DeserializeCacheDataFromFolder(string folder)
        {
            using IDisposable scope = _fileLogger.CreateScope("Reading cache data from disk.");
            bool deserializedData = false;
            foreach (string file in Directory.GetFiles(folder, "*.json"))
            {
                // serialize it to disk.
                using (StreamReader sr = new StreamReader(file))
                {
                    string key = Path.GetFileNameWithoutExtension(file);
                    Type cacheType = StringTemplate.GetType(key);

                    object deserializedObject = JsonSerializer.Deserialize(sr.ReadToEnd(), cacheType);

                    _cache.Set(key, deserializedObject, DateTimeOffset.Now.AddMinutes(10));
                    deserializedData = true;
                }
            }

            return deserializedData;
        }

        internal void RemoveRepoFromCache(string owner, string repo)
        {
            _cache.Remove(StringTemplate.Repo(owner, repo));
        }

        internal void RemoveEpicFromCache(string owner, string repo)
        {
            _cache.Remove(StringTemplate.Epic(owner, repo));
        }

        internal static class StringTemplate
        {
            public static Type GetType(string template)
            {
                // figure out the type we need.
                if (template.ToLowerInvariant().StartsWith("epic")) return typeof(EpicList);
                else if (template.ToLowerInvariant().StartsWith("issue")) return typeof(IssueObject);
                else if (template.ToLowerInvariant().StartsWith("repo")) return typeof(RepositoryInfo);
                else if (template.ToLowerInvariant().StartsWith("milestones")) return typeof(List<IssueMilestone>);
                else if (template.ToLowerInvariant().StartsWith("labels")) return typeof(List<RepoLabel>);
                else if (template.ToLowerInvariant().StartsWith("contributors")) return typeof(List<GitHubContributor>);
                return null;
            }


            public static string Epic(string owner, string repo) => $"epic_{owner}_{repo}";
            public static string Repo(string owner, string repo) => $"repo_{owner}_{repo}";
            public static string Milestones(string owner, string repo) => $"milestones_{owner}_{repo}";
            public static string Labels(string owner, string repo) => $"labels_{owner}_{repo}";
            public static string Contributors(string owner, string repo) => $"contributors_{owner}_{repo}";
            internal static string Issue(long repoId, int issueNumber) => $"issue_{repoId}_{issueNumber}";
        }
    }
}

