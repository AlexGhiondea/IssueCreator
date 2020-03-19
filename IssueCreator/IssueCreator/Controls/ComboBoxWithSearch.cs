using IssueCreator.Helpers;
using System.Windows.Forms;

namespace IssueCreator.Controls
{
    public partial class ComboBoxWithSearch : ComboBox
    {
        public ComboBoxWithSearch() : base()
        {
            AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var returnValue = base.CreateParams;
                returnValue.Style |= 0x2; // Add LBS_SORT
                returnValue.Style ^= 128; // Remove LBS_USETABSTOPS (optional)
                return returnValue;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Back))
            {
                if (SelectionStart > 0)
                {
                    int i = (SelectionStart - 1);

                    // Potentially trim white space:
                    if (char.IsWhiteSpace(Text, i))
                        i = Text.StartIndexOfSameCharacterClass(i) - 1;

                    // Find previous marker:
                    if (i > 0)
                        i = Text.StartIndexOfSameCharacterClass(i);
                    else
                        i = 0; // Limit i as it may become -1 on trimming above.

                    // Remove until previous marker or the beginning:
                    Text = Text.Remove(i, SelectionStart - i);
                    SelectionStart = i;
                    return (true);
                }
                else
                {
                    return (true); // Ignore to prevent a white box being placed.
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
