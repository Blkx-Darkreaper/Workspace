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
            allOpcodes.Add("add", "add");
            allOpcodes.Add("sub", "sub");
            allOpcodes.Add("subtract", "sub");
            allOpcodes.Add("return", "ret");
        }
    }

    public class CompilerException : Exception
    {
        public CompilerException(string message) : base(message) { }
    }

    public class BinaryTreeNode<T>
    {
        public T Value { get; protected set; }
        public bool IsOperator { get; protected set; }
        public BinaryTreeNode<T> Parent { get; protected set; }
        public BinaryTreeNode<T> Left { get; protected set; }
        public BinaryTreeNode<T> Right { get; protected set; }

        public BinaryTreeNode() : this(null) { }

        public BinaryTreeNode(BinaryTreeNode<T> parent)
        {
            this.Parent = parent;
        }

        public BinaryTreeNode(BinaryTreeNode<T> parent, BinaryTreeNode<T> left, BinaryTreeNode<T> right) : this(parent)
        {
            this.Left = left;
            this.Right = right;
        }

        public BinaryTreeNode(T value, bool isOperator, BinaryTreeNode<T> parent) : this(value, isOperator, parent, null, null) { }

        public BinaryTreeNode(T value, bool isOperator, BinaryTreeNode<T> parent, BinaryTreeNode<T> left, BinaryTreeNode<T> right) : this(parent, left, right)
        {
            this.Value = value;
            this.IsOperator = isOperator;
        }

        public void SetValue(T value, bool isOperator)
        {
            this.Value = value;
            this.IsOperator = isOperator;
        }

        public void AddLeftBranch()
        {
            if (Left != null)
            {
                return;
            }

            this.Left = new BinaryTreeNode<T>(this);
        }

        public void AddLeftBranch(T value, bool isOperator)
        {
            if (Left != null)
            {
                return;
            }

            this.Left = new BinaryTreeNode<T>(value, isOperator, this);
        }

        public void AddRightBranch()
        {
            if (Right != null)
            {
                return;
            }

            this.Right = new BinaryTreeNode<T>(this);
        }

        public void AddRightBranch(T value, bool isOperator)
        {
            if (Right != null)
            {
                return;
            }

            this.Right = new BinaryTreeNode<T>(value, isOperator, this);
        }

        public void SplitLeft()
        {
            BinaryTreeNode<T> parent = this.Parent;
            BinaryTreeNode<T> nodeToInsert = new BinaryTreeNode<T>(parent, this, null);
            this.Parent = nodeToInsert;
            if (parent == null)
            {
                return;
            }

            parent.Right = nodeToInsert;
        }
    }

    public class BinaryTree<T>
    {
        public BinaryTreeNode<T> Root { get; protected set; }

        public BinaryTree()
        {
            this.Root = new BinaryTreeNode<T>();
        }

        public void Clear()
        {
            this.Root = null;
        }

        public BinaryTree(T value)
        {
            this.Root = new BinaryTreeNode<T>(value, true, null);
        }

        public void UpdateRoot()
        {
            while (Root.Parent != null)
            {
                this.Root = Root.Parent;
            }
        }
    }

    public class CodeBlock
    {
        public string signature { get; protected set; }
        public Dictionary<string, Variable> allLocalVariables { get; protected set; }
        public bool isGlobal { get; protected set; }
        protected int stackPointerAddress;
        protected int stackLimit;
        protected int stackBottom;

        public CodeBlock(int stackLimit = 0, int stackCapacity = 32)
        {
            this.signature = null;
            this.allLocalVariables = new Dictionary<string, Variable>();
            this.isGlobal = true;

            this.stackLimit = stackLimit;
            this.stackBottom = stackLimit + stackCapacity;
            this.stackPointerAddress = stackBottom;
        }

        public CodeBlock(string signature) : this(signature, 32) { }

        public CodeBlock(string signature, int stackPointerAddress, int stackCapacity = 32) : this(stackPointerAddress - stackCapacity)
        {
            this.signature = signature;
            this.isGlobal = false;
        }

        public int GetNextAddress(int typeSize)
        {
            int address = stackPointerAddress;
            stackPointerAddress -= typeSize;
            return address;
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
        public Dictionary<string, int> allOperators = new Dictionary<string, int>();

        public Compiler()
        {
            allBlocks.Add(new CodeBlock());

            // Expression
            allOperators.Add("[]", 1);
            allOperators.Add("()", 1);
            allOperators.Add(".", 1);
            allOperators.Add("->", 1);
            allOperators.Add(" ++", 1); // postfix
            allOperators.Add(" --", 1); // postfix
            // Unary: Right to Left
            allOperators.Add("++ ", 1); // prefix
            allOperators.Add("-- ", 1); // prefix
            allOperators.Add("~", 1);
            allOperators.Add("!", 1);
            // Unary typecasts: Right to left
            // Multiplicative
            allOperators.Add("*", 4);
            allOperators.Add("/", 4);
            allOperators.Add("%", 4);
            // Additive
            allOperators.Add("+", 5);
            allOperators.Add("-", 5);
            // Bitwise shift
            allOperators.Add("<<", 6);
            allOperators.Add(">>", 6);
            // Relational
            allOperators.Add(">", 7);
            allOperators.Add("<", 7);
            allOperators.Add(">=", 7);
            allOperators.Add("<=", 7);
            // Equality
            allOperators.Add("==", 8);
            allOperators.Add("!=", 8);
            // Bitwise-AND
            allOperators.Add("&", 9);
            // Bitwise-exclusive-OR
            allOperators.Add("^", 10);
            // Bitwise-inclusive-OR
            allOperators.Add("|", 11);
            // Logical-AND
            allOperators.Add("&&", 12);
            // Logical-OR
            allOperators.Add("||", 13);
            // Conditional-expression: Right to left
            // Simple and compound assignment: Right to left
            allOperators.Add("=", 15);
            allOperators.Add("*=", 15);
            allOperators.Add("/=", 15);
            allOperators.Add("%=", 15);
            allOperators.Add("+=", 15);
            allOperators.Add("-=", 15);
            allOperators.Add("<<=", 15);
            allOperators.Add(">>=", 15);
            allOperators.Add("&=", 15);
            allOperators.Add("^=", 15);
            allOperators.Add("|=", 15);
            // Sequential evaluation
            allOperators.Add(",", 16);
        }

        public int CompareOperators(string op, string otherOp)
        {
            int opValue = allOperators[op];
            int otherOpValue = allOperators[otherOp];

            return otherOpValue - opValue;
        }

        public int Get32BitTypeSize(string type)
        {
            return GetTypeSize(type, 32);
        }

        public bool IsValidType(string type, int datapathWidth = 32)
        {
            int size = GetTypeSize(type, datapathWidth);
            return size != 0;
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
                    break;

                default:
                    throw new CompilerException(string.Format("The type or namespace '{0}' could not be found", type));
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

        //public void AddBlockVariable(string name, string type, int address)
        //{
        //    if (allBlocks.Count < 2)
        //    {
        //        throw new InvalidOperationException("No open block");
        //    }

        //    var localVars = allBlocks[allBlocks.Count - 1].allLocalVariables;
        //    AddVariable(name, type, address, localVars);
        //}

        public void AddVariableToContext(string name, string type)
        {
            int typeSize = GetTypeSize(type);
            int nextAddress = allBlocks[allBlocks.Count - 1].GetNextAddress(typeSize);
            AddVariableToContext(name, type, nextAddress);
        }

        public void AddVariableToContext(string name, string type, int address)
        {
            var contextVars = allBlocks[allBlocks.Count - 1].allLocalVariables;
            AddVariable(name, type, address, contextVars);
        }

        protected void AddVariable(string name, string type, int address, Dictionary<string, Variable> globalVars)
        {
            bool nameConflict = globalVars.ContainsKey(name);
            if (nameConflict == true)
            {
                throw new CompilerException(String.Format("A local variable or parameter named '{0}' cannot be declared in this scope because that name is used in an enclosing local scope to define a local or parameter"));
            }

            globalVars.Add(name, new Variable(name, type, address));
        }

        public bool IsVariableInContext(string name)
        {
            for (int i = allBlocks.Count; i >= 0; i--)
            {
                CodeBlock block = allBlocks[i];
                bool hasVar = block.allLocalVariables.ContainsKey(name);
                if (hasVar == false)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public Variable GetVariable(string name)
        {
            for (int i = allBlocks.Count - 1; i >= 0; i--)
            {
                CodeBlock block = allBlocks[i];
                bool hasVar = block.allLocalVariables.ContainsKey(name);
                if (hasVar == false)
                {
                    continue;
                }

                return block.allLocalVariables[name];
            }

            throw new CompilerException(String.Format("The name '{0}' does not exist in the current context"));
        }

        public bool IsValidVariableName(string name)
        {
            // Variable names must start with _ or alphabetical character
            // Variable names cannot contain any non-alphanumeric characters, except for _'s

            char firstChar = name[0];
            if (char.IsLetter(firstChar) == false)
            {
                if (firstChar != '_')
                {
                    return false;
                }
            }

            string pattern = @"[^\d\w_]";
            Match match = Regex.Match(name, pattern);
            return !match.Success;
        }

        public List<Instruction> Compile(string code, out string errors)
        {
            List<Instruction> allInstructions = new List<Instruction>();
            allBlocks.Add(new CodeBlock());

            errors = string.Empty;

            StringReader reader = new StringReader(code);

            int lineNumber = 1;
            string nextLine = reader.ReadLine();
            while (nextLine != null)
            {
                try
                {
                    nextLine = CompileFunctionDef(ref allInstructions, nextLine);
                    nextLine = CompileForLoop(ref allInstructions, nextLine);
                    nextLine = CompileOpenBrace(nextLine);

                    nextLine = CompileMultiOperandOperation(ref allInstructions, nextLine);

                    nextLine = CompileEndBlock(ref allInstructions, nextLine);
                }
                catch (CompilerException ex)
                {
                    errors = DisplayError(errors, lineNumber, ex.Message);
                }

                if (nextLine.Length > 0)
                {
                    errors = DisplayError(errors, lineNumber, "; expected");
                    //throw new InvalidOperationException(String.Format("Line {0} was not entirely consumed", lineNumber));
                }

                nextLine = reader.ReadLine();
                lineNumber++;
            }

            return allInstructions;
        }

        protected string DisplayError(string errors, int lineNumber, string message)
        {
            errors += string.Format("{0}\t{1}", lineNumber, message);
            return errors;
        }

        protected string CompileEqualityOperation(ref List<Instruction> allInstructions, string line)
        {
            string opPattern = @"^(\w+)?\s*(\w+)\s*([!><=+\-*\/%]?=)\s*(\d|\w*)\s*;";
            Match opMatch = Regex.Match(line, opPattern);
            if (opMatch.Success == false)
            {
                return line;
            }

            string lineEnd = line.Substring(line.IndexOf(";"));
            string comment = GetComment(line);

            throw new NotImplementedException();

            string operation = opMatch.Groups[0].Value;
            string remainder = line.Substring(operation.Length + lineEnd.Length);
            return remainder;
        }

        protected string GetComment(string line)
        {
            string comment = string.Empty;
            int commentIndex = line.IndexOf("//");
            if (commentIndex < 0)
            {
                return comment;
            }

            comment = line.Substring(commentIndex);
            return comment;
        }

        protected string CompileUnaryOperation(ref List<Instruction> allInstructions, string line)
        {
            string opPattern = @"";
            Match opMatch = Regex.Match(line, opPattern);
            if (opMatch.Success == false)
            {
                return line;
            }

            string lineEnd = line.Substring(line.IndexOf(";"));
            string comment = GetComment(line);

            throw new NotImplementedException();

            string operation = opMatch.Groups[0].Value;
            string remainder = line.Substring(operation.Length + lineEnd.Length);
            return remainder;
        }

        protected string CompileMultiOperandOperation(ref List<Instruction> allInstructions, string line)
        {
            string opPattern = @"^([\w\s]+)([!><=+\-*\/%]?=)\s*([^;]+)\s*;";
            //string opPattern = @"^(\w+)?\s*(\w+)\s*([!><=+\-*\/%]?=)\s*(\d|\w*)\s*([+]|-|[*]|[\/]|%)\s*(\d|\w*)\s*;";
            Match opMatch = Regex.Match(line, opPattern);
            if (opMatch.Success == false)
            {
                return line;
            }

            //string commentPattern = @";\s*([\/]{2}.*)?$";
            //Match commentMatch = Regex.Match(line, commentPattern);
            string lineEnd = line.Substring(line.IndexOf(";"));
            string comment = GetComment(line);

            // int c = a + b;   // Add a and b and put the result in c

            // add [ebp - 4], [ebp + 8], [ebp + 12] ; Add a and b and put the result in c

            string solution, equalityOperator, equation;
            string result, resultType = string.Empty;

            solution = opMatch.Groups[1].Value.Trim();
            equalityOperator = opMatch.Groups[2].Value;
            equation = opMatch.Groups[3].Value;

            var resultArr = solution.Split(' ');
            result = resultArr[resultArr.Length - 1];

            if (resultArr.Length > 1)
            {
                resultType = resultArr[0];

                bool validType = IsValidType(resultType);
                if (validType == false)
                {
                    throw new CompilerException(string.Format("Keyword '{0}' cannot be used in this context"));
                }

                AddVariableToContext(result, resultType);
            }

            if (resultType.Equals(string.Empty) == true)
            {
                Variable resultVar = GetVariable(result);
                result = resultVar.memoryAddress.ToString();
            }

            BinaryTree<string> allOperations = OrderOperations(equation);
            PerformAllOperations(ref allInstructions, allOperations, result, comment);

            string operation = opMatch.Groups[0].Value;
            string remainder = line.Substring(operation.Length + lineEnd.Length);
            return remainder;
        }

        public BinaryTree<string> OrderOperations(string equation)
        {
            string firstOperand, arithOperator, secondOperand, leftParens, rightParens;
            BinaryTree<string> allOperations = new BinaryTree<string>();
            BinaryTreeNode<string> currentNode = allOperations.Root;

            // Strip out all white space
            equation = equation.Replace(" ", "");

            //string operandPattern = @"(-?\w*)\s*([+\-*\/%&|^])\s*(-?\w*)";    // Does not handle parentheses
            string operandPattern = @"([(]*)(-?\w*)([+\-*\/%&|^]|<<|>>)?([)]*)";    // Handles parentheses
            foreach (Match match in Regex.Matches(equation, operandPattern))
            {
                leftParens = match.Groups[1].Value;
                rightParens = match.Groups[match.Groups.Count - 1].Value;

                if (match.Groups[3].Value.Length > 0)
                {
                    firstOperand = match.Groups[2].Value;
                    arithOperator = match.Groups[3].Value;
                }
                else
                {
                    secondOperand = match.Groups[2].Value;
                }

                // Handle parentheses
                int leftBranches = leftParens.Length - rightParens.Length;
                if (leftBranches < 0)
                {
                    leftBranches = 0;
                }

                for (int i = 0; i < leftBranches; i++)
                {
                    currentNode.AddLeftBranch();
                    currentNode = currentNode.Left;
                }

                // Set left operand
                if (firstOperand.Length > 0)
                {
                    currentNode.AddLeftBranch(firstOperand, false);
                }

                // Split branch in accordance to order of operations
                if (currentNode.Value != null)
                {
                    string previousOperator = currentNode.Value;

                    int comparison = CompareOperators(arithOperator, previousOperator);
                    while (comparison < 0)
                    {
                        currentNode = currentNode.Parent;   // Retrace up the tree

                        previousOperator = currentNode.Value;
                        comparison = CompareOperators(arithOperator, previousOperator);
                    }

                    // New operator has higher precedence
                    if (comparison > 0)
                    {
                        currentNode = currentNode.Right;
                    }

                    currentNode.SplitLeft();
                    currentNode = currentNode.Parent;
                    allOperations.UpdateRoot();
                }

                // Set operator
                currentNode.SetValue(arithOperator, true);

                // Next match contains left parentheses
                if (secondOperand.Length == 0)
                {
                    currentNode.AddRightBranch();
                    currentNode = currentNode.Right;
                    continue;
                }

                // Set right operand
                currentNode.AddRightBranch(secondOperand, false);

                // Retrace up the tree
                for (int i = 0; i < rightParens.Length; i++)
                {
                    currentNode = currentNode.Parent;
                }
            }

            return allOperations;
        }

        public void PerformAllOperations(ref List<Instruction> allInstructions, string equation, string result, string comment)
        {
            BinaryTree<string> allOperations = OrderOperations(equation);
            PerformAllOperations(ref allInstructions, allOperations, result, comment);
        }

        protected void PerformAllOperations(ref List<Instruction> allInstructions, BinaryTree<string> allOperations, string result, string comment)
        {
            if (comment.Length > 0)
            {
                allInstructions.Add(new Instruction(string.Empty, comment: comment));
            }

            BinaryTreeNode<string> root = allOperations.Root;
            PerformOperation(ref allInstructions, root, result);
        }

        protected string PerformOperation(ref List<Instruction> allInstructions, BinaryTreeNode<string> operation, string result)
        {
            string firstOperand = GetOperand(ref allInstructions, operation.Left, result);
            string arithOperator = operation.Value;
            string secondOperand = GetOperand(ref allInstructions, operation.Right, result);

            //bool isVar = IsValidVariableName(firstOperand);
            //if (isVar == true)
            //{
            //    Variable firstVar = GetVariable(firstOperand);
            //    firstOperand = firstVar.memoryAddress.ToString();
            //}

            //isVar = IsValidVariableName(secondOperand);
            //if (isVar == true)
            //{
            //    Variable secondVar = GetVariable(secondOperand);
            //    secondOperand = secondVar.memoryAddress.ToString();
            //}

            switch (arithOperator)
            {
                case "+":
                    allInstructions.Add(new Instruction(opcodes["Add"], new string[] { result, firstOperand, secondOperand }));
                    break;

                case "-":
                    allInstructions.Add(new Instruction(opcodes["Subtract"], new string[] { result, firstOperand, secondOperand }));
                    break;

                case "*":
                    throw new NotImplementedException("This functionality is not yet supported");
                //break;

                case "/":
                    throw new NotImplementedException("This functionality is not yet supported");
                //break;

                case "%":
                    throw new NotImplementedException("This functionality is not yet supported");
                //break;

                case "&":
                    throw new NotImplementedException("This functionality is not yet supported");

                case "^":
                    throw new NotImplementedException("This functionality is not yet supported");

                case "|":
                    throw new NotImplementedException("This functionality is not yet supported");
            }

            return result;
        }

        protected string GetOperand(ref List<Instruction> allInstructions, BinaryTreeNode<string> node, string result)
        {
            if (node.IsOperator == true)
            {
                return PerformOperation(ref allInstructions, node, result);
            }

            return node.Value;
        }

        protected string CompileOpenBrace(string line)
        {
            int index = line.IndexOf("{");
            if (index < 0)
            {
                return line;
            }

            string lineEnd = line.Substring(index);
            string comment = GetComment(line);

            string remainder = line.Substring(1 + lineEnd.Length);
            return remainder;
        }

        protected string CompileEndBlock(ref List<Instruction> allInstructions, string line)
        {
            int index = line.IndexOf("}");
            if (index < 0)
            {
                return line;
            }

            CompileFunctionDefEnd(ref allInstructions);
            CompileLoopEnd(ref allInstructions);

            string lineEnd = line.Substring(index);
            string comment = GetComment(line);

            string remainder = line.Substring(1 + comment.Length);
            return remainder;
        }

        protected string CompileFunctionDef(ref List<Instruction> allInstructions, string line)
        {
            string functionPattern = @"\s*function\s+(\w+)\s+(\w+)[(]?([^)]*)[)]\s*{?";

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

            allInstructions.Add(new Instruction(opcodes["Push"], new string[] { "ebp" }, functionName, "save the value of base pointer"));
            allInstructions.Add(new Instruction(opcodes["Move"], new string[] { "ebp", "esp" }, comment: "base pointer now points to top of stack"));

            if (returnTypeSize > 0)
            {
                allInstructions.Add(new Instruction(opcodes["Sub"], new string[] { "esp", returnTypeSize.ToString() }, comment: "adjust stack pointer to allocate space for var c 4 * 1"));
            }

            // Consume function signature
            string signature = functionMatch.Groups[0].Value;
            OpenBlock(signature.Trim());

            string lineEnd = line.Substring(signature.Length);
            string comment = GetComment(line);

            string remainder = line.Substring(signature.Length + lineEnd.Length);
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
                allInstructions.Add(new Instruction(opcodes["Move"], new string[] { "eax", string.Format("[ebp - {0}]", returnTypeSize) }, comment: "save return value to EAX register"));
            }

            allInstructions.Add(new Instruction(opcodes["Move"], new string[] { "esp", "ebp" }, comment: "epilog: restore stack pointer"));
            allInstructions.Add(new Instruction(opcodes["Pop"], new string[] { "ebp" }, comment: "restore base pointer"));
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