using NWScript_Obfuscator.Services;
using System;
using System.Windows.Forms;

namespace NWScript_Obfuscator.Forms
{
    public partial class F_Main : Form
    {
        private readonly NWScriptObfuscator obfuscator = new NWScriptObfuscator();

        public F_Main()
        {
            InitializeComponent();

            RTB_Input.Text = @"void main()
{
  object oPC = GetPCSpeaker();

  string name = "" name "";
  name = ""name"";
  string new = name;

  return;
}";
        }

        #region Events
        private void B_Go_Click(object sender, EventArgs e)
        {
            String input = RTB_Input.Text;

            String output = obfuscator.Obfuscate(input);

            RTB_Output.Text = output;
        }

        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO");
        }
        #endregion
    }
}
