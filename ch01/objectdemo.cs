// 演示C#中的object类型

#region object类型简介

// object（对应 System.Object）是 C# 类型系统的根类型，所有类型——无论是值类型还是引用类型，无论是内置类型还是自定义类型——都直接或间接继承自 object。
// 这意味着任何类型的值都可以赋给 object 类型的变量（所谓的多态性）。这个特性是 C# 类型统一性的基础，使得可以写出处理任意类型的通用代码。
// 但在泛型普及之前，object 经常被滥用作为"通用容器"，带来了频繁的装箱/拆箱开销和运行时类型错误。
// 现在，绝大多数"存储任意类型"的需求应该用泛型（T）来满足，保留类型安全和性能。
// object 提供了三个所有类型都继承的基本方法：Equals()（相等比较）、GetHashCode()（哈希值）、ToString()（字符串表示）。

// 基本语法

/*

// 声明 object 变量
object 变量名 = 任意值;

// 类型检查
if (变量名 is 类型名 局部变量名) { ... }

// 类型转换
类型名 变量名 = (类型名)object变量;    // 强制转换，失败抛异常
类型名? 变量名 = object变量 as 类型名; // as 转换，失败返回 null

*/
#endregion


#region 基本示例
// object类型可以存储任意类型的值

// 存储值类型时，会发生装箱
object intObj = 123;
object floatObj = 3.14f;
object boolObj = true;

// 存储引用类型时，不会发生装箱，直接存储引用
object stringObj = "Hello, World!";
object listObj = new List<int> { 1, 2, 3 };
object dictObj = new Dictionary<string, int> { { "key1", 1 }, { "key2", 2 } };
object datetimeObj = DateTime.Now;

// object类型的共同方法
Console.WriteLine(intObj.ToString());
Console.WriteLine(intObj.Equals(123)); // True
Console.WriteLine(intObj.GetHashCode());
Console.WriteLine(intObj.GetType()); // System.Int32

#endregion


#region 类型检查和转换
// 创建一个object数组，数组元素指定为object类型，也就是可以存储任意类型的值
object[] objArray = [42, "Hello", 3.14f, new List<int>() { 1, 2, 3 }];

// 遍历数组，检查每个元素的类型
foreach (object obj in objArray)
{
    // 使用C#7引入的is模式匹配：is模式匹配可以检查对象是否是某个类型，如果是，则将其绑定到一个该类类型的局部变量
    if (obj is int intVal)
    {
        Console.WriteLine($"int: {intVal}");
    }
    else if (obj is string strVal)
    {
        Console.WriteLine($"string: {strVal}");
    }
    else if (obj is float floatVal)
    {
        Console.WriteLine($"float: {floatVal}");
    }
    else if (obj is List<int> listVal)
    {
        Console.WriteLine($"List<int>: {string.Join(", ", listVal)}");
    }
    else
    {
        Console.WriteLine($"Unknown type: {obj.GetType()}");
    }
}

// 使用switch进行模式匹配可以更加简洁
foreach (object obj in objArray)
{
    switch (obj)
    {
        case int intVal:
            Console.WriteLine($"int: {intVal}");
            break;
        case string strVal:
            Console.WriteLine($"string: {strVal}");
            break;
        case float floatVal:
            Console.WriteLine($"float: {floatVal}");
            break;
        case List<int> listVal:
            Console.WriteLine($"List<int>: {string.Join(", ", listVal)}");
            break;
        default:
            Console.WriteLine($"Unknown type: {obj.GetType()}");
            break;
    }
}

// 使用as进行安全转换，如果转换失败，返回null
object obj2 = "Hello, World!";

string? strVal2 = obj2 as string;
Console.WriteLine($"strVal2={strVal2}");

List<int>? listVal2 = obj2 as List<int>; // as转换失败，返回null
Console.WriteLine($"listVal2={listVal2}");

// 检查并绑定（最推荐的现代写法）
if (obj2 is string strVal3)
{
    Console.WriteLine($"strVal3={strVal3}");
}

// 强制类型转换，失败抛异常，需要手动捕获异常
try
{
    int intVal3 = (int)obj2; // 强制转换失败，抛异常
}
catch (InvalidCastException ex)
{
    Console.WriteLine($"InvalidCastException: {ex.Message}"); // InvalidCastException: Unable to cast object of type 'System.String' to type 'System.Int32'.
}

// 精确类型匹配，不包含子类
if (obj2.GetType() == typeof(string))
{
    string strVal4 = (string)obj2;
    Console.WriteLine($"strVal4={strVal4}");
}

#endregion

#region 总结

// 常见问题
// object.Equals() 和 == 有什么区别？
// 对于引用类型，默认情况下 == 比较引用（是否是同一个对象），object.Equals() 也比较引用。
// 但很多类型（如 string、record、以及自定义覆盖了 Equals 的类）覆盖了这两者以实现值比较。
// 判断规则：检查该类型是否覆盖了 Equals/==，不要凭感觉假设。

// 所有类型都继承自 object，那值类型（如 int）也继承自 object 吗？
// 是的。int 作为值类型，其对应的 System.Int32 继承自 System.ValueType，
// System.ValueType 再继承自 System.Object。所以 int 可以赋给 object 变量，
// 但这个过程会发生装箱（堆分配），不是免费的。

// is 和 as 有什么优劣，现在该用哪个？
// 现代 C# 推荐用 is 模式匹配（if (obj is string s)），它一步完成类型检查和绑定，比先 as 再判空更简洁，也支持更复杂的模式（属性模式、位置模式等）。
// as 在需要"不绑定变量，只转换"或者在方法返回值中使用时仍然有价值。

// 适用场景
// 框架和基础库的通用接口：如 Dictionary<string, object> 用于传递不同类型的元数据（虽然现在也可以用 dynamic 或泛型替代）。
// 覆盖基类方法：ToString()、Equals()、GetHashCode() 在所有类中都可以覆盖。
// 反射和序列化：object 作为参数类型出现在很多反射 API 中，如 Activator.CreateInstance() 返回 object?。
// 日志框架：日志方法通常接收 object? 参数，可以传入任何类型的值。

// 注意事项
// 不要把 object 当作"万能集合"使用，这是前泛型时代的做法。现代 C# 中几乎所有"存储任意类型"的需求都应该用泛型。
// is 类型模式检查包含继承关系（obj is Animal 对 Dog 实例为 true），GetType() == typeof(Animal) 则要求精确类型匹配（Dog 实例不匹配 Animal）。
// 覆盖 Equals() 时必须同时覆盖 GetHashCode()，否则在字典和哈希集合中行为不一致（逻辑相等的对象可能映射到不同的哈希桶）。
// as 运算符只能用于引用类型或可空值类型，不能对不可空值类型使用（如 obj as int 不合法，应用 obj as int? 或模式匹配）。

// 总结
// object 是 C# 类型系统的根，所有类型都继承自它，这是 C# 类型统一性的基础。
// 它提供了 ToString()、Equals()、GetHashCode() 三个基础方法，所有类型都可以覆盖它们自定义行为。
// 类型检查和转换推荐使用现代的 is 模式匹配写法，比旧的强制转换和 as 更清晰安全。
// 在新代码中，用泛型代替 object 作为通用容器，保留类型安全和避免装箱开销。

#endregion