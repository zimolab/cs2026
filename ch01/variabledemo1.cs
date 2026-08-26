namespace variabledemo1;


struct Point
{
    public int x;
    public int y;

}


internal static class Program
{
    public static void Main(string[] args)
    {
        // ===================================================================================
        // 示例：C#中的变量声明
        // ===================================================================================


        // 1、显式类型声明
        int age = 10;
        double pi = 3.14;
        float floatNum = 2.71F;
        string greetingStr = "Hello, World!";
        decimal decimalNum = 1234567890.123456789M;
        //  C# 9 目标类型 new 表达式：右侧已知类型时，new 后可省略类型
        List<string> strList = new List<string>();
        Dictionary<string, int> dict = new Dictionary<string, int>();


        // 2、隐式类型声明：对于局部变量，可以使用var关键字自动推断其类型
        // `var` 是编译时类型推断，一旦推断完成，变量类型就固定了，与显式声明完全等价。
        var aInt = 10; // 类型推断为int
        var aDouble = 3.14; // 类型推断为double
        var aFloat = 2.71F; // 类型推断为float
        var aString = "Hello, World!"; // 类型推断为string
        var aDecimal = 1234567890.123456789M; // 类型推断为decimal
        var aBoolean = true; // 类型推断为bool
        var aStringList = new List<string>(); // 类型推断为List<string>

        // 初始化：局部变量必须赋值后才能使用
        int aLocaleVar;
        // Console.WriteLine(aLocaleVar); // error CS0165: 使用了未赋值的局部变量“aLocaleVar”
        aLocaleVar = 10;
        Console.WriteLine($"aLocalVar={aLocaleVar}"); // 输出：10


        // ===================================================================================
        // 示例：C#中的变量赋值：基本赋值与复合赋值
        // ===================================================================================
        // 声明变量同时赋值（初始化）
        int score = 10;
        Console.WriteLine($"score={score}"); // 输出：10

        // 声明变量，随后赋值
        char grade;
        if (score >= 90)
        {
            grade = 'A';
        }
        else if (score >= 80)
        {
            grade = 'B';
        }
        else if (score >= 70)
        {
            grade = 'C';
        }
        else
        {
            grade = 'D';
        }
        Console.WriteLine($"grade={grade}"); // 输出：B


        // 复合赋值
        score += 5; // 等价于 score = score + 5
        Console.WriteLine($"score={score}");

        score -= 3; // 等价于 score = score - 3
        Console.WriteLine($"score={score}");

        score *= 2; // 等价于 score = score * 2
        Console.WriteLine($"score={score}");

        score /= 4; // 等价于 score = score / 4 （整数除法，结果为整数）
        Console.WriteLine($"score={score}");

        score %= 2; // 等价于 score = score % 2 （取模运算，结果为整数）
        Console.WriteLine($"score={score}");




        // 值类型和引用类型的赋值

        // 值类型：基本类型和结构体类型
        // 基本类型
        int a = 10;
        int b = a; // 值类型赋值，将a的值复制给b
                   // 修改a的值不会影响b的值
        a = 20;
        Console.WriteLine($"a={a}, b={b}"); // 输出：a=20, b=10

        // struct也是值类型
        Point p1 = new() { x = 10, y = 20 };
        Point p2 = p1; // 值类型赋值，会将p1整个拷贝一份副本，然后让p2指向这个副本
                       // 修改p1的值不会影响p2的值
        p1.x = 30;
        Console.WriteLine($"p1.x={p1.x}, p2.x={p2.x}"); // 输出：p1.x=30, p2.x=30

        // 引用类型赋值：共享引用
        // 声明一个List<int>类型变量listA，创建一个list对象，通过赋值将listA指向这个list对象
        var listA = new List<int> { 1, 2, 3, 4 };

        // 声明一个List<int>类型变量listB，将listA赋值给listB
        // 赋值的效果是让listA和listB共同持有对同一个list对象的引用，而不是复制一个list对象的副本给listB
        // 因此对于通过listA对list对象所做的修改，在listB上也会反映出来
        var listB = listA;
        // 验证listA和listB是否引用同一个对象
        Console.WriteLine($"(listA == listB) == {listA == listB}");
        Console.WriteLine($"Object.ReferenceEquals(listA, listB) == {Object.ReferenceEquals(listA, listB)}");
        // 通过listA修改被应用的list对象，通过listB查看到这种修改
        listA.Add(5);
        Console.WriteLine($"listA: {string.Join(", ", listA)}"); // 输出：listA: 1, 2, 3, 4, 5
        Console.WriteLine($"listB: {string.Join(", ", listB)}"); // 输出：listB: 1, 2, 3, 4, 5
        // 通过listB修改被应用的list对象，通过listA查看到这种修改
        listB.RemoveAt(0);
        Console.WriteLine($"listA: {string.Join(", ", listA)}"); // 输出：listA: 2, 3, 4, 5
        Console.WriteLine($"listB: {string.Join(", ", listB)}"); // 输出：listB: 2, 3, 4, 5

        // 对于引用类型，如果确实需要一个独立的副本，需要创建一个新的对象，但是C#中没有一个通用的复制对象的方法，比如Clone()之类的
        // 因为复制对象语义其实并不是很明确，有时候可能指的是浅拷贝，有时候又可能指深拷贝，
        // 因此，需要根据具体需求来决定如何实现。
        // 但对于List<int>这种特定的类型，其已经实现了类似C++中复制构造函数的构造函数：
        var listC = new List<int>(listA); // 创建一个listC，包含listA的所有元素
        Console.WriteLine($"(listA == listC) == {listA == listC}");
        Console.WriteLine($"Object.ReferenceEquals(listA, listC) == {Object.ReferenceEquals(listA, listC)}");

        // ===================================================================================
        // 示例：元组复制与解构赋值
        // ===================================================================================
        // 创建一个元组类型的变量，它的类型是ValueTuple<int, int, int>
        ValueTuple<int, int, int> colorTuple = (255, 128, 0);
        (int colorR, int colorG, int colorB) = colorTuple; // 解构赋值，将元组的元素分别赋值给r, g, b
        Console.WriteLine($"r={colorR}, g={colorG}, b={colorB}");

        // 通过元组和解构赋值实现无中间变量交换变量值
        int a1 = 10;
        int b1 = 20;
        Console.WriteLine($"a1={a1}, b1={b1}");
        (a1, b1) = (b1, a1); // 解构赋值，交换a1和b1的值
        Console.WriteLine($"a1={a1}, b1={b1}");

        // 利用元组和解构赋值可以实现多值返回。
        // 下面定义一个局部函数，该函数接受一个代表圆的半径的double类型参数，返回一个ValueTuple<double, double>类型的元组，两个元素分别表示圆的面积和周长
        static (double, double) CalculateCircle(double radius)
        {
            double area = Math.PI * radius * radius;
            double circumference = 2 * Math.PI * radius;
            return (area, circumference);
        }

        // 使用解构赋值，将返回的元组的元素分别赋值给area和circumference
        var (area, circumference) = CalculateCircle(5);
        Console.WriteLine($"area={area}, circumference={circumference}");

        // 可以弃用解构赋值的某个元素，使用下划线（_）表示
        var (area2, _) = CalculateCircle(6.0);
        Console.WriteLine($"area2={area2}");

        var (_, circumference2) = CalculateCircle(7.0);
        Console.WriteLine($"circumference2={circumference2}");



    }

}