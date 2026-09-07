#region C#中的赋值运算符

/*
赋值运算符将右侧表达式的值写入左侧变量。除了基本的 = 运算符，C# 提供了一系列复合赋值运算符，将运算和赋值合并成一步，是"读取-修改-写回"模式的简写形式。


复合赋值运算符（如 +=、-=、*=）不仅语法简洁，在某些场景下还有实际的语义价值：
对于属性而言，x.Value += 1 只触发一次 getter 和一次 setter，而不是两次 getter 加一次 setter（实际上编译后两者等价，但语义清晰）。

C# 还有几个特殊的赋值相关运算符：??=（空合并赋值，C# 8）用于只在变量为 null 时赋值，=>（lambda 表达式）在广义上也是一种"赋值"表达式体。

+----------+----------------+----------------------------------+
| 运算符    | 等价于         | 说明                             |
+----------+----------------+----------------------------------+
| a = b    | -              | 基本赋值                         |
| a += b   | a = a + b      | 加法赋值                         |
| a -= b   | a = a - b      | 减法赋值                         |
| a *= b   | a = a * b      | 乘法赋值                         |
| a /= b   | a = a / b      | 除法赋值                         |
| a %= b   | a = a % b      | 取余赋值                         |
| a &= b   | a = a & b      | 按位与赋值                       |
| a |= b   | a = a | b      | 按位或赋值                       |
| a ^= b   | a = a ^ b      | 按位异或赋值                     |
| a <<= b  | a = a << b     | 左移赋值                         |
| a >>= b  | a = a >> b     | 有符号右移赋值                   |
| a >>>= b | a = a >>> b    | 无符号右移赋值（C# 11+）        |
| a ??= b  | a = a ?? b     | 空合并赋值（C# 8+）             |
+----------+----------------+----------------------------------+

*/

#endregion

#region 基本示例

{
    int x = 10; // 最基本的赋值

    // 接下来是复合赋值演示
    x += 5; // 等价于 x = x + 5
    Console.WriteLine(x); // 输出 15
    x -= 3; // 等价于 x = x - 3
    Console.WriteLine(x); // 输出 12
    x *= 2; // 等价于 x = x * 2
    Console.WriteLine(x); // 输出 24
    x /= 4; // 等价于 x = x / 4
    Console.WriteLine(x); // 输出 6
    x %= 2; // 等价于 x = x % 2
    Console.WriteLine(x); // 输出 0

    // 位运算的符合赋值
    int flag = 0B_0000_0001;
    flag |= 0B_0000_0010; // 等价于 flag = flag | 0B_0000_0010，含义是将 flag 的第 2 位设置为 1
    Console.WriteLine(flag); // 输出 3
    flag &= 0B_0000_0001; // 等价于 flag = flag & 0B_0000_0001 ，含义是将 flag 的第 1 位设置为 0
    Console.WriteLine(flag); // 输出 1

    // 清除对应bit
    flag &= ~0B_0000_0001; // 等价于 flag = flag & ~0B_0000_0001，含义是将 flag 的第 1 位清除为 0
    Console.WriteLine(flag); // 输出 0

    // 翻转对应位
    flag ^= 0B_0000_0011; // 等价于 flag = flag ^ 0B_0000_0001，含义是将 flag 的第1位和第2位翻转
    Console.WriteLine(flag); // 输出：0

    // 左移和右移复合赋值
    int num = 10;
    num <<= 2; // 等价于 num = num << 2，含义是将 num 左移两位，相当于 num * 4
    Console.WriteLine(num); // 输出 40
    num >>= 1; // 等价于 num = num >> 1，含义是将 num 右移一位，相当于 num / 2
    Console.WriteLine(num); // 输出 20
    num >>>= 1; // 等价于 num = num >>> 1，含义是将 num 无符号右移一位，相当于 num / 2
    Console.WriteLine(num); // 输出 10

    // 赋值是一种表达式，表达式意味着它是有值的
    int a;
    int b = a = 5; // 等价于 b = (a=5);
    Console.WriteLine($"a={a}, b={b}");

    // 通过out参数给变量赋值
    int parsedInt;
    if (int.TryParse("123", out parsedInt))
    {
        // parsedInt在这里可用，其已经被TryParse方法赋值
        Console.WriteLine($"parsedInt={parsedInt}"); // 输出 parsedInt=123  
    }

}
#endregion

