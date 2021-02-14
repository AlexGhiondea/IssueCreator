using CommandLine;
using CsvHelper;
using CsvHelper.Configuration;
using IssueCreator;
using IssueCreator.Logging;
using IssueCreator.Models;
using OutputColorizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            using (var fileLogger = new FileLogWriter("IssueCreator.log"))
            {
                Colorizer.SetupWriter(fileLogger);
                if (!Parser.TryParse<Arguments>(args, out s_arguments))
                {
                    return;
                }

                s_logger = new FileLogger(Path.Combine(Settings.SettingsFolder, "bulkIssueCreatorCLI.log"));

                if (s_arguments.Type == ActionType.createIssues)
                {
                    CreateIssuesActionAsync().GetAwaiter().GetResult();
                }
            }
        }

        private static async Task CreateIssuesActionAsync()
        {
            using (LoggingScope main = new LoggingScope("Create issues in bulk on GitHub"))
            {

                using (LoggingScope li = new LoggingScope("Loading settings..."))
                {
                    using (IDisposable scope = s_logger.CreateScope("Loading settings"))
                    {
                        s_settings.Initialize(Settings.Deserialize(Settings.SettingsFile, s_logger));

                        s_issueManager = IssueManager.Create(s_settings, ".", s_logger);
                    }
                }

                List<IssueToCreateWithEpic> issuesToCreate = null;
                using (LoggingScope li = new LoggingScope("Processing input file [Yellow!{0}]", s_arguments.InputFile))
                {
                    //Colorizer.WriteLine(, );

                    // Parse the issue data from the input file
                    issuesToCreate = ParseInputFile(new CsvConfiguration(CultureInfo.InvariantCulture, missingFieldFound: null));
                }

                using (LoggingScope li = new LoggingScope("Identifying issues with existing parent epics."))
                {
                    IdentifyIssueDependencies(issuesToCreate);
                }

                using (LoggingScope li = new LoggingScope("Validate and set milestone data"))
                {
                    // Validate the milestone data (and create the milestone objects as needed)
                    await ValidateAndSetMilestonesAsync(issuesToCreate).ConfigureAwait(false);
                }

                using (LoggingScope li = new LoggingScope("Determine order in which to create issues"))
                {
                    // Sort the issues based on the parent Epic
                    issuesToCreate = SortIssuesByParent(issuesToCreate);
                }

                // Display all the information about the issues and prompt before creating

                foreach (IssueToCreateWithEpic issue in issuesToCreate)
                {
                    Colorizer.WriteLine($"[Cyan!{issue.Repository}]/[Yellow!{issue.Title}] Assigned:[White!{issue.AssignedTo}], Labels:[DarkGreen!{string.Join(',', issue.Labels)}], Milestone:[Yellow!{issue.Milestone}] {Environment.NewLine}  > Parent:[Cyan!{issue.EpicRepo}]/[Yellow!{issue.EpicTitle}]");
                }

                Colorizer.WriteLine("Proceed? [Green!y]/[Red!n]");
                if (Console.ReadKey().Key != ConsoleKey.Y)
                {
                    Colorizer.WriteLine("[Red!Nothing changed]");
                    return;
                }
                else
                {
                    using (LoggingScope li = new LoggingScope("Create issues on GitHub"))
                    {
                        // Create the issues on GitHub
                        await CreateIssuesAsync(issuesToCreate);
                    }
                }
            }
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
                    Colorizer.WriteLine("Selecting [Yellow!{0}] as next to create", current.Title);
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

            return results.ToList();
        }

        private static async Task CreateIssuesAsync(List<IssueToCreateWithEpic> parsedIssueData)
        {
            // The issues are sorted such that no epic shows up before its parent epic.
            // this means I should be able to get the parent epic all the time.

            int issuesCreated = parsedIssueData.Count;
            // create all the issues, without being in an epic.
            foreach (IssueToCreateWithEpic issue in parsedIssueData)
            {
                Colorizer.WriteLine("Create [Yellow!{0}]? [Green!y]/[Red!/n]", issue.Title);
                if (Console.ReadKey().Key != ConsoleKey.Y)
                {
                    continue;
                }

                using (LoggingScope ls = new LoggingScope("Creating [Yellow!{0}]", issue.Title))
                {
                    if (!string.IsNullOrEmpty(issue.EpicTitle))
                    {
                        issue.Epic = await GetEpicFromTitle(issue);
                    }

                    (bool issueCreated, string errorOrUrl) = await s_issueManager.TryCreateNewIssueAsync(issue, false);

                    if (issueCreated == true)
                    {
                        Colorizer.WriteLine("Issue [Yellow!{0}] created with url [Cyan!{1}]", issue.Title, errorOrUrl);
                    }
                    else
                    {
                        Colorizer.WriteLine("[Red!Error]: Could not create issue [Yellow!{0}]. [Red!Stopping]", issue.Title);
                        //break;
                    }
                    await Task.Delay(500);
                }
            }
        }

        private static async Task<IssueDescription> GetEpicFromTitle(IssueToCreateWithEpic issue)
        {
            List<IssueDescription> epics = await s_issueManager.GetEpicsWithTitleAsync(issue.EpicTitle, $"{issue.EpicOrg}\\{issue.EpicRepo}");
            await Task.Delay(500);

            Colorizer.WriteLine("Found parent epic [Cyan!{0}].", epics.FirstOrDefault().Issue.Title);

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


                    Colorizer.WriteLine("Found: {0}", issue.ToString());

                    issues.Add(issue);
                }

                Colorizer.WriteLine("Found [Yellow!{0}] issues.", issues.Count);


                Colorizer.WriteLine("[Green!Done]!");
            }
            return issues;
        }

        private static void IdentifyIssueDependencies(List<IssueToCreateWithEpic> issues)
        {
            // once we read the entries we can decide which ones require us to create issues vs the ones that we should expect already exist.

            foreach (IssueToCreateWithEpic issue in issues)
            {
                // does the issue need a parent epic to be created?
                if (string.IsNullOrEmpty(issue.EpicTitle))
                {
                    Colorizer.WriteLine("Marked issue [Yellow!{0}] as root (no dependencies).", issue.Title);
                    issue.HasParent = true;
                    continue;
                }

                // check to see if the parent of this issue is in the list to be created.
                // if not, mark hasParent=true
                if (!issues.Where(entry => StringComparer.OrdinalIgnoreCase.Equals(entry.Title, issue.EpicTitle) &&
                        StringComparer.OrdinalIgnoreCase.Equals(entry.Repository, issue.EpicRepo) &&
                        StringComparer.OrdinalIgnoreCase.Equals(entry.Organization, issue.EpicOrg)).Any())
                {
                    Colorizer.WriteLine("Marked issue [Yellow!{0}] as having no dependencies that need to be created in the list of issues.", issue.Title);
                    issue.HasParent = true;
                }
            }
        }
    }

    public class LoggingScope : IDisposable
    {
        private string _scopeName;
        private object[] _arguments;
        private Stopwatch _timer;

        public LoggingScope(string scopeName, params object[] arguments)
        {
            _scopeName = scopeName;
            _arguments = arguments;

            Colorizer.Write("[Magenta!Start] ");
            Colorizer.WriteLine(scopeName, arguments);
            _timer = new Stopwatch();
            _timer.Start();
        }


        public void Dispose()
        {
            _timer.Stop();

            Colorizer.Write("[Green!Done] ");
            Colorizer.Write(_scopeName, _arguments);
            Colorizer.WriteLine(" in [Cyan!{0}ms]", _timer.Elapsed.Milliseconds);
        }
    }

    public class FileLogWriter : IOutputWriter, IDisposable
    {
        private string _fileName;
        private StreamWriter _sw;
        private ConsoleWriter _cw;
        public FileLogWriter(string fileName)
        {
            _fileName = fileName;
            _cw = new ConsoleWriter();
            _cw.ForegroundColor = ConsoleColor.Gray;

            _sw = new StreamWriter(_fileName, true);
        }

        public ConsoleColor ForegroundColor
        {
            get => _cw.ForegroundColor;
            set => _cw.ForegroundColor = value;
        }

        public void Dispose()
        {
            _sw.Flush();
            _sw.Dispose();
        }

        public void Write(string text)
        {
            _sw.Write(text);
            _cw.Write(text);
        }

        public void WriteLine(string text)
        {
            _sw.WriteLine(text);
            _cw.WriteLine(text);
        }
    }
}
