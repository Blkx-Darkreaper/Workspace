using System;
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

    public class CodeBlock
    {
        public string signature { get; protected set; }
        public Dictionary<string, Variable> allLocalVariables { get; protected set; }
        public bool isGlobal { get; protected set; }

        public CodeBlock()
        {
            this.signature = null;
            this.allLocalVariables = new Dictionary<string, Variable>();
            this.isGlobal = true;
        }

        public CodeBlock(string signature)
        {
            this.signature = signature;
            this.allLocalVariables = new Dictionary<string, Variable>();
            this.isGlobal = false;
        }
    }

    public struct Variable
    {
        public string name;
        public string type;
        public int memoryAddress;

        public Variable(string name, string type, int address)
        {
            this.name = name;
            this.type = type;
            this.memoryAddress = address;
        }
    }

    public class Compiler
    {
        protected OpcodeSet opcodes = new OpcodeSet();
        protected List<CodeBlock> allBlocks = new List<CodeBlock>();

        public int Get32BitTypeSize(string type)
        {
            return GetTypeSize(type, 32);
        }

        public int GetTypeSize(string type, int datapathWidth = 32)
        {
            int size = 0;

            switch (type)
            {
                case "int":
                    size = datapathWidth / 8;
                    break;

                case "char":
                    size = datapathWidth / 8;
                    break;

                case "byte":
                    size = 8;
                    break;

                case "word":
                    break;

                case "dword":
                    break;

                case "void":
                default:
                    break;
            }

            return size;
        }

        public void OpenBlock(string signature)
        {
            allBlocks.Add(new CodeBlock(signature));
        }

        public void CloseBlock()
        {
            allBlocks.RemoveAt(allBlocks.Count - 1);
        }

        public void AddGlobalVariable(string name, string type, int address)
        {
            var globalVars = allBlocks[0].allLocalVariables;
            AddVariable(name, type, address, globalVars);
        }

        public void AddBlockVariable(string name, string type, int address)
        {
            if (allBlocks.Count < 2)
            {
                throw new InvalidOperationException("No open block");
            }

            var localVars = allBlocks[allBlocks.Count - 1].allLocalVariables;
            AddVariable(name, type, address, localVars);
        }

        protected void AddVariable(string name, string type, int address, Dictionary<string, Variable> globalVars)
        {
            bool nameConflict = globalVars.ContainsKey(name);
            if (nameConflict == true)
            {
                throw new InvalidOperationException(String.Format("A local variable or parameter named '{0}' cannot be declared in this scope because that name is used in an enclosing local scope to define a local or parameter"));
            }

            globalVars.Add(name, new Variable(name, type, address));
        }

        public Variable GetVariable(string name)
        {
            for(int i = allBlocks.Count; i >= 0; i--)
            {
                CodeBlock block = allBlocks[i];
                bool hasVar = block.allLocalVariables.ContainsKey(name);
                if(hasVar == false)
                {
                    continue;
                }

                return block.allLocalVariables[name];
            }

            throw new InvalidOperationException(String.Format("The name '{0}' does not exist in the current context"));
        }

        public List<Instruction> Compile(string code)
        {
            List<Instruction> allInstructions = new List<Instruction>();
            allBlocks.Add(new CodeBlock());

            StringReader reader = new StringReader(code);

            int lineNumber = 1;
            string nextLine = reader.ReadLine();
            while (nextLine != null)
            {
                nextLine = CompileFunctionDef(ref allInstructions, nextLine);
                nextLine = CompileForLoop(ref allInstructions, nextLine);
                nextLine = CompileOpenBrace(nextLine);

                nextLine = CompileOperation(nextLine);

                nextLine = CompileEndBlock(ref allInstructions, nextLine);

                if (nextLine.Length > 0)
                {
                    throw new InvalidOperationException(String.Format("Line {0} was not entirely consumed", lineNumber));
                }

                nextLine = reader.ReadLine();
                lineNumber++;
            }

            return allInstructions;
        }

        protected string CompileOperation(string line)
        {
            string pattern = @"^(\w+)?\s*(\w+)\s*([!><=+\-*\/%]?=)\s*(\d|\w*)\s*([+]|-|[*]|[\/]|%)\s*(\d|\w*);$";
            Match match = Regex.Match(line, pattern);
            if (match.Success == false)
            {
                return line;
            }

            // int c = a + b;

            // add [ebp - 4], [ebp + 8], [ebp + 12] ; c = a + b

            int totalMatches = match.Groups.Count;

            string resultType, result, equalityOperator, firstOperand, secondOperand, mathOperator = string.Empty;

            result = match.Groups[totalMatches - 5].Value;
            equalityOperator = match.Groups[totalMatches - 4].Value;
            firstOperand = match.Groups[totalMatches - 3].Value;
            secondOperand = match.Groups[totalMatches - 2].Value;
            mathOperator = match.Groups[totalMatches - 1].Value;

            if (totalMatches == 7)
            {
                resultType = match.Groups[1].Value;
            }

            string operation = match.Groups[0].Value;
            string remainder = line.Substring(operation.Length);
            return remainder;
        }

        protected string CompileOpenBrace(string line)
        {
            string pattern = @"{";
            Match match = Regex.Match(line, pattern);
            if (match.Success == false)
            {
                return line;
            }

            string remainder = line.Substring(1);
            return remainder;
        }

        protected string CompileEndBlock(ref List<Instruction> allInstructions, string line)
        {
            string pattern = @"}";

            Match match = Regex.Match(line, pattern);
            if (match.Success == false)
            {
                return line;
            }

            CompileFunctionDefEnd(ref allInstructions);
            CompileLoopEnd(ref allInstructions);

            string remainder = line.Substring(1);
            return remainder;
        }

        protected string CompileFunctionDef(ref List<Instruction> allInstructions, string line)
        {
            string functionPattern = @"function\s+(\w+)\s+(\w+)[(]?([^)]*)[)]\s*{?";

            Match functionMatch = Regex.Match(line, functionPattern);
            if (functionMatch.Success == false)
            {
                return line;
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

            string returnType = functionMatch.Groups[1].Value;
            int returnTypeSize = GetTypeSize(returnType);

            string functionName = functionMatch.Groups[2].Value;

            allInstructions.Add(new Instruction(functionName, opcodes["Push"], new string[] { "ebp" }, "save the value of base pointer"));
            allInstructions.Add(new Instruction(opcodes["Move"], new string[] { "ebp", "esp" }, "base pointer now points to top of stack"));

            if (returnTypeSize > 0)
            {
                allInstructions.Add(new Instruction(opcodes["Sub"], new string[] { "esp", returnTypeSize.ToString() }, "adjust stack pointer to allocate space for var c 4 * 1"));
            }

            // Consume function signature
            string signature = functionMatch.Groups[0].Value;
            OpenBlock(signature);

            string remainder = line.Substring(signature.Length);
            return remainder;
        }

        protected void CompileFunctionDefEnd(ref List<Instruction> allInstructions)
        {
            string functionPattern = @"function\s+(\w+)\s+(\w+)[(]?([^)]*)[)]\s*{?";
            string signature = allBlocks[allBlocks.Count - 1].signature;

            Match functionMatch = Regex.Match(signature, functionPattern);
            if (functionMatch.Success == false)
            {
                return;
            }

            CloseBlock();

            string parameterPattern = @"[(]([^()]*)[)]\s*{?";

            Match parameterMatch = Regex.Match(signature, parameterPattern);

            string paramStr = parameterMatch.Groups[1].Value;

            string[] allParams = paramStr.Split(',');
            foreach (string param in allParams)
            {
                string[] paramArr = param.Split(' ');
                string type = paramArr[0];
                string paramName = paramArr[1];
            }

            string returnType = functionMatch.Groups[1].Value;
            int returnTypeSize = GetTypeSize(returnType);
            if (returnTypeSize > 0)
            {
                allInstructions.Add(new Instruction(opcodes["Move"], new string[] { "eax", string.Format("[ebp - {0}]", returnTypeSize) }, "save return value to EAX register"));
            }

            allInstructions.Add(new Instruction(opcodes["Move"], new string[] { "esp", "ebp" }, "epilog: restore stack pointer"));
            allInstructions.Add(new Instruction(opcodes["Pop"], new string[] { "ebp" }, "restore base pointer"));
            allInstructions.Add(new Instruction(opcodes["Return"]));
        }

        protected string CompileLoop(ref List<Instruction> allInstructions, string line)
        {
            string remainder = CompileForLoop(ref allInstructions, line);
            if (remainder.Length != line.Length)
            {
                return remainder;
            }

            return line;
        }

        protected string CompileForLoop(ref List<Instruction> allInstructions, string line)
        {
            string pattern = @"for\s*[(]\w+\s*(\w+\s*=\s*\d+);\s*(\w+\s*[<>=!]+\s*\d+);\s*(\w+[+]{2}|[-]{2})\s*[)]\s*{?";

            Match match = Regex.Match(line, pattern);

            string signature = match.Groups[0].Value;

            string startCondition = match.Groups[1].Value;
            string endCondition = match.Groups[2].Value;
            string incrementor = match.Groups[3].Value;

            // Consume signature
            string remainder = line.Substring(signature.Length);
            return remainder;
        }

        protected void CompileLoopEnd(ref List<Instruction> allInstructions)
        {
            throw new NotImplementedException();
        }
    }
}