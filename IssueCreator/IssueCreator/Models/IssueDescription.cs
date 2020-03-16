using Octokit;

namespace IssueCreator.Models
{
    public class IssueDescription
    {
        public Issue Issue { get; set; }
        public Repository Repo { get; set; }
        public override string ToString()
        {
            return $"{Issue.Title} ({Repo.FullName})";
        }
    }
}
