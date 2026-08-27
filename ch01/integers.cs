namespace integers;

// 演示C#中的整数类型

class Program
{

    static void PrintIntTypesSummary()
    {

        const int keywordPadding = -10;
        const int typePadding = -15;
        const int bytePadding = -25;
        const int rangePadding = -55;
        const int literalPadding = 30;
        const int hlineCount = 150;

        Console.WriteLine(new string('-', hlineCount));
        Console.WriteLine($"| {"type",keywordPadding}|{".NET Type",typePadding}|{"Bytes",bytePadding}|{"Range",rangePadding}|{"Literal",literalPadding} |");
        Console.WriteLine(new string('-', hlineCount));
        // 这个字典的结构是：类型名称（关键字） -> (实际类型，字节数，最大值，最小值，字面量示例)
        // 知识点：
        // 1.typeof(type) 返回指定类型的Type对象
        // 2.sizeof(type) 返回指定类型的字节数
        var intTypes = new Dictionary<string, (Type, object, object, object, object)>
        {
            ["byte"] = (typeof(byte), sizeof(byte), byte.MaxValue, byte.MinValue, 255),
            ["sbyte"] = (typeof(sbyte), sizeof(sbyte), sbyte.MaxValue, sbyte.MinValue, -128),
            ["short"] = (typeof(short), sizeof(short), short.MaxValue, short.MinValue, -32768),
            ["ushort"] = (typeof(ushort), sizeof(ushort), ushort.MaxValue, ushort.MinValue, 65535),
            ["int"] = (typeof(int), sizeof(int), int.MaxValue, int.MinValue, -2147483648),
            ["uint"] = (typeof(uint), sizeof(uint), uint.MaxValue, uint.MinValue, 4294967295U.ToString() + "U"),
            ["uint"] = (typeof(uint), sizeof(uint), uint.MaxValue, uint.MinValue, 4294967295U.ToString() + "U"),
            ["long"] = (typeof(long), sizeof(long), long.MaxValue, long.MinValue, (-9223372036854775808L).ToString() + "L"),
            ["ulong"] = (typeof(ulong), sizeof(ulong), ulong.MaxValue, ulong.MinValue, 18446744073709551615UL.ToString() + "UL"),
            ["nint"] = (typeof(nint), "Depends on platform", "Depends on platform", "N/A", "(nint)-1234"),
            ["nuint"] = (typeof(nuint), "Depends on platform", "Depends on platform", "N/A", "(nuint)1234U"),

        };

        foreach (var (keyworld, value) in intTypes)
        {
            var (netType, byteCount, max, min, literal) = value;
            var rangeStr = $" [ {min}, {max} ] ";
            Console.WriteLine($"| {keyworld,keywordPadding}|{netType,typePadding}|{byteCount,bytePadding}|{rangeStr,rangePadding}|{literal,literalPadding} |");
        }
        Console.WriteLine(new string('-', hlineCount));
    }

