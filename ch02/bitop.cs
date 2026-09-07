#region C#中的位运算符


/*
位运算符直接操作整数在内存中的二进制位。它们在底层系统编程、网络协议解析、权限标志位处理和高性能数值计算中非常有用。

C# 支持按位与（&）、按位或（|）、按位异或（^）、按位取反（~）、左移（<<）、右移（>>）以及 C# 11 引入的无符号右移（>>>）。

位运算的前提是理解整数的二进制表示：int 是 32 位有符号整数，最高位是符号位（0 正 1 负）。
位运算在这些二进制位上逐位操作，不考虑数值意义，只关注每一位的 0/1 状态。

在日常开发中，位运算最常见的用途是：用 [Flags] 枚举实现权限组合（多个权限合并到一个整数中），
处理颜色值（ARGB 通道）、网络掩码等需要位操作的数据格式。

+----------+--------------+-------------+------------------------------------------------------+
| 运算符   | 名称         | 示例        | 说明                                                 |
+----------+--------------+-------------+------------------------------------------------------+
| &        | 按位与       | a & b       | 对应位都为 1 才为 1                                   |
| |        | 按位或       | a | b       | 对应位至少一个为 1 即为 1                             |
| ^        | 按位异或     | a ^ b       | 对应位不同则为 1                                     |
| ~        | 按位取反     | ~a          | 所有位取反（0→1，1→0）                               |
| <<       | 左移         | a << n      | 所有位向左移 n 位，右侧补 0                          |
| >>       | 有符号右移   | a >> n      | 所有位向右移 n 位，左侧补符号位                      |
| >>>      | 无符号右移   | a >>> n     | 所有位向右移 n 位，左侧补 0（C# 11+）               |
+----------+--------------+-------------+------------------------------------------------------+

*/

#endregion


#region 基本位运算

{
    int a = 0b_1010_1100; //十进制：172
    int b = 0b_0110_1010; //十进制106

    // 按位与(&)，对应位都为1才为1，否则为20
    int result = a & b; // 0b_0010_1000，十进制：40
    Console.WriteLine($"a & b = {result} (0b{Convert.ToString(result, 2).PadLeft(8, '0')})");

    // 按位或(|)，对应位至少一个为1即为1，否则为0
    result = a | b; // 0b_1110_1110，十进制：238
    Console.WriteLine($"a | b = {result} (0b{Convert.ToString(result, 2).PadLeft(8, '0')})");

    // 按位异或(^)，对应位不同则为1，否则为0
    result = a ^ b; // 0b_1100_0110，十进制：198
    Console.WriteLine($"a ^ b = {result} (0b{Convert.ToString(result, 2).PadLeft(8, '0')})");

    // 按位取反（~），所有位取反（0→1，1→0）
    result = ~a; // 0b11111111111111111111111101010011，十进制：-173（补码表示）
    Console.WriteLine($"~a = {result} (0b{Convert.ToString(result, 2).PadLeft(8, '0')})");
}

#endregion

#region 移位运算

