#region 空合并运算符

/*
?? 是空合并运算符（Null-coalescing Operator），语义是：
- 如果左侧操作数不为 null，返回左侧；
- 如果左侧为 null，返回右侧。
它是处理 null 值时最常用的简化工具，能将多行的 if (x == null) x = default; 写法压缩成一个简洁的表达式。

?? 只关心 null，不检查其他"假值"（如 0、false、空字符串 ""）。
它适用于引用类型和可空值类型（T?，Nullable<T>）。

?? 可以链式连接，从左到右依次检查，返回第一个非 null 值：
a ?? b ?? c 表示"a 不为 null 就用 a，否则看 b，b 不为 null 就用 b，否则用 c"。

语法结构

// 基本形式
结果 = 可能为null的表达式 ?? 默认值;

// 链式形式
结果 = a ?? b ?? c ?? d;

// 与空合并赋值结合（??=，见 2.7 章节）
变量 ??= 默认值;

*/

#endregion


#region 

{
    // 用法1：用来提供默认值
    string? name = null;

    name ??= "unknown"; // 如果 name 为 null，name = "unknown"
    Console.WriteLine($"name={name}");

    // 等价的三目运算符形式
    name = name is not null ? name : "unknown";
    Console.WriteLine($"name={name}"); // 输出：name=unknown

    // 可空值类型
    int? maybeInt = null;
    int value = maybeInt ?? 0; // 如果 maybeInt 为 null，value = 0
    Console.WriteLine($"value={value}"); // 输出：value=0
    maybeInt = 42;

    value = maybeInt ?? 0; // 如果 maybeInt 不为 null，value = 42
    Console.WriteLine($"value={value}"); // 输出：value=42

    // 链式形式
    string? fromDatabase = null;
    string? fromCache = null;
    string? fromUser = Environment.GetEnvironmentVariable("USER_NAME");
    string defaultName = "Guest";

    // 从数据库、缓存、用户变量中获取用户名，如果都为空，则使用默认值
    // 从左到右依次检查，返回第一个非 null 值
    var userName = fromDatabase ?? fromCache ?? fromUser ?? defaultName;
    Console.WriteLine($"userName={userName}");

    // 与方法调用组合使用
    // ??右侧可以是任意表达式，包括方法调用，具有短路特性，只有在左侧为 null 时才会执行右侧
    string? config = null;
    string cached = "cachedConfig";
    static string GetDefaultConfig()
    {
        return "defaultConfig";
    }

    config = cached ?? GetDefaultConfig(); // 如果cached为null，则调用GetDefaultConfig()获取默认配置
    // 因为这里cached不为null，由于??的短路特性，所以不会调用GetDefaultConfig()
    Console.WriteLine($"config={config}"); // 输出：config=cachedConfig

    static string GetDisplayName(UserProfile user)
    {
        // 按照Name、Nickname、Email的优先级返回，如果皆为null，则返回"Anonymous"
        return user.Name ?? user.Nickname ?? user.Email ?? "Anonymous";
    }

    var user1 = new UserProfile { Name = "Alice", Nickname = "Ali", Email = "alice@example.com" }; // 所有字段皆不为null
    var user2 = new UserProfile { Nickname = "Bob" }; // Name, Email为null
    var user3 = new UserProfile { Email = "Charlie" }; // Name, Nickname为null
    var user4 = new UserProfile(); // Name, Nickname, Email皆为null

    Console.WriteLine($"DisplayName(user1)={GetDisplayName(user1)}"); // 输出：DisplayName(user1)=Alice
    Console.WriteLine($"DisplayName(user2)={GetDisplayName(user2)}"); // 输出：DisplayName(user2)=Bob
    Console.WriteLine($"DisplayName(user3)={GetDisplayName(user3)}"); // 输出：DisplayName(user3)=Charlie
    Console.WriteLine($"DisplayName(user4)={GetDisplayName(user4)}"); // 输出：DisplayName(user4)=Anonymous




}

#endregion


#region 自定义类型

public class UserProfile
{
    public string? Name { get; set; }
    public string? Nickname { get; set; }
    public string? Email { get; set; }

}


#endregion

#region 总结

/*
常见问题
Q: ?? 会检查空字符串 "" 吗？

A: 不会。?? 只检查 null，空字符串 "" 对 ?? 来说是非 null 值，不会触发右侧的默认值。
如果需要同时处理 null 和空字符串，应该用：
string result = string.IsNullOrEmpty(text) ? "default" : text;
// 或者
string result = !string.IsNullOrEmpty(text) ? text : "default";

Q: ?? 的右侧什么时候执行？

A: 只有在左侧为 null 时才执行右侧表达式，这是 ?? 的短路特性。
如果左侧非 null，右侧的表达式（包括方法调用）不会执行。
这与 || 的短路逻辑类似，可以用来做"惰性求值"——只在需要默认值时才去计算它。

Q: ?? 能用于值类型（如 int）吗？

A: 普通值类型（int、double 等）不能为 null，因此 int x = someInt ?? 5 中 someInt 必须是 int?（可空值类型），
否则编译报错。?? 对不可空类型没有意义，编译器会给出警告。


适用场景
1.方法参数默认值：当参数允许传 null 时，方法内用 ?? 提供实际使用的默认值。
2.数据库可空字段映射：int? 类型的字段读取后用 ?? 给出业务默认值。
3.配置读取：多个配置来源（环境变量 → 文件 → 硬编码默认值）的优先级链。
4.用户显示信息：如上面的例子，按优先级取显示名、邮件、用户名等。
5.取消令牌：方法接受可选的 CancellationToken? 时，token ?? CancellationToken.None 是常见模式。

注意事项
1.?? 运算符是右结合的：a ?? b ?? c 等价于 a ?? (b ?? c)，从左到右找第一个非 null，语义与大多数人的直觉一致。
2.在可空引用类型（NRT）项目中，?? throw new ArgumentNullException(nameof(param)) 是一个有用的模式，用于检查必须非 null 的值：string name = rawName ?? throw new ArgumentNullException(nameof(rawName));。
3.?? 和 ?.（空条件运算符）经常配合使用：user?.Name ?? "Unknown" 表示"如果 user 不为 null，取其 Name；如果 Name 也为 null，则返回 'Unknown'"。
4.不要用 ?? 来掩盖 null 问题。如果一个变量"不应该"为 null 但实际上可能是，用 ?? 给默认值只是让问题悄悄消失，应该考虑从根源解决 null 的来源。

总结
?? 空合并运算符是处理 null 的利器，语义清晰：非 null 返回原值，null 返回指定默认值。
它的短路特性确保右侧只在需要时才求值。
链式 ?? 实现了优雅的多级回退逻辑，比嵌套三目运算符更可读。
?? 只检查 null，不检查空字符串等"空值"。
配合 ?.（空条件运算符）和 ??=（空合并赋值），三者共同构成了 C# 中处理 null 的核心工具集。
*/

#endregion