using Azure;
using IssueCreator.Models;
using Microsoft.Extensions.Caching.Memory;
using Octokit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ZenHub;
using ZenHub.Models;

namespace IssueCreator
{
    public class IssueManager
    {
        ZenHubClient _zenHubClient;
        GitHubClient _githubClient;
        MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public static IssueManager Create(Settings settings)
        {
            IssueManager manager = new IssueManager();
            if (!manager.RefreshGitHubToken(settings.GitHubToken))
            {
                return null;
            }

            manager.RefreshZenHubToken(settings.ZenHubToken);

            return manager;
        }

        private IssueManager()
        {
        }

        public void RefreshZenHubToken(string newToken)
        {
            _zenHubClient = new ZenHubClient(newToken);
        }

        public bool RefreshGitHubToken(string newToken)
        {
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
            catch
            {
                _githubClient = null;
                return false;
            }
        }

        public async Task<IReadOnlyList<Milestone>> GetMilestonesAsync(string owner, string repo)
        {
            IReadOnlyList<Milestone> milestones = await GetValueFromCache(StringTemplate.Milestones(owner, repo), () => _githubClient.Issue.Milestone.GetAllForRepository(owner, repo));

            return milestones;
        }

        public async Task<IReadOnlyList<Label>> GetLabelsAsync(string owner, string repo)
        {
            IReadOnlyList<Label> labels = await GetValueFromCache(StringTemplate.Labels(owner, repo), () => _githubClient.Issue.Labels.GetAllForRepository(owner, repo));

            return labels;
        }

        public async Task<IReadOnlyList<RepositoryContributor>> GetContributorsAsync(string owner, string repo)
        {
            IReadOnlyList<RepositoryContributor> contributors = await GetValueFromCache(StringTemplate.Contributors(owner,repo), () => _githubClient.Repository.GetAllContributors(owner, repo));
            return contributors;
        }

        public async Task<bool> AddIssueToEpicAsync(long repoId, int epicNumber, long repoIdIssue, int issueNumber)
        {
            try
            {
                //create an issue with just the number and repository set
                Issue issue = new Issue(default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, default, issueNumber, default, default, new Repository(repoIdIssue), default);
                await _zenHubClient.GetEpicClient(repoId, epicNumber).AddIssuesAsync(new[] { issue });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task SetIssueEstimateAsync(long repoId, int issueNumber, int estimate)
        {
            if (estimate > 0)
            {
                await _zenHubClient.GetIssueClient(repoId, issueNumber).SetEstimateAsync(estimate);
            }
        }

        public async Task ConvertToEpicAsync(long repoId, int issueNumber)
        {
            await _zenHubClient.GetIssueClient(repoId, issueNumber).ConvertToEpicAsync(Enumerable.Empty<Issue>());
        }

        public async Task<Issue> CreateIssueAsync(string owner, string repo, string title, string description, string assignedTo, List<string> labels, int? milestone = null)
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

        public async Task<Repository> GetRepositoryAsync(string owner, string repo)
        {
            return await GetValueFromCache(StringTemplate.Repo(owner,repo), () => _githubClient.Repository.Get(owner, repo));
        }

        internal async Task<(bool, string)> TryCreateNewIssueAsync(IssueToCreate issueToCreate)
        {
            bool validEstimate = false;
            int estimate = 0;
            if (string.IsNullOrEmpty(issueToCreate.Estimate))
            {
                validEstimate = true;
            }
            else if (int.TryParse(issueToCreate.Estimate, out estimate) && estimate > 0)
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
                    issueToCreate.Milestone.Number);
            }
            catch
            {
                return (false, "Could not create issue.");
            }

            try
            {
                Repository repoFromGH = await GetRepositoryAsync(issueToCreate.Organization, issueToCreate.Repository);
                // assign the epic, if one is selected
                if (issueToCreate.Epic != null)
                {
                    await AddIssueToEpicAsync(issueToCreate.Epic.Repo.Id, issueToCreate.Epic.Issue.Number, repoFromGH.Id, createdIssue.Number);
                }

                // set the estimate
                if (estimate > 0)
                {
                    await SetIssueEstimateAsync(repoFromGH.Id, createdIssue.Number, estimate);
                }

                // Convert to Epic
                if (issueToCreate.CreateAsEpic)
                {
                    await ConvertToEpicAsync(repoFromGH.Id, createdIssue.Number);
                }
            }
            catch
            {
                return (false, "Failed to create the epic. Please check the issue on the website.");
            }

            Process.Start(createdIssue.HtmlUrl);

            return (true, string.Empty);
        }

        public async Task<Issue> GetIssueAsync(long repoId, int issueNumber)
        {
            Issue issue = await GetValueFromCache(StringTemplate.Issue(repoId, issueNumber), () => _githubClient.Issue.Get(repoId, issueNumber));
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
                    Repository gitHubRepoObj = await GetRepositoryAsync(owner, repo);

                    Response<EpicList> epicList = await GetValueFromCache(StringTemplate.Epic(owner, repo), () => _zenHubClient.GetRepositoryClient(gitHubRepoObj.Id).GetEpicsAsync(), DateTimeOffset.Now.AddHours(1));

                    foreach (EpicInfo epic in epicList.Value.Epics)
                    {
                        long repoId = epic.RepositoryId;
                        int issueNumber = epic.IssueNumber;
                        // from the issue link, get the cached issue from the repo
                        Issue issue = await GetIssueAsync(repoId, issueNumber);

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

        private async Task<TResult> GetValueFromCache<TResult>(string key, Func<Task<TResult>> getValue, DateTimeOffset cacheDuration = default)
        {
            TResult itemInCache = _cache.Get<TResult>(key);
            if (itemInCache != null)
            {
                return itemInCache;
            }

            // add the value to the cache.

            if (cacheDuration == default)
            {
                cacheDuration = DateTimeOffset.Now.AddHours(8);
            }

            TResult valueToCache = await getValue();
            _cache.Set(key, valueToCache, cacheDuration);

            return valueToCache;
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
            public static string Epic(string owner, string repo) => $"epic_{owner}_{repo}";
            public static string Repo(string owner, string repo) => $"repo_{owner}_{repo}";
            public static string Milestones(string owner, string repo) => $"milestones_{owner}_{repo}";
            public static string Labels(string owner, string repo) => $"labels_{owner}_{repo}";
            public static string Contributors(string owner, string repo) => $"contributors_{owner}_{repo}";
            internal static string Issue(long repoId, int issueNumber) => $"issue_{repoId}_{issueNumber}";
        }
    }
}
