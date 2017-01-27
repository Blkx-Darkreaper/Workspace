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
        public string Opcode { get; protected set; }
        public string Expression { get; protected set; }
        public string Comment { get; protected set; }
        public static int nextValue = 1;

        public Instruction(string opcode, string expression, string comment)
        {
            this.Value = nextValue;
            nextValue++;
            this.Opcode = opcode;
            this.Expression = expression;
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