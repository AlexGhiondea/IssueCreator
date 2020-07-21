using Octokit;
using System.Collections.Generic;

namespace IssueCreator.Models
{
    public class RepoLabel
    {
        public string Name { get; set; }

        public RepoLabel(Label label)
        {
            Name = label.Name;
        }

        public RepoLabel() // for deserialization
        {

        }

        internal static List<RepoLabel> FromLabelList(IReadOnlyList<Label> labels)
        {
            List<RepoLabel> result = new List<RepoLabel>();
            foreach (Label label in labels)
            {
                result.Add(new RepoLabel(label));
            }
            return result;
        }
    }
}
