using System;
using System.Collections.Generic;

namespace KdTree
{
    public class KdTree<T> where T : IDimensions
    {
        private Node<T> root;

        public int Count { get; private set; }

        /// <summary>
        /// Constructs a uniform 2d-Tree
        /// </summary>
        /// <param name="initialSet"></param>
        public KdTree(List<T> initialSet)
        {
            this.root = this.Construct(initialSet, 0, initialSet.Count - 1);
        }

        private Node<T> Construct(List<T> points, int lo, int hi, int depth = 0)
        {
            if (lo > hi)
            {
                return null;
            }

            var d = depth % 2;
            points.Sort(lo, hi - lo + 1, Comparer<T>.Create(
                (p1, p2) => p1.Dimensions[d].CompareTo(p2.Dimensions[d])));

            var median = lo + (hi - lo) / 2;
            var node = new Node<T>(points[median]);
            node.Left = this.Construct(points, lo, median - 1, depth + 1);
            node.Right = this.Construct(points, median + 1, hi, depth + 1);
            this.Count++;

            return node;
        }

        public void Insert(T item)
        {
            if (this.root == null)
            {
                this.root = new Node<T>(item);
            }
            else
            {
                this.Insert(this.root, item, 0);
            }

            this.Count++;
        }

        public void Insert(Node<T> node, T item, int level)
        {
            var d = level % 2;
            if (item.Dimensions[d].CompareTo(node.Value.Dimensions[d]) >= 0)
            {
                if (node.Right == null)
                {
                    node.Right = new Node<T>(item);
                }
                else
                {
                    this.Insert(node.Right, item, level + 1);
                }
            }
            else
            {
                if (node.Left == null)
                {
                    node.Left = new Node<T>(item);
                }
                else
                {
                    this.Insert(node.Left, item, level + 1);
                }
            }
        }

        public void GetInRadius(float x, float y, float radius, List<T> outResult)
        {
            if (this.root == null)
            {
                return;
            }

            GetInRadius(this.root, new float[] { x, y }, radius, outResult);
        }

        private static void GetInRadius(Node<T> node, float[] dimens, float radius, List<T> outResult, int level = 0)
        {
            var d = level % 2;
            if (node.Left != null && node.Value.Dimensions[d] >= dimens[d] - radius)
            {
                GetInRadius(node.Left, dimens, radius, outResult, level + 1);
            }

            var deltaD1 = Math.Abs(node.Value.Dimensions[0] - dimens[0]);
            var deltaD2 = Math.Abs(node.Value.Dimensions[1] - dimens[1]);
            if (Math.Sqrt(deltaD1 * deltaD1 + deltaD2 * deltaD2) <= radius)
            {
                outResult.Add(node.Value);
            }

            if (node.Right != null && node.Value.Dimensions[d] < dimens[d] + radius)
            {
                GetInRadius(node.Right, dimens, radius, outResult, level + 1);
            }
        }
    }
}
