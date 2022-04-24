using System;
using System.Collections;
using System.Collections.Generic;
using Tasks.DoNotChange;

namespace Tasks
{
    public class DoublyLinkedList<T> : IDoublyLinkedList<T>
    {
        private Node<T> _head;
        private Node<T> _tail;

        public int Length { get; private set; }

        public void Add(T e)
        {
            var newNode = new Node<T>(e);
            if (_head == null)
            {
                _head = newNode;
            }
            else
            {
                newNode.Previous = _tail;
                _tail.Next = newNode;
            }

            _tail = newNode;
            Length++;
        }

        public void AddAt(int index, T e)
        {
            var newNode = new Node<T>(e);

            // If list is empty.
            if (_head == null)
            {
                _head = newNode;
                _tail = _head;
                Length++;
                return;
            }

            // Add first.
            if (index == 0)
            {
                newNode.Next = _head;
                _head.Previous = newNode;
                _head = newNode;
                Length++;
                return;
            }

            // Add last.
            if (index == Length)
            {
                newNode.Previous = _tail;
                _tail.Next = newNode;
                _tail = newNode;
                Length++;
                return;
            }
            
            // Add in the middle.
            var position = 0;
            var current = _head;
            while ((position < index) && (current.Next != null))
            {
                current = current.Next;
                position++;
            }

            if (position < --index)
            {
                throw new IndexOutOfRangeException();
            }
            
            newNode.Next = current;
            newNode.Previous = current.Previous;
            if (current.Previous != null)
            {
                current.Previous.Next = newNode;
            }

            current.Previous = newNode;
            Length++;
        }

        public T ElementAt(int index)
        {
            if (index < 0 || _head == null)
            {
                throw new IndexOutOfRangeException();
            }

            var position = 0;
            var current = _head;
            while ((position < index) && (current.Next != null))
            {
                current = current.Next;
                position++;
            }

            if (position < index)
            {
                throw new IndexOutOfRangeException();
            }

            return current.Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>) new NodeEnumerator<T>(_head);
        }

        public void Remove(T item)
        {
            var current = _head;
            while (current != null && !current.Value.Equals(item))
            {
                current = current.Next;
            }

            // Item is not found.
            if (current == null)
            {
                return;
            }

            if (current.Previous != null)
            {
                current.Previous.Next = current.Next;
            }
            else
            {
                _head = current.Next;
            }

            Length--;
        }

        public T RemoveAt(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            var position = 0;
            var current = _head;
            while ((position < index) && (current.Next != null))
            {
                current = current.Next;
                position++;
            }

            if ((position < index) || (current == null))
            {
                throw new IndexOutOfRangeException();
            }

            if (current.Previous != null)
            {
                current.Previous.Next = current.Next;
            }
            else
            {
                _head = current.Next;
            }

            Length--;
            return current.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
