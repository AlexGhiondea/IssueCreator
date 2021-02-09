using CommandLine;
using CsvHelper;
using CsvHelper.Configuration;
using IssueCreator;
using IssueCreator.Logging;
using IssueCreator.Models;
using OutputColorizer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BulkIssueCreatorCLI
{
    class Program
    {
        private static FileLogger s_logger;
        private static readonly Settings s_settings = new Settings();
        private static IssueManager s_issueManager;
        private static Arguments s_arguments;
        static void Main(string[] args)
        {
            if (!Parser.TryParse<Arguments>(args, out s_arguments))
            {
                return;
            }

            s_logger = new FileLogger(Path.Combine(Settings.SettingsFolder, "bulkIssueCreatorCLI.log"));

            Colorizer.Write("Loading settings...");
            using (IDisposable scope = s_logger.CreateScope("Loading settings"))
            {
                s_settings.Initialize(Settings.Deserialize(Settings.SettingsFile, s_logger));

                s_issueManager = IssueManager.Create(s_settings, ".", s_logger);
            }
            Colorizer.WriteLine("[Green!Done]!");

            Colorizer.Write("Reading input file [Yellow!{0}]", s_arguments.InputFile);

            CsvConfiguration cfg = new CsvConfiguration(CultureInfo.InvariantCulture, missingFieldFound: null);
            List<IssueToCreateWithEpic> parsedIssueData = ParseInputFile(cfg);

            //ValidateAndSetMilestonesAsync(parsedIssueData).GetAwaiter().GetResult();

            parsedIssueData = SortIssuesByParent(parsedIssueData);

            //CreateIssuesAsync(parsedIssueData).GetAwaiter().GetResult();
        }

        private static List<IssueToCreateWithEpic> SortIssuesByParent(List<IssueToCreateWithEpic> parsedIssueData)
        {
            Queue<IssueToCreateWithEpic> toBeSorted = new Queue<IssueToCreateWithEpic>(parsedIssueData);
            Queue<IssueToCreateWithEpic> results = new Queue<IssueToCreateWithEpic>();

            while (toBeSorted.Count > 0)
            {
                IssueToCreateWithEpic current = toBeSorted.Dequeue();

                if (current.HasParent)
                {
                    results.Enqueue(current);

                    // if this was a parent for any other issue, those issues can now be considered good to go
                    foreach (IssueToCreateWithEpic entry in toBeSorted)
                    {
                        if (StringComparer.OrdinalIgnoreCase.Equals(entry.EpicTitle, current.Title) &&
                            StringComparer.OrdinalIgnoreCase.Equals(entry.EpicRepo, current.Repository) &&
                            StringComparer.OrdinalIgnoreCase.Equals(entry.EpicOrg, current.Organization))
                        {
                            entry.HasParent = true;
                        }
                    }
                }
                else
                {
                    toBeSorted.Enqueue(current);
                }
            }

            foreach (IssueToCreateWithEpic item in results)
            {
                Console.WriteLine(item.Title + "< " + item.EpicTitle);
            }

            return results.ToList();
        }

        private static async Task CreateIssuesAsync(List<IssueToCreateWithEpic> parsedIssueData)
        {
            // The issues are sorted such that no epic shows up before its parent epic.
            // this means I should be able to get the parent epic all the time.

            // create all the issues, without being in an epic.
            foreach (IssueToCreateWithEpic issue in parsedIssueData)
            {
                Console.WriteLine("Creating" + issue.Title);
                if (!string.IsNullOrEmpty(issue.EpicTitle))
                {
                    issue.Epic = await GetEpicFromTitle(issue);
                }

                (bool result, string error) = await s_issueManager.TryCreateNewIssueAsync(issue, false);

                if (result == false)
                {
                    throw new InvalidOperationException("Error creating epic");
                }
            }
        }

        private static async Task<IssueDescription> GetEpicFromTitle(IssueToCreateWithEpic issue)
        {
            List<IssueDescription> epics = await s_issueManager.GetEpicsWithTitleAsync(issue.EpicTitle, $"{issue.EpicOrg}\\{issue.EpicRepo}");

            Console.WriteLine("Using parent epic " + epics.FirstOrDefault().Issue.Title);

            return epics.FirstOrDefault();
        }

        private static async Task ValidateAndSetMilestonesAsync(List<IssueToCreateWithEpic> parsedIssueData)
        {
            foreach (IssueToCreateWithEpic issue in parsedIssueData)
            {
                // get the milestone

                if (string.IsNullOrEmpty(issue.MilestoneText))
                {
                    continue;
                }

                issue.Milestone = await s_issueManager.GetMilestoneAsync(issue.Organization, issue.Repository, issue.MilestoneText);
                if (issue.Milestone == null)
                {
                    Colorizer.WriteLine("[Red!Error] Could not find milestone [Yellow!{0}] in repository [Yellow!{1}\\{2}]", issue.MilestoneText, issue.Organization, issue.Repository);
                }
            }
        }

        private static List<IssueToCreateWithEpic> ParseInputFile(CsvConfiguration cfg)
        {
            List<IssueToCreateWithEpic> issues = new List<IssueToCreateWithEpic>();

            using (StreamReader sr = new StreamReader(s_arguments.InputFile))
            using (CsvReader csv = new CsvReader(sr, cfg))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    IssueToCreateWithEpic issue = new IssueToCreateWithEpic
                    {
                        Organization = csv.GetField<string>("Organization")?.Trim(),
                        Repository = csv.GetField<string>("Repository")?.Trim(),
                        Title = csv.GetField<string>("Title")?.Trim(),
                        Description = csv.GetField<string>("Description")?.Trim(),
                        AssignedTo = csv.GetField<string>("AssignedTo")?.Trim(),
                        Estimate = csv.GetField<string>("Estimate")?.Trim(),
                        EpicOrg = csv.GetField<string>("EpicOrganization")?.Trim(),
                        EpicRepo = csv.GetField<string>("EpicRepository")?.Trim(),
                        EpicTitle = csv.GetField<string>("EpicTitle")?.Trim(),
                        MilestoneText = csv.GetField<string>("Milestone")?.Trim(),
                    };

                    string isEpic = csv.GetField<string>("CreateAsEpic");
                    if (!string.IsNullOrEmpty(isEpic))
                    {
                        if (bool.TryParse(isEpic, out bool isEpicValue))
                        {
                            issue.CreateAsEpic = isEpicValue;
                        }
                    }

                    string labels = csv.GetField<string>("Labels");
                    if (!string.IsNullOrWhiteSpace(labels))
                    {
                        labels = labels.Trim();
                        issue.Labels = new List<string>(labels.Split(','));
                    }

                    issues.Add(issue);
                }

                // once we read the entries we can decide which ones require us to create issues vs the ones that we should expect already exist.

                foreach (IssueToCreateWithEpic issue in issues)
                {
                    // does the issue need a parent epic to be created?
                    if (string.IsNullOrEmpty(issue.EpicTitle))
                    {
                        issue.HasParent = true;
                        continue;
                    }

                    // check to see if the parent of this issue is in the list to be created.
                    // if not, mark hasParent=true

                    if (!issues.Where(entry => StringComparer.OrdinalIgnoreCase.Equals(entry.Title, issue.EpicTitle) &&
                            StringComparer.OrdinalIgnoreCase.Equals(entry.Repository, issue.EpicRepo) &&
                            StringComparer.OrdinalIgnoreCase.Equals(entry.Organization, issue.EpicOrg)).Any())
                    {
                        issue.HasParent = true;
                    }
                }

                Colorizer.WriteLine("[Green!Done]!");
            }
            return issues;
        }
    }

    class IssueToCreateWithEpic : IssueToCreate
    {
        public string EpicOrg { get; set; }
        public string EpicRepo { get; set; }
        public string EpicTitle { get; set; }
        public string MilestoneText { get; set; }

        public bool HasParent { get; set; } = false;

        public override string ToString()
        {
            return $"{Title} ({Repository})";
        }
    }
}
