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

namespace BulkIssueCreatorCLI
{
    class Program
    {
        private static FileLogger s_logger;
        private static readonly Settings s_settings = new Settings();
        private static Arguments s_arguments;
        static void Main(string[] args)
        {
            if (!Parser.TryParse<Arguments>(args, out Arguments s_arguments))
            {
                return;
            }

            s_logger = new FileLogger(Path.Combine(Settings.SettingsFolder, "bulkIssueCreatorCLI.log"));

            Colorizer.Write("Loading settings...");
            using (IDisposable scope = s_logger.CreateScope("Loading settings"))
            {
                s_settings.Initialize(Settings.Deserialize(Settings.SettingsFile, s_logger));
            }
            Colorizer.WriteLine("[Green!Done]!");

            Colorizer.Write("Reading input file [Yellow!{0}]", s_arguments.InputFile);

            CsvConfiguration cfg = new CsvConfiguration(CultureInfo.InvariantCulture, missingFieldFound: null);
            using (StreamReader sr = new StreamReader(s_arguments.InputFile))
            using (CsvReader csv = new CsvReader(sr, cfg))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {

                    IssueToCreate issue = new IssueToCreate
                    {
                        Organization = csv.GetField<string>("Organization")?.Trim(),
                        Repository = csv.GetField<string>("Repository")?.Trim(),
                        Title = csv.GetField<string>("Title")?.Trim(),
                        Description = csv.GetField<string>("Description")?.Trim(),
                        AssignedTo = csv.GetField<string>("AssignedTo")?.Trim(),
                        Estimate = csv.GetField<string>("Estimate")?.Trim(),
                        CreateAsEpic = csv.GetField<bool>("IsEpic")
                    };

                    string milestone = csv.GetField<string>("Milestone");
                    if (!string.IsNullOrWhiteSpace(milestone))
                    {
                        milestone = milestone.Trim();
                       // issue.Milestone = await _issueManager.GetMilestoneAsync(issue.Organization, issue.Repository, milestone);
                    }

                    string labels = csv.GetField<string>("Labels");
                    if (!string.IsNullOrWhiteSpace(labels))
                    {
                        labels = labels.Trim();
                        issue.Labels = new List<string>(labels.Split(','));
                    }

                    //issues.Add(issue);

                    Colorizer.WriteLine("[Green!Done]!");
                }
            }
        }
    }
}
