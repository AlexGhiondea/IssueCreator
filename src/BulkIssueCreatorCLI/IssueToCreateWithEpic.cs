using IssueCreator.Models;

namespace BulkIssueCreatorCLI
{
    class IssueToCreateWithEpic : IssueToCreate
    {
        public string EpicOrg { get; set; }
        public string EpicRepo { get; set; }
        public string EpicTitle { get; set; }
        public string MilestoneText { get; set; }

        public bool HasParent { get; set; } = false;

        public override string ToString()
        {
            return $"{Title} ({Repository}), Labels: {LabelsCollection}, Parent: {EpicTitle} ({EpicRepo})";
        }
    }
}