{
    int value = 0b_0000_0101; //十进制：5
    int leftShift = value << 1; // 左移1位，右侧补0，0b_0000_1010，十进制：10
    Console.WriteLine($"value << 1 = {leftShift} (0b{Convert.ToString(leftShift, 2).PadLeft(8, '0')})");
    leftShift = value << 2; // 左移2位，右侧补0，0b_0001_0100，十进制：20
    Console.WriteLine($"value << 2 = {leftShift} (0b{Convert.ToString(leftShift, 2).PadLeft(8, '0')})");
    leftShift = value << 3; // 左移3位，右侧补0，0b_0010_1000，十进制：40
    Console.WriteLine($"value << 3 = {leftShift} (0b{Convert.ToString(leftShift, 2).PadLeft(8, '0')})");
    leftShift = value << 4; // 左移4位，右侧补0，0b_0101_0000，十进制：80
    Console.WriteLine($"value << 4 = {leftShift} (0b{Convert.ToString(leftShift, 2).PadLeft(8, '0')})");
    // 结论：左移n位，相当于乘以2的n次方（不溢出时）

    // 有符号右移（>>），所有位向右移n位，左侧补符号位
    int rightShift = value >> 1; // 右移1位，左侧补符号位，0b_0000_0010，十进制：2
    Console.WriteLine($"value >> 1 = {rightShift} (0b{Convert.ToString(rightShift, 2).PadLeft(8, '0')})");
    rightShift = value >> 2; // 右移2位，左侧补符号位，0b_0000_0001，十进制：1
    Console.WriteLine($"value >> 2 = {rightShift} (0b{Convert.ToString(rightShift, 2).PadLeft(8, '0')})");
    rightShift = value >> 3; // 右移3位，左侧补符号位，0b_0000_0000，十进制：0
    Console.WriteLine($"value >> 3 = {rightShift} (0b{Convert.ToString(rightShift, 2).PadLeft(8, '0')})");
    rightShift = value >> 4; // 右移4位，左侧补符号位，0b_0000_0000，十进制：0
    Console.WriteLine($"value >> 4 = {rightShift} (0b{Convert.ToString(rightShift, 2).PadLeft(8, '0')})");
    // 结论：有符号右移n位，相当于整除2的n次方，带符号

    // 有符号有异对负数：符号位补1，保持负号不变
    int negativeValue = -40; // 
    rightShift = negativeValue >> 1; // 右移1位，左侧补符号位，0b_11111111111111111111111111101100，十进制：-20
    // 相当于： -40 / 2^1
    Console.WriteLine($"negativeValue >> 1 = {rightShift} (0b{Convert.ToString(rightShift, 2).PadLeft(8, '0')})");
    rightShift = negativeValue >> 2; // 右移2位，左侧补符号位，0b_11111111111111111111111111110110，十进制：-10
    // 相当于： -40 / 2^2
    Console.WriteLine($"negativeValue >> 2 = {rightShift} (0b{Convert.ToString(rightShift, 2).PadLeft(8, '0')})");

    // 无符号右移（>>>），所有位向右移n位，左侧补0，不保留符号
    rightShift = value >>> 1; // 右移1位，左侧补0，0b00000010
    Console.WriteLine($"value >>> 1 = {rightShift} (0b{Convert.ToString(rightShift, 2).PadLeft(8, '0')})");
    rightShift = value >>> 2; // 右移2位，左侧补0，0b00000001
    Console.WriteLine($"value >>> 2 = {rightShift} (0b{Convert.ToString(rightShift, 2).PadLeft(8, '0')})");

}

#endregion

#region 配合怕[Flags]特性标记枚举，启用位语义
{
    // 组合多个权限
    FilePermission permission = FilePermission.Read | FilePermission.Write;
    Console.WriteLine($"permission = {permission}"); // 输出：permission = ReadWrite
    // 检查权限
    bool canRead = (permission & FilePermission.Read) != 0;
    bool canWrite = (permission & FilePermission.Write) != 0;
    bool canExecute = (permission & FilePermission.Execute) != 0;
    Console.WriteLine($"canRead = {canRead}, canWrite = {canWrite}, canExecute = {canExecute}"); // 输出：canRead = True, canWrite = True, canExecute = False

    // .NET 提供的 HasFlag 方法（语义更清晰）
    canRead = permission.HasFlag(FilePermission.Read);
    canWrite = permission.HasFlag(FilePermission.Write);
    canExecute = permission.HasFlag(FilePermission.Execute);
    Console.WriteLine($"canRead = {canRead}, canWrite = {canWrite}, canExecute = {canExecute}"); // 输出：canRead = True, canWrite = True, canExecute = False

    // 添加一个权限
    permission |= FilePermission.Execute;
    Console.WriteLine($"permission = {permission}"); // 输出：permission = All
    // 检查权限
    canRead = permission.HasFlag(FilePermission.Read);
    canWrite = permission.HasFlag(FilePermission.Write);
    canExecute = permission.HasFlag(FilePermission.Execute);
    Console.WriteLine($"canRead = {canRead}, canWrite = {canWrite}, canExecute = {canExecute}"); // 输出：canRead = True, canWrite = True, canExecute = True

    // 移除一个权限（用&配合~清除特定位）
    permission &= ~FilePermission.Execute;
    permission &= ~FilePermission.Read;
    Console.WriteLine($"permission = {permission}"); // 输出：permission = ReadWrite
    // 检查权限
    canRead = permission.HasFlag(FilePermission.Read);
    canWrite = permission.HasFlag(FilePermission.Write);
    canExecute = permission.HasFlag(FilePermission.Execute);
    Console.WriteLine($"canRead = {canRead}, canWrite = {canWrite}, canExecute = {canExecute}"); // 输出：canRead = False, canWrite = True, canExecute = False


}
#endregion

