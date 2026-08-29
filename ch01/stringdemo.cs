// 演示C#中的字符串类型string

#region 关于string的说明
//string（对应 System.String）是 C# 中最常用的引用类型之一，表示不可变的 Unicode 字符序列。
// "不可变"意味着一旦字符串对象创建，它的内容就无法改变——所有看起来在"修改"字符串的操作（拼接、替换、大小写转换）实际上都是创建了新的字符串对象。
// 这个特性有重要的使用影响：循环体内大量拼接字符串（str += newPart）会产生大量临时对象，性能很差，应该改用 StringBuilder。
// 同时，字符串的不可变性使它天然线程安全，可以在多线程中安全共享，不需要加锁。
// C# 12 引入了原始字符串字面量（"""），C# 13 进一步完善，可以更自然地表示多行文本和包含特殊字符的内容，避免过多的转义字符。
#endregion

#region 字符串字面量的表示方式
// 把这一节的代码包裹到一个代码块中，利用变量遮蔽，防止后面实例代码中出现变量命名冲突，小巧思这一块！
{
    // 普通字符串字面量
    var normalString = "Hello, World!\nGreetings from \"Zimolab\" "; // 普通字符串字面量，特殊字符需要转义
    var pathString = "C:\\Users\\Alice\\Documents\\File.txt"; // 尤其是在表示路径时，反斜杠需要转义
    Console.WriteLine(normalString);
    Console.WriteLine(pathString); // 输出：C:\Users\Alice\Documents\File.txt

    // 逐字符字符串字面量，反斜杠不需要转义，适合表示路径、正则表达式
    var pathString2 = @"C:\Users\Alice\Documents\File.txt"; // 等价于 pathString
    Console.WriteLine(pathString2); // 输出：C:\Users\Alice\Documents\File.txt
    var regexString = @"\d{3}-\d{4}"; ; // 正则表达式
    Console.WriteLine(regexString); // 输出：\d{3}-\d{4}

    // 插值字符串，使用 $ 符号，可以方便地将变量插入到字符串中
    var name = "Alice";
    var greeting = $"Hello, {name}!"; // 插值字符串，使用 $ 符号
    Console.WriteLine(greeting); // 输出：Hello, Alice!
    // ${...} 语法可以嵌入表达式，包括算术运算、方法调用等
    var age = 30;
    var message = $"My age is {age} and I will be {age + 1} years old in November."; // 插值字符串，嵌入表达式
    Console.WriteLine(message); // 输出：My age is 30 and I love 31 years old.
    message = $"Average(Sum(1 to 100), 100) = {Enumerable.Range(1, 100).ToArray().Sum() / 100.0}"; // 插值字符串，嵌入表达式
    Console.WriteLine(message); // 输出：Average(Sum(1 to 100), 100) = 50.0
    // 插值时还可以指定字符串格式
    var formattedString = $"Name: {name,-10}, Age: {age,3}"; // 插值字符串，指定字符串格式，-10 表示右对齐10位，3 表示左对齐3位
    Console.WriteLine(formattedString); // 输出：Name: Alice     , Age:  30
    // :C 表示货币格式，:F 表示浮点数格式
    Console.WriteLine($"price = {99.99m:C}");
    Console.WriteLine($"price = {99.99m:F}");
    // F后面可以跟一个数字，用于指定显示的小数位数
    Console.WriteLine($"{0.123456789:F2}"); // 输出：0.12
    // 关于字符串格式化，可以参考：https://learn.microsoft.com/zh-cn/DOTNET/api/system.string.format?view=netcore-1.1#2


    // C# 11 原始字符串字面量，可以表示多行文本，不需要转义换行符
    var jsonStr = """
    {
        "name": "Alice",
        "age": 30,
        "address": "123 Main St",
        "profile": usere\profile\alice.json
    }
    """;
    Console.WriteLine(jsonStr);
    // 原始字符串插值
    var name2 = "Bob";
    var email = "bob@example.com";
    var profile = @"user\profile\bob.json";
    var jsonStr2 = $$"""
    {
        "name": "{{name2}}",
        "age": 25,
        "address": "456 Elm St",
        "profile": {{profile}}
    }
    """;
    Console.WriteLine(jsonStr2);
}
#endregion

