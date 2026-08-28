// 演示C#中的浮点数类型

// 这里使用顶级语句，关于顶级语句的几个知识点点：
// 1.C# 9.0+
// 2.一个项目中只能有一个使用顶级语句的文件
// 3.任何 using 语句必须位于文件顶部
// 4.如果你声明任何类或其他类型，它们必须位于文件底部


PrintFloatTypes();

// double类型（双精度浮点数）是C#中的默认浮点数类型，该类型占用8个字节，可用于表示[ -1.7976931348623157E+308, 1.7976931348623157E+308 ]范围的浮点数
// 精度为：15~17 位
var doubleValue = 3.14;
Console.WriteLine($"doubleValue = {doubleValue}, {doubleValue.GetType()}");

// float类型（单精度浮点数），占用4个字节，可用于表示[ -3.4028235E+38, 3.4028235E+38 ]范围的浮点数
// 精度为：6~9 位
var floatValue = 3.14F;
Console.WriteLine($"floatValue = {floatValue}, {floatValue.GetType()}");

// decimal类型（十进制浮点数），占用16个字节，可用于表示 [ -79228162514264337593543950335, 79228162514264337593543950335 ] 范围的浮点数
// 精度为：约29 位
var decimalValue = 3.14M;
Console.WriteLine($"decimalValue = {decimalValue}, {decimalValue.GetType()}");


// 有效位数验证
float fPi = 3.141592653589793f;
double dPi = 3.141592653589793;
decimal mPi = 3.1415926535897934395033543951335439503354395M;
Console.WriteLine($"\nfloat pi = {fPi}"); // 输出：float pi = 3.1415927 (约7位有效数字)
Console.WriteLine($"double pi = {dPi}"); // 输出：double pi = 3.141592653589793 (约16位有效数字)
Console.WriteLine($"decimal pi = {mPi}"); //输出：decimal pi = 3.1415926535897934395033543950（约29位有效数字，超过截断）


// float 和 double 采用 IEEE 754 二进制表示，因此无法精确表示某些十进制小数（如 0.1），因为它们在二进制中无限循环。
// 运算速度快，但存在舍入误差。
// decimal 采用 BCD（二进制编码十进制） 或类似方式，可以精确表示十进制小数，但运算较慢，内存占用更大（16 字节）。
// 精度误差示例

float f1 = 0.1f;
double d1 = 0.1;
decimal m1 = 0.1M;

float fSum = f1 + 0.1F;   // 期望 0.3，实际可能 0.300000012
double dSum = d1 + 0.2;    // 期望 0.3，实际 0.30000000000000004
decimal mSum = m1 + 0.2m; // 精确 0.3
Console.WriteLine($"fSum = {fSum}");
Console.WriteLine($"dSum = {dSum}");
Console.WriteLine($"mSum = {mSum}");


// 隐式转换：float -> double（安全转换，不会丢失精度）
double d = 3.14F;
Console.WriteLine($"d = {d}, {d.GetType()}"); // 输出：3.140000104904175, System.Double
// 显式转换：double -> float（不安全转换，可能会丢失精度）
float f = (float)d;
Console.WriteLine($"f = {f}, {f.GetType()}"); // 输出：f = 3.14, System.Single

// decimal 不能隐式与 float/double 互转，必须显式
decimal m = 3.1415926535897932384626433833M;
decimal mFromDouble = (decimal)d; // 可能引发 OverflowException
double dFromDecimal = (double)m;  // 可能丢失精度或溢出
Console.WriteLine($"mFromDouble = {mFromDouble}, {mFromDouble.GetType()}"); // 输出：mFromDouble = 3.14000010490418, System.Decimal
Console.WriteLine($"dFromDecimal = {dFromDecimal}, {dFromDecimal.GetType()}"); //输出：dFromDecimal = 3.141592653589793, System.Double

// 浮点数的比较：不要使用 == 直接比较 float 或 double，而应使用容差（epsilon）方法。
double a = 0.1 + 0.2;
Console.WriteLine($"a = {a}, {a.GetType()}"); // 输出：a = 0.30000000000000004, System.Double
double b = 0.3;
Console.WriteLine($"b = {b}, {b.GetType()}"); // 输出：b = 0.3, System.Double
Console.WriteLine($"a == b: {a == b}"); // 输出：a == b: False
double epsilon = 0.000000000000001; // 容差值，用于比较浮点数是否相等
Console.WriteLine($"a == b within epsilon: {Math.Abs(a - b) < epsilon}"); // 输出：a == b within epsilon: True


// 对于decimal类型，因为可以精确表示十进制小数，因此可以安全地进行比较。
decimal decimal1 = 0.1M + 0.2M;
decimal decimal2 = 0.3M;
Console.WriteLine($"decimal1 = {decimal1}, {decimal1.GetType()}"); // 输出：decimal1 = 0.3, System.Decimal
Console.WriteLine($"decimal2 = {decimal2}, {decimal2.GetType()}"); // 输出：decimal2 = 0.3, System.Decimal
Console.WriteLine($"decimal1 == decimal2: {decimal1 == decimal2}"); // 输出：decimal1 == decimal2: True


