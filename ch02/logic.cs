#region 逻辑运算符

/*
逻辑运算符用于组合或反转 bool 值，是构建复合条件的基础工具。
C# 提供了三种主要的逻辑运算符：&&（逻辑与）、||（逻辑或）、!（逻辑非），以及 ^（异或）。

理解逻辑运算符的关键在于短路求值（Short-circuit Evaluation）行为：
- && 的左侧为 false 时，右侧不执行（因为结果已经确定是 false）；
- || 的左侧为 true 时，右侧不执行（因为结果已经确定是 true）。

这不只是性能优化，更是避免空引用异常和副作用的重要机制。

C# 还提供了非短路版本 &（按位与）和 |（按位或），它们对 bool 类型也适用，但两侧始终都会求值。


+----------+----------+------------------------------------+------------------------------+
| 运算符   | 名称     | 说明                               | 短路                         |
+----------+----------+------------------------------------+------------------------------+
| &&       | 逻辑与   | 两者都为 true 才为 true            | 是（左 false 跳过右）       |
| ||       | 逻辑或   | 至少一个为 true 即为 true          | 是（左 true 跳过右）        |
| !        | 逻辑非   | 取反                               | 无                           |
| ^        | 逻辑异或 | 两者不同时为 true                  | 否（两侧都求值）            |
| &        | 非短路与 | 与 && 逻辑相同，但两侧都求值       | 否                           |
| |        | 非短路或 | 与 || 逻辑相同，但两侧都求值       | 否                           |
+----------+----------+------------------------------------+------------------------------+

*/

#endregion


#region 基本示例
{

    // && 逻辑与：两者都 true 才 true
    Console.WriteLine(true && true);   // True
    Console.WriteLine(true && false);  // False
    Console.WriteLine(false && true);  // False
    Console.WriteLine(false && false); // False

    // || 逻辑或：至少一个 true 即 true
    Console.WriteLine(true || false);  // True
    Console.WriteLine(false || true);  // True
    Console.WriteLine(false || false); // False

    // ! 逻辑非：取反
    Console.WriteLine(!true);   // False
    Console.WriteLine(!false);  // True

    // ^ 逻辑异或：两者不同时为 true
    Console.WriteLine(true ^ false);  // True（不同）
    Console.WriteLine(true ^ true);   // False（相同）
    Console.WriteLine(false ^ false); // False（相同）

}
#endregion

#region 短路求值

{
    string content = null;
    // 错误的写法，不作空检查总结访问对象成员
    // var hasContent = content.Length > 0; // 报NullReferenceException

    // 正确的写法，使用&&短路求值，当左侧操作数（可以是一个表达式，比如：content is not null）为 false 时，右侧表达式不会执行
    // 这样可以确保不会在变量为null时访问其成员，避免空引用异常
    // 逻辑表达式的结果是一个bool值，对于&&，只有两个操作数都为true时，结果才为true，否则为false
    // 这里第一个操作数（content is not null）已经是false了，所以不必执行第二个操作数（content.Length > 0），整个表达式直接返回false
    var hasContent = content is not null && content.Length > 0;
    Console.WriteLine(hasContent); // False



    content = "Hello, World!"; // 让content不为null
    hasContent = content is not null && content.Length > 0;  // 因为左侧操作数为true，所以会执行右侧操作数，又因为conent.Length > 0，所以右侧操作数也为true，最终整个表达式结果为true
    Console.WriteLine(hasContent); // True

    // 让content指向一个空字符串，此时content不为null，但content.Length == 0
    content = ""; // 让content不为null
    hasContent = content is not null && content.Length > 0;  // 因为左侧操作数为true，所以会执行右侧操作数，又因为conent.Length == 0，所以右侧操作数也为true，最终整个表达式结果为false
    Console.WriteLine(hasContent); // False


    // || 的短路：提供默认值
    string? name = null;
    // 如果 name 为 null，使用 GetDefaultName()
    // 短路：name 有值时 GetDefaultName() 不会被调用
    string displayName = !string.IsNullOrEmpty(name) ? name : GetDefaultName();
    // 更简洁的等价写法（使用 ?? 运算符）
    // displayName = name ?? GetDefaultName();
    Console.WriteLine(displayName); // Guest

    static string GetDefaultName()
    {
        Console.WriteLine("GetDefaultName called");  // 用于演示短路效果
        return "Guest";
    }


    // 逻辑运算符的优先级（从高到低）：! > && > ||
    // 建议：只要有混合运算，就加括号，比记忆优先级更可靠

    int age = 25;
    bool hasLicense = true;
    bool hasCar = false;

    // 复合条件：! 优先级最高，然后 &&，然后 ||
    // 以下等价于：(age >= 18 && hasLicense) || hasCar
    bool canDrive = age >= 18 && hasLicense || hasCar;
    Console.WriteLine(canDrive);  // True

    // 加括号明确意图（推荐：不要依赖优先级记忆）
    bool canDrive2 = (age >= 18 && hasLicense) || hasCar;
    Console.WriteLine(canDrive2);  // True（与上面等价）

    // 短路副作用的控制
    int callCount = 0;

    bool IsCalled()
    {
        callCount++;  // 追踪调用次数
        return true;
    }

    // && 左侧为 false，右侧 IsCalled() 不执行
    callCount = 0;
    bool result1 = false && IsCalled();
    Console.WriteLine($"callCount={callCount}");  // callCount=0（短路，未调用）

    // || 左侧为 true，右侧 IsCalled() 不执行
    callCount = 0;
    bool result2 = true || IsCalled();
    Console.WriteLine($"callCount={callCount}");  // callCount=0（短路，未调用）

    // 非短路 &：两侧都执行
    callCount = 0;
    bool result3 = false & IsCalled();
    Console.WriteLine($"callCount={callCount}");  // callCount=1（无短路，调用了）

}

