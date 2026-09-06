#region 

/*

C#中的比较运算符

比较运算符对两个操作数进行比较，返回 bool 类型的结果（true 或 false）。
它们是所有条件判断（if、while、for、三目运算符）的基础。

C# 的比较运算符有几个重要特点：
1.== 和 != 的语义因类型而异：
  - 对于值类型，比较值本身，
  - 对于引用类型默认比较引用，但许多类型如 string、record 重载了它们以比较内容；
2.大小比较运算符（<、>、<=、>=）适用于数值类型和实现了 IComparable<T> 的类型；
3.is 运算符在 C# 7 之后大幅扩展，支持模式匹配，已不只是类型检查工具。

*/

#endregion


#region 基本数值类型的比较

{
  // 整数类型的比较
  int a = 10;
  int b = 20;
  int c = 10;
  uint d = 20U;
  long e = 20;
  ulong f = 20U;
  short g = 20;

  Console.WriteLine(a == b); // False
  Console.WriteLine(a != b); // True
  Console.WriteLine(a == c); // True
  Console.WriteLine(b == d); // True
  Console.WriteLine(b == e); // True

  // 会报error CS0034：运算符“==”对于“int”和“ulong”类型的操作数具有二义性
  // int（有符号 32 位）可以隐式转换为 long、float、double 或 decimal。
  // ulong（无符号 64 位）可以隐式转换为 float、double 或 decimal（但不能转为 long，因为可能溢出）。
  // 两者没有共同的原生整数类型（例如都转成 long 不行，因为 ulong 不能隐式转 long；都转成 ulong 也不行，因为 int 不能隐式转 ulong——但可以显式转）
  // 然而它们都可以隐式转换为 float、double 或 decimal，这就产生了多条候选路径，编译器无法判断该选择哪一个，于是报错。
  // Console.WriteLine(b == f); 
  // 解决办法，手动将二者转到同一类型，再进行比较
  Console.WriteLine((ulong)b == f); // True

  // 这里引出了一条经验：避免直接比较不同类型，如确需比较，先手动统一为精度更高的类型

  Console.WriteLine(b == g); // True

  Console.WriteLine(a < b); // True
  Console.WriteLine(a > b); // False
  Console.WriteLine(a <= b); // True
  Console.WriteLine(a >= b); // False

  // 比较表达式的结果是一个bool类型的值，可以直接赋值给bool类型的变量
  bool isGreaterOrEqual = a >= b;
  Console.WriteLine(isGreaterOrEqual); // False
  bool isEqual = a == b;
  Console.WriteLine(isEqual); // False

  // 老生常谈的浮点数比较问题
  double f1 = 0.1;
  double f2 = 0.2;
  // IEEE 754 浮点数由于二进制表示的精度限制，0.1 + 0.2 的结果是 0.30000000000000004 
  // 而非精确的 0.3，所以 0.1 + 0.2 == 0.3 是 False。
  Console.WriteLine(f1 + f2); // 0.30000000000000004
  Console.WriteLine(f1 + f2 == 0.3); // False

  // 浮点数比较应该用容差判断：Math.Abs(result - target) < epsilon。
  const double epsilon = 1e-10;
  Console.WriteLine(Math.Abs(f1 + f2 - 0.3) < epsilon); // True

  // 只有 decimal 可以安全地用 == 比较精确的十进制小数。
  decimal d1 = 0.1m;
  decimal d2 = 0.2m;
  // 注意比较的目标是0.3m，而不是0.3，0.3是double类型的字面量，
  // 在C#中，运算符“==”无法应用于“decimal”和“double”类型的操作数
  // 这也是C#是强类型的一个例证
  Console.WriteLine(d1 + d2 == 0.3m); // True
}

#endregion


