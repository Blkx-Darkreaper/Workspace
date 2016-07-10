using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Advanced
{
    public class Node<T>
    {
        protected NodeList<T> Neighbours { get; set; }
        public T Value { get; set; }

        public Node()
        {
            Neighbours = null;
        }

        public Node(T value) : this(value, null) { }

        public Node(T value, NodeList<T> neighbours)
        {
            this.Value = value;
            this.Neighbours = neighbours;
        }
    }

    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList() : base() { }

        public NodeList(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
            {
                base.Items.Add(default(Node<T>));
            }
        }

        public Node<T> FindByValue(T value)
        {
            foreach (Node<T> node in Items)
            {
                bool match = node.Value.Equals(value);
                if (match == false)
                {
                    continue;
                }

                return node;
            }

            return null;
        }
    }

    public class BinaryTreeNode<T> : Node<T>
    {
        public BinaryTreeNode<T> Left
        {
            get
            {
                if (base.Neighbours == null)
                {
                    return null;
                }
                else
                {
                    return (BinaryTreeNode<T>)base.Neighbours[0];
                }
            }
            set
            {
                if (base.Neighbours == null)
                {
                    base.Neighbours = new NodeList<T>(2);
                }

                base.Neighbours[0] = value;
            }
        }

        public BinaryTreeNode<T> Right
        {
            get
            {
                if (base.Neighbours == null)
                {
                    return null;
                }
                else
                {
                    return (BinaryTreeNode<T>)base.Neighbours[1];
                }
            }
            set
            {
                if (base.Neighbours == null)
                {
                    base.Neighbours = new NodeList<T>(2);
                }

                base.Neighbours[1] = value;
            }
        }

        public BinaryTreeNode() : base() { }

        public BinaryTreeNode(T value) : base(value, null) { }

        public BinaryTreeNode(T value, BinaryTreeNode<T> left, BinaryTreeNode<T> right)
        {
            base.Value = value;
            NodeList<T> children = new NodeList<T>(2);
            children[0] = left;
            children[1] = right;
            base.Neighbours = children;
        }
    }

    public class BinaryTree<T>
    {
        public BinaryTreeNode<T> Root { get; set; }

        public BinaryTree()
        {
            Root = null;
        }

        public virtual void Clear()
        {
            Root = null;
        }
    }

    public class BinarySearchTree<T> : BinaryTree<T>, ICollection, ICollection<T>, IEnumerable, IEnumerable<T>
    {
        public int Count { get; protected set; }
        protected IComparer<T> comparer { get; set; }

        public BinarySearchTree()
            : base()
        {
            Count = 0;
        }

        public override string ToString()
        {
            string output = ToStringInorderTraversal(Root, "");
            return output;
        }

        protected string ToStringInorderTraversal(BinaryTreeNode<T> current, string output)
        {
            if (current == null)
            {
                return output;
            }

            output = ToStringInorderTraversal(current.Left, output);

            output += current.Value.ToString();

            output = ToStringInorderTraversal(current.Right, output);

            return output;
        }

        public bool Contains(T value)
        {
            BinaryTreeNode<T> match, parent;
            match = FindNode(value, out parent);
            if (match != null)
            {
                return true;
            }

            return false;
        }

        public virtual void Add(T value)
        {
            BinaryTreeNode<T> nodeToAdd = new BinaryTreeNode<T>(value);

            BinaryTreeNode<T> match, parent;
            match = FindNode(value, out parent);

            if (match != null)
            {
                // duplicate found, do nothing
                return;
            }

            Count++;
            if (parent == null)
            {
                // empty tree
                Root = nodeToAdd;
                return;
            }

            int result = comparer.Compare(parent.Value, value);
            if (result > 0)
            {
                parent.Left = nodeToAdd;
            }
            else
            {
                parent.Right = nodeToAdd;
            }
        }

        public bool Remove(T value)
        {
            if (Root == null)
            {
                // no items to remove
                return false;
            }

            BinaryTreeNode<T> current = Root, parent = null;
            current = FindNode(value, out parent);

            if (current == null)
            {
                // no item found
                return false;
            }

            Count--;

            // current has no right child, move left child up to replace current
            if (current.Right == null)
            {
                MoveNode(parent, current, current.Left);
            }
            // right child has no left child, move right child up to replace current
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                MoveNode(parent, current, current.Right);
            }
            // right child has a left child, move right child's leftmost child up to replace current
            else
            {
                // Find right node's leftmost child
                BinaryTreeNode<T> leftmost = current.Right.Left, leftmostParent = current.Right;
                while (leftmost.Left != null)
                {
                    leftmostParent = leftmost;
                    leftmost = leftmost.Left;
                }

                // parent's left subtree becomes the leftmost's right subtree
                leftmostParent.Left = leftmost.Right;

                // move leftmost's left and right to current's left and right children
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                MoveNode(parent, current, leftmost);
            }

            return true;
        }

        protected void MoveNode(BinaryTreeNode<T> parent, BinaryTreeNode<T> child, BinaryTreeNode<T> grandChild)
        {
            if (parent == null)
            {
                Root = grandChild;
                return;
            }

            int result = comparer.Compare(parent.Value, child.Value);
            if (result > 0)
            {
                parent.Left = grandChild;
            }
            else if (result < 0)
            {
                parent.Right = grandChild;
            }
        }

        protected BinaryTreeNode<T> FindNode(T value, out BinaryTreeNode<T> parent)
        {
            BinaryTreeNode<T> current = Root;
            parent = null;
            int result;

            while (current != null)
            {
                result = comparer.Compare(current.Value, value);
                if (result == 0)
                {
                    return current;
                }
                else if (result > 0)
                {
                    // search left subtree
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // search right subtree
                    parent = current;
                    current = current.Right;
                }
            }

            return null;
        }

        //public IEnumerable<T> Preorder()
        //{

        //}

        //public IEnumerable<T> Inorder()
        //{

        //}

        //public IEnumerable<T> Postorder()
        //{

        //}

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class SkipListNode<T> : Node<T>
    {
        private SkipListNode() { }
        public int Height { get; protected set; }

        public SkipListNode(int height)
        {
            base.Neighbours = new SkipListNodeList<T>(height);
        }

        public SkipListNode(T value, int height)
            : base(value)
        {
            base.Neighbours = new SkipListNodeList<T>(height);
        }

        public SkipListNode<T> this[int index]
        {
            get { return (SkipListNode<T>)base.Neighbours[index]; }
            set { base.Neighbours[index] = value; }
        }
    }

    public class SkipListNodeList<T> : NodeList<T>
    {
        public SkipListNodeList(int height) : base(height) { }

        internal void IncrementHeight()
        {
            // Add dummy
            base.Items.Add(default(Node<T>));
        }

        internal void DecrementHeight()
        {
            // remove last entry
            base.Items.RemoveAt(base.Items.Count - 1);
        }
    }

    //public class SkipList<T> : IEnumerable<T>, ICollection<T>
    //{
    //    protected SkipListNode<T> head { get; set; }
    //    public int Count { get; protected set; }
    //    protected Random random { get; set; }
    //    protected IComparer<T> comparer { get; set; }
    //    protected readonly double prob = 0.5;
    //    public int Height { get { return head.Height; } }

    //    public SkipList() : this(-1, null) { }

    //    public SkipList(int randomSeed) : this(randomSeed, null) { }

    //    public SkipList(IComparer<T> comparer) : this(-1, comparer) { }

    //    public SkipList(int randomSeed, IComparer<T> comparer)
    //    {
    //        this.head = new SkipListNode<T>(1);
    //        this.Count = 0;

    //        if (randomSeed < 0)
    //        {
    //            this.random = new Random();
    //        }
    //        else
    //        {
    //            this.random = new Random(randomSeed);
    //        }

    //        if (comparer != null)
    //        {
    //            this.comparer = comparer;
    //        }
    //        else
    //        {
    //            this.comparer = Comparer<T>.Default;
    //        }
    //    }

    //    protected virtual int ChooseRandomHeight(int maxLevel)
    //    {
    //        int level = 1;
    //        while (random.NextDouble() < prob && level < maxLevel)
    //        {
    //            level++;
    //        }

    //        return level;
    //    }

    //    public bool Contains(T value)
    //    {
    //        SkipListNode<T> current = head;

    //        int topLevel = head.Height - 1;
    //        for (int level = topLevel; level >= 0; level--)
    //        {
    //            while (current[level] != null)
    //            {
    //                int results = comparer.Compare(current[level].Value, value);
    //                if (results == 0)
    //                {
    //                    return true;
    //                }
    //                else if (results < 0)
    //                {
    //                    current = current[level];   // the element is to the left, move down a level
    //                }
    //                else
    //                {
    //                    break;  // element is to the right of node, at or lower than the current level
    //                }
    //            }
    //        }

    //        return false;
    //    }

    //    public void Add(T value)
    //    {
    //        SkipListNode<T>[] updates = BuildUpdateTable(value);
    //        SkipListNode<T> current = updates[0];

    //        // see if a duplicate is being inserted
    //        if (current[0] != null && current[0].Value.CompareTo(value) == 0)
    //            // cannot enter a duplicate, handle this case by either just returning or by throwing an exception
    //            return;

    //        // create a new node
    //        SkipListNode<T> n = new SkipListNode<T>(value, ChooseRandomHeight(head.Height + 1));
    //        Count++;   // increment the count of elements in the skip list

    //        // if the node's level is greater than the head's level, increase the head's level
    //        if (n.Height > head.Height)
    //        {
    //            head.IncrementHeight();
    //            head[head.Height - 1] = n;
    //        }

    //        // splice the new node into the list
    //        for (int i = 0; i < n.Height; i++)
    //        {
    //            if (i < updates.Length)
    //            {
    //                n[i] = updates[i][i];
    //                updates[i][i] = n;
    //            }
    //        }
    //    }

    //    protected SkipListNode<T>[] BuildUpdateTable(T value)
    //    {
    //        SkipListNode<T>[] updates = new SkipListNode<T>[head.Height];
    //        SkipListNode<T> current = head;

    //        // determine the nodes that need to be updated at each level
    //        for (int i = head.Height - 1; i >= 0; i--)
    //        {
    //            while (current[i] != null && comparer.Compare(current[i].Value, value) < 0)
    //                current = current[i];

    //            updates[i] = current;
    //        }

    //        return updates;
    //    }

    //    public void Remove(T value)
    //    {
    //        SkipListNode<T>[] updates = BuildUpdateTable(value);
    //        SkipListNode<T> current = updates[0][0];

    //        if (current != null && comparer.Compare(current.Value, value) == 0)
    //        {
    //            Count--;

    //            // We found the data to delete
    //            for (int i = 0; i < head.Height; i++)
    //            {
    //                if (updates[i][i] != current)
    //                    break;
    //                else
    //                    updates[i][i] = current[i];
    //            }

    //            // finally, see if we need to trim the height of the list
    //            if (head[head.Height - 1] == null)
    //                // we removed the single, tallest item... reduce the list height
    //                head.DecrementHeight();

    //            return true;   // item removed, return true
    //        }
    //        else
    //        {
    //            // the data to delete wasn't found – return false
    //            return false;
    //        }
    //    }

    //    public IEnumerator<T> GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public void Clear()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void CopyTo(T[] array, int arrayIndex)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool IsReadOnly
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    bool ICollection<T>.Remove(T item)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
