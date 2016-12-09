using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class PriorityNode<T> : IComparable<PriorityNode<T>>, IEquatable<PriorityNode<T>>
    {
        public static int nextSeqNo = 1;
        protected int seqNo { get; set; }
        public int Priority { get; set; }
        public PriorityNode<T> Previous { get; set; }
        public T Value { get; protected set; }

        public PriorityNode(T value) : this(-1, value) { }

        public PriorityNode(int priority, T value)
        {
            this.seqNo = nextSeqNo;
            nextSeqNo++;

            this.Priority = priority;
            this.Value = value;
            this.Previous = null;
        }

        public int CompareTo(PriorityNode<T> other)
        {
            int difference = Priority - other.Priority;

            if (difference == 0)
            {
                difference = seqNo - other.seqNo;
            }

            return difference;
        }

        public bool Equals(PriorityNode<T> other)
        {
            bool match = Value.Equals(other.Value);
            return match;
        }
    }
}
