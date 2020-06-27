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

            RTB_Input.Text = @"const int C4 = 1337;
void main()
{
  object oPC = GetFirstPC();
  
  int junky = Meth(oPC);
  
  if (junky)
    SpeakString(""i love Meth"");
}

int Meth(object oPC)
{
  int result = GetIsObjectValid(oPC);
  
  return result;
}";
        }

        #region Events
        private void B_Go_Click(object sender, EventArgs e)
        {
            String input = RTB_Input.Text;

            String output = obfuscator.Obfuscate(input, CB_RemoveWS.Checked, CB_Rename.Checked);

            RTB_Output.Text = output;
        }

        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO");
        }
        #endregion
    }
}
