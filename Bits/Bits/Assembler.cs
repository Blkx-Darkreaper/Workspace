using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Bits
{
    public static class Assembler
    {
        public static SortedDictionary<string, Instruction> InstructionSet { get; private set; }

        // X, Y, Z, D, and M can be either values or registers

        // this set assumes port mapped I/O
        private static string fiveBitSet =
            "mov X, Y // Move data from X to Y"
            + "lod D, M, X // Load address D offset by M and put in X"
            + "sto D, M, X // Store value X in address D offset by M"
            + "add X, Y, Z  // Adds X and Y and puts the result in Z"
            + "sub X, Y, Z // Subtracts X and Y and puts the result in Z"
            + "br L, M // Branch to label L offset by M"
            + "cmp X, Y // Compare X and Y and set flags"
            + "bc // Branch if carry flag on"
            + "bo // Branch if overflow flag on"
            + "bz // Branch if zero flag on"
            + "bnc // Branch if carry flag off"
            + "bno // Branch if overflow flag off"
            + "bnz // Branch if zero flag off"
            + "be // Branch if equal flag on"
            + "bge // Brnach if greater than or equal flags on"
            + "ble // Branch if less than or equal flags on"
            + "bg // Branch if greater than flag on"
            + "bl // Branch if less than flag on"
            + "and X, Y, Z // Ands X and Y and puts the result in Z"
            + "or X, Y, Z // Ors X and Y and puts the result in Z"
            + "xor X, Y, Z // Exclusive ors X and Y and puts the result in Z"
            + "psh X // Pushes X onto the stack"
            + "pop X // Pops the stack and puts the value into X"
            + "cll L // Call procedure at label L"
            + "ret // Return from current procedure"
            + "shl X, Y, Z // Shift left by adding Y zeroes to X and put the result in Z"
            + "shr X, Y, Z // Shift right by removing Y bits from X and put the result in Z"
            + "rol X, Y, Z // Rotate left by moving Y bits of X to right end and put the result in Z"
            + "ror X, Y, Z // Rotate right by moving Y bits of X to left end and put the result in Z"
            + "in D, M, X // Get input from device D offset by M and put result in X"
            + "out D, M, X // Send X to device D offset by M"
            + "end // Ends the program";

        public static string Parse(string input)
        {
            string message = string.Empty;

            if (InstructionSet.Count == 0)
            {
                message += "Error! No instruction set loaded";
                return message;
            }

            StringReader reader = new StringReader(input);

            string nextLine = reader.ReadLine();
            while (nextLine != null)
            {
                message += nextLine;

                string opcode = GetOpcode(nextLine);
                string[] parameters = GetParameters(nextLine);

                bool validInstruction = InstructionSet.ContainsKey(opcode);
                if (validInstruction == false)
                {
                    message += "\nInstruction not recognized";
                    nextLine = reader.ReadLine();
                    continue;
                }

                Instruction instruction = InstructionSet[opcode];

                nextLine = reader.ReadLine();
            }

            return message;
        }

        public static void BuildInstructionSet()
        {
            InstructionSet = new SortedDictionary<string, Instruction>();

            StringReader reader = new StringReader(fiveBitSet);

            string nextLine = reader.ReadLine();
            while (nextLine != null)
            {
                string[] lineFragments = Regex.Split(nextLine, "//");

                string expression = lineFragments[0].Trim();

                string comment = string.Empty;
                if (lineFragments.Length > 1)
                {
                    comment = lineFragments[1].Trim();
                }

                string opcode = GetOpcode(expression);
                if (opcode.Equals(string.Empty))
                {
                    nextLine = reader.ReadLine();
                    continue;
                }

                int totalParams = expression.Split(',').Length;

                Instruction nextInstruction = new Instruction(opcode, expression, comment);
                InstructionSet.Add(opcode, nextInstruction);

                nextLine = reader.ReadLine();
            }
        }

        private static string GetOpcode(string expression)
        {
            string[] operands = expression.Split(' ');
            string opcode = operands[0].Trim().ToLower();
            return opcode;
        }

        private static string[] GetParameters(string expression)
        {
            string[] operands = expression.Split(' ');
            int totalOperands = operands.Length;
            int totalParams = totalOperands - 1;
            string[] parameters = new string[totalParams];
            for (int i = 1; i < totalOperands; i++)
            {
                string parameter = operands[i].Replace(",", "").Trim().ToLower();
                //if (parameter.Equals(string.Empty))
                //{
                //    continue;
                //}

                parameters[i - 1] = parameter;
            }

            return parameters;
        }
    }
}
