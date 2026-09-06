#region 
/*

C#中的算术运算符

针对数值类型的基本数学运算，包括：加（+）、减（-）、乘（*）、除（/）、取余（%）、自增（++）、自减（--）等。

+----------+--------+-------------+----------------------------------+
| 运算符   | 名称   | 示例        | 说明                             |
+----------+--------+-------------+----------------------------------+
| +        | 加法   | a + b       | 两数相加；字符串拼接             |
| -        | 减法   | a - b       | 两数相减；一元负号               |
| *        | 乘法   | a * b       | 两数相乘                         |
| /        | 除法   | a / b       | 整数除法截断；浮点除法精确       |
| %        | 取余   | a % b       | 除法后的余数                     |
| ++       | 自增   | a++ / ++a   | 值加 1                           |
| --       | 自减   | a-- / --a   | 值减 1                           |
+----------+--------+-------------+----------------------------------+

*/

#endregion

#region 基本示例代码

{
    int a = 10;
    int b = 3;
    Console.WriteLine($"a = {a}, b = {b}");
    Console.WriteLine($"a + b = {a + b}"); // 加法，输出：a + b = 13
    Console.WriteLine($"a - b = {a - b}"); // 减法，输出：a - b = 7
    Console.WriteLine($"a * b = {a * b}"); // 乘法，输出：a * b = 30
    Console.WriteLine($"a / b = {a / b}"); // 整数除法截断，输出：a / b = 3
    Console.WriteLine($"a % b = {a % b}"); // 取余，输出：a % b = 1
    a++; // 后缀自增，在表达式中时，先读变量的值，再自增
    Console.WriteLine($"a++, a = {a}"); // 自增，输出：a++，a=11
    a--; // 后缀自减，在表达式中时，先读变量的值，再自减
    Console.WriteLine($"a--, a = {a}"); // 自减，输出：a--, a=10
    ++a; // 前缀自增，在表达式中时，先自增，再读变量的值
    Console.WriteLine($"++a, a = {a}"); // 自增，输出：++a, a=11
    --a; // 前缀自减，在表达式中时，先自减，再读变量的值
    Console.WriteLine($"--a, a = {a}"); // 自减，输出：--a, a=10

    // 一元运算符：取相反数
    Console.WriteLine($"-a = {-a}"); // 取相反数，输出：-a = -10
    Console.WriteLine($"-(-a) = {-(-a)}"); // 取相反数，输出：-(-a) = 10


    // 整数除法，两个操作数均为整数类型，结果为整数，直接截断小数部分
    int intResult = 5 / 2; // 整数除法，结果为整数，输出：intResult = 2
    Console.WriteLine($"intResult = {intResult}");

    // 浮点除法，至少一个操作数为浮点类型，结果为浮点数
    double doubleResult = 5.0 / 2; // 浮点除法，结果为浮点数，输出：doubleResult = 2.5
    Console.WriteLine($"doubleResult = {doubleResult}");

    // 将浮点数转换为整数类型，结果为整数，直接截断小数部分
    int intResult2 = (int)doubleResult; // 将浮点数转换为整数类型，结果为整数，输出：intResult2 = 2
    Console.WriteLine($"intResult2 = {intResult2}");

    // 负整数除法，C#规则是向零取整
    Console.WriteLine($"-7 / 2 = {-7 / 2}");// -3（截断，不是-4）
    Console.WriteLine($"-7 / -3 = {7 / -2}");// -2（截断，不是-3）
    Console.WriteLine($"-7 / -2 = {-7 / -2}");// 3

    // 取余运算，结果符号与被除数（左操作数）相同
    Console.WriteLine(10 % 3);    // 1（正数，结果正）
    Console.WriteLine(-10 % 3);   // -1（负数被除数，结果负）
    Console.WriteLine(10 % -3);   // 1（负数除数，结果取决于被除数：正）
    Console.WriteLine(-10 % -3);  // -1（两者都负，结果跟被除数：负）

    // 实用：判断奇偶（当 n 为负时，-5 % 2 = -1，不是 1，需要注意）
    int n = -5;
    bool isOdd = n % 2 != 0;    // 正确：-1 != 0，是奇数
    bool isEven = n % 2 == 0;   // 正确：-1 != 0，不是偶数

    int i = 5;

    // 后置 ++/--：先使用当前值，再自增/自减
    int postIncrement = i++;   // postIncrement = 5（使用的是自增前的值）
    Console.WriteLine($"i={i}, postIncrement={postIncrement}");  // i=6, postIncrement=5

    // 前置 ++/--：先自增/自减，再使用新值
    i = 5;  // 重置
    int preIncrement = ++i;    // preIncrement = 6（使用的是自增后的值）
    Console.WriteLine($"i={i}, preIncrement={preIncrement}");  // i=6, preIncrement=6

    // 自增/自减运算符在for循环中广泛使用
    for (int j = 0; j < 5; j++) // 在这种场景下下j++和++j效果一样
    {
        Console.Write($"{j} "); // 输出：0 1 2 3 4
    }
    Console.WriteLine();

    // 算术溢出，默认静默，产生回绕行为
    int intMax = int.MaxValue;
    int intMin = int.MinValue;
    Console.WriteLine($"intMax = {intMax}, intMin = {intMin}"); // intMax = 2147483647, intMin = -2147483648
    ++intMax; // 不会抛出异常，而是发生溢出，回绕到最小值，intMax = -2147483648
    Console.WriteLine($"++intMax = {intMax}"); // intMax = -2147483648
    --intMin; // 不会抛出异常，而是发生溢出，回绕到最大值，intMin = 2147483647
    Console.WriteLine($"--intMin = {intMin}"); // intMin = 2147483647

    // 使用checked进行溢出检查
    try
    {
        int result = checked(intMax + intMax); // 会抛出OverflowException
    }
    catch (OverflowException)
    {
        Console.WriteLine("发生算术溢出！");
    }

    // 浮点数精度问题

    // 浮点数的比较应当使用容差法，而不是直接使用 ==，因为浮点数在计算机中表示时可能存在精度误差。
    // 先确定一个可以接受的误差，epsilon
    const double epsilon = 1e-9;
    double x = 0.1;
    double y = 0.2;
    Console.WriteLine($"x + y = {x + y}"); // x + y = 0.30000000000000004
    // 如果直接比较，会发现 x + y != 0.3
    Console.WriteLine($"x + y == 0.3: {x + y == 0.3}"); // x + y == 0.3: False
    // 这里将运算结果与 0.3 的差的绝对值与 epsilon 比较，如果小于 epsilon，则认为两者相等
    bool isApproximatelyEqual = Math.Abs(x + y - 0.3) < epsilon;
    Console.WriteLine($"isApproximatelyEqual = {isApproximatelyEqual}"); // isApproximatelyEqual = True

    // decimal精确计算
    decimal price = 19.99m;
    decimal quantity = 3m;
    decimal total = price * quantity; // 59.97m
    Console.WriteLine($"total = {total}"); // total = 59.97m
}