#region 字符串常用方法
{
    var str = "Hello, World!";
    // 获取字符串长度
    Console.WriteLine($"Length of str: {str.Length}"); // 输出：13
    // 大小写转换
    Console.WriteLine($"Lowercase of str: {str.ToLower()}"); // 输出：hello, world!
    Console.WriteLine($"Uppercase of str: {str.ToUpper()}"); // 输出：HELLO, WORLD!

    str = "   Hello, World!    ";
    // 去除前导空白字符
    Console.WriteLine($"Trimmed str: {str.TrimStart()}"); // 输出：Hello, World!    (去掉前导空白字符)
    // 去除后导空白字符
    Console.WriteLine($"Trimmed str: {str.TrimEnd()}"); // 输出：   Hello, World! (去掉后导空白字符)
    // 去除前后空白字符
    Console.WriteLine($"Trimmed str: {str.Trim()}"); // 输出：Hello, World! (去掉前后空白字符)

    str = "Hello, World!";
    // 判断是否存在子串
    Console.WriteLine($"""Contains "World": {str.Contains("World")}""");
    // 判断是否以某个字符(串)开头或结尾
    Console.WriteLine($"""StartsWith "Hello": {str.StartsWith("Hello")}""");
    Console.WriteLine($"EndsWith '!': {str.EndsWith('!')}");
    // 查找子串索引（从零开始）
    var index = str.IndexOf("World");
    Console.WriteLine($"""Index of "World": {index}"""); // 输出：Index of "World": 7
    // 取子串，范围为[start, end)
    var subStr = str.Substring(0, index);
    Console.WriteLine($"Substr: {subStr}"); // 输出：Substr: Hello,
    // C#8之后引入了范围运算符，类似python的切片
    subStr = str[0..index]; // 等价于 str.Substring(0, index)
    Console.WriteLine($"Substr: {subStr}"); // 输出：Substr: Hello,

    // 字符串判空
    string? maybeNull = null;
    Console.WriteLine($"Is maybeNull null: {maybeNull == null}"); // 输出：Is maybeNull null: True
    Console.WriteLine($"Is maybeNull null: {maybeNull is null}"); // 输出：Is maybeNull null: True

    string empty = "";
    Console.WriteLine($"IsNullOrEmpty(): {string.IsNullOrEmpty(empty)}");
    Console.WriteLine($"IsNullOrEmpty(): : {string.IsNullOrEmpty(maybeNull)}");

    string withSpaces = "   ";
    Console.WriteLine($"IsNullOrEmpty(): {string.IsNullOrEmpty(withSpaces)}"); /// False
    Console.WriteLine($"IsNullOrWhiteSpace(): {string.IsNullOrWhiteSpace(withSpaces)}"); // True
}
#endregion


#region 字符串连接
{
    // 字符串是不可变对象，每次拼接都会创建新的字符串对象
    var str1 = "Hello";
    var str2 = "World";
    var str3 = str1 + " " + str2; // 每次拼接都会创建新的字符串对象
    Console.WriteLine(str3); // 输出：Hello World
    // 存在大量拼接时（比如在循环中）可能会产生性能问题
    string result = "Hello, World! ";
    // 每次拼接都会创建新的字符串对象，旧的字符串对象会被垃圾回收，因此性能较差
    for (int i = 0; i < 10; i++) // 如果将循环次数设置到27，很可能会触发Out of memory.
    {
        // result += result 会创建一个全新的字符串对象，其长度为原字符串长度的 2 倍，并复制旧字符串的全部字符。
        // 字符串的长度是呈指数增长的，当循环次数设置到27次左右时，长度 ≈ 2 GB（超出单对象限制），可能导致OOM
        // 即便是次数小一些，没有超出内存限制，但因为存在大量的分配新内存、复制原有字符串、回收旧内存的操作，性能也会下降的很厉害
        //
        result += result;
    }
    // Console.WriteLine(result);
    // 如果需要拼接大量的字符串，更好的办法时使用缓冲区，比如 StringBuilder
    // 当然，StringBuilder也不能超出物理极限，如果拼接的字符串长度超过内存限制，也会导致OOM
    var sb = new System.Text.StringBuilder();
    for (int i = 0; i < 10; i++)
    {
        // 直接追加"Hello, World!"到缓冲区尾部，不会产生中间字符串对象
        sb.Append("Hello, World! ");
    }
    Console.WriteLine(sb.Length);
    // 拼接完成后，获取结果字符串
    var finalResult = sb.ToString();

    // 循环内做字符串拼接一定要用 StringBuilder 吗？
    // 少量的（5 个以内）固定字符串拼接，编译器会优化成 string.Concat，性能完全没问题。
    // 需要 StringBuilder 的场景是：循环次数较多（几十次以上）或者字符串很长。
    // 另外，C# 10 引入的内插字符串处理器（Interpolated String Handler）让 $"" 在某些场景下也能达到 StringBuilder 的性能。

}
#endregion

#region 字符串比较
{
    // 比较两个字符串是否相等
    string str1 = "apple";
    string str2 = "Apple";
    // 字符串是引用类型，为什么 s1 == s2 比较的是内容而不是引用？
    // string 重载了 == 运算符，使其比较字符串内容（值相等），而不是引用地址。这是一个刻意的设计，因为字符串在使用上更接近值类型的语义。
    // 如果需要比较引用（判断是否是同一个对象），用 object.ReferenceEquals(str1, str2)。
    Console.WriteLine($"str1 == str2: {str1 == str2}"); // 输出：str1 == str2: False
    Console.WriteLine($"str1.Equals(str2): {str1.Equals(str2)}"); // 输出：str1.Equals(str2): False

    // 忽略大小写比较
    Console.WriteLine(str1.Equals(str2, StringComparison.OrdinalIgnoreCase)); // 输出：str1.Equals(str2, StringComparison.OrdinalIgnoreCase): True
    Console.WriteLine(string.Compare(str1, str2, StringComparison.OrdinalIgnoreCase) == 0); // True

    // 推荐用 StringComparison 枚举明确语义
    // Ordinal：纯字节比较，最快，适合 ID、密码等技术性字符串
    // OrdinalIgnoreCase：忽略大小写的字节比较
    // CurrentCulture：当前文化语言规则，适合显示给用户看的排序
}
#endregion