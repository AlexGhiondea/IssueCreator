using Octokit;
using System.Collections.Generic;
using System.Linq;

namespace IssueCreator.Models
{
    public class IssueObject
    {
        public string Title { get; set; }
        public string State { get; set; }
        public int Number { get; set; }
        public string HtmlUrl { get; set; }
        public string Body { get; set; }
        public List<string> Tags { get; set; }
        public IssueMilestone Milestone { get; set; }

        public IssueObject(Issue issue)
        {
            Title = issue.Title;
            State = issue.State.StringValue;
            Number = issue.Number;
            HtmlUrl = issue.HtmlUrl;
            Body = issue.Body;
            Tags = issue.Labels.Select(l => l.Name).ToList();
            if (issue.Milestone != null)
            {
                Milestone = new IssueMilestone(issue.Milestone);
            }
        }

        public IssueObject() //for deserialization
        {

        }
    }
}
