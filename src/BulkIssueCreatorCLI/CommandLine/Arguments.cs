using CommandLine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkIssueCreatorCLI
{
    class Arguments
    {
        [RequiredArgument(0, "type","What operation would you like to perform?")]
        public ActionType Type { get; set; }


        [RequiredArgument(1, "input", "The csv file containing the descriptions of the epics")]
        public string InputFile { get; set; }
    }
}
