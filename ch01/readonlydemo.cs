namespace readonlydemo;

// 演示C#中的readonly字段

// 与const不同，const是看起来像变量的字面量，实际上没有内存地址
// 而readonly字段则是看起来像常量的变量，和变量一样，会实际分配内存地址，
// 但是行为又像“常量”一样，值一旦确定就不能对其进行修改。

// 与 const 不同，readonly 的值可以在运行时确定——在字段声明处赋值，或者在构造函数中赋值，
// 之后无论任何地方都不能修改。这使得 readonly 适合那些在对象构建完成后就不应该改变的值，
// 比如通过依赖注入传入的服务实例、由外部配置读取的设置等。

// readonly 可以用于实例字段和静态字段（static readonly）。
// 静态字段在静态构造函数中赋值，实例字段在实例构造函数中赋值。
// readonly 没有类型限制，任意类型（包括引用类型和自定义 struct）都可以声明为 readonly。

// 注意：readonly 只保证字段引用本身不变，对于可变（mutable）引用类型，
// 仍然可以修改对象的内部状态（比如 readonly List<int> 不能替换为其他列表，但可以调用 Add() 修改列表内容）。

// 语法结构:

// 实例 readonly 字段
// [访问修饰符] readonly 类型 字段名;
// [访问修饰符] readonly 类型 字段名 = 初始值;

// 静态 readonly 字段
// [访问修饰符] static readonly 类型 字段名;
// [访问修饰符] static readonly 类型 字段名 = 初始值;

// 实例readonly字段（模拟依赖注入的场景）
internal class OrderProcessor
{


    // readonly 字段：只能在声明处或构造函数中赋值
    // 外部无法修改，内部在构造后也无法修改

    // TIPS:  Action<string>表示一个无返回值、接受一个 string 参数的委托（函数）
    private readonly Action<string> _logFunc; // 可以看到，readonly字段的类型可以是任何类型

    private readonly ulong _orderId;
    private readonly string _region;

    // 当可变引用类型作为 readonly 字段时，需要注意的点：
    // 1. 不可以通过赋值改变该字段的引用
    // 2. 无法保证该字段引用的对象状态不被修改
    private readonly List<string> _items;


    public OrderProcessor(Action<string> logFunc, ulong orderId, string region = "default", List<string>? items = null)
    {
        // 在构造函数中将值赋给 readonly 字段
        // 一旦赋值，就无法修改字段的值
        _logFunc = logFunc;
        _orderId = orderId;
        _region = region;
        // 如果 items 为 null，则初始化一个空的 List<string>
        if (items == null)
        {
            _items = new();
        }
        else
        {
            _items = new List<string>(items); // 如果 items 不为 null，则直接创建一个副本
        }

    }

    public string[] GetItems()
    {
        // 这里使用了列表表达式
        return [.. _items]; // 返回Array而不是List，防止外部修改
    }

    public void AddItem(string item)
    {
        // 尽管 _items 是 readonly 的，但其内部状态依然可变
        // 比如通过Add方法添加元素
        if (!_items.Contains(item))
        {
            _items.Add(item);
        }
    }

    public string? RemoveItem(string item)
    {
        // 尽管 _items 是 readonly 的，但其内部状态依然可变
        // 比如通过Remove方法移除元素
        if (!_items.Contains(item))
        {
            return null;
        }
        _items.Remove(item);
        return item;
    }

    public void ListItems()
    {
        if (_items.Count == 0)
        {
            _logFunc($"No items for order {_orderId}");
            return;
        }
        _logFunc($"Listing items for order {_orderId}: {_items.Count} items");
        foreach (var item in _items)
        {
            _logFunc($"- {item}");
        }
    }


    public void ProcessOrder()
    {
        _logFunc($"Processing order {_orderId} in region {_region}. This oreder has {_items.Count} items.");
    }

}




internal class Program
{
    static void Main(string[] args)
    {
        // 创建 OrderProcessor 对象
        // 将需要的依赖通过构造函数注入
        var orderProcessor = new OrderProcessor(
            logFunc: msg => Console.WriteLine($"[OrderProcessor]({DateTime.Now}) {msg}"),
            orderId: 12345,
            region: "North America"
        );
        orderProcessor.ProcessOrder();
        orderProcessor.AddItem("Apple");
        orderProcessor.AddItem("Banana");
        orderProcessor.AddItem("Cherry");
        orderProcessor.ListItems();
        orderProcessor.RemoveItem("Apple");
        orderProcessor.ListItems();
        orderProcessor.ProcessOrder();
    }
}