// 演示C#中的布尔类型

// bool （对应 .NET 的 System.Boolean ）是 C# 中最简单的类型，只有两个值： true 和 false 。它是所有条
// 件判断、循环控制的基础，逻辑运算的输入和输出都是 bool 类型。
// bool 是值类型，占用 1 字节内存（尽管逻辑上只需要 1 位，CLR 出于内存对齐考虑使用了 1 字节）。在数组
// 或结构体中大量存储布尔值时，可以考虑使用 BitArray 或位运算来节省内存。

// C# 的 bool 不能隐式转换为整数，也不能用整数来替代 bool （这与 C/C++ 不同）。 if (1) 这样的写法在
// C# 中是编译错误。

bool b1 = true;
var b2 = false;
// 可空 bool（允许 true、false、null 三种状态）
bool? b3 = null;
Console.WriteLine($"b1 = {b1}, b2 = {b2}, b3 = {b3}"); // 输出：b1 = True, b2 = False, b3 = 

b3 = b1 || b1;
Console.WriteLine($"b3 = {b3}"); // 输出：b3 = True

// 比较表达式的结果是一个 bool 值，比较表达式包括：
// 大于（>）、小于（<）、大于等于（>=）、小于等于（<=）、等于（==）、不等于（!=）
var b4 = 1 > 2;
Console.WriteLine($"b4 = {b4}"); // 输出：b4 = False
b4 = 1 < 2;
Console.WriteLine($"b4 = {b4}"); // 输出：b4 = True
b4 = 1 >= 2;
Console.WriteLine($"b4 = {b4}"); // 输出：b4 = False
b4 = 1 <= 2;
Console.WriteLine($"b4 = {b4}"); // 输出：b4 = True
b4 = 1 == 2;
Console.WriteLine($"b4 = {b4}"); // 输出：b4 = False
b4 = 1 != 2;
Console.WriteLine($"b4 = {b4}"); // 输出：b4 = True

// 逻辑运算的结果是一个 bool 值，逻辑运算包括：
// 与（&&）、或（||）、非（!）、异或（^）
var b5 = true && false;
Console.WriteLine($"b5 = {b5}"); // 输出：b5 = False
b5 = true || false;
Console.WriteLine($"b5 = {b5}"); // 输出：b5 = True
b5 = !true;
Console.WriteLine($"b5 = {b5}"); // 输出：b5 = False
b5 = true ^ false; //异或运算：当两个操作数不同时为true，否则为false
Console.WriteLine($"b5 = {b5}"); // 输出：b5 = True

// 演示短路求值：
// && 的短路：左侧为 false 时，右侧不执行
// 这在避免空引用时非常有用
string? text = null;
// 先检查 null，再访问 Length，不会 NullReferenceException
bool hasContent = (text != null) && text.Length > 0;
Console.WriteLine($"hasContent = {hasContent}"); // 输出：hasContent = False

// || 的短路：左侧为 true 时，右侧不执行
// 可以用于提供默认值
string name = "";
bool hasName = (name.Length > 0) || ((name = "Default") != "");
Console.WriteLine($"hasName = {hasName}"); // 输出：hasName = True
Console.WriteLine($"name = {name}"); // 输出：name = Default

// bool? 允许 true、false、null 三种状态
// 常见于数据库查询结果、未确定状态的标志
bool? userConsented = null; // 用户尚未作出选择
// 从 bool? 获取值：使用 ?? 空合并运算符提供默认值
bool consent = userConsented ?? false; // null 时默认为 false
Console.WriteLine(consent); // False

string connectMessage;
bool connected = TryConnect("config1", out connectMessage);
Console.WriteLine($"connected = {connected}, connectMessage = {connectMessage}"); // 输出：connected = True, connectMessage = Connection successful.

User user = new();
Console.WriteLine($"IsAdmin = {user.IsAdmin}, IsActive = {user.IsActive}, IsActivAdmin = {user.IsActivAdmin}");
user.Login("admin");
Console.WriteLine($"IsAdmin = {user.IsAdmin}, IsActive = {user.IsActive}, IsActivAdmin = {user.IsActivAdmin}");
user.Logout();
Console.WriteLine($"IsAdmin = {user.IsAdmin}, IsActive = {user.IsActive}, IsActivAdmin = {user.IsActivAdmin}");



// 配合函数out参数使用，将函数返回值定义为bool用于指示函数是否成功
static bool TryConnect(string config, out string message)
{
    // 模拟连接逻辑
    if (string.IsNullOrEmpty(config))
    {
        message = "Configuration is null or empty.";
        return false;
    }
    message = "Connection successful.";
    return true;
}

// 用属性包装复杂的布尔表达式，提高代码可读性
public class User
{
    public bool IsAdmin { get; private set; }
    public bool IsActive { get; private set; }

    public bool IsActivAdmin
    {
        get { return IsAdmin && IsActive; }
        private set; // 只读属性，不能直接赋值
    }

    public void Login(string username)
    {
        if (username == "admin")
        {
            IsAdmin = true;
        }
        else
        {
            IsAdmin = false;
        }
        IsActive = true;
    }

    public void Logout()
    {
        IsActive = false;
        IsAdmin = false;
    }


}