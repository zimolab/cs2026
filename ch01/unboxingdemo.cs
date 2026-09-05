#region C#中的拆箱
/*
拆箱是装箱的逆过程，将装箱后存储在堆上的值类型数据提取出来，复制回值类型变量中。
拆箱必须是显式的，编译器不会自动完成，需要通过强制转换语法完成。

拆箱同样有开销：需要运行时检查 object 中实际存储的类型是否与目标类型匹配，
如果不匹配，抛出 InvalidCastException；如果匹配，从堆上把值复制到栈或目标变量中。
这两步操作（类型检查 + 复制）比直接访问值类型快不了多少，但比不装箱/不拆箱慢很多。

理解装箱和拆箱的完整流程，有助于识别哪些代码在悄悄做无谓的内存分配，进而用泛型等手段消除它们。

拆箱的语法：

// 通过强制类型转换拆箱：将 object 或接口类型显式转换回值类型
// 如果类型不匹配，抛出 InvalidCastException
值类型 变量名 = (值类型)boxedObject;

// 用模式匹配拆箱（C# 7+ 推荐写法，类型不匹配时安全处理）
// 如果类型不匹配，不会抛出异常，而是跳过if语句块
if (boxedObject is int n)
{
    // 拆箱成功，n 持有值类型的值
}
*/
#endregion

#region 拆箱示例
int orginalValue = 10;
// 先装箱，可以看到装箱是隐式的，没有明显的语法标志
// 装箱：int -> object（堆上分配）
object boxedValue = orginalValue;

// 拆箱：object -> int（从堆上复制到栈）
// 拆箱有明确的语法标志，比如这里的强制类型转换
int unboxedValue = (int)boxedValue;

// 拆箱的值是独立的副本，修改不影响装箱对象
unboxedValue = 20;
Console.WriteLine($"boxedValue: {boxedValue}");
Console.WriteLine($"unboxedValue: {unboxedValue}");


// 类型不匹配拆箱抛出异常
object boxedValue2 = 1;
try
{
    // boxedValue2 实际上是 int 类型
    // 拆箱时类型不匹配，抛出 InvalidCastException
    double unboxedValue2 = (double)boxedValue2;
}
catch (InvalidCastException ex)
{
    // 输出：InvalidCastException: Unable to cast object of type 'System.Int32' to type 'System.Double'.
    Console.WriteLine($"InvalidCastException: {ex.Message}");
}
// 正确的做法是先拆箱到正确类型，再进行类型转换
// 当然由于int类型可以隐式转换为double类型，
// 所以这里使用(double)进行强制类型转换也是可以的
double unboxedValue2Correct = (double)(int)boxedValue2;

// 模式匹配拆箱（C# 7+ 推荐写法）
// 一个包含了各种类型的数组
object[] items = [10, 1.0, 10.5F, true, 10000000000000UL, "Hello", "Hello World!", 'A', System.DateTime.Now];
foreach (var item in items)
{
    // 使用 is Type n 可以安全地进行类型匹配
    // is Type n实际上包含两个步骤：类型检查 + 拆箱/绑定
    if (item is int n)
    {
        Console.WriteLine($"int: {n}");
    }
    else if (item is double d)
    {
        Console.WriteLine($"double: {d}");
    }
    else if (item is float f)
    {
        Console.WriteLine($"float: {f}");
    }
    else if (item is bool b)
    {
        Console.WriteLine($"bool: {b}");
    }
    else if (item is ulong ul)
    {
        Console.WriteLine($"long: {ul}");
    }
    else if (item is string s)
    {
        Console.WriteLine($"string: {s}");
    }
    else if (item is char c)
    {
        Console.WriteLine($"char: {c}");
    }
    else if (item is System.DateTime dt)
    {
        Console.WriteLine($"DateTime: {dt}");
    }
    else
    {
        Console.WriteLine($"Unknown type: {item.GetType().Name}");
    }

    // switch表达式中的拆箱，最简洁现代的写法
    // 注意switch 表达式不能用在返回 void 的方法体中
    static string ProcessValue(object value) => value switch
    {

        int n when n > 0 => "positive int: {n}",
        // 不仅可以做类型匹配，还可以做更精细的条件判断，比如检查类型为int且大于0
        int n => $"int: {n}",
        double d => $"double: {d}",
        float f => $"float: {f}",
        bool b => $"bool: {b}",
        ulong ul => $"positive long: {ul}",
        // 检查类型为string且长度大于5
        string s when s.Length > 5 => $"long string: {s}",
        string s => $"string: {s}",
        char c => $"char: {c}",
        _ => $"Unknown type: {value.GetType().Name}"

    };

    foreach (var obj in items)
    {
        Console.WriteLine($"ProcessValue: {ProcessValue(obj)}");
    }

    // 同样，使用switch语句也可以进行描述匹配拆箱
    static void ProcessValue2(object value)
    {
        switch (value)
        {
            case int n when n > 0:
                Console.WriteLine($"positive int: {n}");
                break;
            case int n:
                Console.WriteLine($"int: {n}");
                break;
            case double d:
                Console.WriteLine($"double: {d}");
                break;
            case float f:
                Console.WriteLine($"float: {f}");
                break;
            case bool b:
                Console.WriteLine($"bool: {b}");
                break;
            case ulong ul:
                Console.WriteLine($"positive long: {ul}");
                break;
            case string s when s.Length > 5:
                Console.WriteLine($"long string: {s}");
                break;
            case string s:
                Console.WriteLine($"string: {s}");
                break;
            case char c:
                Console.WriteLine($"char: {c}");
                break;
            default:
                Console.WriteLine($"Unknown type: {value.GetType().Name}");
                break;
        }
    }
    foreach (var obj2 in items)
    {
        ProcessValue2(obj2);
    }
}
#endregion

