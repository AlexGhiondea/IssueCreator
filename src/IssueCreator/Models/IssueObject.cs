using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueCreator.Models
{
    public class IssueObject
    {
        public string Title { get; set; }
        public string State { get; set; }
        public int Number { get; set; }

        public IssueObject(Issue issue)
        {
            Title = issue.Title;
            State = issue.State.StringValue;
            Number = issue.Number;
        }

        public IssueObject() //for deserialization
        {

        }
    }
}
