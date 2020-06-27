using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NWScript_Obfuscator
{
    public class NWScriptObfuscator
    {
        private readonly String[] varTypes;
        private readonly String[] reservedKeywords;

        private const String ERROR_INVALID_CODE = "Invalid code";

        public NWScriptObfuscator()
        {
            varTypes = new String[] { "int", "float", "string", "object" };
            reservedKeywords = varTypes.Concat(new String[] { "void", "return", "const", "include" }).ToArray();
        }

        public String Obfuscate(String input, bool removeWS = true, bool rename = true)
        {
            if (String.IsNullOrWhiteSpace(input))
                return input;

            String output = input;
            if (removeWS)
                output = RemoveWhiteSpaces(output);
            if (rename)
                output = Rename(output);

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
                        prevWord = "";
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

        private String RenameVariables(String input)
        {
            Char[] varNameEndChars = new Char[] { ')', '=', ';' };

            List<String> varNames;
            try
            {
                varNames = GetVariablesNames(input);
            }
            catch (InvalidCodeException ex)
            {
                input = !String.IsNullOrWhiteSpace(ex.Message)
                    ? $"{ERROR_INVALID_CODE}: {ex.Message}"
                    : ERROR_INVALID_CODE;

                return input;
            }

            String output = "";
            Dictionary<String, String> newNames = new Dictionary<String, String>();
            bool str = false;
            String prevWord = "";
            foreach (var ch in input)
            {
                if (ch == '"')
                {
                    str = !str;
                    prevWord = "";
                    output += ch;
                    continue;
                }

                if (str)
                {
                    prevWord += ch;
                    output += ch;
                    continue;
                }

                if (Char.IsWhiteSpace(ch) || varNameEndChars.Contains(ch))
                {
                    if (varNames.Contains(prevWord))
                    {
                        if (!newNames.TryGetValue(prevWord, out String newName))
                        {
                            newName = $"v{newNames.Count}";
                            newNames.Add(prevWord, newName);
                        }

                        output = output.Remove(output.Length - prevWord.Length, prevWord.Length);
                        output += newName;

                        output += ch;

                        prevWord = "";
                        continue;
                    }

                    prevWord = "";
                }
                else
                {
                    prevWord += ch;
                }

                output += ch;
            }

            //foreach (var varName in varNames)
            //{
            //    String newName = $"v{++prevIndex}";

            //    input = Regex.Replace(input, $@"({varName})", $" {newName}");
            //}

            return output;
        }

        private String Rename(String input)
        {
            Char[] varNameEndChars = new Char[] { ')', '=', ';' };

            List<String> varNames;
            try
            {
                varNames = GetVariablesNames(input);
            }
            catch (InvalidCodeException ex)
            {
                input = !String.IsNullOrWhiteSpace(ex.Message)
                    ? $"{ERROR_INVALID_CODE}: {ex.Message}"
                    : ERROR_INVALID_CODE;

                return input;
            }

            String output = "";
            Char[] startNewWordAfter = { ' ', '(', ')', ';', '=', '+', '-', '*', '/' };
            Dictionary<String, String> newNames = new Dictionary<String, String>();
            bool str = false;
            String prevWord = "", currWord = "";

            foreach (var ch in input)
            {
                if (ch == '"')
                {
                    str = !str;
                    prevWord = "";
                    currWord = "";
                    output += ch;
                    continue;
                }

                if (str)
                {
                    prevWord = "";
                    currWord = "";
                    output += ch;
                    continue;
                }

                if (startNewWordAfter.Contains(ch))
                {
                    if (varNames.Contains(currWord))
                    {
                        if (!newNames.TryGetValue(currWord, out String newName))
                        {
                            newName = $"v{newNames.Count}";
                            newNames.Add(currWord, newName);
                        }

                        output = output.Remove(output.Length - currWord.Length);
                        output += newName;
                    }

                    prevWord = currWord;
                    currWord = "";
                }
                else
                {
                    currWord += ch;
                }

                output += ch;
            }

            return output;
        }

        private List<String> GetVariablesNames(String input)
        {
            List<String> result = new List<string>();

            String varPattern = "";
            foreach (var varType in varTypes)
            {
                if (varPattern != "")
                    varPattern += "|";

                varPattern += $"({varType})";
            }

            varPattern = "(" + varPattern + ")";

            var matches = Regex.Matches(input, $@"{varPattern}\s+\w+");

            foreach (Match match in matches)
            {
                String varName = match.Value;

                try
                {
                    varName = varName.Substring(varName.LastIndexOf(' ') + 1);
                    result.Add(varName);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidCodeException();
                }

                if (reservedKeywords.Contains(varName))
                    throw new InvalidCodeException("reserved keyword variable name");
            }

            return result;
        }

        /*
        private String RenameFunctions(String input)
        {
            //Char[] funcNameEndChars = new Char[] { '(' };

            List<String> funcNames;
            try
            {
                funcNames = GetFunctionsNames(input);
            }
            catch (InvalidCodeException ex)
            {
                input = !String.IsNullOrWhiteSpace(ex.Message)
                    ? $"{ERROR_INVALID_CODE}: {ex.Message}"
                    : ERROR_INVALID_CODE;

                return input;
            }

            foreach (var funcName in funcNames)
            {
                String pattern = $@"{funcName}(";
            }




            String output = "";
            Dictionary<String, String> newNames = new Dictionary<String, String>();
            bool str = false;
            String prevWord = "";
            /*
            foreach (var ch in input)
            {
                if (ch == '"')
                {
                    str = !str;
                    prevWord = "";
                    output += ch;
                    continue;
                }

                if (str)
                {
                    prevWord += ch;
                    output += ch;
                    continue;
                }

                if (Char.IsWhiteSpace(ch) || varNameEndChars.Contains(ch))
                {
                    if (varNames.Contains(prevWord))
                    {
                        if (!newNames.TryGetValue(prevWord, out String newName))
                        {
                            newName = $"v{newNames.Count}";
                            newNames.Add(prevWord, newName);
                        }

                        output = output.Remove(output.Length - prevWord.Length, prevWord.Length);
                        output += newName;

                        output += ch;

                        prevWord = "";
                        continue;
                    }

                    prevWord = "";
                }
                else
                {
                    prevWord += ch;
                }

                output += ch;
            }
            return output;
        }

        private List<String> GetFunctionsNames(String input)
        {
            List<String> result = new List<String>();

            String funcPattern = "";
            foreach (var varType in varTypes)
            {
                if (funcPattern != "")
                    funcPattern += "|";

                funcPattern += $"({varType})";
            }
            if (funcPattern != "")
                funcPattern += "|";
            funcPattern += "(void)";

            funcPattern = $@"({funcPattern})\s+\w+\(";

            var matches = Regex.Matches(input, funcPattern);

            foreach (Match match in matches)
            {
                String funcName = match.Value;

                try
                {
                    funcName = funcName.Substring(funcName.LastIndexOf(' ') + 1);
                    funcName = funcName.Remove(funcName.IndexOf('('));
                    if (funcName == "main")
                        continue;
                    result.Add(funcName);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidCodeException();
                }

                if (reservedKeywords.Contains(funcName))
                    throw new InvalidCodeException("reserved keyword variable name");
            }

            return result;
        }
    */
    }

    [System.Serializable]
    public class InvalidCodeException : Exception
    {
        public InvalidCodeException() { }
        public InvalidCodeException(string message) : base(message) { }
        public InvalidCodeException(string message, Exception inner) : base(message, inner) { }
        protected InvalidCodeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
