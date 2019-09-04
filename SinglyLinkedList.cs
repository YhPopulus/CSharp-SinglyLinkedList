/**
 *Copyright: Copyright (c) 2019
 *Created on 2019.3.1
 *Author:YhPopulus
 *Version 0.0
 *Title: CSharp LinkedList(Singly)
 **/
using System;
namespace System.Collections.Generic
{
    public class SinglyLinkedList<T> : ICollection<T>, ICollection
    {
        internal SinglyListNode<T> head;
        internal int _count;
        internal int _version;
        private Object _syncRoot;

        public int Count => _count;

        public SinglyListNode<T> Fast => head;

        public bool IsReadOnly => false;

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
                }
                return _syncRoot;
            }
        }
        bool ICollection.IsSynchronized => false;

        public SinglyLinkedList()
        {

        }
        public SinglyLinkedList(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            foreach (var tmp in collection)
            {
                InternalAdd(tmp);
            }
        }
        public void Add(T item)
        {
            InternalAdd(item);
        }
        private void InternalAdd(T item)
        {
            //处理第一次添加内容和后续添加内容
            if (head == null)
            {
                SinglyListNode<T> newNode = new SinglyListNode<T>(this, item);
                head = newNode;
            }
            else
            {
                SinglyListNode<T> newNode = new SinglyListNode<T>(this, item);
                head.next = newNode;
            }
            _version++;
            _count++;
        }
        public void Clear()
        {
            if (head == null) return;
            SinglyListNode<T> current = head;

            while (current != null)
            {
                SinglyListNode<T> tmp = current;
                current = current.next;
                tmp.Invalidate();
            }
            _count = 0;
            _version++;
        }
        public bool Contains(T item)
        {
            return FindItem(item) == null ? false : true;
        }
        private SinglyListNode<T> FindItem(T item)
        {
            SinglyListNode<T> current = head;
            EqualityComparer<T> eq = EqualityComparer<T>.Default;
            if (current != null)
            {
                if (item == null)
                {
                    while (current != null)
                    {
                        if (eq.Equals(item, current.Value)) return current;
                        current = current.next;
                    }
                }
                else
                {
                    while (current != null)
                    {
                        if (eq.Equals(item, current.Value)) return current;
                        current = current.next;
                    }
                }
            }
            return null;
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public bool Remove(T item)
        {
            SinglyListNode<T> targetNode = FindItem(item);
            if (targetNode != null)
            {
                InternalRemove(targetNode);
                return true;
            }
            return false;
        }
        public void Remove(SinglyListNode<T> node)
        {
            if (node != null)
                InternalRemove(node);
        }
        private void InternalRemove(SinglyListNode<T> node)
        {
            if (node.list != this) throw new Exception("试图从另外一个链表删除元素");
            if (head == null) throw new ArgumentNullException("Collection");
            EqualityComparer<SinglyListNode<T>> eq = EqualityComparer<SinglyListNode<T>>.Default;
            if (eq.Equals(node, head))
            {
                //处理头部相等的情况
                if (head.next == null)
                    head.Invalidate();
                else
                    head = head.next;
            }
            else
            {
                SinglyListNode<T> pres = head;
                SinglyListNode<T> current = head.next;
                while (current != null)
                {
                    //移除
                    if (eq.Equals(current, node))
                    {
                        pres.next = current.next;
                        current.Invalidate();
                    }
                    else
                    {
                        pres = current;
                        current = current.next;
                    }
                }
            }
            _version++;
            _count--;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public struct Enumerator : IEnumerator<T>
        {
            SinglyLinkedList<T> _list;
            SinglyListNode<T> _node;

            int _index;
            int _version;
            T _current;
            public T Current => _current;


            internal Enumerator(SinglyLinkedList<T> list)
            {
                this._list = list;
                this._node = list.head;
                this._index = 0;
                this._version = _list._version;
                this._current = default(T);
            }
            object IEnumerator.Current
            {
                get
                {
                    if (_index == 0 || (_index == _list.Count + 1))
                    {
                        return "不能够进行枚举操作";
                    }
                    return _current;
                }
            }


            public void Dispose()
            {
                //throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                if (_version != _list._version) throw new InvalidOperationException("不可在枚举过程中进行额外操作!");
                if (_node == null)
                {
                    return false;
                }
                _index++;
                _current = _node.item;
                _node = _node.next;
                return true;
            }

            public void Reset()
            {
                if (_version != _list._version)
                {
                    throw new InvalidOperationException("不可在枚举过程中进行额外操作");
                }

                _current = default(T);
                _node = _list.head;
                _index = 0;
            }
        }

    }
    public sealed class SinglyListNode<T>
    {
        internal SinglyLinkedList<T> list;
        internal SinglyListNode<T> next;
        internal T item;


        public SinglyListNode(T value)
        {
            this.item = value;
        }
        public SinglyListNode(SinglyLinkedList<T> list, T value)
        {
            this.list = list;
            this.item = value;
        }

        public SinglyListNode<T> Next
        {
            get => next;
        }

        public T Value
        {
            get => item;
            set
            {
                item = value;
            }
        }
        internal void Invalidate()
        {
            list = null;
            next = null;
        }

    }
}
