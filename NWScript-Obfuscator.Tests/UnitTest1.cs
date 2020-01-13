using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NWScript_Obfuscator.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly NWScriptObfuscator obfuscator = new NWScriptObfuscator();

        [TestMethod]
        public void RemovingWSTest()
        {
            String code1 = @"const int TEST = 1;
void main()
{
  object oPC = GetPCSpeaker();

  string name = "" name "";
  name = ""name"";
  string newStr = name;

  int intVar = TEST;

  return;
}";

            String code2 = @"const string STR1 = ""STR1"";
const string STR2 = ""STR2"";

int StartingConditional()
{
    int result = STR1 == STR2;

    return resut;
}
";

            String code1Exp = @"const int TEST=1;void main(){object oPC=GetPCSpeaker();string name="" name "";name=""name"";string newStr=name;int intVar=TEST;return;}";
            String code1Obf = obfuscator.Obfuscate(code1, true, false);

            String code2Exp = @"const string STR1=""STR1"";const string STR2=""STR2"";int StartingConditional(){int result=STR1==STR2;return resut;}";
            String code2Obf = obfuscator.Obfuscate(code2, true, false);


            Assert.IsTrue(code1Obf == code1Exp);
            Assert.IsTrue(code2Exp == code2Obf);
        }

        [TestMethod]
        public void RenamingVarsTest()
        {
            String code1 = @"const int TEST = 1;
void main()
{
  object oPC = GetPCSpeaker();
  string name = "" name "";
  name = ""name"";
  string newStr = name;
  int intVar = TEST;
  return;
}";

            String code1Exp = @"const int v0 = 1;
void main()
{
  object v1 = GetPCSpeaker();
  string v2 = "" name "";
  v2 = ""name"";
  string v3 = v2;
  int v4 = v0;
  return;
}";

            String code2 = @"const string STR1 = ""STR1"";
const string STR2 = ""STR2"";
int StartingConditional()
{
    int result = STR1 == STR2;
    return result;
}";

            String code2Exp = @"const string v0 = ""STR1"";
const string v1 = ""STR2"";
int StartingConditional()
{
    int v2 = v0 == v1;
    return v2;
}";



            String code1Obf = obfuscator.Obfuscate(code1, false, true);
            String code2Obf = obfuscator.Obfuscate(code2, false, true);


            Assert.IsTrue(code1Obf == code1Exp);
            Assert.IsTrue(code2Exp == code2Obf);
        }
    }
}
