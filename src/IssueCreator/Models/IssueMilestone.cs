using System;

namespace IssueCreator.Models
{
    public class IssueMilestone
    {
        public int? Number { get; private set; }
        public string Title { get; private set; }
        public DateTimeOffset? DueOn { get; private set; }

        public static IssueMilestone FromGitHubMilestone(Octokit.Milestone m)
        {
            return new IssueMilestone()
            {
                Number = m.Number,
                Title = m.Title,
                DueOn = m.DueOn
            };
        }

        public override string ToString()
        {
            return $"{Title}";
        }
    }
}
