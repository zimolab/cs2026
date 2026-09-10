#region C#中的空索引运算符 ?[]

/*
?[] 是空条件索引运算符，是 ?. 的索引器版本。它在通过索引访问集合、数组或实现了索引器的对象时，先检查对象是否为 null：
为 null 时返回 null 而不是崩溃，不为 null 时正常执行索引访问。

?[] 和 ?. 的语义完全类似，区别只在于访问形式：?. 用于访问属性和方法，?[] 用于通过方括号进行索引访问。
两者可以在同一个链式表达式中混合使用。

语法：

// 基本形式：对象为 null 时返回 null，否则执行索引访问
结果 = collection?[index];

// 与其他运算符组合
结果 = collection?[index]?.Property;   // 索引后继续链式访问
结果 = collection?[index] ?? 默认值;   // 为 null 时提供默认值
结果 = obj?.Property?[index];          // 先访问属性，再索引

*/

#endregion


#region 基本示例

{
    // ?[] 用于访问数组元素
    int[]? array = [1, 2, 3, 4, 5];
    int? result = array?[2]; // 3
    Console.WriteLine(result);

    // 将array设置为null
    array = null;
    result = array?[2]; // 返回null，而不是引发NullReferenceException
    Console.WriteLine(result is null ? "null" : result.ToString());  // null    

    // 与??联合使用，用以提供默认值
    int element = array?[2] ?? 0; // 如果array为null，element将被设置为0
    Console.WriteLine(element); // 0

    // ?[]用于访问字典
    Dictionary<string, int>? scores = new()
    {
        ["Alice"] = 90,
        ["Bob"] = 85,
        ["Charlie"] = 95,
        ["David"] = 88
    };

    int? score = scores?["Alice"]; // 90
    Console.WriteLine(score); // 90
    // 现在将字典设置为null
    scores = null;
    score = scores?["Alice"]; // 返回null，而不是引发NullReferenceException
    Console.WriteLine(score is null ? "null" : score.ToString());  // null
    // 与??联合使用，用以提供默认值
    int score2 = scores?["Alice"] ?? -1; // 如果scores为null，score2将被设置为-1
    Console.WriteLine(score2); // -1

    // 如果 scores 非 null 但键不存在，?[] 不会防止 KeyNotFoundException
    scores = new()
    {
        ["Alice"] = 90,
        ["Bob"] = 85
    };

    try
    {
        // 由于当前字典中没有键 "Charlie"，因此会引发KeyNotFoundException
        int score3 = scores?["Charlie"] ?? -1;
        Console.WriteLine(score3);
    }
    catch (KeyNotFoundException e)
    {
        Console.WriteLine($"KeyNotFoundException: {e.Message}");
    }
    // 更安全的方式是调用GetValueOrDefault 方法
    int score4 = scores!.GetValueOrDefault("Charlie", -1);
    Console.WriteLine(score4); // -1

}
#endregion

#region ?.  ?[] ??的混合使用

{
    List<Person>? students = null;
    Person? firstFriend = students?[0]?.Friends?[0];
    // 只要链条上有一处为null，整个链条都会返回null
    Console.WriteLine(firstFriend is null ? "null" : firstFriend); // null\

    students =
    [
        new() { Name = "Alice", Age = 20 },
        new() { Name = "Bob", Age = 22 }
    ];
    firstFriend = students?[0]?.Friends?[0];
    // students[0] 不为null，但students[0].Friends为null，因此firstFriend为null
    Console.WriteLine(firstFriend is null ? "null" : firstFriend); // null
    students[0].Friends =
    [
        new() { Name = "Charlie", Age = 21 },
        new() { Name = "David", Age = 23 }
    ];
    firstFriend = students?[0]?.Friends?[0];
    Console.WriteLine(firstFriend is null ? "null" : firstFriend.Name); // Charlie
    string firstFriendName = students?[0]?.Friends?[0]?.Name ?? "No Friend";
    Console.WriteLine(firstFriendName); // Charlie

}

#endregion

#region 自定义类型

class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; } = 0;
    public List<Person>? Friends { get; set; } = null;
}


#endregion

#region 总结

/*

常见问题
Q: dict?["key"] 能防止 KeyNotFoundException 吗？

A: 不能。?[] 只防止字典对象本身为 null 时的崩溃。
如果字典对象不为 null 但键不存在，仍然会抛出 KeyNotFoundException。
对于字典的安全访问，应该用 dict?.GetValueOrDefault("key") 或 dict?.TryGetValue("key", out var value) == true ? value : null。

Q: 为什么 ?[] 返回的值类型变成了可空类型（如 int?）？

A: 与 ?. 一样，?[] 的结果可能为 null（当集合本身为 null 时），而 int 不能持有 null，
所以编译器自动将结果包装为 int?。如果需要确定的 int，用 ?? 0 提供默认值：numbers?[0] ?? 0。

Q: 能对 Span 或 ReadOnlySpan 使用 ?[] 吗？

A: 不能。Span<T> 是 ref struct，不能为 null，也不适用于 ?[]。
Span<T> 的索引直接用 span[i]，索引越界会抛 IndexOutOfRangeException，需要自行边界检查。


适用场景
1.可能为 null 的集合属性：对象中的集合属性（如 Order.Items）允许为 null 时，order?.Items?[0] 安全地获取第一个元素。
2.方法返回可能为 null 的集合：GetUsers()?[0]?.Name 安全地获取第一个用户的名称。
3.嵌套集合访问：多层集合嵌套时，list?[i]?.SubList?[j] 避免每层都写 null 检查。
4.可选配置项：配置对象中的可选数组，config?.AllowedHosts?[0] 安全读取第一个允许的主机名。

注意事项
1. ?[] 只在集合引用本身为 null 时才短路返回 null，不处理索引越界（IndexOutOfRangeException）和键不存在（KeyNotFoundException）的情况。
2. 在使用 ?[] 之前，如果索引可能越界，应该先检查集合长度：list?.Count > 0 ? list[0] : null，或者使用 LINQ 的 FirstOrDefault()。
3. ?[] 的结果类型遵循与 ?. 相同的规则：如果索引器返回值类型（如 int），结果变为 int?。
4. 不要混淆 ?[] 和 C# 8 的 ^（末尾索引）、..（范围）运算符，后两者不涉及 null 检查，是独立的语法特性（见 2.11、2.12 章节）。

总结
?[] 是 ?. 的索引器版本，在通过索引访问数组、列表、字典或自定义索引器时，先检查对象是否为 null。
为 null 时返回 null 而不是抛异常，不为 null 时正常执行索引访问。
它与 ?. 可以任意混合使用，构建出安全的多级链式访问表达式。
需要注意的是，?[] 只防止对象本身为 null 的情况，不防止键不存在或索引越界，这两种情况需要额外处理。
*/


#endregion