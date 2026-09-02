#region C#中值类型的装箱

/* 概念说明：
装箱（Boxing）是将值类型转换为引用类型的过程。
当把一个值类型（如 int、struct）赋给 object 类型变量，或者赋给该值类型实现的接口类型变量时，就会发生装箱。

装箱的具体过程：在堆上分配一块内存，将值类型的数据复制到这块内存，返回指向该内存的引用。
之后，这个对象就像普通的引用类型对象一样由 GC 管理。

装箱是有开销的操作：
- 在堆上分配内存（触发 GC 工作）
- 复制值类型的数据到堆
- 产生额外的对象供 GC 追踪和回收

在性能敏感的路径上（如高频调用的循环、实时系统），频繁装箱是需要优化的问题。
泛型的出现正是为了避免这种不必要的装箱。

+--------------------------+--------------------------------+----------------------------------+
| 场景                     | 示例                           | 说明                             
+------------------------- +--------------------------------+----------------------------------+
| 赋给object               | object o = 42;                 | 最常见的装箱                       
| 赋给接口                 | IComparable c = 42;            | 值类型实现的接口                    
| 添加元素到非泛型集合      | ArrayList.Add(42)              | 旧式非泛型集合                     
| 字符串格式化（旧写法）    |string.Format("{0}", 42)        | 参数是 object[]                    
| 反射                     | typeof(int).GetMethod("...")   | 反射操作常涉及装箱                 
+--------------------------+--------------------------------+----------------------------------+

*/


/*

装箱过程图示（以 int i = 123; object o = i; 为例）
初始状态：值类型变量 i 存在于线程栈上

 栈 (Stack)
┌─────────────┐
│  i          │   值: 123
│  (int)      │
└─────────────┘

Step 1：执行 object o = i; 触发装箱
运行时会：
1.在托管堆中分配一块内存，大小等于 int 加上对象头（这里简化忽略对象头）。
2.将 i 的值 123 复制到这块内存中。

 栈 (Stack)                    堆 (Heap)
┌─────────────┐              ┌──────────────────┐
│  i          │   值: 123    │  新分配的 object │
│  (int)      │              │  (System.Int32)  │
└─────────────┘              │  值: 123         │
                             └──────────────────┘
Step 2：将堆上对象的地址赋给栈上的引用变量 o
现在 o 指向堆中的那个装箱对象，而 i 仍然独立存在于栈上。

栈 (Stack)                    堆 (Heap)
┌─────────────┐              ┌──────────────────┐
│  i          │   值: 123    │  System.Int32    │
│  (int)      │              │  值: 123         │
├─────────────┤              └────────▲─────────┘
│  o          │  引用 ────────────────┘
│  (object)   │
└─────────────┘

最终状态
i 依然是值类型，独立持有 123。
o 是一个引用类型变量，指向堆上的装箱对象，该对象包含了原始值的副本。


 栈                              堆
┌───────┐                    ┌─────────────┐
│  i    │  123               │  装箱对象   │
│ (int) │                    │  值: 123    │
├───────┤                    └──────▲──────┘
│  o    │  ─────────────────────────┘
│(obj)  │
└───────┘

*/

#endregion

