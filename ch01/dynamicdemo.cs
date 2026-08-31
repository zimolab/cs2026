#region 概念说明
// dynamic 是 C# 4.0 引入的特殊类型，它绕过了编译器的静态类型检查，将所有类型解析和成员访问推迟到运行时（通过 DLR，即动态语言运行时）。
// 对 dynamic 变量的任何操作，编译器都不会在编译阶段验证，只有运行时才知道是否合法。

// dynamic 的典型用途是与动态语言（Python、Ruby via IronPython/IronRuby）互操作、处理 COM 对象（如 Office 自动化）、以及处理运行时才知道结构的数据（如动态 JSON 解析）。
// 这些场景在 .NET 生态中是小众需求，日常业务代码中几乎不需要用到 dynamic。

// 重要认知：dynamic 不是弱类型，运行时仍然有类型；它只是告诉编译器"不要在编译时检查这个变量，交给运行时"。如果运行时操作失败，会抛出 RuntimeBinderException。

// 语法结构
/*

// dynamic 变量声明
dynamic 变量名 = 任意值;

// 可以调用任意成员（编译器不验证，运行时验证）
dynamic d = GetSomething();
var result = d.SomeMethod();       // 编译通过，运行时才知道是否有效
var prop = d.SomeProperty;         // 编译通过，运行时才知道是否有效

*/


// 使用dynamic变量时，编译器会可能发出警告：
// warning IL2026: Using member 'Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags, String, IEnumerable<Type>, Type, IEnumerable<CSharpArgumentInfo>)' 
// which has 'RequiresUnreferencedCodeAttribute' can break functionality when trimming application code. Using dynamic types might cause types or members to be removed by trimmer.

// warning IL3050: Using member 'Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags, String, IEnumerable<Type>, Type, IEnumerable<CSharpArgumentInfo>)' 
// which has 'RequiresDynamicCodeAttribute' can break functionality when AOT compiling. The 'dynamic' feature requires runtime-code generation, which is incompatible with AOT.

#endregion


#region dynamic的基本行为

{

    // dynamic类型的变量可以持有任意类型的值
    dynamic d = 123;
    Console.WriteLine($"d = {d}, type = {d.GetType()}");

    // 将string类型赋值给dynamic变量
    d = "Hello, World!";
    Console.WriteLine($"d = {d}, type = {d.GetType()}");

    // 将List<int>类型赋值给dynamic变量
    d = new List<int> { 1, 2, 3 };
    Console.WriteLine($"d = {d}, type = {d.GetType()}");
}

#endregion

#region dynamic的成员访问

{
    // dynamic变量d被绑定到一个字典对象
    dynamic d = new Dictionary<string, object> { ["Name"] = "John", ["Age"] = 30 };
    // 可以通过点语法对象成员
    // 但编译器不会在编译时检查成员是否存在，这种行为类似于动态语言如Python
    Console.WriteLine($"d.Keys = {string.Join(", ", d.Keys)}");

    // 如果访问了一个不存在的成员，运行时会抛出RuntimeBinderException
    try
    {
        // 这里d实际上是一个字典，没有Address属性，运行时会抛出RuntimeBinderException
        Console.WriteLine($"d.Address = {d.Address}");
    }
    catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e)
    {
        Console.WriteLine($"RuntimeBinderException: {e.Message}");
        // 输出：RuntimeBinderException: 'System.Collections.Generic.Dictionary<string,object>' does not contain a definition for 'Address'
    }


}

#endregion

