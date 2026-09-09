#region 

/*

空条件访问运算符 ?.（也叫 null 条件运算符或安全导航运算符）在访问对象的成员之前，先检查对象是否为 null。
如果对象为 null，则整个表达式返回 null 而不是抛出 NullReferenceException；如果不为 null，则正常访问成员。

这个运算符在 C# 6 引入，解决了长期以来"访问对象链式成员前，每一步都要判 null"的冗长写法问题。
它使 null 安全的链式调用成为可能，与 ?? 配合是处理可能为 null 的对象访问的黄金组合。

语法结构
// 访问属性
result = obj?.Property;         // obj 为 null 时返回 null，否则返回属性值

// 调用方法
result = obj?.Method();         // obj 为 null 时返回 null，否则调用方法

// 链式访问（每个 ?. 都做 null 检查）
result = a?.B?.C?.D;

// 与 ?? 组合（提供最终默认值）
result = obj?.Property ?? defaultValue;

*/

#endregion


#region 基本示例

{
    // 解决的基本问题，防止对象为null引发NullReferenceException
    string? maybeNull = null;

    // 如果不对对象作null检查，直接访问对象属性，在对象为null时会引发NullReferenceException
    try
    {
        var strLength = maybeNull.Length; // 这里会引发NullReferenceException
    }
    catch (NullReferenceException)
    {
        Console.WriteLine("对象为null，引发NullReferenceException!");
    }

    // 一般的做法，先检查对象是否为null，再访问对象属性
    var strLength2 = -1;
    if (maybeNull is not null)
    {
        strLength2 = maybeNull.Length; // 这里不会引发NullReferenceException
    }
    Console.WriteLine($"strLength: {strLength2}"); // 输出: strLength: -1

    // 使用空条件访问运算符 ?.，可以简化上述代码
    int? strLength3 = maybeNull?.Length; // 这里不会引发NullReferenceException
    Console.WriteLine($"strLength==null: {strLength3 is null}"); // 输出: strLength==null: True

    // 由于?.在对象为null时返回null，导致strLength3的类型是int?，而不是int
    // 如果我们需要在对象为null将strLength变量的值设置为-1，可以配件使用 ?? 运算符使用
    int strLength4 = maybeNull?.Length ?? -1;
    Console.WriteLine($"strLength4: {strLength4}"); // 输出: strLength4: -1
}

#endregion 通过?.链式调用简化判空
{
    Person? person = null;

    // 假设我们要安全地获取person对象Address属性中的Street属性s
    // 传统的做法要使用if-else进行多次null检查
    string? street = null;
    // 首先判断person本身是否为null
    if (person is not null)
    {
        var address = person.Address;
        // 然后判断address是否为null
        if (address is not null)
        {
            street = address.Street;
        }
    }

    // 使用?.链式调用可以简化上述代码
    string? street2 = person?.Address?.Street;

    // 如果我们需要一个默认值还可以和??结合使用
    string street3 = person?.Address?.Street ?? "Unknown Street";
    Console.WriteLine($"street3: {street3}"); // 输出: street3: Unknown Street
}


#region 通过?.调用方法

{
    string? maybeNullStr = null;

    string? upperStr = maybeNullStr?.ToUpper(); // 如果maybeNullStr为null，upperStr也会为null，否则调用ToUpper方法得到大写字符串
    Console.WriteLine($"upperStr == null: {upperStr is null}"); // 输出: upperStr: True



    maybeNullStr = "Hello, World!";
    upperStr = maybeNullStr?.ToUpper(); // 如果maybeNullStr不为null，upperStr也会不为null，否则调用ToUpper方法得到大写字符串
    Console.WriteLine($"upperStr: {upperStr}"); // 输出: upperStr: HELLO, WORLD!
}

#endregion


#region ?. 与值类型（返回可空值类型）

{
    // 当 ?. 应用于返回值类型的成员时，结果变为可空值类型（T?）
    Person person = new();
    // 虽然Address类定义时City属性是string类型，但使用?.访问city的类型变为string?
    string? city = person.Address?.City;
}

#endregion


#region ?.调用委托和事件

{
    // ?. 也可以用于调用委托和事件
    Action? action = null;
    // 老式写法，调用前先检查委托是否为null
    if (action is not null)
    {
        action();
    }

    // 使用?.简化代码（而且是线程安全的事件触发）
    action?.Invoke(); // 如果为null，则什么都不做

    // 绑定一个委托，测试一下
    static void onActionCalled() => Console.WriteLine("Action called!");


    action += onActionCalled;
    action?.Invoke(); // 输出: Action invoked!

    action -= onActionCalled; // 移除委托
    action?.Invoke(); // 输出: (无输出，因为委托已经被移除)

    // 事件触发中的标准用法（线程安全）
    DataManager dataManager = new(10);
    DataObserver observer = new();
    // 绑定事件
    dataManager.DataChanged += observer.OnDataChanged;
    // 调用方法触发事件，observer会收到通知
    dataManager.Increse(5); // 输出: Data changed from 10 to 15
    dataManager.Decrease(3); // 输出: Data changed from 15 to 12


}

