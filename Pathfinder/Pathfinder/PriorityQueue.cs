using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class PriorityQueue<T> : SortedDictionary<PriorityNode<T>, PriorityNode<T>>
    {
        public virtual bool Empty
        {
            get
            {
                bool isEmpty = this.Count == 0;
                return isEmpty;
            }
        }

        public virtual bool Contains(PriorityNode<T> value)
        {
            bool containsValue = this.ContainsValue(value);
            return containsValue;
        }

        public virtual void Add(PriorityNode<T> node)
        {
            this.Add(node, node);
        }

        public virtual KeyValuePair<PriorityNode<T>, PriorityNode<T>> Find(PriorityNode<T> nodeToFind)
        {
            KeyValuePair<PriorityNode<T>, PriorityNode<T>> item;
            
            try
            {
                item = this.Select(i => i).First(i => i.Value.Equals(nodeToFind));
            }
            catch (Exception)
            {
                item = new KeyValuePair<PriorityNode<T>, PriorityNode<T>>(null, null);
            }

            return item;
        }

        public virtual void SetPriority(int updatedPriority, PriorityNode<T> nodeToFind)
        {
            KeyValuePair<PriorityNode<T>, PriorityNode<T>> item = Find(nodeToFind);

            this.Remove(item.Key);

            PriorityNode<T> node = item.Value;
            node.Priority = updatedPriority;

            this.Add(node, node);
        }

        public virtual int GetPriority(PriorityNode<T> nodeToFind)
        {
            KeyValuePair<PriorityNode<T>, PriorityNode<T>> item = Find(nodeToFind);

            int priority = item.Key.Priority;
            return priority;
        }

        public virtual PriorityNode<T> PopMin()
        {
            KeyValuePair<PriorityNode<T>, PriorityNode<T>> item = this.ElementAt(0);

            this.Remove(item.Key);

            PriorityNode<T> min = item.Value;
            return min;
        }

        //public void SetPrevious(PriorityNode<T> current, PriorityNode<T> previous)
        //{
        //    KeyValuePair<int, PriorityNode<T>> currentItem = Find(current);
        //    PriorityNode<T> currentNode = currentItem.Value;

        //    KeyValuePair<int, PriorityNode<T>> previousItem = Find(previous);
        //    PriorityNode<T> previousNode = previousItem.Value;

        //    currentNode.Previous = previousNode;
        //}

        //public T GetPrevious(T current)
        //{
        //    KeyValuePair<int, PriorityNode<T>> currentItem = Find(current);
        //    PriorityNode<T> currentNode = currentItem.Value;

        //    T previous = currentNode.Previous.Value;
        //    return previous;
        //}

        //public List<T> GetAllPrevious(T current)
        //{
        //    List<T> allPrevious = new List<T>();

        //    KeyValuePair<int, PriorityNode<T>> currentItem = Find(current);
        //    PriorityNode<T> currentNode = currentItem.Value;

        //    PriorityNode<T> previousNode;
        //    do
        //    {
        //        previousNode = currentNode.Previous;
        //        T previous = previousNode.Value;
        //        allPrevious.Add(previous);

        //        currentNode = previousNode;
        //    }
        //    while (previousNode != null);

        //    return allPrevious;
        //}
    }
}