#region dynamic与JSON动态解析（ExpandoObject）
{

    // ExpandoObject类对象可以在运行时动态添加属性，然后通过dynamic变量访问这些属性
    dynamic obj = new System.Dynamic.ExpandoObject();
    // 动态添加属性
    obj.Name = "John";
    obj.Age = 30;
    obj.Address = "123 Main St";
    obj.Greet = new Action(() => Console.WriteLine($"Hello, {obj.Name}!"));
    // 动态访问属性
    Console.WriteLine($"obj.Name = {obj.Name}");
    Console.WriteLine($"obj.Age = {obj.Age}");
    Console.WriteLine($"obj.Address = {obj.Address}");
    obj.Greet(); // 输出：Hello, John!

    // 因为ExpandoObject实现了IDictionary<string, object>接口
    // 可以通过foreach循环遍历所有属性
    foreach (var (key, value) in (obj as IDictionary<string, object?>))
    {
        Console.WriteLine($"Key: {key}, Value: {value}");
    }
    // 将其转换到IDictionary<string, object?>类型后，也可以像字典一样访问元素
    var objDict = (obj as IDictionary<string, object?>) ?? throw new InvalidOperationException("obj is not an ExpandoObject"); // 转化为IDictionary<string, object?>类型
    Console.WriteLine($"objDict['Name'] = {objDict["Name"]}");
    Console.WriteLine($"objDict['Age'] = {objDict["Age"]}");
    Console.WriteLine($"objDict['Address'] = {objDict["Address"]}");
    Console.WriteLine($"objDict['Greet'] = {objDict["Greet"]}");
    ((Action)objDict["Greet"]).Invoke(); // 输出：Hello, John!

}
#endregion

#region dynamic与COM对象交互
// 简化与COM的交互，是dynamic的主要应用场景

// 在没有 dynamic 之前，操作 COM 对象（如 Excel）非常繁琐
// 需要大量的显式转换：
// ((Excel.Range)worksheet.Cells[1, 1]).Value2 = "Hello";

// 有了 dynamic 之后：
// dynamic worksheet = ...;  // 从 Excel 应用程序对象获取
// worksheet.Cells[1, 1].Value2 = "Hello";  // 直接访问，编译器不检查

#endregion


#region 常见问题、适用场景、注意事项

/*
常见问题：
1. dynamic与var的区别？
var是编译时推断类型，推断出类型后，类型后续不可改变，变量只能持有该类型的值。
dynamic是运行时的动态类型，dynamic类型变量可以持有任意类型的值，编译器不对其做任何检查。

2.dynamic是否有性能开销？
有，dynamic需要通过DLR在运行时动态解析成员访问，这比静态访问慢得多，
在热路径（高频调用代码）中不要使用dynamic。

3.dynamic 能用于泛型参数吗？
可以，List<dynamic> 是合法的，等效于 List<object>, 但成员访问是动态的。
不过这种用法在实际代码中几乎看不到，通常是设计不合理的信号。
*/

/*
适用场景：
1、COM 互操作：Office 自动化（Excel、Word）、旧版 Windows Shell 对象等 COM 接口，
用 dynamic 显著简化代码。
2、动态语言互操作：与 IronPython、IronRuby 等运行在 .NET 上的动态语言交互。
3、动态 JSON 处理：配合 ExpandoObject 或 System.Text.Json 的 JsonObject，处理运行时才知道结构的 JSON。
4、反射替代：某些场景下，dynamic 比反射代码更简洁，但性能差不多。
*/

/*
注意事项：
1、dynamic 关闭了编译器的类型检查，错误只能在运行时发现，不适合业务逻辑代码。
在新代码中，几乎所有 dynamic 的使用场景都有更好的替代方案（泛型、接口、JsonNode）。
2、对 dynamic 变量调用方法时，如果运行时没有该方法，抛出的是 RuntimeBinderException，
不是 NullReferenceException 或 MissingMethodException，排查时需要注意。
3、dynamic 可以和 null 一起使用（dynamic d = null），
但对 null 的 dynamic 变量调用成员会抛出 NullReferenceException，与普通引用类型相同。
4、dynamic 不支持扩展方法——扩展方法是编译时静态解析的，dynamic 对象无法调用扩展方法。
*/

/*
dynamic 是 C# 中的运行时动态类型机制，绕过编译器类型检查，将成员解析推迟到运行时。
它的主要价值在于 COM 互操作和动态语言互操作这两个特定场景，以及通过 ExpandoObject 实现动态属性对象。
在日常业务代码中，应尽量避免 dynamic——泛型、接口、模式匹配能覆盖绝大多数需要"灵活类型"的场景，
且保持了编译时安全和更好的性能。

dynamic 和 var 是两个完全不同的概念：前者是运行时动态，后者是编译时推断的静态类型。
*/

#endregion