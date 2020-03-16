using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IssueCreator.Helpers;

namespace IssueCreator.Controls
{
    public partial class TextBoxEx : TextBox
    {
        public TextBoxEx()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message m, Keys keyData)
        {
            if (ShortcutsEnabled)
            {
                if (keyData == (Keys.Control | Keys.Back))
                {
                    if (!ReadOnly)
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
                }
                else if (keyData == (Keys.Control | Keys.A))
                {
                    if (!ReadOnly && Multiline)
                    {
                        SelectAll();
                        return (true);
                    }
                }
            }

            return base.ProcessCmdKey(ref m, keyData);
        }
    }
}