#endregion


#region 自定义类型定义

class Address
{
    public string? Street;
    public string City = "Unknown City";
}

class Person
{
    public string? Name;
    public int? Age;
    public Address? Address;
}


class DataEventArgs(int before, int after) : EventArgs
{
    public int Before { get; } = before;
    public int After { get; } = after;


}

class DataManager(int initData = 0)
{
    public event EventHandler? DataChanged;

    private int _data = initData;

    public void Increse(int amount = 1)
    {
        var oldData = _data;
        _data += amount;
        // 触发事件，通知观察者数据变化
        DataChanged?.Invoke(this, new DataEventArgs(oldData, _data));
    }

    public void Decrease(int amount = 1)
    {
        var oldData = _data;
        _data -= amount;
        // 触发事件，通知观察者数据变化
        DataChanged?.Invoke(this, new DataEventArgs(oldData, _data));
    }

}

class DataObserver
{
    public void OnDataChanged(object? sender, EventArgs e)
    {
        if (e is DataEventArgs args)
        {
            Console.WriteLine($"Data changed from {args.Before} to {args.After}");
        }
    }
}


#endregion

#region 总结

/*

Q: ?. 和先判 null 再访问（if (obj != null)）的区别是什么？
A: 两者语义完全等价，?. 是语法糖。区别仅在于简洁性：
链式访问（如 a?.B?.C?.D?.E）用 ?. 比逐层 if 判断简洁得多。
在性能上两者没有差别，编译器会生成等价的代码。

Q: 为什么 ?. 访问值类型属性后结果变成了 int? 而不是 int？
A: 因为整个 ?. 链式表达式可能返回 null（当左侧为 null 时），而 int 不能持有 null。
所以编译器自动将结果类型包装为 int?（即 Nullable<int>）。
如果需要确定的 int，用 ?? 0 提供默认值：obj?.Count ?? 0。

Q: ?. 是线程安全的吗？
A: 在事件触发场景中，eventField?.Invoke(...) 比 if (eventField != null) eventField(...) 更安全。
后者在 if 检查通过后、调用之前，事件订阅可能被另一个线程取消，导致 NullReferenceException。
?.Invoke() 内部会先把委托捕获到临时变量，再检查和调用，避免了这个竞态条件。

适用场景
1.导航属性链：ORM（如 EF Core）中的对象关系导航属性，order?.Customer?.Address?.City 是典型用法。
2.可选参数访问：方法参数可能为 null 的情况，比每次都写 if (param != null) 更简洁。
3.事件和委托调用：OnEvent?.Invoke(args) 是 C# 中触发事件的标准惯用法。
4.LINQ 结果处理：list.FirstOrDefault()?.Name 安全地获取可能为 null 的第一个元素的属性。
5.配置和设置读取：深层配置对象的访问，config?.Database?.ConnectionString。

注意事项
1.?. 链中，只要有一级为 null，整个链的结果就是 null，后续所有的 ?. 都不再执行。
这叫"短路"行为，与 && 类似。

2.不要对一个明确不可能为 null 的对象使用 ?.，这会在启用 NRT 的项目中产生编译警告（CS8602）。
?. 的使用应该与 ? 注解的可空性声明保持一致。

3.?. 不能用于赋值的左侧：obj?.Property = value; 是不合法的。
如果需要条件赋值，需要先判 null 再赋值。
在大量调用链中，?. 可能隐藏了设计问题：如果代码中到处都是 ?.，可能说明对象图的 null 情况没有被好好管理，
应该在更早的层次（比如工厂方法、数据验证）保证对象的非空性，而不是在使用处到处防御。


总结
?. 空条件访问运算符在访问成员前自动检查 null，为 null 时返回 null 而非抛出 NullReferenceException。
它是长链对象访问的 null 安全版本，与 ?? 搭配使用可以优雅地处理"null 则提供默认值"的场景。
在事件触发中，?.Invoke() 是线程安全的标准写法。
?. 作用于返回值类型的成员时，结果自动包装为可空值类型（T?）。
这个运算符极大地简化了防御性 null 检查代码，但不应被滥用来掩盖对象图中不合理的 null 分布。

*/

#endregion