#region 
{
    // 出于性能的考虑，如可以使用泛型集合的地方尽量不要使用老式object通用集合类型
    // 以避免装箱、拆箱带来的性能开销

    const int Count = 1_000_000;

    // 旧式：每次 Add 装箱，每次访问拆箱，产生大量 GC 压力
    var arrayList = new System.Collections.ArrayList(Count);
    for (int i = 0; i < Count; i++)
        arrayList.Add(i);            // 装箱

    int sum1 = 0;
    for (int i = 0; i < Count; i++)
        sum1 += (int)arrayList[i];   // 拆箱

    // 现代：泛型 List，零装箱零拆箱
    var genericList = new List<int>(Count);
    for (int i = 0; i < Count; i++)
        genericList.Add(i);          // 无装箱

    int sum2 = 0;
    for (int i = 0; i < Count; i++)
        sum2 += genericList[i];      // 无拆箱，直接读取 int

    Console.WriteLine($"sum1={sum1}, sum2={sum2}");  // 两者结果相同，但性能差异巨大

}
#endregion

#region 

/* 
常见问题

1.拆箱后修改变量，装箱的对象会变吗？
不会。拆箱时从堆上把值复制到新变量中，之后两者完全独立。
这和装箱时的逻辑一样，装箱和拆箱都是复制操作，堆上的对象和栈上的变量各自独立。

2.能拆箱为与装箱时不同但兼容的类型吗？
拆箱的类型必须与装箱时的原始类型精确匹配，不支持隐式数值转换：

object o = 42;（装的是 int）
(long)o → InvalidCastException（不能直接拆箱为 long）
(long)(int)o → 正确（先拆为 int，再隐式转为 long）

枚举类型与其底层整型之间也遵循此规则：
(int)(object)myEnum 是对的，
(MyEnum)(object)42 也是对的，但两者之间不能直接互换。

3.is 检查类型后访问是否算拆箱？
if (obj is int n) 这个模式匹配会进行类型检查，
如果匹配，n 得到的是值类型的副本，这个过程本质上也是拆箱。
只是相比 (int)obj，模式匹配的写法类型不匹配时不抛异常，更安全。

适用场景

拆箱主要出现在以下场景（多数情况下应该用泛型来避免它）：
- 处理非泛型集合返回的数据：旧代码中的 ArrayList、Hashtable 等返回 object，读取时必须拆箱。
- 反射操作：MethodInfo.Invoke() 返回 object?，调用值类型方法时需要拆箱。
- 动态类型处理：接收 object 参数的通用处理函数，用模式匹配安全拆箱。

注意事项

拆箱时必须知道 object 中装的是什么类型，类型不精确匹配会导致运行时异常。
在类型不确定的场景，优先使用 is 模式匹配代替强制转换。

可空值类型（int?）的装箱拆箱有特殊规则：
装箱时，null 的 int? 被装箱为 null 引用，有值的 int? 被装箱为 int（不是 Nullable<int>）。
拆箱时，需要根据具体情况决定目标类型是 int 还是 int?。

频繁的装箱/拆箱是 .NET 性能问题的常见原因，
可以用 Visual Studio 的性能分析工具（Profiler）发现 GC 压力高的地方，通常与装箱密切相关。

在使用 Dictionary<string, object> 存储混合类型值的场景（如动态配置），读取值时涉及拆箱，应注意类型安全和异常处理。

总结
拆箱是装箱的逆操作，将存在堆上的装箱对象中的值类型数据提取回来。
拆箱是显式操作，必须精确指定目标值类型，类型不匹配抛 InvalidCastException。
现代 C# 推荐用 is 模式匹配代替强制转换做安全拆箱。
装箱和拆箱的根本解决方案是泛型，使用泛型集合和泛型方法彻底消除不必要的装箱/拆箱，提升性能并保持类型安全。
理解装箱/拆箱的完整机制，是优化 C# 代码性能的基础知识。
*/
#endregion