namespace IssueCreator.Models
{
    public class IssueDescription
    {
        public IssueObject Issue { get; set; }
        public RepositoryInfo Repo { get; set; }

        public override string ToString()
        {
            return $"{Issue.Title} ({Repo.FullName})";
        }
    }
}
