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


}

#endregion