// double和float支持特殊值：NaN（Not a Number），PositiveInfinity，NegativeInfinity
// decimal不支持这些特殊值
double nan = double.NaN;
double posInf = double.PositiveInfinity;
double negInf = double.NegativeInfinity;
Console.WriteLine($"nan = {nan}, {nan.GetType()}"); // 输出：nan = NaN, System.Double
Console.WriteLine($"posInf = {posInf}, {posInf.GetType()}"); // 输出：posInf =  ∞, System.Double
Console.WriteLine($"negInf = {negInf}, {negInf.GetType()}"); // 输出：negInf = -∞, System.Double

// 检查是否为特殊值的方法
Console.WriteLine($"IsNaN(nan): {double.IsNaN(nan)}"); // 输出：IsNaN(nan): True
Console.WriteLine($"IsPositiveInfinity(posInf): {double.IsPositiveInfinity(posInf)}"); // 输出：IsPositiveInfinity(posInf): True
Console.WriteLine($"IsNegativeInfinity(negInf): {double.IsNegativeInfinity(negInf)}"); // 输出：IsNegativeInfinity(negInf): True
Console.WriteLine($"IsInfinity(posInf): {double.IsInfinity(posInf)}"); // 输出：IsInfinity(posInf): True
Console.WriteLine($"IsInfinity(negInf): {double.IsInfinity(negInf)}"); // 输出：IsInfinity(negInf): True

// 舍入：decimal 适合需要精确舍入的业务（如货币）。double 和 float 的舍入可能因二进制表示而不如预期。
decimal mVal = 1.2345m;
decimal mRounded = Math.Round(mVal, 2, MidpointRounding.ToEven); // 银行家舍入
decimal mRoundedAway = Math.Round(mVal, 2, MidpointRounding.AwayFromZero); // 四舍五入

double dVal = 1.2345;
double dRounded = Math.Round(dVal, 2); // 可能因二进制误差导致意外

Console.WriteLine($"\ndecimal 舍入 (ToEven): {mRounded}"); // 输出：decimal 舍入 (ToEven): 1.23
Console.WriteLine($"decimal 舍入 (AwayFromZero): {mRoundedAway}"); // 输出：decimal 舍入 (AwayFromZero): 1.23
Console.WriteLine($"double 舍入: {dRounded} (注意可能误差)"); // 输出：double 舍入: 1.23 (注意可能误差)

// 混合运算规则：
// float 和 double 运算时，float 被隐式提升为 double，结果为 double。
// float / double 与 decimal 运算时，必须显式转换，否则编译错误。

// 错误：不能隐式转换 decimal 和 double
// double result = 1.0 + 1.0m; //  error CS0019: 运算符“+”无法应用于“double”和“decimal”类型的操作数

// 正确：显式转换
double resultMix = 1.0 + (double)1.0m; // 精度损失
decimal resultMix2 = (decimal)1.0 + 1.0m; // 精度保留但可能溢出
Console.WriteLine($"resultMix = {resultMix}, {resultMix.GetType()}"); // 输出：resultMix = 2 System.Double
Console.WriteLine($"resultMix2 = {resultMix2}, {resultMix2.GetType()}"); // 输出：resultMix2 = 2.0, System.Decimal")


static void PrintFloatTypes()
{

    const int keywordPadding = -10;
    const int typePadding = -15;
    const int bytePadding = -25;
    const int rangePadding = -70;
    const int literalPadding = 30;
    const int hlineCount = 160;

    Console.WriteLine(new string('-', hlineCount));
    Console.WriteLine($"| {"type",keywordPadding}|{".NET Type",typePadding}|{"Bytes",bytePadding}|{"Range",rangePadding}|{"Literal",literalPadding} |");
    Console.WriteLine(new string('-', hlineCount));
    // 这个字典的结构是：类型名称（关键字） -> (实际类型，字节数，最大值，最小值，字面量示例)
    // 知识点：
    // 1.typeof(type) 返回指定类型的Type对象
    // 2.sizeof(type) 返回指定类型的字节数
    var floatTypes = new Dictionary<string, (Type, object, object, object, object)>
    {
        ["float"] = (typeof(float), sizeof(float), float.MaxValue, float.MinValue, 3.14F.ToString() + "F"),
        ["double"] = (typeof(double), sizeof(double), double.MaxValue, double.MinValue, 3.14),
        ["decimal"] = (typeof(decimal), sizeof(decimal), decimal.MaxValue, decimal.MinValue, 3.14M.ToString() + "M"),

    };

    foreach (var (keyworld, value) in floatTypes)
    {
        var (netType, byteCount, max, min, literal) = value;
        var rangeStr = $" [ {min}, {max} ] ";
        Console.WriteLine($"| {keyworld,keywordPadding}|{netType,typePadding}|{byteCount,bytePadding}|{rangeStr,rangePadding}|{literal,literalPadding} |");
    }
    Console.WriteLine(new string('-', hlineCount));
}
