using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bits;

namespace Test
{
    [TestClass]
    public class CompilerTest
    {
        protected Compiler compiler;
        protected string errors;
        protected List<Instruction> allInstructions;

        [TestInitialize]
        public void TestInitialize()
        {
            Assembler.BuildInstructionSet6Bit();

            this.compiler = new Compiler();
            this.errors = string.Empty;
            this.allInstructions = new List<Instruction>();
        }

        [TestMethod]
        public void HandleBrackets()
        {
            string equation = "(2+(4-1))-3";

            compiler.AddGlobalVariable("Result", "int", 0);
            compiler.PerformAllOperations(ref allInstructions, equation, "Result", string.Empty);

            Assert.IsTrue(allInstructions.Count == 3);

            Instruction first = allInstructions[0];
            Instruction testFirst = Assembler.InstructionSet["sub A, B, C"];
            Assert.IsTrue(first.Opcode.Equals(testFirst.Opcode));
            Assert.IsTrue(first.Operands.Length == 3);
            Assert.IsTrue(first.Operands[0].Equals("Result"));
            Assert.IsTrue(first.Operands[1].Equals("4"));
            Assert.IsTrue(first.Operands[2].Equals("1"));

            Instruction second = allInstructions[1];
            Instruction testSecond = Assembler.InstructionSet["add A, B, C"];
            Assert.IsTrue(second.Opcode.Equals(testSecond.Opcode));
            Assert.IsTrue(second.Operands.Length == 3);
            Assert.IsTrue(second.Operands[0].Equals("Result"));
            Assert.IsTrue(second.Operands[1].Equals("2"));
            Assert.IsTrue(second.Operands[2].Equals("Result"));

            Instruction third = allInstructions[2];
            Instruction testThird = Assembler.InstructionSet["sub A, B, C"];
            Assert.IsTrue(third.Opcode.Equals(testThird.Opcode));
            Assert.IsTrue(third.Operands.Length == 3);
            Assert.IsTrue(third.Operands[0].Equals("Result"));
            Assert.IsTrue(third.Operands[1].Equals("Result"));
            Assert.IsTrue(third.Operands[2].Equals("3"));
        }

        [TestMethod]
        public void HandleArithmeticOrder()
        {
            string equation = "3+1*5-4/2";

            compiler.AddGlobalVariable("Result", "int", 0);

            BinaryTree<string> allOperations = compiler.OrderOperations(equation);
        }

        /* Registers:
         * |-   16bits  -|- 8 -|- 8 -|
         * eax          ax  ah    al
         * ebx          bx  bh    bl
         * ecx          cx  ch    cl
         * edx          dx  dh    dl
         * esi
         * edi
         * ebp - Base pointer
         * esp - Stack pointer
         * eip - Instruction pointer
         * |-       32 bits         -|
         */

        [TestMethod]
        public void CompileVoidFunctionDef()
        {
            string code = @"function void Nothing(int a, int b) {"
                + "\tint c = a + b;"
                + "\t return;"
                + "}";

            List<Instruction> allInstructions = compiler.Compile(code, out errors);
        }

