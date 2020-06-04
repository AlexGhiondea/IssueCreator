using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueCreator.Helpers
{
    internal static class UIHelpers
    {
        public static (string, string) GetRepoOwner(object selectedItem)
        {
            if (selectedItem == null)
                return (string.Empty, string.Empty);

            return StringHelpers.GetOwnerAndRepoFromString(selectedItem.ToString());
        }
    }
}
