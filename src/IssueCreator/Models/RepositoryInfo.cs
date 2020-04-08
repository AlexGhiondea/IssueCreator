using Octokit;

namespace IssueCreator.Models
{
    public class RepositoryInfo
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public RepositoryInfo(Repository repo)
        {
            Id = repo.Id;
            FullName = repo.FullName;
        }
        public RepositoryInfo() //for deserialization
        {

        }
    }
}
