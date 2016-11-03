using System;

namespace IntervalTree
{
    public class Node<T> where T : IComparable<T>
    {
        public Node(Interval<T> interval)
        {
            this.Interval = interval;
            this.Max = interval.High;
        }

        public Interval<T> Interval { get; set; }

        public T Max { get; set; }

        public Node<T> Left { get; set; }

        public Node<T> Right { get; set; }
    }

    public class Interval<T> where T : IComparable<T>
    {
        public Interval(T low, T high)
        {
            this.Low = low;
            this.High = high;
        }

        public T Low { get; set; }

        public T High { get; set; }
    }
}
