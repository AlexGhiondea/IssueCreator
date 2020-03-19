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
                        bool processResult = StringHelpers.ProcessCtrlBackspace(Text, SelectionStart, out string remainingText, out int newSelectionIndex);
                        Text = remainingText;
                        SelectionStart = newSelectionIndex;
                        return processResult;
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
