using Octokit;
using System;
using System.Collections.Generic;

namespace IssueCreator.Models
{
    public class IssueMilestone
    {
        public int? Number { get; set; }
        public string Title { get; set; }
        public DateTimeOffset? DueOn { get; set; }

        public IssueMilestone(Milestone m)
        {
            Number = m.Number;
            Title = m.Title;
            DueOn = m.DueOn;
        }

        public IssueMilestone() // for deserialization
        {

        }

        public override string ToString()
        {
            return $"{Title}";
        }

        internal static IEnumerable<IssueMilestone> FromMilestoneList(IReadOnlyList<Milestone> milestones)
        {
            List<IssueMilestone> result = new List<IssueMilestone>();
            foreach (Milestone milestone in milestones)
            {
                result.Add(new IssueMilestone(milestone));
            }
            return result;
        }
    }
}
