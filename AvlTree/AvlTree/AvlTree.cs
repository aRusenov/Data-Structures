using System;
using System.Collections;
using System.Collections.Generic;

namespace AvlTree
{
    public class AvlTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private Node<T> root;

        public int Count { get; private set; }

        public void Add(T item)
        {
            var inserted = true;
            if (this.root == null)
            {
                this.root = new Node<T>(item);
            }
            else
            {
                inserted = this.InsertInternal(this.root, item);
            }

            if (inserted)
            {
                this.Count++;
            }
        }

        public void ForeachBfs(Action<T> action)
        {
            if (this.Count == 0)
            {
                return;
            }

            var queue = new Queue<Node<T>>();
            queue.Enqueue(this.root);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                action(currentNode.Value);
                if (currentNode.LeftChild != null)
                {
                    queue.Enqueue(currentNode.LeftChild);
                }

                if (currentNode.RightChild != null)
                {
                    queue.Enqueue(currentNode.RightChild);
                }
            }
        }

        public bool Remove(T item)
        {
            var nodeToRemove = this.Find(item);
            if (nodeToRemove == null)
            {
                return false;
            }

            Node<T> retraceStartNode = null;
            if (nodeToRemove.ChildrenCount == 2)
            {
                // Find largest node (Y) from left subtree and replace it with X
                var swapNode = nodeToRemove.LeftChild;
                while (swapNode.RightChild != null)
                {
                    swapNode = swapNode.RightChild;
                }

                if (swapNode.IsLeftChild)
                {
                    swapNode.Parent.LeftChild = swapNode.LeftChild;
                    swapNode.Parent.BalanceFactor--;
                }
                else
                {
                    swapNode.Parent.RightChild = swapNode.LeftChild;
                    swapNode.Parent.BalanceFactor++;
                }

                nodeToRemove.Value = swapNode.Value;
                retraceStartNode = swapNode.Parent;
            }
            else if (nodeToRemove.ChildrenCount == 1)
            {
                // Remove X
                var subtree = nodeToRemove.LeftChild ?? nodeToRemove.RightChild;
                subtree.Parent = nodeToRemove.Parent;
                if (nodeToRemove.IsLeftChild)
                {
                    nodeToRemove.Parent.LeftChild = subtree;
                    nodeToRemove.Parent.BalanceFactor--;
                }
                else
                {
                    nodeToRemove.Parent.RightChild = subtree;
                    nodeToRemove.Parent.BalanceFactor++;
                }

                // Retrace from X's parent
                retraceStartNode = nodeToRemove.Parent;
            }
            else
            {
                if (nodeToRemove.IsLeftChild)
                {
                    nodeToRemove.Parent.LeftChild = null;
                    nodeToRemove.Parent.BalanceFactor--;
                }
                else if (nodeToRemove.IsRightChild)
                {
                    nodeToRemove.Parent.RightChild = null;
                    nodeToRemove.Parent.BalanceFactor++;
                }

                retraceStartNode = nodeToRemove.Parent;
            }

            this.RetraceDelete(retraceStartNode);
            this.Count--;

            return true;
        }

        public IEnumerable<T> FindRange(T start, T end)
        {
            List<T> range = new List<T>();
            InOrderDfsRange(this.root, start, end, range);
            return range;
        }

        private void InOrderDfsRange(Node<T> node, T start, T end, List<T> items)
        {
            if (node.LeftChild != null && node.LeftChild.Value.CompareTo(start) >= 0)
            {
                InOrderDfsRange(node.LeftChild, start, end, items);
            }

            if (node.Value.CompareTo(start) >= 0 && node.Value.CompareTo(end) <= 0)
            {
                items.Add(node.Value);
            }

            if (node.RightChild != null && node.RightChild.Value.CompareTo(end) <= 0)
            {
                InOrderDfsRange(node.RightChild, start, end, items);
            }
        }

        public bool Contains(T item)
        {
            return this.Find(item) != null;
        }

        /// <summary>
        /// Traverses the tree using Depth-First Search
        /// and invokes the given action for each element.
        /// </summary>
        /// <param name="action">The action method that will be invoked. 
        /// Takes the depth of the element in the tree and its value.</param>
        /// 
        public void ForeachDfs(Action<int, T> action)
        {
            if (this.Count == 0)
            {
                return;
            }

            this.Dfs(this.root, 1, action);
        }

        /// <summary>
        /// Performs an iterative in-order traversal to iterate 
        /// through the data in ascending sorted order
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            var nodes = new Stack<Node<T>>();
            var currentNode = this.root;
            var done = false;
            while (!done)
            {
                if (currentNode != null)
                {
                    nodes.Push(currentNode);
                    currentNode = currentNode.LeftChild;
                }
                else
                {
                    if (nodes.Count == 0)
                    {
                        done = true;
                    }
                    else
                    {
                        var top = nodes.Pop();
                        yield return top.Value;

                        currentNode = top.RightChild;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private bool InsertInternal(Node<T> node, T item)
        {
            var currentNode = node;
            var newNode = new Node<T>(item);
            while (true)
            {
                if (currentNode.Value.CompareTo(item) < 0)
                {
                    if (currentNode.RightChild == null)
                    {
                        currentNode.RightChild = newNode;
                        this.RetraceInsert(newNode);
                        return true;
                    }

                    currentNode = currentNode.RightChild;
                }
                else if (currentNode.Value.CompareTo(item) > 0)
                {
                    if (currentNode.LeftChild == null)
                    {
                        currentNode.LeftChild = newNode;
                        this.RetraceInsert(newNode);
                        return true;
                    }

                    currentNode = currentNode.LeftChild;
                }
                else
                {
                    return false;
                }
            }
        }

        private void RetraceInsert(Node<T> node)
        {
            if (node == null)
            {
                return;
            }

            var parent = node.Parent;
            while (parent != null)
            {
                if (node.IsLeftChild)
                {
                    if (parent.BalanceFactor == 1)
                    {
                        parent.BalanceFactor++;
                        if (node.BalanceFactor == -1)
                        {
                            this.RotateLeft(node);
                        }

                        this.RotateRight(parent);
                        break;
                    }

                    if (parent.BalanceFactor == -1)
                    {
                        parent.BalanceFactor = 0;
                        break;
                    }

                    parent.BalanceFactor = 1;
                }
                else
                {
                    if (parent.BalanceFactor == -1)
                    {
                        parent.BalanceFactor--;
                        if (node.BalanceFactor == 1)
                        {
                            this.RotateRight(node);
                        }

                        this.RotateLeft(parent);
                        break;
                    }

                    if (parent.BalanceFactor == 1)
                    {
                        parent.BalanceFactor = 0;
                        break;
                    }

                    parent.BalanceFactor = -1;
                }

                node = parent;
                parent = node.Parent;
            }
        }

        private void RetraceDelete(Node<T> node)
        {
            while (node != null)
            {
                if (node.BalanceFactor == 1 || node.BalanceFactor == -1)
                {
                    break;
                }
                else if (node.BalanceFactor == -2)
                {
                    if (node.RightChild.BalanceFactor == 1)
                    {
                        this.RotateRight(node.RightChild);
                    }
                    this.RotateLeft(node);
                }
                else if (node.BalanceFactor == 2)
                {
                    if (node.LeftChild.BalanceFactor == -1)
                    {
                        this.RotateLeft(node.LeftChild);
                    }
                    this.RotateRight(node);
                }
                else if (node.BalanceFactor == 0)
                {
                    if (node.IsLeftChild)
                    {
                        node.Parent.BalanceFactor--;
                    }
                    else if (node.IsRightChild)
                    {
                        node.Parent.BalanceFactor++;
                    }
                    node = node.Parent;
                }
            }
        }

        private void RotateLeft(Node<T> node)
        {
            var parent = node.Parent;
            var child = node.RightChild;

            if (parent != null)
            {
                if (node.IsLeftChild)
                {
                    parent.LeftChild = child;
                }
                else
                {
                    parent.RightChild = child;
                }
            }
            else
            {
                this.root = child;
                this.root.Parent = null;
            }

            node.RightChild = child.LeftChild;
            child.LeftChild = node;

            node.BalanceFactor += 1 - Math.Min(child.BalanceFactor, 0);
            child.BalanceFactor += 1 + Math.Max(node.BalanceFactor, 0);
        }

        private void RotateRight(Node<T> node)
        {
            var parent = node.Parent;
            var child = node.LeftChild;

            if (parent != null)
            {
                if (node.IsLeftChild)
                {
                    parent.LeftChild = child;
                }
                else
                {
                    parent.RightChild = child;
                }
            }
            else
            {
                this.root = child;
                this.root.Parent = null;
            }

            node.LeftChild = child.RightChild;
            child.RightChild = node;

            node.BalanceFactor -= 1 + Math.Max(child.BalanceFactor, 0);
            child.BalanceFactor -= 1 - Math.Min(node.BalanceFactor, 0);
        }

        private Node<T> Find(T item)
        {
            var node = this.root;
            while (node != null)
            {
                if (node.Value.CompareTo(item) == 0)
                {
                    return node;
                }

                if (node.Value.CompareTo(item) < 0)
                {
                    node = node.RightChild;
                }
                else
                {
                    node = node.LeftChild;
                }
            }

            return null;
        }

        private void Dfs(Node<T> node, int depth, Action<int, T> action)
        {
            if (node.LeftChild != null)
            {
                this.Dfs(node.LeftChild, depth + 1, action);
            }

            var deptDiff = this.Depth(node.LeftChild) - this.Depth(node.RightChild);
            if (deptDiff != node.BalanceFactor)
            {
                throw new ArgumentException(string.Format(
                    "BF is {0}, should be {1}", deptDiff, node.BalanceFactor));
            }

            action(depth, node.Value);

            if (node.RightChild != null)
            {
                this.Dfs(node.RightChild, depth + 1, action);
            }
        }

        private int Depth(Node<T> n)
        {
            if (n == null)
            {
                return 0;
            }

            return Math.Max(this.Depth(n.LeftChild), this.Depth(n.RightChild)) + 1;
        }
    }
}