using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class ConcurrentPriorityQueue<T> : PriorityQueue<T>
    {
        protected readonly object sync = new object();

        public override bool Empty
        {
            get
            {
                lock (sync)
                {
                    return base.Empty;
                }
            }
        }

        public override bool Contains(PriorityNode<T> value)
        {
            lock (sync)
            {
                return base.Contains(value);
            }
        }

        public override void Add(PriorityNode<T> node)
        {
            lock (sync)
            {
                base.Add(node);
            }
        }

        public override KeyValuePair<PriorityNode<T>, PriorityNode<T>> Find(PriorityNode<T> nodeToFind)
        {
            lock (sync)
            {
                return base.Find(nodeToFind);
            }
        }

        public override void SetPriority(int updatedPriority, PriorityNode<T> nodeToFind)
        {
            lock (sync)
            {
                base.SetPriority(updatedPriority, nodeToFind);
            }
        }

        public override int GetPriority(PriorityNode<T> nodeToFind)
        {
            lock (sync)
            {
                return base.GetPriority(nodeToFind);
            }
        }

        public override PriorityNode<T> PopMin()
        {
            lock (sync)
            {
                return base.PopMin(); 
            }
        }
    }
}