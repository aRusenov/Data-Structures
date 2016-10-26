using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedList
{
    public class DoublyLinkedList<T> : IEnumerable<T>
    {
        private class LinkedListNode<T>
        {
            public T Value { get; private set; }

            public LinkedListNode<T> Next { get; set; }

            public LinkedListNode<T> Prev { get; set; }

            public LinkedListNode(T value)
            {
                this.Value = value;
            }
        }

        private LinkedListNode<T> head;
        private LinkedListNode<T> tail;

        public int Count { get; private set; }

        public void AddFirst(T element)
        {
            if (this.Count == 0)
            {
                this.head = this.tail = new LinkedListNode<T>(element);
            }
            else
            {
                var newHead = new LinkedListNode<T>(element);
                newHead.Next = this.head;
                this.head.Prev = newHead;
                this.head = newHead;
            }

            this.Count++;
        }

        public void AddLast(T element)
        {
            if (this.Count == 0)
            {
                this.head = this.tail = new LinkedListNode<T>(element);
            }
            else
            {
                var newTail = new LinkedListNode<T>(element);
                newTail.Prev = this.tail;
                this.tail.Next = newTail;
                this.tail = newTail;
            }

            this.Count++;
        }

        public T RemoveFirst()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("List is empty");
            }

            var firstElement = this.head.Value;
            this.head = this.head.Next;
            if (this.head != null)
            {
                this.head.Prev = null;
            }
            else
            {
                this.tail = null;
            }

            this.Count--;
            return firstElement;
        }

        public T RemoveLast()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("List is empty");
            }

            var lastElement = this.tail.Value;
            this.tail = this.tail.Prev;
            if (this.tail != null)
            {
                this.tail.Next = null;
            }
            else
            {
                this.head = null;
            }

            this.Count--;
            return lastElement;
        }

        public void ForEach(Action<T> action)
        {
            var currentNode = this.head;
            while (currentNode != null)
            {
                action(currentNode.Value);
                currentNode = currentNode.Next;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var currentNode = this.head;
            while (currentNode != null)
            {
                yield return currentNode.Value;
                currentNode = currentNode.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public T[] ToArray()
        {
            var arr = new T[this.Count];
            int index = 0;
            var currentNode = this.head;
            while (currentNode != null)
            {
                arr[index++] = currentNode.Value;
                currentNode = currentNode.Next;
            }

            return arr;
        }
    }
}
