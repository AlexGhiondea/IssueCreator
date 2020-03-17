using System;
using System.Collections.Generic;

namespace IssueCreator.Models
{
    internal class IssueToCreate : IEquatable<IssueToCreate>
    {
        public string Organization { get; set; }
        public string Repository { get; set; }
        public string AssignedTo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Labels { get; set; }
        public IssueDescription Epic { get; set; }
        public IssueMilestone Milestone { get; set; }
        public string Estimate { get; set; }
        public bool CreateAsEpic { get; set; }

        public bool Equals(IssueToCreate other)
        {
            if (other == null)
            {
                return false;
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(this.Organization, other.Organization) &&
                StringComparer.OrdinalIgnoreCase.Equals(this.Repository, other.Repository) &&
                StringComparer.OrdinalIgnoreCase.Equals(this.Title, other.Title) &&
                StringComparer.OrdinalIgnoreCase.Equals(this.Description, other.Description))
            {
                return true;
            }

            return false;
        }
    }
}
