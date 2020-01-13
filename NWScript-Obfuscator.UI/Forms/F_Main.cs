using System;
using System.Windows.Forms;

namespace NWScript_Obfuscator.UI.Forms
{
    public partial class F_Main : Form
    {
        private readonly NWScriptObfuscator obfuscator = new NWScriptObfuscator();

        public F_Main()
        {
            InitializeComponent();

            RTB_Input.Text = @"
const int TEST = 1;
void main()
{
  object oPC = GetPCSpeaker();

  string name = "" name "";
  name = ""name"";
  string newStr = name;

  int intVar = TEST;

  return;
}";
        }

        #region Events
        private void B_Go_Click(object sender, EventArgs e)
        {
            String input = RTB_Input.Text;

            String output = obfuscator.Obfuscate(input, CB_RemoveWS.Checked, CB_RenameVars.Checked);

            RTB_Output.Text = output;
        }

        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO");
        }
        #endregion
    }
}