    static void Main(string[] args)
    {
        PrintIntTypesSummary();

        // 演示整数类型及其字面量
        // int是最常用的整数类型，其对应.NET类型为System.Int32，字节数4
        int intVal = 12;
        var intVal2 = -1234;
        // 除了十进制还可以使用16进制、8进制、2进制字面量来表示整数
        var intVal3 = 0x1A3F; // 16进制
        var intVal4 = 0b1101011; // 2进制
        var intVal5 = 01234; // 8进制
        Console.Write($"intVal = {intVal}, intVal2 = {intVal2}, intVal3 = {intVal3}, intVal4 = {intVal4}, intVal5 = {intVal5},");
        Console.WriteLine($"maxInt == {int.MaxValue}, minInt == {int.MinValue}");

        // uint，无符号整数类型，对应.NET类型为System.UInt32，字节数4
        // uint的字面量后缀是U
        uint uintVal = 1234567890U;
        Console.WriteLine($"uintVal = {uintVal}, maxUInt == {uint.MaxValue}");

        // long，长整数类型，对应.NET类型为System.Int64，字节数8
        // long的字面量后缀是L
        long longVal = 1234567890123456789L;
        // 从C# 7.0开始，可以使用下划线分隔长整数字面量，提高可读性
        long longVal2 = -9_000_000_000L;
        Console.WriteLine($"longVal = {longVal}, longVal2 = {longVal2}, maxLong == {long.MaxValue}, minLong == {long.MinValue}");

        // ulong，无符号长整数类型，对应.NET类型为System.UInt64，字节数8
        // ulong的字面量后缀是UL
        ulong ulongVal = 1234567890123456789UL;
        Console.WriteLine($"ulongVal = {ulongVal}, maxULong == {ulong.MaxValue}");

        // short，短整数类型，对应.NET类型为System.Int16，字节数2
        short shortVal = 123; // 注意字面量值不要超出该类型能表示的范围
        Console.WriteLine($"shortVal = {shortVal}, maxShort == {short.MaxValue}, minShort == {short.MinValue}");

        // ushort，无符号短整数类型，对应.NET类型为System.UInt16，字节数2
        ushort ushortVal = 12345; // 注意字面量值不要超出该类型能表示的范围
        Console.WriteLine($"ushortVal = {ushortVal}, maxUshort == {ushort.MaxValue}");

        // byete，字节类型，对应.NET类型为System.Byte，字节数1，范围0-255，常用于表示二进制数据
        byte byteVal = 123; // 注意字面量值不要超出该类型能表示的范围
        Console.WriteLine($"byteVal = {byteVal}, maxByte == {byte.MaxValue}");

        // sbyte，无符号字节类型，对应.NET类型为System.SByte，字节数1，范围-128-127
        sbyte sbyteVal = -123; // 注意字面量值不要超出该类型能表示的范围
        Console.WriteLine($"sbyteVal = {sbyteVal}, maxSbyte == {sbyte.MaxValue}, minSbyte == {sbyte.MinValue}");

        // 演示整数类型的溢出
        // 当一个整数类型超出其表示范围时，会发生溢出，默认情况下溢出不会导致程序发生异常
        // 而是产生经典的“回绕”行为，有时这会造成非常隐蔽的bug，但有时也把这种行为作为一种“特性”来利用
        int intMax = int.MaxValue;
        int overflowInt = intMax + 1;
        // 可以看到，当在int类型的最大值一侧溢出后，结果回到了最小值
        Console.WriteLine($"intMax = {intMax}, overflowInt = {overflowInt}");
        // 同样，当在int类型最小值一侧溢出后，结果回到了最大值
        int intMin = int.MinValue;
        overflowInt = intMin - 1;
        Console.WriteLine($"intMin = {intMin}, overflowInt = {overflowInt}");
        // 这就是所谓的“回绕”行为

        // 为了避免溢出回绕带来的潜在问题，C#提供了checked，它会在检测到溢出时抛出OverflowException异常
        try
        {
            checked
            {
                overflowInt = intMax + 1;
            }

        }
        catch (OverflowException e)
        {
            // 由于checked块中的代码发生了整数溢出，所以这里会捕获到OverflowException异常
            Console.WriteLine($"OverflowException occurred: {e.Message}"); // 输出：OverflowException occurred:  Arithmetic operation resulted in an overflow.
        }

        // 可以全局范围内启用checked，这样在任何地方发生整数溢出都会抛出异常，方法是：
        // 在工程文件的csproj文件中添加以下代码：
        // <PropertyGroup>
        //    <CheckForOverflowUnderflow>true</CheckForOverflowUnderflow>
        // </PropertyGroup>

        // 对于file-based应用，在使用dotnet命令时，添加/p:CheckForOverflowUnderflow=true参数，如：
        // dotnet run /path/to/your/program.cs /p:CheckForOverflowUnderflow=true

        // 从字符串转换为整数
        string strVal = "12345";
        int intValFromStr = int.Parse(strVal);
        Console.WriteLine($"intValFromStr = {intValFromStr}");
        // 如果字符串不是一个合法的整数字符串，那么int.Parse会抛出FormatException异常
        try
        {
            intValFromStr = int.Parse("abc123");
        }
        catch (FormatException e)
        {
            Console.WriteLine($"FormatException occurred: {e.Message}"); // 输出：FormatException occurred: The input string 'abc123' was not in a correct format.
        }

        // 也可以在不抛出异常的情况下，使用int.TryParse方法，它会返回一个布尔值，表示转换是否成功，并使用out参数接收转换后的整数
        bool success = int.TryParse("12345", out intValFromStr);
        Console.WriteLine($"success = {success}, intValFromStr = {intValFromStr}");
        // 如果字符串不是一个合法的整数字符串，那么int.TryParse会返回false，并且out参数接收的整数值为0
        success = int.TryParse("abc123", out intValFromStr);
        Console.WriteLine($"success = {success}, intValFromStr = {intValFromStr}");

        // 如果要在转换失败时保持原来的整数值，可以使用临时变量来存储原来的值
        intValFromStr = 12345;
        var tempIntValFromStr = intValFromStr;
        success = int.TryParse("abc123", out intValFromStr);
        // 当success为false时，intValFromStr会被重置为tempIntValFromStr的值，即原来的值
        intValFromStr = success ? intValFromStr : tempIntValFromStr;
        Console.WriteLine($"success = {success}, intValFromStr = {intValFromStr}"); // 输出：success = False, intValFromStr = 12345
    }
}