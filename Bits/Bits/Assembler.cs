using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Bits
{
    public static class Assembler
    {
        public static SortedDictionary<string, Instruction> InstructionSet { get; private set; }

        // X, Y, Z, D, and M can be either values or registers

        // this set assumes port mapped I/O
        private static string fiveBitMappedIOSet = string.Join("\n", new string[] {
            "mov X, Y // Move data from X to Y\n"
            , "lod D, M, X // Load address D offset by M and put in X"
            , "sto D, M, X // Store value X in address D offset by M"
            , "add X, Y, Z  // Adds X and Y and puts the result in Z"
            , "sub X, Y, Z // Subtracts X and Y and puts the result in Z"
            , "br L, M // Branch to label L offset by M"
            , "cmp X, Y // Compare X and Y and set flags"
            , "bc // Branch if carry flag on\n"
            , "bo // Branch if overflow flag on\n"
            , "bz // Branch if zero flag on\n"
            , "bnc // Branch if carry flag off\n"
            , "bno // Branch if overflow flag off\n"
            , "bnz // Branch if zero flag off\n"
            , "be // Branch if equal flag on"
            , "bge // Branch if greater than or equal flags on"
            , "ble // Branch if less than or equal flags on"
            , "bg // Branch if greater than flag on"
            , "bl // Branch if less than flag on"
            , "and X, Y, Z // Ands X and Y and puts the result in Z"
            , "or X, Y, Z // Ors X and Y and puts the result in Z"
            , "xor X, Y, Z // Exclusive ors X and Y and puts the result in Z"
            , "psh X // Pushes X onto the stack"
            , "pop X // Pops the stack and puts the value into X"
            , "cll L // Call procedure at label L"
            , "ret // Return from current procedure"
            , "shl X, Y, Z // Shift left by adding Y zeroes to X and put the result in Z"
            , "shr X, Y, Z // Shift right by removing Y bits from X and put the result in Z"
            , "rol X, Y, Z // Rotate left by moving Y bits of X to right end and put the result in Z"
            , "ror X, Y, Z // Rotate right by moving Y bits of X to left end and put the result in Z"
            , "in D, M, X // Get input from device D offset by M and put result in X"
            , "out D, M, X // Send X to device D offset by M"
            , "end // Ends the program" });

        private static string sixBitSet = string.Join("\n", new string[] {
            "nop // No operation"
            , "mov X, Y // Move data from Y to X"
            , "mov X, M, Y // Move data from Y to X offset by M"
            , "lod X, D // Load address D and put in X"
            , "lod X, D, M // Load address D offset by M and put in X"
            , "sto D, X // Store value X in address D"
            , "sto D, M, X // Store value X in address D offset by M"
            , "inc X // Increment value X and put the result in X"
            , "inc Y, X // Increment value X and put the result in Y"
            , "dec X // Decrement value X and put the result in X"
            , "dec Y, X // Decrement value X and put the result in X"
            , "add X, Y // Adds X and Y and puts the result in X; Sets the carry, overflow, parity, sign and zero flags"
            , "add Z, X, Y // Adds X and Y and puts the result in Z; Sets the carry, overflow, parity, sign and zero flags"
            , "sub X, Y // Subtracts X and Y and puts the result in X; Sets the carry, overflow, parity, sign and zero flags"
            , "sub Z, X, Y // Subtracts X and Y and puts the result in Z; Sets the carry, overflow, parity, sign and zero flags"
            , "br L // Branch to label L"
            , "br L, M // Branch to label L offset by M"
            , "cmp X, Y // Compare X and Y and set flags"
            , "bc // Branch if carry flag on"
            , "bo // Branch if overflow flag on"
            , "bp // Branch if parity flag on (even)"
            , "bs // Branch if sign flag on (negative)"
            , "bz // Branch if zero flag on"
            , "bnc // Branch if carry flag off"
            , "bno // Branch if overflow flag off"
            , "bnp // Branch if parity flag off(odd)"
            , "bns // Branch if sign flag off(positive)"
            , "bnz // Branch if zero flag off"
            , "be // Branch if equal flag on"
            , "bne // Branch if equal flag is off"
            , "bge // Branch if greater than or equal flags on"
            , "ble // Branch if less than or equal flags on"
            , "bg // Branch if greater than flag on"
            , "bl // Branch if less than flag on"
            , "and X, Y // Ands X and Y and puts the result in X; Sets the carry, overflow, parity, sign and zero flags"
            , "and Z, X, Y // Ands X and Y and puts the result in Z; Sets the carry, overflow, parity, sign and zero flags"
            , "or X, Y // Ors X and Y and puts the result in X; Sets the carry, overflow, parity, sign and zero flags"
            , "or Z, X, Y // Ors X and Y and puts the result in Z; Sets the carry, overflow, parity, sign and zero flags"
            , "xor X, Y // Exclusive ors X and Y and puts the result in X; Sets the carry, overflow, parity, sign and zero flags"
            , "xor Z, X, Y // Exclusive ors X and Y and puts the result in Z; Sets the carry, overflow, parity, sign and zero flags"
            , "not X // Inverts the value of X; Sets the carry, overflow, parity, sign and zero flags"
            , "not Y, X // Inverts the value of X and puts the result in Y; Sets the carry, overflow, parity, sign and zero flags"
            , "tst X, Y // Ands X and Y but does not change either value; Sets the carry, overflow, parity, sign and zero flags"
            , "psh X // Pushes X onto the stack"
            , "pop X // Pops the stack and puts the value into X"
            , "cll L // Call procedure at label L"
            , "ret // Return from current procedure"
            , "shl X, Y // Shift left by adding Y zeroes to X and put the result in X"
            , "shl Z, X, Y // Shift left by adding Y zeroes to X and put the result in Z"
            , "shr X, Y // Shift right by removing Y bits from X and put the result in X"
            , "shr Z, X, Y // Shift right by removing Y bits from X and put the result in Z"
            , "rol X, Y // Rotate left by moving Y bits of X to right end and put the result in X"
            , "rol Z, X, Y // Rotate left by moving Y bits of X to right end and put the result in Z"
            , "ror X, Y // Rotate right by moving Y bits of X to left end and put the result in X"
            , "ror Z, X, Y // Rotate right by moving Y bits of X to left end and put the result in Z"
            , "end // Ends the program" });

        private static string x86 = string.Join("\n", new string[] {
            "nop // No operation"
            , "mov X, Y // Move data from Y to X"
            , "mov X, M, Y // Move data from Y to X offset by M"
            , "lod X, D // Load address D and put in X"
            , "lod X, D, M // Load address D offset by M and put in X"
            , "sto D, X // Store value X in address D"
            , "sto D, M, X // Store value X in address D offset by M"
            , "inc X // Increment value X and put the result in X"
            , "inc Y, X // Increment value X and put the result in Y"
            , "dec X // Decrement value X and put the result in X"
            , "dec Y, X // Decrement value X and put the result in X"
            , "add X, Y // Adds X and Y and puts the result in X; Sets the carry, overflow, parity, sign and zero flags"
            , "add Z, X, Y // Adds X and Y and puts the result in Z; Sets the carry, overflow, parity, sign and zero flags"
            , "sub X, Y // Subtracts X and Y and puts the result in X; Sets the carry, overflow, parity, sign and zero flags"
            , "sub Z, X, Y // Subtracts X and Y and puts the result in Z; Sets the carry, overflow, parity, sign and zero flags"
            , "br L // Branch to label L"
            , "br L, M // Branch to label L offset by M"
            , "cmp X, Y // Compare X and Y and set flags"
            , "bc // Branch if carry flag on"
            , "bo // Branch if overflow flag on"
            , "bp // Branch if parity flag on (even)"
            , "bs // Branch if sign flag on (negative)"
            , "bz // Branch if zero flag on"
            , "bnc // Branch if carry flag off"
            , "bno // Branch if overflow flag off"
            , "bnp // Branch if parity flag off(odd)"
            , "bns // Branch if sign flag off(positive)"
            , "bnz // Branch if zero flag off"
            , "be // Branch if equal flag on"
            , "bne // Branch if equal flag is off"
            , "bge // Branch if greater than or equal flags on"
            , "ble // Branch if less than or equal flags on"
            , "bg // Branch if greater than flag on"
            , "bl // Branch if less than flag on"
            , "and X, Y // Ands X and Y and puts the result in X; Sets the carry, overflow, parity, sign and zero flags"
            , "and Z, X, Y // Ands X and Y and puts the result in Z; Sets the carry, overflow, parity, sign and zero flags"
            , "or X, Y // Ors X and Y and puts the result in X; Sets the carry, overflow, parity, sign and zero flags"
            , "or Z, X, Y // Ors X and Y and puts the result in Z; Sets the carry, overflow, parity, sign and zero flags"
            , "xor X, Y // Exclusive ors X and Y and puts the result in X; Sets the carry, overflow, parity, sign and zero flags"
            , "xor Z, X, Y // Exclusive ors X and Y and puts the result in Z; Sets the carry, overflow, parity, sign and zero flags"
            , "not X // Inverts the value of X; Sets the carry, overflow, parity, sign and zero flags"
            , "not Y, X // Inverts the value of X and puts the result in Y; Sets the carry, overflow, parity, sign and zero flags"
            , "tst X, Y // Ands X and Y but does not change either value; Sets the carry, overflow, parity, sign and zero flags"
            , "psh X // Pushes X onto the stack"
            , "pop X // Pops the stack and puts the value into X"
            , "cll L // Call procedure at label L"
            , "ret // Return from current procedure"
            , "shl X, Y // Shift left by adding Y zeroes to X and put the result in X"
            , "shl Z, X, Y // Shift left by adding Y zeroes to X and put the result in Z"
            , "shr X, Y // Shift right by removing Y bits from X and put the result in X"
            , "shr Z, X, Y // Shift right by removing Y bits from X and put the result in Z"
            , "rol X, Y // Rotate left by moving Y bits of X to right end and put the result in X"
            , "rol Z, X, Y // Rotate left by moving Y bits of X to right end and put the result in Z"
            , "ror X, Y // Rotate right by moving Y bits of X to left end and put the result in X"
            , "ror Z, X, Y // Rotate right by moving Y bits of X to left end and put the result in Z"
            , "end // Ends the program" });

        public static void BuildInstructionSet5Bit()
        {
            BuildInstructionSet5Bit(false);
        }

        public static void BuildInstructionSet5Bit(bool memoryMappedIO)
        {
            if (memoryMappedIO == true)
            {
                BuildInstructionSet(fiveBitMappedIOSet);
            }
            else
            {
                
            }
        }

        public static void BuildInstructionSet6Bit()
        {
            BuildInstructionSet6Bit(false);
        }

        public static void BuildInstructionSet6Bit(bool memoryMappedIO)
        {
            if (memoryMappedIO == true)
            {

            }
            else
            {
                BuildInstructionSet(sixBitSet);
            }
        }

        private static void BuildInstructionSet(string instructions)
        {
            InstructionSet = new SortedDictionary<string, Instruction>();

            StringReader reader = new StringReader(instructions);

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

                string[] operands = GetOperands(expression);
                int totalParams = operands.Length;

                string instructionTemplate = GetInstructionTemplate(opcode, totalParams);

                Instruction nextInstruction = new Instruction(opcode, operands, comment: comment);
                InstructionSet.Add(instructionTemplate, nextInstruction);

                nextLine = reader.ReadLine();
            }
        }

        public static string Parse(string input)
        {
            string message = string.Empty;

            if (InstructionSet.Count == 0)
            {
                message += "Error! No instruction set loaded";
                return message;
            }

            StringReader reader = new StringReader(input);

            int lineNumber = 1;
            string nextLine = reader.ReadLine();
            while (nextLine != null)
            {
                string opcode, label, comment;
                string[] operands;

                GetInstructionDetails(nextLine, out label, out opcode, out operands, out comment);

                //string label = GetLabel(nextLine);
                //string opcode = GetOpcode(nextLine);
                //string[] operands = GetOperands(nextLine);
                //string comment = GetComment(nextLine);

                string instructionTemplate = GetInstructionTemplate(opcode, operands.Length);

                message += lineNumber + " " + nextLine;

                bool validInstruction = InstructionSet.ContainsKey(instructionTemplate);
                if (validInstruction == false)
                {
                    message += "\tError: Instruction not recognized";
                    nextLine = reader.ReadLine();
                    continue;
                }

                Instruction instruction = new Instruction(opcode, operands, label, comment);
                // TODO

                nextLine = reader.ReadLine();
                lineNumber++;
            }

            return message;
        }

        private static string GetInstructionTemplate(string opcode, int totalParams)
        {
            string instructionTemplate = opcode + " ";
            char nextOperand = 'A';
            for (int i = 0; i < totalParams; i++)
            {
                if (i > 0)
                {
                    instructionTemplate += ", ";
                }

                instructionTemplate += nextOperand.ToString();
                nextOperand = (Char)(Convert.ToUInt16(nextOperand) + 1);    // Increment Operand
            }

            return instructionTemplate;
        }

        private static void GetInstructionDetails(string expression, out string label, out string opcode, out string[] operands, out string comment)
        {
            label = string.Empty;
            int labelIndex = expression.IndexOf(':');
            if(labelIndex > 0)
            {

                label = expression.Substring(0, labelIndex).Trim();
                expression = expression.Substring(labelIndex + 1).Trim();
            }

            comment = string.Empty;
            int commentIndex = expression.IndexOf(';');
            if(commentIndex > 0)
            {
                comment = expression.Substring(commentIndex + 1).Trim();
                expression = expression.Substring(0, commentIndex).Trim();
            }

            int operandIndex = expression.IndexOf(' ');
            opcode = expression.Substring(0, operandIndex).Trim();
            expression = expression.Substring(operandIndex + 1);

            expression.Replace(" ", "");
            operands = expression.Split(',');
        }

        private static string GetLabel(string expression)
        {
            int index = expression.IndexOf(':');
            if(index < 0)
            {
                return string.Empty;
            }

            string label = expression.Substring(0, index - 1);
            return label;
        }

        private static string GetOpcode(string expression)
        {
            // Remove label if present
            int index = expression.IndexOf(":");
            if(index > 0)
            {
                expression = expression.Substring(index + 1);
            }

            string[] operands = expression.Split(' ');
            string opcode = operands[0].Trim().ToLower();
            return opcode;
        }

        private static string[] GetOperands(string expression)
        {
            // Remove label if present
            int index = expression.IndexOf(":");
            if (index > 0)
            {
                expression = expression.Substring(index + 1);
            }

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

        private static string GetComment(string expression)
        {
            int index = expression.IndexOf(';');
            if (index < 0)
            {
                return string.Empty;
            }

            string comment = expression.Substring(index + 1);
            return comment;
        }
    }
}
