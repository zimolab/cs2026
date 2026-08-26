namespace variabledemo2;

// 演示var类型推断

// `var` 是 C# 3.0 引入的隐式类型局部变量关键字，让编译器根据右侧的初始化表达式自动推断变量类型。
// 它是纯粹的编译时特性，对运行时没有任何影响——使用 `var` 声明的变量和显式声明完全等价，
// 生成的 IL 代码完全相同。变量的类型一旦推断出来就是固定的，后续不能改变。
// `var` 的价值在于消除冗余的类型书写，在泛型、LINQ 和匿名类型等场景下尤其明显。

// 语法结构：
// var 声明：必须在声明时赋值（因为编译器需要右侧值来推断类型）
// var 变量名 = 初始化表达式;

// 不合法的用法（编译报错）
// var x;           // 无法推断类型
// var x = null;    // null 没有类型，无法推断（除非强制转换），如下：
// var x = (string)null;  // 合法：推断为 string


// `var` 只能用于**局部变量**，不能用于字段、属性、方法参数、返回值类型。
// `var`在泛型、LINQ 和匿名类型场景下是必要或推荐的选择。

internal class Program
{
    static void Main(string[] args)
    {
        // 显式类型声明
        int explictInt = 10;
        Console.WriteLine($"explictInt.GetType() == {explictInt.GetType()}"); // 输出：System.Int32
        // var自动类型推动
        var inferedInt = 10; // var 声明，编译器推断类型为 int
        var inferedDouble = 10.5; // var 声明，编译器推断类型为 double
        var inferedFloat = 10.5f;  // var 声明，编译器推断类型为 float
        var inferedString = "Hello"; // var 声明，编译器推断类型为 string
        var inferedBool = true; // var 声明，编译器推断类型为 bool
        var inferedChar = 'A'; // var 声明，编译器推断类型为 char
        var inferedShort = (short)10; // var 声明，编译器推断类型为 short
        var inferedByte = (byte)10; // var 声明，编译器推断类型为 byte
        var inferedSbyte = (sbyte)10; // var 声明，编译器推断类型为 sbyte
        var inferedUshort = (ushort)10; // var 声明，编译器推断类型为 ushort
        var inferedLong = 1_000_000_000L; // var 声明，编译器推断类型为 long
        var inferedULong = 1_000_000_000UL; // var 声明，编译器推断类型为 ulong
        var inferedDecimal = 10.5M; // var 声明，编译器推断类型为 decimal
        var inferedComplex = new System.Numerics.Complex(1, 2); // var 声明，编译器推断类型为 System.Numerics.Complex
        var inferedList = new List<int>(); // var 声明，编译器推断类型为 List<int>
        var inferedDictionary = new Dictionary<string, int>(); // var 声明，编译器推断类型为 Dictionary<string, int>
        var inferedTuple = (1, "Hello", 3.14); // var 声明，编译器推断类型为 VarTuple<int, string, double>
        Console.WriteLine($"inferedInt.GetType() == {inferedInt.GetType()}"); // 输出：System.Int32
        Console.WriteLine($"inferedDouble.GetType() == {inferedDouble.GetType()}"); // 输出：System.Double
        Console.WriteLine($"inferedFloat.GetType() == {inferedFloat.GetType()}"); // 输出：System.Single
        Console.WriteLine($"inferedString.GetType() == {inferedString.GetType()}"); // 输出：System.String
        Console.WriteLine($"inferedBool.GetType() == {inferedBool.GetType()}"); // 输出：System.Boolean
        Console.WriteLine($"inferedChar.GetType() == {inferedChar.GetType()}"); // 输出：System.Char
        Console.WriteLine($"inferedShort.GetType() == {inferedShort.GetType()}"); // 输出：System.Int16
        Console.WriteLine($"inferedByte.GetType() == {inferedByte.GetType()}"); // 输出：System.Byte    
        Console.WriteLine($"inferedSbyte.GetType() == {inferedSbyte.GetType()}"); // 输出：System.SByte
        Console.WriteLine($"inferedUshort.GetType() == {inferedUshort.GetType()}"); // 输出：System.UInt16
        Console.WriteLine($"inferedLong.GetType() == {inferedLong.GetType()}"); // 输出：System.Int64       
        Console.WriteLine($"inferedULong.GetType() == {inferedULong.GetType()}"); // 输出：System.UInt64
        Console.WriteLine($"inferedDecimal.GetType() == {inferedDecimal.GetType()}"); // 输出：System.Decimal
        Console.WriteLine($"inferedComplex.GetType() == {inferedComplex.GetType()}"); // 输出：System.Numerics.Complex
        Console.WriteLine($"inferedList.GetType() == {inferedList.GetType()}"); // 输出：System.Collections.Generic.List`1[System.Int32]    
        Console.WriteLine($"inferedDictionary.GetType() == {inferedDictionary.GetType()}"); // 输出：System.Collections.Generic.Dictionary`2[System.String,System.Int32]
        Console.WriteLine($"inferedTuple.GetType() == {inferedTuple.GetType()}"); // 输出：VarTuple`3[System.Int32,System.String,System.Double]

        // var的用途二：LINQ 查询必须用 var（匿名类型无法显式命名）
        var people = new[]
        {
            new {name="Alice", age=25},
            new {name="Bob", age=30},
            new {name="Charlie", age=35}
        };
        Console.WriteLine($"people.GetType() == {people.GetType()}"); // 是一个匿名类型：<>f__AnonymousType0`2[System.String,System.Int32][]

        // LINQ 查询结果是 IEnumerable<匿名类型>，无法显式写出类型名
        // 此处必须用 var
        var query = people.Where(p => p.age > 25).Select(p => p.name);
        Console.WriteLine($"query.GetType() == {query.GetType()}"); // 输出： System.Linq.Enumerable+IEnumerableSelectIterator`2[<>f__AnonymousType0`2[System.String,System.Int32],System.String
        var names = query.ToList(); // 将 IEnumerable<匿名类型> 转换为 List<string>
        Console.WriteLine($"names.GetType() == {names.GetType()}"); // 输出: System.Collections.Generic.List`1[System.String]

        // var的用途三：在foreach循环中，可以不用显式声明变量类型
        // 因为names是一个 List<string>，编译器可以推断出循环变量name的类型为 string
        foreach (var name in names)
        {
            Console.WriteLine($"name == {name}"); // 输出：Alice Bob Charlie
        }

        // var的用途四：配合C#12引入的集合表达式
        // 以Array和List为例，指定类型的写法
        int[] arr = [1, 2, 3, 4, 5];
        List<int> list = [1, 2, 3, 4, 5];
        Console.WriteLine($"arr.GetType() == {arr.GetType()}"); // 输出：System.Int32[]
        Console.WriteLine($"list.GetType() == {list.GetType()}"); // 输出：System.Collections.Generic.List`1[System.Int32]

        // 使用var自动类型推断
        var arr2 = new int[] { 1, 2, 3, 4, 5 };
        var list2 = new List<int> { 1, 2, 3, 4, 5 };
        Console.WriteLine($"arr2.GetType() == {arr2.GetType()}"); // 输出：System.Int32[]
        Console.WriteLine($"list2.GetType() == {list2.GetType()}"); // 输出：System.Collections.Generic.List`1[System.Int32]

    }
}