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

        public SinglyListNode<T> First => head;

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
        ///*整体的遍历过程就相当于while(object.MoveNext)*/
        //IL_0000: nop
        //IL_0001: nop
        //IL_0002: ldarg.0
        //IL_0003: ldfld class [mscorlib] System.Collections.Generic.List`1<int32> Program::list//将list对象引用的值进行入栈操作。
        //IL_0008: callvirt instance valuetype[mscorlib] System.Collections.Generic.List`1/Enumerator<!0> class [mscorlib] System.Collections.Generic.List`1<int32>::GetEnumerator()
        ////调用list中实现的GetEnumerator()方法获得枚举对象。
        //IL_000d: stloc.0//将[枚举对象]从计算堆栈的顶部弹出并将其存储到索引 0 处的局部变量列表中。
        //.try
        //{
        //    // sequence point: hidden
        //    IL_000e: br.s IL_0021//进入循环跳转到0021
        //    // loop start (head: IL_0021)
        //        IL_0010: ldloca.s 0//将位于0索引处的枚举器局部变量的地址加载到计算堆栈上。
        //        IL_0012: call instance !0 valuetype[mscorlib] System.Collections.Generic.List`1/Enumerator<int32>::get_Current()//调用枚举器中的GetCurrent方法返回枚举器当前对象
        //        IL_0017: stloc.1//将crrent从计算堆栈的顶部弹出并将其存储到索引 1 处的局部变量列表中。
        //        IL_0018: nop
        //        IL_0019: ldloc.1//将current从局部变量列表索引1处推入到计算机堆栈中
        //        IL_001a: call void[mscorlib] System.Console::WriteLine(int32)//输出值
        //        IL_001f: nop
        //        IL_0020: nop

        //        IL_0021: ldloca.s 0////将位于0索引处的枚举器局部变量的地址加载到计算堆栈上。
        //        IL_0023: call instance bool valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>::MoveNext()//调用movenext()返回一个bool值[下一个索引是否存在]
        //        IL_0028: brtrue.s IL_0010//如果返回值为true就跳转回0010
        //    // end loop

        //    IL_002a: leave.s IL_003b//如果返回值为false就退出循环
        //} // end .try
        //finally
        //{
        //    // sequence point: hidden
        //    IL_002c: ldloca.s 0
        //    IL_002e: constrained.valuetype[mscorlib] System.Collections.Generic.List`1/Enumerator<int32>
        //    IL_0034: callvirt instance void[mscorlib] System.IDisposable::Dispose()
        //    IL_0039: nop
        //    IL_003a: endfinally
        //} // end handler