#region 基本示例
{
    int value = 42; // 局部变量的情况下，值类型在栈上

    // 装箱：将值类型装箱赋值给object变量
    // 此时将在堆上分配一块内存，将值类型的数据复制到这块内存，
    // 并返回指向该内存的引用给object变量。
    object boxedValue = value; // 将值类型装箱为引用类型
    // 验证：boxedValue的实际类型为System.Int32
    Console.WriteLine(boxedValue.GetType()); // 输出：System.Int32

    // 装箱后的值是独立副本，修改原变量不影响装箱对象
    value = 100;
    Console.WriteLine(boxedValue); // 输出：42

    // 装箱：装箱到接口类型
    IComparable comparableValue = value; // int类型实现IComparable接口
    // 验证：comparableValue的实际类型为System.Int32
    Console.WriteLine(comparableValue.GetType()); // 输出：System.Int32
    // 同时也可以通过装箱对象调用该接口的方法
    Console.WriteLine(comparableValue.CompareTo(100)); // 输出：-1

    // 旧式非泛型集合由于要兼容不同的类型，因此其元素的值被设定为object类型
    // 因此在添加元素时会发生装箱，在添加大量值的情况下会产生性能问题
    // 因为装箱是有开销的操作，包括：分配内存、复制数据、产生额外的对象供GC追踪和回收
    var arrayList = new System.Collections.ArrayList();
    for (int i = 0; i < 10000; i++)
    {
        arrayList.Add(i); // 每次添加元素都会发生装箱
    }
    // 应当使用泛型集合来避免不必要的装箱，当然前提是元素的类型得是统一的
    var genericList = new List<int>();
    for (int i = 0; i < 10000; i++)
    {
        genericList.Add(i); // 每次添加元素不会发生装箱，性能更好
    }

    // 老式的格式化字符串也会发生装箱，因为参数是object[]
    int count = 100;
    double sum = 200.5;
    string oldFormattedString = string.Format("Count: {0}, Sum: {1}", count, sum);

    // 推荐使用字符串插值，编译器会优化，某些情况下会避免不必要的装箱
    string newFormattedString = $"Count: {count}, Sum: {sum}";

    // 利用泛型避免装箱

    // 不好的做法，使用object类型作为参数
    static void BadPrintValue(object value)
    {
        Console.WriteLine(value);
    }

    // 好的做法，使用泛型
    static void GoodPrintValue<T>(T value)
    {
        Console.WriteLine(value);
    }

    BadPrintValue(42); // 发生装箱
    GoodPrintValue(42); // 不发生装箱

    // 使用泛型接口避免装箱
    static int CompareGeneric<T>(T a, T b) where T : IComparable<T>
    {
        // IComparable<T> 是泛型接口，实现它的值类型不需要装箱就能调用
        return a.CompareTo(b); // 不发生装箱
    }
    int result = CompareGeneric(42, 100); // 不发生装箱
    Console.WriteLine(result); // 输出：-1
}
#endregion

#region 总结

/*常见问题：
结构体装箱后，还能修改结构体的字段吗？
不能直接修改。装箱后的对象看起来是引用类型，
但没有暴露修改内部字段的接口。
如果要修改，需要先拆箱到局部变量，修改局部变量，再重新装箱。
不过如果你在写这样的代码，通常意味着设计上有问题，应该用 class 代替。
*/

/*
适用场景

装箱本身不是"好的"操作，而是一个需要理解其发生时机的机制：
- 有意为之的 object 接口：某些需要存储任意类型的场景（反射、动态配置），
装箱是不可避免的，这时使用 object 是合理的。

- 旧代码的 API 兼容：旧式 API（ArrayList、非泛型 IEnumerable）基于 object，
使用时装箱不可避免，在新代码中应该迁移到泛型版本。

需要避免装箱的场景：
- 高性能循环和实时代码：游戏循环、数据处理管道、网络协议解析等高频路径。
- 大量数值类型存储：用泛型集合（List<int>）代替非泛型集合（ArrayList）。


注意事项
- 装箱是隐式发生的，没有明显的语法标记，需要有意识地识别触发点（赋给 object、非泛型集合、非泛型接口参数等）。
- 判断代码是否有装箱，可以用工具查看生成的 IL 代码,
装箱对应 box 指令，拆箱对应 unbox/unbox.any 指令。
Visual Studio 的"反汇编"窗口或 SharpLab（sharplab.io）可以查看 IL。
- Span<T> 和 Memory<T> 等高性能类型完全避免了装箱，
是现代 .NET 高性能代码的核心工具。
- 枚举类型（enum）也是值类型，赋给 object 时会装箱。
如果代码中大量使用枚举作为字典键，用 Dictionary<TEnum, TValue> 
而非 Dictionary<object, TValue>，前者无装箱。

总结
装箱是值类型到引用类型（object 或接口）的隐式转换，过程是将值复制到堆上的新对象中。
它有实际的性能代价：堆分配、GC 压力、内存占用。现代 C# 代码应该用泛型来避免不必要的装箱，如：
- 使用List<T> 代替 ArrayList，
- 使用泛型方法 void Print<T>(T value) 代替 void Print(object value)，
- 字符串插值 $"..." 代替 string.Format。

理解装箱发生的时机，是写出高性能 C# 代码的基础知识点之一。

*/

#endregion