#region 应用类型的比较运算
{
  // 自定义class是典型的引用类型
  // 在Point类中，没有重载==运算符
  var point1 = new Point { X = 1, Y = 2 };
  var point2 = new Point { X = 1, Y = 2 };
  var point3 = point1;

  // 对于引用类型，没有重载==运算的情况下，==默认比较的是引用（地址），而不是内容
  Console.WriteLine(point1 == point2); // False，因为point1和point2引用的是两个不同的对象，虽然内容相同
  Console.WriteLine(point1 == point3); // True，因为point3与point1引用了同一个对象
  // 如果明确需要比较引用，可以使用ReferenceEquals，语义上更加没有歧义
  Console.WriteLine(ReferenceEquals(point1, point2)); // False
  Console.WriteLine(ReferenceEquals(point1, point3)); // True

  // 对于record class，会自动重载==运算符，实现值相等的语义
  var record1 = new PointRecord(1, 2);
  var record2 = new PointRecord(1, 2);
  var record3 = record1;

  //虽然record1和record2引用的是两个不同的对象，但内容相同，而record class自动重载了==运算符，用于比较内容，而非引用
  Console.WriteLine(record1 == record2); // True
  // 如果明确需要比较引用，可以使用ReferenceEquals
  Console.WriteLine(ReferenceEquals(record1, record2)); // False
  Console.WriteLine(ReferenceEquals(record1, record3)); // True

  // 与null的比较
  string? maybeNull = null;

  Console.WriteLine(maybeNull == null); // True
  Console.WriteLine(maybeNull != null); // False
  // 推荐的写法：使用is运算符，更清晰，无法通过重载==运算符绕过
  Console.WriteLine(maybeNull is null); // True
  Console.WriteLine(maybeNull is not null); // False

  // 对于值类型，默认不能为null，如果需要为null，需要显式声明为Nullable<T>，或者使用Nullable<T>的语法糖T?
  int? nullableInt = null;
  Console.WriteLine(nullableInt == null); // True
  Console.WriteLine(nullableInt != null); // False
  Console.WriteLine(nullableInt is null); // True
  Console.WriteLine(nullableInt is not null); // False
  // 还可以使用HasValue属性来检查Nullable<T>是否为null
  Console.WriteLine(nullableInt.HasValue); // False
  nullableInt = 10;
  Console.WriteLine(nullableInt.HasValue); // True

  // 对于非可空数值类型，与null的比较总是返回false（当然null == null 返回的是true）
  int num = 10;
  Console.WriteLine($"num >= null == {num >= null}"); // null >= null == False
  Console.WriteLine($"num >= null == {num <= null}"); // null <= null == False



  // is运算符的模式匹配
  object[] items = [42, "hello", 3.14, null, new List<int>()];
  foreach (object? item in items)
  {
    if (item is int i)
    {
      Console.WriteLine($"Found an int: {i}");
    }
    else if (item is string s)
    {
      Console.WriteLine($"Found a string: {s}");
    }
    else if (item is double d)
    {
      Console.WriteLine($"Found a double: {d}");
    }
    else if (item is null)
    {
      Console.WriteLine($"Found a null");
    }
    else if (item is List<int> list)
    {
      Console.WriteLine($"Found a list of integers: {list}");
    }
    else
    {
      Console.WriteLine($"Found an unknown type: {item}");
    }
  }

  // 关系模式（C# 9）：和值进行比较
  int score = 85;
  // switch表达式
  string grade = score switch
  {
    >= 90 => "A",
    >= 80 => "B",
    >= 70 => "C",
    >= 60 => "D",
    _ => "F"
  };
  Console.WriteLine($"The grade for {score} is {grade}"); // The grade for 85 is B

  // 字符串的比较
  // string是一种引用类型，但是它是一种特殊的引用类型，在使用上表现出值类型的特性
  // 一是它是不可变的引用类型，每次修改（例如拼接）都会创建一个新的对象，而不是修改原对象
  // 二是它重载了==运算符，用于比较内容，而不是引用
  // 对于同一个字符串字面量，编译器会优化，同一个字符串字面量只会创建一个对象
  string str1 = "hello";
  string str2 = "world";
  string str3 = "hello";
  string str4 = str1;
  Console.WriteLine(str1 == str2); // False
  Console.WriteLine(str1 == str3); // True
  Console.WriteLine(str1 == str4); // True
  // 对于同一个字符串字面量，编译器会优化，同一个字符串字面量只会创建一个对象
  Console.WriteLine(ReferenceEquals(str1, str3)); // True
  Console.WriteLine(ReferenceEquals(str1, str4)); // True

  // 如果比较字符串时需要忽略大小写，使用string.Equals方法
  Console.WriteLine(string.Equals(str1, "HEllo", StringComparison.OrdinalIgnoreCase)); // True

  // 排序比较，返回三种情况：-1、0、1
  // 返回-1表示第一个参数小于第二个参数，0表示相等，1表示第一个参数大于第二个参数
  // 一般是安装字典序来排大小
  string apple = "apple";
  string banana = "banana";
  // apple的第一个字符'a'在banana的第一个字符'b'前面，所以apple小于banana
  Console.WriteLine(string.Compare(apple, banana, StringComparison.Ordinal)); // -1

  // 数字字符串的"比较陷阱"
  // 字符串比较是字典序，不是数值顺序
  var strings = new[] { "10", "9", "100", "2" };
  Array.Sort(strings);  // 字典序排序
  Console.WriteLine(string.Join(", ", strings));  // "10, 100, 2, 9"（字典序）

  var numbers = new[] { 10, 9, 100, 2 };
  Array.Sort(numbers);  // 数值顺序排序
  Console.WriteLine(string.Join(", ", numbers));  // 2, 9, 10, 100（数值顺序）
}
#endregion

