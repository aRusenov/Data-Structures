using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervalTree
{
    public class IntervalTree<T> where T : IComparable<T>
    {
        private Node<T> root;

        public int Count { get; set; }

        public void Insert(Interval<T> interval)
        {
            if (this.root == null)
            {
                this.root = new Node<T>(interval);
            }
            else
            {
                this.InsertInternal(this.root, interval);
            }

            this.Count++;
        }

        private void InsertInternal(Node<T> node, Interval<T> interval)
        {
            if (interval.Low.CompareTo(node.Interval.Low) >= 0)
            {
                if (node.Right == null)
                {
                    node.Right = new Node<T>(interval);
                }
                else
                {
                    InsertInternal(node.Right, interval);
                }
            }
            else
            {
                if (node.Left == null)
                {
                    node.Left = new Node<T>(interval);
                }
                else
                {
                    InsertInternal(node.Left, interval);
                }
            }

            if (node.Left != null && node.Left.Max.CompareTo(node.Max) > 0)
            {
                node.Max = node.Left.Max;
            }

            if (node.Right != null && node.Right.Max.CompareTo(node.Max) > 0)
            {
                node.Max = node.Right.Max;
            }
        }

        public void GetOverlappingIntervals(Interval<T> interval, List<Interval<T>> outResult)
        {
            if (this.root == null)
            {
                return;
            }

            GetOverlappingIntervals(this.root, interval, outResult);
        }

        private static void GetOverlappingIntervals(Node<T> node, Interval<T> interval, List<Interval<T>> outResult)
        {
            var goLeft = node.Left != null && node.Left.Max.CompareTo(interval.Low) > 0;
            var goRight = node.Right != null && node.Right.Interval.Low.CompareTo(interval.High) < 0;
            
            if (goLeft)
            {
                GetOverlappingIntervals(node.Left, interval, outResult);
            }
            
            if (node.Interval.Low.CompareTo(interval.High) < 0 &&
                node.Interval.High.CompareTo(interval.Low) > 0)
            {
                outResult.Add(node.Interval);
            }

            if (goRight)
            {
                GetOverlappingIntervals(node.Right, interval, outResult);
            }
        }
    }
}
