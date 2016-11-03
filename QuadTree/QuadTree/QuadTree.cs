using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QuadTree
{
    public class QuadTree<T> where T : IBoundable
    {
        public const int DefaultMaxDepth = 5;

        public readonly int MaxDepth;

        private Node<T> root;

        public QuadTree(int width, int height, int maxDepth = DefaultMaxDepth)
        {
            this.root = new Node<T>(0, 0, width, height);
            this.Bounds = this.root.Bounds;
            this.MaxDepth = maxDepth;
        }

        public int Count { get; private set; }

        public Rectangle Bounds { get; private set; }

        public bool Insert(T item)
        {
            if (!item.Bounds.IsInside(this.Bounds))
            {
                return false;
            }

            int depth = 1;
            var currentNode = this.root;
            while (currentNode.Children != null)
            {
                var quadrant = GetQuadrant(currentNode, item.Bounds);
                if (quadrant == -1)
                {
                    break;
                }

                currentNode = currentNode.Children[quadrant];
            }

            currentNode.Items.Add(item);
            this.Split(currentNode, depth);
            this.Count++;

            return true;
        }

        private void Split(Node<T> node, int nodeDepth)
        {
            if (!(node.ShouldSplit && nodeDepth < MaxDepth))
            {
                return;
            }

            var leftWidth = node.Bounds.Width / 2;
            var rightWidth = node.Bounds.Width - leftWidth;
            var topHeight = node.Bounds.Height / 2;
            var bottomHeight = node.Bounds.Height - topHeight;

            node.Children = new Node<T>[4];
            node.Children[0] = new Node<T>(node.Bounds.MidX, node.Bounds.Top, rightWidth, topHeight);
            node.Children[1] = new Node<T>(node.Bounds.Left, node.Bounds.Top, leftWidth, topHeight);
            node.Children[2] = new Node<T>(node.Bounds.Left, node.Bounds.MidY, leftWidth, bottomHeight);
            node.Children[3] = new Node<T>(node.Bounds.MidX, node.Bounds.MidY, rightWidth, bottomHeight);

            for (int i = 0; i < node.Items.Count; i++)
            {
                var item = node.Items[i];
                var quandrant = GetQuadrant(node, item.Bounds);
                if (quandrant != -1)
                {
                    Debug.Assert(quandrant >= 0 && quandrant <= 4);
                    node.Children[quandrant].Items.Add(item);
                    node.Items.RemoveAt(i);
                    i--;
                }
            }

            foreach (var child in node.Children)
            {
                Split(child, nodeDepth + 1);
            }
        }

        private static int GetQuadrant(Node<T> node, Rectangle bounds)
        {
            var isInLeftPart = node.Bounds.Left <= bounds.Left &&
                bounds.Right <= node.Bounds.MidX;
            var isInRightPart = node.Bounds.MidX <= bounds.Left &&
                bounds.Right <= node.Bounds.Right;
            var isInTopPart = node.Bounds.Top <= bounds.Top &&
                bounds.Bottom <= node.Bounds.MidY;
            var isInBottomPart = node.Bounds.MidY <= bounds.Top &&
                bounds.Bottom <= node.Bounds.Bottom;

            int quadrant = -1;
            if (isInTopPart)
            {
                if (isInRightPart)
                    quadrant = 0;
                else if (isInLeftPart)
                    quadrant = 1;
            }
            else if (isInBottomPart)
            {
                if (isInRightPart)
                    quadrant = 3;
                else if (isInLeftPart)
                    quadrant = 2;
            }

            return quadrant;
        }

        public List<T> Report(Rectangle bounds)
        {
            var collisionCandidates = new List<T>();

            this.GetCollisionCandidates(this.root, bounds, collisionCandidates);

            return collisionCandidates;
        }

        private void GetCollisionCandidates(Node<T> node, Rectangle bounds, List<T> results)
        {
            var quadrant = GetQuadrant(node, bounds);
            if (quadrant == -1)
            {
                GetSubtreeContents(node, bounds, results);
            }
            else
            {
                if (node.Children != null)
                {
                    this.GetCollisionCandidates(node.Children[quadrant], bounds, results);
                }

                results.AddRange(node.Items);
            }
        }

        private static void GetSubtreeContents(Node<T> node, Rectangle bounds, List<T> results)
        {
            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    if (child.Bounds.Intersects(bounds))
                    {
                        GetSubtreeContents(child, bounds, results);
                    }
                }
            }

            results.AddRange(node.Items);
        }

        public void ForEachDfs(Action<List<T>, int, int> action)
        {
            this.ForEachDfs(this.root, action);
        }

        private void ForEachDfs(Node<T> node, Action<List<T>, int, int> action, int depth = 1, int quadrant = 0)
        {
            if (node.Items.Any())
            {
                action(node.Items, depth, quadrant);
            }

            if (node.Children != null)
            {
                var count = 0;
                foreach (var child in node.Children)
                {
                    this.ForEachDfs(child, action, depth + 1, count);
                    count++;
                }
            }
        }
    }
}