#region

// 自定义数据类型，top-level的写法下要放到最后
class Point
{
  public int X { get; set; }
  public int Y { get; set; }
}

// 定义一个record class
// record class会自动重载==运算符，实现值相等的语义
record PointRecord(int X, int Y);

#endregion


#region 总结

/*
常见问题
Q:为什么两个内容一样的字符串用 == 比较结果是 True？

A:因为 string 重载了 == 运算符，使它比较字符串内容而非引用。
这是微软的刻意设计，字符串在使用上具有值语义，所以让 == 表现得和值类型一样。
如果需要检查是否是同一个字符串对象（引用相等），用 ReferenceEquals(s1, s2)。
但要注意对于两个内容一样的字符串字面量，编译器实际上只会创建一个对象。

Q:is null 和 == null 有什么区别？

A:大多数情况下等价，但 is null 更安全。is null 是一种模式，不会被 == 运算符重载绕过。
如果某个类型重载了 ==，在 == null 时可能执行自定义逻辑（甚至不检查真正的 null），
而 is null 始终是对引用的真实空值检查。推荐在可空性检查中优先使用 is null / is not null。

Q:浮点数为什么不能用 == 比较？

A:IEEE 754 浮点数由于二进制表示的精度限制，0.1 + 0.2 的结果是 0.30000000000000004 而非精确的 0.3，
所以 0.1 + 0.2 == 0.3 是 False。浮点数比较应该用容差判断：Math.Abs(a - b) < epsilon。epsilon一般是一个很小的数，如1e-10。
只有 decimal 可以安全地用 == 比较精确的十进制小数。

适用场景
- 条件判断：if/while/三目表达式中的条件，所有条件最终都是 bool，大量依赖比较运算符。
- 数据验证：年龄范围（age >= 0 && age <= 150）、分数区间、字符串非空检查。
- 排序：实现 IComparable<T> 接口时，<、>、== 是 CompareTo 方法返回值的基础。
- 模式匹配：is 运算符的关系模式、类型模式结合 switch 表达式，是现代 C# 处理复杂条件的标准方式。
- 集合查找：LINQ 的 Where(x => x.Age > 18) 等条件，本质都是比较运算符的组合。


注意事项：
- 重载 == 时必须同时重载 !=，并且应该同时覆盖 Equals() 和 GetHashCode()，
保持一致性。如果只重载了 == 而没有覆盖 Equals()，Dictionary 和 HashSet 等基于哈希的集合会出现不一致行为。

- 在 LINQ 中对字符串用 == 比较时，是否区分大小写取决于具体的数据源：内存中的 LINQ 用 C# 的 ==（区分大小写），
数据库 LINQ（EF Core）可能映射到 SQL 的 =（数据库的大小写规则）。

- null 参与比较运算（<、>、<=、>=）时，结果是 false（除了可空值类型 T? 的特殊规则）。null == null 是 true。

- C# 的比较运算符不支持链式比较，1 < a < 10 不合法（在 Python 中合法但 C# 不行）。
应该写成 a > 1 && a < 10。

总结：
1.比较运算符返回 bool 值，是所有条件逻辑的基础。
2.== 和 != 的语义依赖类型：值类型比较值，引用类型默认比较引用，但 string 和 record 重载为内容比较。
3.is null 比 == null 更安全可靠，推荐优先使用。
4.浮点数的 == 比较不可靠，要用容差判断。
5.is 运算符在模式匹配中已远超简单类型检查，支持关系模式、类型模式等，
与 switch 表达式结合是处理复杂分支逻辑的现代方式。

*/

#endregion