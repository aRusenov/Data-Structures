using System;
using System.Collections;
using System.Collections.Generic;

namespace Stack
{
    public class CustomStack<T> : IEnumerable<T>
    {
        public const int DefaultCapacity = 16;

        private T[] elements;
        private int capacity;

        public CustomStack(int capacity = DefaultCapacity)
        {
            this.Capacity = capacity;
            this.elements = new T[capacity];
        }

        public int Count { get; private set; }

        public int Capacity
        {
            get { return this.capacity; }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Capacity must be a positive integer number");
                }

                this.capacity = value;
            }
        }

        public void Push(T item)
        {
            if (this.Count == this.Capacity)
            {
                this.capacity *= 2;
                var newItems = new T[this.capacity];
                Array.Copy(this.elements, newItems, this.elements.Length);
                this.elements = newItems;
            }

            this.elements[this.Count] = item;
            this.Count++;
        }

        public T Pop()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }

            var popped = this.elements[this.Count - 1];
            this.elements[this.Count - 1] = default(T);
            this.Count--;
            return popped;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.elements[i].Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this.elements[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