        [TestMethod]
        public void CompileFunctionDef()
        {
            string code = @"function int Add(int a, int b) {"
                + "\tint c = a + b;"
                + "\treturn c;"
                + "}";

            List<Instruction> allInstructions = compiler.Compile(code, out errors);

            /* Add: psh ebp ; save the value of base pointer
             * mov ebp, esp ; base pointer now points to top of stack
             * sub esp, 4 ; adjust stack pointer to allocate space for var c 4 * 1
             * add [ebp - 4], [ebp + 8], [ebp + 12] ; c = a + b
             * mov eax, [ebp - 4] ; save return value to EAX register
             * mov esp, ebp ; epilog: restore stack pointer
             * pop ebp ; restore base pointer
             * ret
             */

            Assert.IsTrue(allInstructions.Count == 8);

            Instruction first = allInstructions[0];
            Instruction testFirst = Assembler.InstructionSet["psh A"];
            Assert.IsTrue(first.Opcode.Equals(testFirst.Opcode));
            Assert.IsTrue(first.Label.Equals("Add"));
            Assert.IsTrue(first.Operands.Length == 1);
            Assert.IsTrue(first.Operands[0].Equals("ebp"));

            Instruction second = allInstructions[1];
            Instruction testSecond = Assembler.InstructionSet["mov A, B"];
            Assert.IsTrue(second.Opcode.Equals(testSecond.Opcode));
            Assert.IsTrue(second.Operands.Length == 2);
            Assert.IsTrue(second.Operands[0].Equals("ebp"));
            Assert.IsTrue(second.Operands[1].Equals("esp"));

            Instruction third = allInstructions[2];
            Instruction testThird = Assembler.InstructionSet["sub A, B"];
            Assert.IsTrue(third.Opcode.Equals(testThird.Opcode));
            Assert.IsTrue(third.Operands.Length == 2);
            Assert.IsTrue(second.Operands[0].Equals("esp"));
            Assert.IsTrue(second.Operands[1].Equals("4"));

            Instruction fourth = allInstructions[3];
            Instruction testFourth = Assembler.InstructionSet["add A, B, C"];
            Assert.IsTrue(fourth.Opcode.Equals(testFourth.Opcode));
            Assert.IsTrue(fourth.Operands.Length == 3);
            //Assert.IsTrue(fourth.Operands[0].Equals("R1"));
            //Assert.IsTrue(fourth.Operands[1].Equals("R1"));
            //Assert.IsTrue(fourth.Operands[2].Equals("R1"));

            Instruction fifth = allInstructions[4];
            Instruction testFifth = Assembler.InstructionSet["mov A, B"];
            Assert.IsTrue(fifth.Opcode.Equals(testFifth.Opcode));
            Assert.IsTrue(fifth.Operands.Length == 2);
            Assert.IsTrue(fifth.Operands[0].Equals("eax"));
            //Assert.IsTrue(fifth.Operands[1].Equals(""));

            Instruction sixth = allInstructions[5];
            Instruction testSixth = Assembler.InstructionSet["mov A, B"];
            Assert.IsTrue(sixth.Opcode.Equals(testSixth.Opcode));
            Assert.IsTrue(sixth.Operands.Length == 2);
            Assert.IsTrue(sixth.Operands[0].Equals("esp"));
            Assert.IsTrue(sixth.Operands[1].Equals("ebp"));

            Instruction seventh = allInstructions[6];
            Instruction testSeventh = Assembler.InstructionSet["pop A"];
            Assert.IsTrue(seventh.Opcode.Equals(testSeventh.Opcode));
            Assert.IsTrue(seventh.Operands.Length == 1);
            Assert.IsTrue(seventh.Operands[0].Equals("ebp"));

            Instruction eighth = allInstructions[7];
            Instruction testEighth = Assembler.InstructionSet["ret"];
            Assert.IsTrue(eighth.Opcode.Equals(testEighth.Opcode));
            Assert.IsTrue(eighth.Operands.Length == 0);
        }

        [TestMethod]
        public void CompileFunctionCall()
        {
            string code = @"int a = 2;"
                + "int b = 3;"
                + "int c = Add(a + b);";

            List<Instruction> allInstructions = compiler.Compile(code, out errors);

            /* sub esp, 12 ; adjust stack pointer to allocate space for var a, b, c 4 * 3
             * mov [ebp - 4], 2 ; set var a = 2
             * mov [ebp - 8], 3 ; set var b = 3
             * psh eax ; save EAX register value
             * psh [ebp - 8] ; prolog: push parameter b
             * psh [ebp - 4] ; push parameter a
             * cll Add ; function call
             * add esp, 8 ; pop parameters 4 * 2
             * mov [ebp - 12], eax ; set var c to return value
             * pop eax ; restore EAX register value
             */
        }

        [TestMethod]
        public void ComplileForLoop()
        {
            string code = @"for(int i = 0; i < 5; i++) {"
                + "// Loop body"
                + "}";

            List<Instruction> allInstructions = compiler.Compile(code, out errors);

            /* mov 0, R1
             * L1:
             * ; Loop body
             * inc R1
             * cmp R1, 5
             * bl L1
             */

            Instruction first = allInstructions[0];
            Instruction testFirst = Assembler.InstructionSet["mov A, B"];
            Assert.IsTrue(first.Opcode.Equals(testFirst.Opcode));
            Assert.IsTrue(first.Operands.Length == 2);
            Assert.IsTrue(first.Operands[0].Equals("10"));
            Assert.IsTrue(first.Operands[1].Equals("R1"));

            Instruction second = allInstructions[1];
            Assert.IsTrue(second.Opcode.Equals(string.Empty));
            Assert.IsTrue(second.Label.Equals("L1"));
            Assert.IsTrue(second.Operands.Length == 0);

            Instruction third = allInstructions[2];
            Assert.IsTrue(third.Opcode.Equals(string.Empty));
            Assert.IsTrue(third.Comment.Equals(" Loop body"));
            Assert.IsTrue(third.Operands.Length == 0);

            Instruction fourth = allInstructions[3];
            Instruction testFourth = Assembler.InstructionSet["dec A"];
            Assert.IsTrue(fourth.Opcode.Equals(testFourth.Opcode));
            Assert.IsTrue(fourth.Operands.Length == 1);
            Assert.IsTrue(fourth.Operands[0].Equals("R1"));

            Instruction fifth = allInstructions[4];
            Instruction testFifth = Assembler.InstructionSet["bnz L1"];
            Assert.IsTrue(fifth.Opcode.Equals(testFifth.Opcode));
            Assert.IsTrue(fifth.Operands.Length == 1);
            Assert.IsTrue(fifth.Operands[0].Equals("L1"));
        }
    }
}
