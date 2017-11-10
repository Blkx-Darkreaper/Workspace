using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Bits
{
    public class OpcodeSet
    {
        protected Dictionary<string, string> allOpcodes = new Dictionary<string, string>();
        public string this[string name]
        {
            get { return allOpcodes[name.ToLower()]; }
        }

        public OpcodeSet()
        {
            allOpcodes.Add("move", "mov");
            allOpcodes.Add("push", "psh");
            allOpcodes.Add("pop", "pop");
            allOpcodes.Add("return", "ret");
        }
    }

    public class Compiler
    {
        protected OpcodeSet opcodes = new OpcodeSet();

        public int Get32BitTypeSize(string type)
        {
            return GetTypeSize(type, 32);
        }

        public int GetTypeSize(string type, int datapathWidth)
        {
            int size = 0;

            switch(type) {
                case "int":
                    size = datapathWidth / 8;
                    break;

                case "char":
                    size = datapathWidth / 8;
                    break;

                case "void":
                default:
                    break;
            }

            return size;
        }

        public List<Instruction> Compile(string code)
        {
            List<Instruction> allInstructions = new List<Instruction>();

            StringReader reader = new StringReader(code);

            int lineNumber = 1;
            string nextLine = reader.ReadLine();
            while (nextLine != null)
            {
                CompileFunctionDef(ref allInstructions, nextLine);
                CompileForLoop(ref allInstructions, nextLine);

                nextLine = reader.ReadLine();
                lineNumber++;
            }

            return allInstructions;
        }

        protected void CompileFunctionDef(ref List<Instruction> allInstructions, string line)
        {
            string functionPattern = @"function\s+(\w+)\s+(\w+)[(]?";

            MatchCollection allFunctionMatches = Regex.Matches(line, functionPattern);
            if (allFunctionMatches.Count == 0)
            {
                return;
            }

            /* function int Add(int a, int b) {
             *  int c = a + b;
             *  return c;
             * }
             */

            /* Add: psh ebp ; save the value of base pointer
             * mov ebp, esp ; base pointer now points to top of stack
             * sub esp, 4 ; adjust stack pointer to allocate space for var c 4 * 1
             * add [ebp - 4], [ebp + 8], [ebp + 12] ; c = a + b
             * mov eax, [ebp - 4] ; save return value to EAX register
             * mov esp, ebp ; epilog: restore stack pointer
             * pop ebp ; restore base pointer
             * ret
             */

            string parameterPattern = @"[(]([^()]*)[)]\s*{?";

            MatchCollection allParameterMatches = Regex.Matches(line, parameterPattern);

            for (int i = 0; i < allFunctionMatches.Count; i++)
            {
                Match functionMatch = allFunctionMatches[i];

                string returnType = functionMatch.Groups[0].Value;
                int returnTypeSize = 0; // TODO

                string functionName = functionMatch.Groups[1].Value;

                allInstructions.Add(new Instruction(functionName, opcodes["Push"], new string[] { "ebp" }, "save the value of base pointer"));
                allInstructions.Add(new Instruction(opcodes["Move"], new string[] { "ebp", "esp" }, "base pointer now points to top of stack"));

                Match parameterMatch = allParameterMatches[i];

                string paramStr = parameterMatch.Groups[0].Value;
                if(paramStr.Length == 0)
                {
                    continue;
                }

                string[] allParams = paramStr.Split(',');
                foreach(string param in allParams)
                {
                    string[] paramArr = param.Split(' ');
                    string type = paramArr[0];
                    string paramName = paramArr[1];
                }

                if (returnTypeSize > 0)
                {
                    allInstructions.Add(new Instruction(opcodes["Move"], new string[] { "eax", string.Format("[ebp - {0}]", returnTypeSize) }, "save return value to EAX register"));
                }

                allInstructions.Add(new Instruction(opcodes["Move"], new string[] { "esp", "ebp" }, "epilog: restore stack pointer"));
                allInstructions.Add(new Instruction(opcodes["Pop"], new string[] { "ebp" }, "restore base pointer"));
                allInstructions.Add(new Instruction(opcodes["Return"]));
            }
        }

        protected void CompileForLoop(ref List<Instruction> allInstructions, string line)
        {
            string pattern = @"for\s*[(]\w+\s*(\w+\s*=\s*\d+);\s*(\w+\s*[<>=!]+\s*\d+);\s*(\w+[+]{2}|[-]{2})\s*[)]\s*{?";

            MatchCollection allMatches = Regex.Matches(line, pattern);
            if(allMatches.Count == 0)
            {
                return;
            }

            foreach(Match match in allMatches)
            {
                string startCondition = match.Groups[0].Value;
                string endCondition = match.Groups[1].Value;
                string incrementor = match.Groups[2].Value;
            }
        }
    }
}