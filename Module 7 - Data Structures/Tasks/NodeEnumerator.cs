using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Tasks
{
    class NodeEnumerator<T> : IEnumerator<T>
    {
        private int _position = -1;
        private Node<T> _head;
        private Node<T> _current;

        public NodeEnumerator(Node<T> head)
        {
            _head = head;
        }

        public T Current => _current.Value;
        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_position == -1)
            {
                _current = _head;
            }
            else
            {
                _current = _current.Next;
            }

            _position++;

            return _current != null;
        }

        public void Reset()
        {
            _position = -1;
        }

        public void Dispose()
        {
        }
    }
}
