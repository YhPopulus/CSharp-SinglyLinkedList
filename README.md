---
tags: [Import-0778]
title: SinglyLinkedList
created: '2019-09-04T07:45:12.394Z'
modified: '2019-09-05T09:49:47.893Z'
---

# SinglyLinkedList<T>
## 构造函数(重载)

|构造|定义|
|:---:|:---:|
|SinglyLinkedList\<T\>()|初始化为空的 SinglyLinkedList<T> 类的新实例。|
|SinglyLinkedList<T>(IEnumerable\<T\>)|	初始化 SinglyLinkedList<T> 类的新实例，该实例包含从指定的 IEnumerable 中复制的元素并且其容量足以容纳所复制的元素数。|
## 属性
* Count
* First
* ICollection.IsSynchronized
* ICollection.SyncRoot
* ICollection.IsReadOnly
* Last
## 方法
0. Add(T item)
1. Clear()
2. Contains(T item)
3. FindItem(T item)
4. CopyTo(T[] array,int arrayIndex)
5. Remove(T item)
6. Remove(SinglyListNode<T> node)
## 接口实现
* ICollection.CopyTo
* ICollection\<T\>.Add
* IEnumerable.GetEnumerator
* IEnumerable\<T\>.GetEnumerator

## **`Tips!`**
>了解一下枚举器的迭代原理。
>>foreach关键字的IL Code
     
    整体的遍历过程就相当于while(object.MoveNext)
    IL_0000: nop
    IL_0001: nop
    IL_0002: ldarg.0
    IL_0003: ldfld class [mscorlib] System.Collections.Generic.List`1<int32> Program::list//将list对象引用的值进行入栈操作。
    IL_0008: callvirt instance valuetype[mscorlib] System.Collections.Generic.List`1/Enumerator<!0> class [mscorlib] System.Collections.Generic.List`1<int32>::GetEnumerator()
    //调用list中实现的GetEnumerator()方法获得枚举对象。
    IL_000d: stloc.0//将[枚举对象]从计算堆栈的顶部弹出并将其存储到索引 0 处的局部变量列表中。
    .try
    {
        // sequence point: hidden
        IL_000e: br.s IL_0021//进入循环跳转到0021
        // loop start (head: IL_0021)
            IL_0010: ldloca.s 0//将位于0索引处的枚举器局部变量的地址加载到计算堆栈上。
            IL_0012: call instance !0 valuetype[mscorlib] System.Collections.Generic.List`1/Enumerator<int32>::get_Current()//调用枚举器中的GetCurrent方法返回枚举器当前对象
            IL_0017: stloc.1//将crrent从计算堆栈的顶部弹出并将其存储到索引 1 处的局部变量列表中。
            IL_0018: nop
            IL_0019: ldloc.1//将current从局部变量列表索引1处推入到计算机堆栈中
            IL_001a: call void[mscorlib] System.Console::WriteLine(int32)//输出值
            IL_001f: nop
            IL_0020: nop

            IL_0021: ldloca.s 0////将位于0索引处的枚举器局部变量的地址加载到计算堆栈上。
            IL_0023: call instance bool valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>::MoveNext()//调用movenext()返回一个bool值[下一个索引是否存在]
            IL_0028: brtrue.s IL_0010//如果返回值为true就跳转回0010
        // end loop
        IL_002a: leave.s IL_003b//如果返回值为false就退出循环
    } // end .try