#endregion

#region 总结

/*
常见问题
Q：&& 和 & 的区别，什么时候用 &？
A: && 有短路，左侧为 false 时右侧不执行；& 无短路，两侧都执行。
对于 bool 类型，99% 的情况用 &&，因为短路是预期行为。
& 只在你明确需要两侧都执行（比如两侧都有副作用且副作用都需要发生）时才用，这种情况很罕见。& 更常见的用途是位运算（对整数类型）。

Q: ! 运算符有多种使用方式吗？

A: C# 中，! 有两种用法：
1.逻辑非运算符：!condition（用于 bool）
2.null 包容运算符：expression!（后缀，告诉编译器表达式不为 null，用于 NRT 检查）

bool isOk = !hasError;    // 逻辑非
string s = GetMaybeNull()!;  // null 包容，告诉编译器 GetMaybeNull() 结果不为 null

Q: !isActive 和 isActive == false 哪个更好？

A:!isActive 更好。isActive == false 是冗余写法，bool 类型直接用逻辑非即可。
但注意：代码逻辑复杂时，!someComplexCondition 不一定比 someComplexCondition == false 更清晰，这取决于条件本身的可读性。

适用场景：

- 守卫条件（Guard Clause）：方法开头用 && 和 ! 做早返回，减少嵌套层级。

- 空值安全链：obj != null && obj.Property != null && obj.Property.Value > 0，
通过短路安全遍历属性链（现在也可以用 ?. 运算符更简洁地实现）。

- 权限和访问控制：多个条件的组合判断，isAdmin || (isOwner && isActive)。

- LINQ 过滤：Where(x => x.IsActive && x.Age >= 18) 中的多条件过滤。

- 异或用于切换状态：isVisible ^= true 相当于反转布尔值，等价于 isVisible = !isVisible（但后者更清晰）。


注意事项
- 不要在单个表达式中写过于复杂的逻辑，超过 3 个逻辑运算符的条件就应该考虑提取到命名变量或方法中，提升可读性。
- 短路求值使右侧表达式不执行时，右侧的方法调用、赋值、自增等副作用也不会发生。
不要依赖这种副作用来控制程序流程（比如 flag || DoSomethingImportant()），这样的代码很难读懂。
- 逻辑运算符的优先级：! > && > ||。混合使用时，加括号比记忆优先级更安全。
- 在可空 bool? 类型上，逻辑运算有三值逻辑规则（null && false = false，null || true = true 等），与普通 bool 不同，处理时需要额外注意。


总结
逻辑运算符 &&、||、! 是组合条件判断的核心工具。
短路求值是它们最重要的特性：&& 左侧为 false 时右侧不执行，|| 左侧为 true 时右侧不执行。
这个特性不只是性能优化，更是安全访问可能为 null 的对象的关键机制。
& 和 | 是非短路版本，对 bool 类型的使用场景极少。
复杂的逻辑条件应该加括号或提取到命名变量，而不是依赖运算符优先级规则。

*/


#endregion