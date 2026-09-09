#region 空合并赋值运算符

/*
??= 是 C# 8.0 引入的空合并赋值运算符。它的语义是：只有当左侧变量为 null 时，才把右侧的值赋给它；如果左侧不为 null，则保持原值不变。

??= 等价于 if (x == null) x = value;，或者等价于 x = x ?? value;，但写法更紧凑。

它是惰性初始化（Lazy Initialization）模式最直接的语法支持，在需要"只初始化一次"的场景下使用频率很高。


语法结构
// 等价于：if (variable == null) variable = value;
// 也等价于：variable = variable ?? value;
variable ??= value;
// 也等价于
variable = variable is null ? value : variable;
*/

#endregion

#region 基本示例

{
    string? resFromNetwork = null;
    resFromNetwork ??= "default value"; // 如果 resFromNetwork 为 null，则赋值为 "default value"，否则保持原值不变
    Console.WriteLine(resFromNetwork); // 输出: default value

    // 此时resFromNetwork 不为 null，所以不会赋值
    resFromNetwork ??= "actual value";
    Console.WriteLine(resFromNetwork); // 输出: default value

    // 等价写法
    if (resFromNetwork is null) resFromNetwork = "default value";
    // 或者用三元运算符
    resFromNetwork = resFromNetwork is null ? "default value" : resFromNetwork;




}

#endregion

#region 可空值类型与空合并赋值运算符

{
    // 一般值类型不能使用空合并赋值运算符，因为它们不能为 null
    // 对于值类型（基本数值类型、枚举类型、结构体类型等），要使用空合并赋值运算符，必须使用其可空形式
    // T? var = null;
    int? maybeNullInt = null;
    maybeNullInt ??= 10;
    Console.WriteLine(maybeNullInt); // 输出: 10

    maybeNullInt ??= 20;
    Console.WriteLine(maybeNullInt); // 输出: 10

    // 配合方法调用，实现赋值前做一些初始化操作
    static int? ComputeCount()
    {
        // 在这里进行一些初始化操作
        Console.WriteLine("Computing count...");
        return 100;
    }
    int? cachedCount = null;
    // 如果cachedCount已有值的话，就不会再调用ComputeCount()方法
    cachedCount ??= ComputeCount();
    Console.WriteLine(cachedCount); // 输出: 100
}

#endregion


#region 使用在类中，惰性初始化


var resourceManager = new ResourceManager(); // 此时_cache 和 _index 为 null

resourceManager.Add("key1", "value1"); // 第一次访问，触发初始化
resourceManager.Add("key2", "value2"); // 后续访问，直接使用已有实例

Console.WriteLine(resourceManager.Cache.Count); // 输出: 2
Console.WriteLine(resourceManager.Index["key1"]); // 输出: value1
Console.WriteLine(resourceManager.Index["key2"]); // 输出: value2



public class ResourceManager
{
    // 说明一些私有字段，但是在类实例创建时不进行初始化，只有第一次真正需要访问这些字段时，才会进行初始化
    private List<string> _cache;
    private Dictionary<string, string> _index;

    // 创建上述字段对应的属性
    public List<string> Cache
    {
        get
        {
            // 当调用者第一次访问 Cache 属性时
            // _cache 为 null，所以执行 _cache = new(); 创建一个 List<string> 对象
            // 下次访问 Cache 属性时，_cache 不为 null，所以不会执行 _cache = new();，直接返回 _cache
            _cache ??= new();
            return _cache;
        }

        private set;
    }

    public Dictionary<string, string> Index
    {
        get
        {
            _index ??= new();
            return _index;
        }

        private set;
    }

    public void Add(string key, string value)
    {
        // 这里通过属性访问_cache 和 _index，确保已经初始化
        Cache.Add(key);
        Index[key] = value;
    }
}


// 应用场景：按照一定优先级顺序获取配置项
public class AppSetings
{
    private int? _timeout = null;

    private const int DefaultTimeout = 1000;


    // 配置读取顺序：内存缓存 -> 环境变量 -> 硬编码的默认值
    public int Timeout
    {
        get
        {
            if (_timeout is null)
            {
                string? timeoutInEnv = Environment.GetEnvironmentVariable("APP_TIMEOUT");
                _ = int.TryParse(timeoutInEnv, out int tmp);
                _timeout = tmp;
            }

            // 硬编码的默认值
            _timeout ??= DefaultTimeout;

            return _timeout;

        }

        private set;
    }


}

#endregion

#region 总结

/*

Q: ??= 右侧的表达式，在左侧不为 null 时会执行吗？

A: 不会。??= 有短路特性，只有当左侧为 null 时才求值并赋值右侧表达式。
这意味着右侧的方法调用、对象创建等操作只在需要时才执行，这正是"惰性初始化"的核心价值所在。

Q: ??= 是线程安全的吗？

A: 不是。??= 不是原子操作，在多线程并发场景下，可能有多个线程同时判断左侧为 null 并执行初始化，
导致多次初始化。线程安全的惰性初始化应使用 Lazy<T> 或 Interlocked.CompareExchange。

Q: 能对属性使用 ??= 吗？

A: 可以，只要属性有 getter 和 setter：

public string? Name { get; set; }
// ...
someObj.Name ??= "Default";  // 合法，等价于 if (someObj.Name == null) someObj.Name = "Default";

但不能用于只读属性（没有 setter）或 init 属性（只能在初始化器中设置）。


适用场景
1.惰性字段初始化：类中某些字段并非每个实例都需要，用 ??= 在第一次访问时才创建，节省内存。
2.可选参数默认值：方法内处理可能为 null 的参数，param ??= GetDefault(); 是常见写法。
3.配置读取缓存：读取外部配置后缓存到字段，后续调用直接使用缓存值。
4.测试辅助对象：单元测试中，测试辅助对象（mock、stub）的延迟创建。

注意事项
1.??= 只适用于可能为 null 的变量（引用类型或可空值类型 T?）。
对不可空值类型（如 int、struct）使用 ??= 会编译报错。

2.在单线程代码中，??= 是安全且惯用的；在多线程代码中，需要额外的同步机制。

3.??= 不能用于 ref 变量（ref 局部变量、ref 参数）。

4.与 ?? 一样，??= 只检查 null，空字符串 "" 对它来说是非 null 值，不会被替换。
如果需要同时处理 null 和空字符串，仍然需要显式的 string.IsNullOrEmpty 检查。


总结：
??= 是 C# 8 引入的空合并赋值运算符，只在变量为 null 时赋值，非 null 时保持不变。
它是惰性初始化模式的语法糖，比 if (x == null) x = value 更紧凑，比 x = x ?? value 更简洁（左侧只求值一次）。
右侧表达式具有短路特性，只在左侧为 null 时才执行，是"按需初始化"的理想工具。
注意它不是线程安全的，多线程场景需要额外同步。

*/

#endregion