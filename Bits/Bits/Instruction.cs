using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bits
{
    public class Instruction : IComparable<Instruction>
    {
        public int Value { get; protected set; }
        public string HexValue { get { return Program.ConvertDecimalToHexadecimal(Value); } }
        public string Label { get; protected set; }
        public string Opcode { get; protected set; }
        public string[] Operands { get; protected set; }
        public string Comment { get; protected set; }
        public static int nextValue = 1;

        public Instruction() : this(string.Empty) { }

        public Instruction(string opcode) : this(opcode, new string[] { }, string.Empty, string.Empty) { }

        public Instruction(string opcode, string[] operands) : this(opcode, operands, string.Empty, string.Empty) { }

        public Instruction(string opcode, string label = "", string comment = "") :this(opcode, new string[] { }, label, comment) { }

        public Instruction(string opcode, string[] operands, string label = "", string comment = "")
        {
            this.Value = nextValue;
            nextValue++;
            this.Label = label;
            this.Opcode = opcode;
            this.Operands = operands;
            this.Comment = comment;
        }

        public int CompareTo(Instruction other)
        {
            int otherValue = other.Value;

            int comparison = Value.CompareTo(otherValue);
            return comparison;
        }
    }
}