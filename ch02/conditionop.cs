#region 

/*
条件运算符（也叫三目运算符或三元运算符）?: 是 C# 中唯一一个接收三个操作数的运算符，
格式为 条件 ? 真值 : 假值。它是 if-else 表达式形式的简写，在一行代码内根据条件返回两个值之一。

条件运算符的核心优势是：它是一个表达式，有返回值。这意味着可以在赋值、方法调用参数、字符串插值等任何期望表达式的地方使用它，
而 if-else 语句则做不到（语句没有返回值）。

条件运算符同样有短路特性：如果条件为 true，假值部分不求值；条件为 false，真值部分不求值。

条件运算符的语法如下：
var result = condition ? trueValue : falseValue

支持嵌套，但不建议，因为可读性差
var result = condition1 ? (condition2 ? trueValue2 : falseValue2) : falseValue1;

*/


#endregion

#region 

// 本示例代码采用传统的类形式而非top-level
static class Program
{
    public static void Main()
    {
        // 基本示例
        int score = 85;
        string status = score >= 60 ? "Pass" : "Fail";
        Console.WriteLine(status);

        // 等价的 if-else 语句
        if (score >= 60)
        {
            status = "Pass";
        }
        else
        {
            status = "Fail";
        }
        Console.WriteLine(status);

        // 条件表达式作为一个表达式是有值的，可以在任何需要表达式的地方使用
        // 比如在字符串插值中，不过记住使用圆括号把整个表达式括起来
        string message = $"Your score is {score} and you are {(score >= 60 ? "Passed" : "Failed")}.";
        Console.WriteLine(message); // 输出: Your score is 85 and you are Passed.

        // 在返回语句中（不过我感觉swith表达式更合适一些），实质上是条件表达式的嵌套
        static string GetGrade(int s) =>
            s >= 90 ? "A" :
            s >= 80 ? "B" :
            s >= 70 ? "C" :
            s >= 60 ? "D" : "F";
        Console.WriteLine(GetGrade(85)); // 输出: B

        // 等价的switch表达式
        static string GetGrade2(int s2) => s2 switch
        {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            >= 60 => "D",
            _ => "F"
        };
        Console.WriteLine(GetGrade2(85)); // 输出: B

        // 条件运算符的两个分支必须是同类型（或者能够隐式转换为同类型）
        int a = 10;
        int b = 20;
        double c = 0.5;

        // 两个分支为同类型
        int max = a > b ? a : b;
        Console.WriteLine(max); // 输出: 20

        // 两个分支为不同类型，但可以隐式转换为同类型
        // a是int类型，c是double类型，int被隐式提升为double类型
        double min = a < c ? a : c;
        Console.WriteLine(min); // 输出: 0.5

        // 应用类型：两个分支可以是不同的子类型，结果是两个子类型的公共父类型
        int age = 18;
        object obj = age > 18 ? "Adult" : age;
        Console.WriteLine(obj); // 输出: 18

        // 条件运算符也有短路特性
        static int SideEffect(string label, int value)
        {
            Console.WriteLine($"Processing {label} with value {value}");
            return value * value;
        }
        bool condition = true;
        int val = condition
        ? SideEffect("A", 10) // 如果condition为true，执行SideEffect("A", 10)，输出: Processing A with value 10，返回100
        : SideEffect("B", 20); // 该分支则不执行
        Console.WriteLine(val); // 输出100

        // 调用CalculateDiscount方法
        Console.WriteLine(CalculateDiscount(100m, true, 150));
        Console.WriteLine(CalculateDiscount(100m, false, 150));
        Console.WriteLine(CalculateDiscount(100m, true, 50));
        Console.WriteLine(CalculateDiscount(100m, false, 50));

    }

    // 实际示例，根据membership、quantity计算折扣率
    static decimal CalculateDiscount(decimal price, bool membership, int quantity)
    {
        // 会员折扣
        decimal discountRate = membership ? 0.1m : 0m;

        // 数量折扣
        decimal quantityDiscount =
           quantity >= 100 ? 0.05m : // 如果quantity >= 100，折扣率为5%
           quantity >= 50 ? 0.03m : // 如果quantity >= 50，折扣率为3%
           quantity >= 20 ? 0.02m : // 如果quantity >= 20，折扣率为2%
           quantity >= 10 ? 0.01m : 0m; // 如果quantity >= 10，折扣率为1%，如果quantity < 10，折扣率为0%

        // 计算折扣率，取折扣率大的
        discountRate = discountRate > quantityDiscount ? discountRate : quantityDiscount;

        // 计算最终金额
        return price * (1 - discountRate);
    }


}

#endregion

#region 

/*
Q: 条件运算符可以嵌套吗，有没有层数限制？

A: 语法上可以无限嵌套，但超过 2 层后可读性急剧下降。
最多建议 2-3 层嵌套，超过后应改用 switch 表达式（C# 8+）或方法抽取：

// 3 层嵌套，还能接受
string level = score >= 90 ? "A" : score >= 80 ? "B" : "C";

// 改为 switch 表达式，更清晰
string level2 = score switch
{
    >= 90 => "A",
    >= 80 => "B",
    _ => "C"
};

Q: 两个分支的类型不同会怎样？

A: 编译器会尝试找到一个公共类型，将两个分支都能隐式转换为该类型。
如果找不到公共类型（如 int 和 string），会编译报错。此时需要显式转换：condition ? (object)intVal : (object)stringVal。

Q: 条件运算符的优先级高还是低？

A: 条件运算符的优先级很低，几乎在所有二元运算符之后。
字符串插值中使用时，需要加括号：$"Result: {(flag ? "yes" : "no")}" 中的括号是必须的，
否则编译器会对 {flag ? "yes" : "no"} 的解析报错。

适用场景
1.单行赋值根据条件决定值：最典型的用法，当赋值逻辑简单时，三目运算符比 if-else 更简洁。
2.表达式上下文（方法参数、属性表达式体、字符串插值）：if-else 语句无法在这些地方使用，必须用三目运算符。
3.简单的二选一逻辑：条件清晰、两个分支各只有一个简单表达式的情况。
4.配合 switch 表达式（C# 8+）：多分支逻辑使用 switch 表达式替代嵌套三目，
C# 14 中 switch 表达式已成为处理多分支值选择的标准。

注意事项
1.不要为了"显得简洁"而过度使用条件运算符。如果条件本身很长，或者两个分支的表达式很复杂，if-else 的可读性反而更好。

2.在字符串插值 $"..." 中使用条件运算符，必须用括号括起来，否则 : 会被误解析为格式说明符。

3.条件运算符的结果类型在编译时确定，两个分支的值必须能转换为同一类型。
如果用于 var 赋值，编译器会推断公共类型：var x = flag ? 1 : 1.5;（x 推断为 double）。

4.C# 的条件运算符不支持类似 Python 的"安全三目"写法（Python 中 x if x is not None else default 可以安全处理 None），
在 C# 中应该用 ?? 处理 null 情况，用 ?: 处理其他条件。

总结：
条件运算符 ?: 是 if-else 的表达式形式，在一行中根据布尔条件返回两个值之一。
它的核心价值在于"表达式"的本质——可以在赋值、方法参数、字符串插值等任何需要表达式的地方使用。
短路特性确保只有满足条件的分支才被求值。简单的二选一场景用 ?:，
多分支场景用 switch 表达式，复杂逻辑用 if-else，不要为了简短而牺牲可读性。

*/

#endregion