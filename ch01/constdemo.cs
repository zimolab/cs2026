namespace constdemo;

// 演示const常量的用法：

// 概念：
// `const` 声明的是编译时常量，它的值在编译阶段就被确定，并直接嵌入到调用方的 IL 代码中，运行时不存在实际的内存槽。这与 `readonly` 字段（运行时只读）有本质区别。
// `const` 最常见的用途是替代代码中散落的"魔数"（magic number），比如把 `360` 替换成 `const int FullCircleDegrees = 360`，让代码意图更清晰，也方便统一修改。
// 由于 `const` 是编译时嵌入的，它只能是基础数值类型（`int`、`double`、`float`、`long` 等）、`char`、`string`、`bool` 或枚举类型，不能是复杂对象。
// `const` 字段是隐式 `static` 的，不需要也不允许再加 `static` 修饰符。

// 语法：
// 类或结构体中的常量字段
// [访问修饰符] const 类型 常量名 = 常量值;
// 方法中的局部常量
// const 类型 常量名 = 常量值;

// Q&A：

// ** 为什么修改 `const` 后，引用它的外部程序集不更新？**
// `const` 值在编译时被嵌入调用方的 IL，而不是运行时读取。假设类库 A 有 `public const int MaxCount = 100`，
// 项目 B 引用 A 并使用了 `MaxCount`，B 的 IL 里直接存着数字 `100`。如果 A 把值改成 `200` 并只更新了 DLL，
// B 在运行时仍然用的是 `100`。B 必须重新编译才能获取新值。
// 这个特性决定了 `const` 不适合在公共类库 API 中暴露，应改用 `static readonly`。

// **能否声明 `const` 数组？**
// 不能直接用 `const` 声明数组，因为数组是引用类型，其内容不是编译时常量。

// 为什么string是引用类型，但依旧可以声明const string常量？
//  `string` 常量虽然是引用类型，但字符串字面量有特殊的"内部化"（interning）机制，
// 相同内容的字符串字面量在程序中指向同一个对象，`const string` 能够正常工作。

internal class Program
{

    // const 字段：隐式 static，编译时嵌入调用方 IL
    public const double Pi = 3.14159265358979323846;
    public const double E = 2.71828182845904523536;

    public const string ApiKey = "This is a secret API key";

    static void Main(string[] args)
    {

        // 因为const字段是隐式static的，所以可以直接通过类名访问，在生成的IL中，会直接将Pi的值替换到调用位置
        var circleArea = Program.Pi * Math.Pow(10, 2);
        Console.WriteLine($"The area of a circle with radius 10 is {circleArea}");

        // 可以声明一个局部const常量，只在当前方法内有效，通用在调用处用实际值进行替换
        const double FullCircleDegrees = 360;
        var rotationAngle = FullCircleDegrees * 0.5;
        Console.WriteLine($"The rotation angle is {rotationAngle}");
        // FullCircleDegrees = 360; // 这里会报错，因为const不能被修改
    }
}