#endregion


#region 总结
/*

常见问题
Q:整数除以零会怎样？浮点数除以零呢？

A:两者不同：整数除以零：抛出 DivideByZeroException（运行时异常）
浮点数除以零：不抛异常，结果是 Infinity（正无穷）、-Infinity（负无穷）或 NaN（0.0 / 0.0），
这是 IEEE 754 标准的规定

// int intResult = 5 / 0;         // 抛出 DivideByZeroException
double inf = 5.0 / 0.0;           // Infinity
double negInf = -5.0 / 0.0;       // -Infinity
double nan = 0.0 / 0.0;           // NaN
Console.WriteLine(double.IsInfinity(inf));  // True
Console.WriteLine(double.IsNaN(nan));       // True

Q：% 取余和数学上的模运算（Modulo）是一回事吗？

A: 不完全是。数学上的模运算结果总是非负的，
但 C# 的 % 取余结果的符号与被除数相同（负数被除数产生负余数）。
如果需要数学意义的正余数，可以用 ((n % m) + m) % m 来保证结果非负。


适用场景
- 计数和索引：循环计数器、数组索引计算，++ 和 -- 是最常见的操作。
- 分页计算：totalItems / pageSize 计算页数，totalItems % pageSize != 0 判断是否有不完整的最后一页。
- 哈希和散列：hashCode % tableSize 计算桶索引，取余是哈希表的核心操作。
- 时间计算：minutes % 60 取分钟数的余数转小时和分钟。
- 周期性逻辑：游戏中的帧数计数、定时触发，frameCount % interval == 0 判断是否触发。

注意事项
- 整数除法结果是整数，小数直接丢弃（不四舍五入）。如果需要浮点结果，至少让一个操作数是浮点类型。
- 不要在一个表达式中对同一变量多次使用 ++/--（如 a++ + ++a），其结果是未定义行为在旧标准中，在现代 C# 中虽然有确定行为但代码极难读懂，应该避免。
- 浮点比较不要用 ==，要用容差判断（Math.Abs(a - b) < epsilon）。金融计算必须用 decimal，不要用 double。
- 默认情况下整数溢出是静默的（不报错，值环绕），这容易掩盖 bug。在关键计算中使用 checked 上下文提早发现溢出问题。


总结
算术运算符是 C# 中最基础的运算工具，加减乘除取余涵盖了所有基本数学运算。整数除法结果为整数（截断），浮点除法结果为浮点数，取余符号与被除数相同。
自增/自减的前置和后置形式在独立语句中等价，但在表达式中有区别（后置先用再改，前置先改再用）。
浮点数的精度限制决定了货币计算必须用 decimal，浮点数比较必须用容差而非相等判断。
默认的静默溢出行为在需要精确计算的场景中要特别防范。

*/
#endregion