using System;
using System.Collections.Generic;

namespace BinaryHeap
{
    public class BinaryHeap<T> where T : IComparable<T>
    {
        private List<T> items;

        public BinaryHeap()
        {
            this.items = new List<T>();
        }

        public BinaryHeap(T[] elements)
        {
            this.items = new List<T>(elements);
            for (int i = this.items.Count / 2; i >= 0; i--)
            {
                this.HeapifyDown(i);
            }
        }

        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        public T ExtractMax()
        {
            var max = this.items[0];
            this.items[0] = this.items[items.Count - 1];
            this.items.RemoveAt(this.items.Count - 1);
            if (this.items.Count > 0)
            {
                this.HeapifyDown(0);
            }

            return max;
        }

        public T PeekMax()
        {
            var max = this.items[0];
            return max;
        }

        public void Insert(T node)
        {
            this.items.Add(node);
            this.HeapifyUp(this.items.Count - 1);
        }

        private void HeapifyDown(int i)
        {
            var left = 2 * i + 1;
            var right = 2 * i + 2;
            var largest = i;
            if (left < this.items.Count && this.items[left].CompareTo(this.items[largest]) > 0)
            {
                largest = left;
            }

            if (right < this.items.Count && this.items[right].CompareTo(this.items[largest]) > 0)
            {
                largest = right;
            }

            if (largest != i)
            {
                T old = this.items[i];
                this.items[i] = this.items[largest];
                this.items[largest] = old;
                this.HeapifyDown(largest);
            }
        }

        private void HeapifyUp(int i)
        {
            var parent = (i - 1) / 2;
            while (i > 0 && this.items[i].CompareTo(this.items[parent]) > 0)
            {
                T old = this.items[i];
                this.items[i] = this.items[parent];
                this.items[parent] = old;
                i = parent;
                parent = (i - 1) / 2;
            }
        }
    }
}