#region 提取特定位
{
    // 一个ARGB颜色值可以用一个32位整数表示，其中前8位是A通道，接下来8位是R通道，再接下来8位是G通道，最后8位是B通道。
    int color = unchecked((int)0xFF3498DB);
    // 对于上述color，其在内存中字节可能是这样的：
    /*
        +-------- +--------+----------+---------+
        | byte 0  | byte 1  | byte 2  | byte 3  |
        +-------- +--------+----------+---------+
        | FF      | 34     | 98       | DB      |
        +-------- +--------+--------+-----------+
    */
    // 将对应通道的值提取出来，实质上就是分别把四个字节提取出来
    // 方法是：将整数右移要提取字节对应的位数，再与0XFF掩码进行与运算，即可得到对应字节的值
    byte alpha = (byte)(color >> 24); // 代表alpha通道的字节在第24-31位
    byte red = (byte)(color >> 16); // 代表red通道的字节在第16-24位
    byte green = (byte)(color >> 8); // 代表green通道的字节在第8-16位
    byte blue = (byte)color; // 代表blue通道的字节在第0-8位
    Console.WriteLine($"alpha = {alpha}, red = {red}, green = {green}, blue = {blue}"); // 输出：alpha = 255, red = 52, green = 152, blue = 219

    // 组合颜色
    int newColor = (alpha << 24) | (red << 16) | (green << 8) | blue;
    Console.WriteLine($"color == newColor == {color == newColor}");

}


#endregion

#region 类型定义

[Flags]
public enum FilePermission
{
    None = 0, // 0b_0000
    Read = 1, // 0b_0001 (1 << 0)
    Write = 2, // 0b_0010 (1 << 1)
    Execute = 4, // 0b_0100 (1 << 2)
    ReadWrite = Read | Write, // 0b_0001 | 0b_0010 = 0b_0011
    All = Read | Write | Execute // 0b_0001 | 0b_0010 | 0b_0100 = 0b_1111
}


#endregion

#region 说明和总结

/*
Q:& 和 && 有什么区别？

A:对于 bool 类型：&& 有短路（左侧 false 时右侧不执行），& 没有短路（两侧都执行）。
对于整数类型：& 是按位与运算，&& 只能用于 bool，不能用于整数。
实际开发中，bool 操作优先用 &&，整数位操作用 &。

Q：为什么 [Flags] 枚举的值要是 2 的幂次方（1、2、4、8...）？

A: 因为每个标志需要对应唯一的一个二进制位，互不干扰。
如果用 1、2、3、4，那么 3（0b011）和 2+1（0b011）无法区分。2 的幂次方确保每个值只有一个位为 1，这样用 | 组合、& 检测才能正确工作。

Q: ~a 为什么结果不是预期的值？

A: ~a 对整数的所有 32 位取反（包括符号位），所以对正整数 a 取反后结果是负数：~a = -(a + 1)。
例如 ~5 = -6，~0 = -1，~(-1) = 0。这在位掩码操作中很有用（如 ~FilePermission.Write 得到"除 Write 外所有位都是 1"的掩码）。



适用场景
- 权限和标志位：用 [Flags] 枚举把多个布尔标志合并到一个整数，比用多个 bool 字段更节省内存，序列化也更方便。
- 颜色处理：ARGB 颜色通道的提取和合并是位运算的经典用途，图像处理库中大量使用。
- 网络编程：IP 地址、子网掩码的计算需要位运算。
- 高性能数值运算：n << 1 比 n * 2 在某些场景下生成更优的机器码（尽管现代编译器通常会自动优化）。
- 数据压缩和协议解析：从二进制数据流中提取特定位置的字段。

注意事项
- 移位运算符的右操作数（移位数量）必须在合理范围内。对于 int（32 位），移位量会对 32 取模：n << 33 等价于 n << 1。这是 C# 的规定，不是 bug。

- >> 对有符号整数是算术右移（补符号位），对无符号整数（uint、ulong）是逻辑右移（补 0）。
C# 11 的 >>> 对有符号整数也做逻辑右移（补 0），用于需要无符号移位行为的有符号整数。

- 位运算的操作数会被提升到 int 或 uint。如果操作数是 byte 或 short，运算前会被提升为 int，结果也是 int，这可能导致需要显式转换才能赋回 byte/short。
- [Flags] 枚举中，None = 0 是常见约定，用 HasFlag(MyFlags.None) 始终返回 true（因为 0 & anything = 0，总是满足），
这是 HasFlag 的已知特性，使用时注意。

总结
位运算符直接操作整数的二进制位，包括按位与（&）、按位或（|）、按位异或（^）、按位取反（~）以及移位运算（<<、>>、>>>）。
在日常开发中最常见的用途是 [Flags] 枚举实现权限组合标志位，以及颜色值、网络地址等二进制格式的处理。
[Flags] 枚举的值必须是 2 的幂次方，用 | 组合权限，用 & 检测权限，用 &= ~flag 移除权限。
C# 11 新增了 >>> 无符号右移，解决了有符号整数按位逻辑右移的需求。

*/


#endregion