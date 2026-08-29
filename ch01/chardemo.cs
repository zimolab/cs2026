// 演示C#中的char类型

// char （对应 .NET 的 System.Char ）表示一个 Unicode UTF-16 编码单元，占用 2 字节。它是值类型，用单引号 '...' 包裹字面量（双引号是字符串）。
// 理解 char 有几个要点：
// 1. 它存储的是 UTF-16 编码单元，不是 Unicode 码位。对于基本多文种平面（BMP）以内的字符（大多数常用字符，包括全部 ASCII 和汉字），一个 char 等于一个字符。但对于 BMP 以外的
// 字符（如部分 emoji、生僻字），一个 Unicode 码位需要两个 char （代理对），这种情况处理起来比较复杂。
// 2.char 可以与整数隐式/显式转换，字符与 Unicode 码位之间可以互相转换，这在文本处理和密码学场景中偶尔用到。


// 声明一个char类型变量

char charA = 'A'; // charA 是一个字符，存储的是字符 'A' 的 UTF-16 编码单元
Console.WriteLine($"charA={charA}"); // 输出字符 'A'

var charB = 'B'; // charB 是一个字符，存储的是字符 'B' 的 UTF-16 编码单元
Console.WriteLine($"charB={charB}"); // 输出字符 'B'

// 用Unicode码点表示
char charC = '\u0041'; // charC 是一个字符，存储的是字符 'A' 的 Unicode 码点，使用4位十六进制表示
Console.WriteLine($"charC={charC}"); // 输出字符 'A'

// 从整数显式转换
char charD = (char)65; // charD 是一个字符，存储的是字符 'A' 的 Unicode 码点，使用整数65表示
Console.WriteLine($"charD={charD}"); // 输出字符 'A'


// ============================================================
// 示例一：char 的基本声明和使用
// ============================================================
char letter = 'A';
char digit = '9';
char space = ' ';
char newline = '\n'; // 换行符
char tab = '\t'; // 制表符
char escChar = '\e'; // C# 13 新增：ESC 字符（用于终端控制序列）
// Unicode 码点形式
char chineseChar = '\u4e2d'; // '中'（Unicode 码点 U+4E2D）
char euro = '\u20AC'; // '€' 欧元符号
Console.WriteLine($"chineseChar={chineseChar}"); // 中
Console.WriteLine($"euro={euro}"); // €

// ============================================================
// 示例二：char 与整数的转换
// ============================================================
// char 隐式转换为 int（获取 Unicode 码点值）
char ch = 'A';
int codePoint = ch; // 隐式：'A' 的 Unicode 值是 65
Console.WriteLine($"codepoint of 'A' is {codePoint}"); // 65
// int 显式转换为 char（用码点值得到字符）
char fromInt = (char)65; // 65 对应 'A'
Console.WriteLine($"char from int 65 is '{fromInt}'"); // A


// 遍历ASCII字母表，从'A'到'Z'，并打印每个字符及其对应的Unicode码点值
for (char i = 'A'; i <= 'Z'; i++)
{
    Console.WriteLine($"ASCII character: '{i}' <=> {(int)i} ");
}
for (char i = 'a'; i <= 'z'; i++)
{
    Console.WriteLine($"ASCII character: '{i}' <=> {(int)i} ");
}

// 大写字母转小写（利用ASCII码规律：'a' - 'A' = 32, lowercase = uppercase + 32）
char upperCase = 'G';
// 计算upperCase+32时，upperCase被隐式转换为int，整个表达式结果为int类型
// 因此需要将结果显式转换为char
char lowerCase = (char)(upperCase + 32);
Console.WriteLine($"lowerCase of '{upperCase}' is '{lowerCase}'"); // lowerCase of 'G' is 'g'

// 更好的方法是，使用ToLower()/ToUpper()方法
Console.WriteLine($"lowerCase of '{upperCase}' is '{char.ToLower(upperCase)}'"); // lowerCase of 'G' is 'g'
Console.WriteLine($"upperCase of '{lowerCase}' is '{char.ToUpper(lowerCase)}'"); // upperCase of 'g' is 'G'


// ============================================================
// 示例三：char 的静态工具方法
// ============================================================
// 字符类别判断
Console.WriteLine(char.IsLetter('A')); // True（字母）
Console.WriteLine(char.IsDigit('5')); // True（数字）
Console.WriteLine(char.IsWhiteSpace(' ')); // True（空白字符）
Console.WriteLine(char.IsUpper('A')); // True（大写字母）
Console.WriteLine(char.IsLower('a')); // True（小写字母）
Console.WriteLine(char.IsPunctuation('!')); // True（标点）
Console.WriteLine(char.IsLetterOrDigit('3')); // True
// 字符转换
Console.WriteLine(char.ToLower('A')); // a（当前文化）
Console.WriteLine(char.ToLowerInvariant('A')); // a（不受文化影响）
Console.WriteLine(char.ToUpper('a')); // A
// 获取字符的 Unicode 类别
var category = char.GetUnicodeCategory('A');
Console.WriteLine(category); // UppercaseLetter

// 在字符串处理中的应用
string str = "Hello, World!";
// 通过索引访问特定位置的字符，范围[0, str.Length - 1]
for (int i = 0; i < str.Length; i++)
{
    Console.WriteLine($"str[{i}] = '{str[i]}'");
}
// 也可以使用C#引入的索引运算符，比如取最后一个位置的字符
var lastChar = str[^1];
Console.WriteLine($"lastChar = '{lastChar}'"); // lastChar = '!'

// 也可以使用foreach遍历字符
foreach (char c in str)
{
    Console.Write($"'{c}'");
}
Console.WriteLine();

// char数组转字符串
char[] charArray = ['H', 'e', 'l', 'l', 'o', ',', ' ', 'W', 'o', 'r', 'l', 'd', '!'];
string strFromArray = new string(charArray);
Console.WriteLine($"strFromArray = '{strFromArray}'"); // strFromArray = 'Hello, World!'
// 也可以使用string.Join()方法
Console.WriteLine($"{string.Join("", charArray)}");

// 字符串转char数组
string strToCharArray = "Hello, World!";
char[] charArrayFromStr = strToCharArray.ToCharArray();
Console.WriteLine($"charArrayFromStr = {string.Join(", ", charArrayFromStr)}"); // charArrayFromStr = H, e, l, l, o, ,, , W, o, r, l, d, !

