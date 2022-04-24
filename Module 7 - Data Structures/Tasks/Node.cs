﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks
{
    public class Node<T>
    {
        public Node<T> Previous { get; set; }
        public Node<T> Next { get; set; }
        public T Value { get; set; }

        public Node(T value)
        {
            Value = value;
        }
    }
}