#region 空值合并
{
    string? str = null;

    // 空值合并赋值运算符（??=）用于只在变量为 null 时赋值
    str ??= "default"; // 等价于 str = str ?? "default";
    Console.WriteLine(str); // 输出 default

    // 此时str已经不为null，所以不会再次赋值
    str ??= "another default"; // 等价于 str = str ?? "another default";
    Console.WriteLine(str); // 输出 default

}
#endregion

#region 一些实际的运用
{
    // 累加统计
    double sum = 0;
    double[] data = [1.5, 2.5, 3.5, 4.5];
    foreach (double value in data)
    {
        sum += value; // 等价于 sum = sum + value;  
    }
    Console.WriteLine($"sum={sum}, average={sum / data.Length}");

    // 累乘
    int product = 1;
    for (int i = 1; i <= 10; i++)
    {
        product *= i; // 等价于 product = product * i;  
    }
    Console.WriteLine($"product={product}");

    // 移位用于构建掩码
    int mask = 0;
    // 构建一个掩码，其中第0位、第1位、第3位和第5位为1，其余位为0
    int[] bitToBeSet = [0, 1, 3, 5];
    foreach (int bit in bitToBeSet)
    {
        mask |= 1 << bit; // 等价于 mask = mask | 1 << bit;  
    }
    Console.WriteLine(Convert.ToString(mask, 2).PadLeft(8, '0'));  // 10100101
}
#endregion

#region 总结

/*
常见问题
Q: x += 1 和 x = x + 1 完全等价吗？

A: 大多数情况下是等价的。有一个微妙的区别：
对于复杂的左值表达式（如 arr[GetIndex()] += 1），
复合赋值只求值一次索引，而展开写法 arr[GetIndex()] = arr[GetIndex()] + 1 
会调用 GetIndex() 两次。如果 GetIndex() 有副作用，两者行为不同。
但对于简单变量 x += 1，两者完全等价。

Q: 赋值运算符的结合方向是从右到左吗？

A: 是的。a = b = c = 5 等价于 a = (b = (c = 5))，先从最右边的赋值开始执行，
结果从右向左传递。这是赋值运算符右结合性的体现。

Q: += 对于字符串不是"修改"原字符串，那它实际做了什么？

A: string greeting = "Hello"; greeting += " World"; 的实际效果是：
1.计算 greeting + " World"，创建新字符串 "Hello World"
2.把新字符串的引用赋给 greeting
原来的字符串对象 "Hello" 没有被修改（字符串是不可变的），greeting 变量现在指向了新的字符串对象。


适用场景
1.累加/累减计数：count++ 或 total += item 是循环中最频繁的操作，复合赋值使代码更简洁。
2.标志位操作：permissions |= Permission.Write 添加权限，permissions &= ~Permission.Read 移除权限，比展开写法更清晰。
3.惰性初始化：field ??= new T() 是延迟初始化模式的标准写法，线程安全版本需要 Interlocked 或 Lazy<T>。
4.字符串拼接（少量）：简单场景的字符串累加，代码量少时直接用 +=。

注意事项
1.复合赋值对于隐式类型转换有一个特殊行为：byte x = 10; x += 5; 是合法的（编译器隐式截断），但 byte x = 10; x = x + 5; 会编译报错（x + 5 结果是 int，不能隐式赋给 byte）。复合赋值运算符内部包含了隐式转换，不需要显式转换。
2.??= 在多线程场景下不是原子的，如果需要线程安全的惰性初始化，应该使用 Interlocked.CompareExchange 或 Lazy<T>，而不是直接用 ??=。
3.连续赋值 a = b = c = 0 虽然简洁，但在可读性不高时（如类型不明显）还是建议分行写。
4.对于属性的复合赋值，底层调用一次 getter 和一次 setter，与显式 prop = prop + val 等价，但如果 getter 和 setter 有不同的逻辑，需要特别注意。

总结
赋值运算符是 C# 中最基础的运算符，基本 = 将值写入变量，复合赋值运算符（+=、-=、*= 等）将运算和赋值合并，
简化了"读取-修改-写回"这个常见模式。C# 8 引入的 ??= 空合并赋值是惰性初始化的便捷写法。
赋值表达式本身有返回值，支持链式赋值。
对于 byte/short 类型，复合赋值内置了隐式截断转换。
在多线程环境下，任何赋值运算符（包括 ??=）都不是原子的，需要显式同步。

*/

#endregion