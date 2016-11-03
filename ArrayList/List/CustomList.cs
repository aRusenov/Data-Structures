using System;
using System.Collections;
using System.Collections.Generic;

namespace List
{
    public class CustomList<T> : IEnumerable<T>
    {
        public const int DefaultCapacity = 16;

        private T[] items;
        private int capacity;

        public CustomList(int capacity = DefaultCapacity)
        {
            this.Capacity = capacity;
            this.items = new T[capacity];
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

        public void Add(T item)
        {
            ResizeIfNeeded();

            this.items[this.Count] = item;
            this.Count++;
        }

        public void Insert(T item, int index)
        {
            this.ValidateIndex(index);
            this.ResizeIfNeeded();
            Array.Copy(this.items, index, this.items, index + 1, this.Count - index);
            this.items[index] = item;
            this.Count++;
        }

        public bool Remove(T item)
        {
            var index = this.IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            this.RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            this.ValidateIndex(index);
            Array.Copy(this.items, index + 1, this.items, index, this.Count - index - 1);
            this.items[this.Count - 1] = default(T);
            this.Count--;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.items[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }

        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException("Index is outside of the list bounds");
            }
        }

        private void ResizeIfNeeded()
        {
            if (this.Count == this.Capacity)
            {
                this.capacity *= 2;
                var newItems = new T[this.capacity];
                Array.Copy(this.items, newItems, this.items.Length);
                this.items = newItems;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this.items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
