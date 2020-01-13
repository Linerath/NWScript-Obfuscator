using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWScript_Obfuscator.Services
{
    public class NWScriptObfuscator
    {
        private readonly String[] reservedKeywords = { "void", "return", "int", "float", "string", "object" };
        //private readonly String[] reservedFunctions = 

        public String Obfuscate(String input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return input;

            String output = RemoveWhiteSpaces(input);

            return output;
        }

        private String RemoveWhiteSpaces(String input)
        {
            String output = "";

            bool str = false;
            String prevWord = "";

            foreach (var ch in input)
            {
                if (str)
                {
                    if (ch == '"')
                    {
                        str = false;
                    }
                }
                else
                {
                    if (ch == '"')
                    {
                        str = true;
                    }
                }

                if (Char.IsWhiteSpace(ch) && !str)
                {
                    // Don't remove if it's variable name (often goes after reserved keywords);
                    if (reservedKeywords.Contains(prevWord))
                    {
                        output += ch;
                        continue;
                    }
                    else
                    {
                        prevWord = "";
                    }
                }
                else
                {
                    prevWord += ch;
                    output += ch;
                }
            }

            return output;
        }

        private List<String> GetVariablesNames(String input)
        {
            List<String> result = new List<string>();

            //TODO...

            return result;
        }
    }
}
