using System.Windows.Forms;

namespace IssueCreator.Controls
{
    public partial class ListBoxWithSearch : ListBox
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams returnValue = base.CreateParams;
                returnValue.Style |= 0x2; // Add LBS_SORT
                returnValue.Style ^= 128; // Remove LBS_USETABSTOPS (optional)
                return returnValue;
            }
        }
    }
}
