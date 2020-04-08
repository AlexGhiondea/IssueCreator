using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueCreator.Models
{
    public class GitHubContributor
    {
        public string Login { get; set; }

        public GitHubContributor(RepositoryContributor contributor)
        {
            Login = contributor.Login;
        }

        public GitHubContributor() //for deserialization
        {

        }

        internal static List<GitHubContributor> FromContributorsList(IReadOnlyList<RepositoryContributor> repoContributorList)
        {
            List<GitHubContributor> contrib = new List<GitHubContributor>();
            foreach (RepositoryContributor contributor in repoContributorList)
            {
                contrib.Add(new GitHubContributor(contributor));
            }
            return contrib;
        }
    }
}
