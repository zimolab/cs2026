#region C#中的值类型
/*
C#中的类型系统将类型划分为两大类：值类型（Value Type）和引用类型（Reference Type）。

这里对值类型进行说明：
1.值类型的特点：
  - 值类型的变量直接存储数据本身
  - 赋值时复制整个数据
  - 方法调用时默认按照值传递（复制传入）
  - 不支持继承其他类型（但可以实现接口）
  - 不能为null，除非声明为可空值类型（T？ or Nullable<T>）
2.值类型包括哪些：
  - 基本数值类型
  - char
  - bool
  - 枚举: enum
  - 结构体（包括struct、readonly struct、ref struct）
  - 元组：(T1, T2, ..., Tn)
  - 可空值类型：T? or Nullable<T>，其中T是值类型
*/
#endregion

#region 值类型的复制语义

{
    // 值类型赋值语义是复制整个数据
    int a = 10;
    int b = a; // b复制了a的值
    a = 20; // a的值改变，b的值不变
    Console.WriteLine($"a: {a}, b: {b}"); // 输出: a: 20, b: 10

    // struct是值类型，赋值时复制整个结构体
    var vec1 = new System.Numerics.Vector2(1, 2);
    var vec2 = vec1; // 复制结构体全部字段
    vec1.X = 3; // vec1的X字段改变，vec2的X字段不变
    Console.WriteLine($"vec1.X: {vec1.X}, vec2.X: {vec2.X}"); // 输出: vec1.X: 3, vec2.X: 1

    // 自定义结构体同样遵循值类型的复制语义
    var point1 = new Point(1, 2);
    var point2 = point1; // 复制结构体全部字段
    point1.X = 3; // point1的X字段改变，point2的X字段不变
    Console.WriteLine($"point1.X: {point1.X}, point2.X: {point2.X}"); // 输出: point1.X: 3, point2.X: 1
    // 调用point1.Move方法改变对象x、y值，这种改变不会影响point2
    point1.Move(1, 1);
    // 这里打印point1和point2实际上调用了Point重载的ToString方法
    Console.WriteLine($"point1={point1}, point2={point2}");
    // 调用DistanceTo方法计算两个点之间的距离
    Console.WriteLine($"point1.DistanceTo(point2): {point1.DistanceTo(point2)}");

    // 元组也是值类型，同样遵循值类型的复制语义
    (int, int) tuple1 = (1, 2);
    var tuple2 = tuple1; // 复制元组全部字段
    tuple1.Item1 = 3; // tuple1的Item1字段改变，tuple2的Item1字段不变
    Console.WriteLine($"tuple1: {tuple1}, tuple2: {tuple2}"); // 输出: tuple1: (3, 2), tuple2: (1, 2)

    // 可空值类型
    // int intVar = null; // 普通值类型不能为null，编译器报错：error CS0037: 无法将 null 转换为“int”，因为后者是不可为 null 的值类型
    // Point point3 = null; // 普通值类型不能为null，编译器报错：error CS0037: 无法将 null 转换为“Point”，因为后者是不可为 null 的值类型
    // 要使类型可空，需要使用T?
    int? intVar = null; // 可空值类型可以为null
    Point? point3 = null; // 可空值类型可以为null

    // T? 是 Nullable<T> 的语法糖，以上代码等价于：
    // Nullable<int> intVar = null; // 可空值类型可以为null
    // Nullable<Point> point3 = null; // 可空值类型可以为null

    // 可空值类型实际上包含两个字段：
    // 一个 bool（HasValue）和一个 T（Value）。
    // 有值时 HasValue=true，Value 存实际值；
    // 为 null 时 HasValue=false。
    // 所以 int? 占 5 字节（实际受内存对齐影响通常是 8 字节）。
    Console.WriteLine($"intVar.HasValue: {intVar.HasValue}");
    Console.WriteLine($"point3.HasValue: {point3.HasValue}");
    intVar = 10;
    point3 = point1;
    Console.WriteLine($"intVar.HasValue: {intVar.HasValue}, intVar= {intVar}"); // 输出: intVar.HasValue: true, intVar= 10
    Console.WriteLine($"point3.HasValue: {point3.HasValue}, point3= {point3}"); // 输出: point3.HasValue: true, point3= Point(X=4, Y=3)

    // 可空值类型复制语义的一个小坑
    point3?.Move(1, 1);
    Console.WriteLine($"point1={point1}, point3={point3}"); // 输出: point1=Point(X=4, Y=3), point3=Point(X=4, Y=3)
    // 为什么point3?.Move(1, 1)之后point3的值没有改变，仍然是Point(X=4, Y=3)，而非预期的Point(X=5, Y=4)？
    // point3的类型实际上是Nullable<Point>，而不是Point ，所以 point3?.Move(1, 1)实际上调用的是point3.Value.Move(1, 1)：
    // 由于point3.Value属性返回的是一个Point对象，它是一个struct（值类型），根据值类型的复制语义，
    // 该对象是原对象的一个副本，因此调用point3.Value.Move(1,1)实际上是在对这个副本进行修改，不会影响原对象point3。
    // 换言之：point3?.Move(1,1)实际上隐含了一个“看不见”、临时的中间变量！
    // 如果你希望将修改后的值保存回 point3，需要重新赋值：
    // 但是请注意，根据值类型的复制语义，将临时变量temp赋值回 point3 时，这里又发生了一次复制
    // // 获取副本
    var temp = point3.Value;
    // 修改副本
    temp!.Move(1, 1);
    // 将修改后的副本赋值回原对象，注意，这里又发生了一次复制
    point3 = temp;
    Console.WriteLine($"point1={point1}, point3={point3}"); // 输出: point1=Point(X=4, Y=3), point3=Point(X=5, Y=4)

    // ??运算符可用于处理可空值类型，如果可空值类型为null，则返回右侧表达式的结果。否则返回原值。
    intVar = null;
    int intVar2 = intVar ?? -10; // intVar2=-10，因为intVar为null
    Console.WriteLine($"intVar2: {intVar2}"); // 输出: intVar2: -10
    intVar = 10;
    intVar2 = intVar ?? -10; // intVar2=10，因为intVar不为null
    Console.WriteLine($"intVar2: {intVar2}"); // 输出: intVar2: 10

    point3 = null;
    var point4 = point3 ?? new Point(0, 0); // point4=Point(X=0, Y=0)，因为point3为null
    Console.WriteLine($"point4: {point4}"); // 输出: point4: Point(X=0, Y=0)
    point3 = point1; // 把point1赋值（复制）给point3
    point4 = point3 ?? new Point(0, 0); // point4=Point(X=4, Y=3)，因为point3不为null
    Console.WriteLine($"point4: {point4}"); // 输出: point4: Point(X=4, Y=3)

    // readonly struct（C# 7.2及以上版本）
    var temperature = new Temperature(25);
    Console.WriteLine($"temperature: {temperature}");
    Console.WriteLine($"temperature.Celsius: {temperature.Celsius}"); // 输出: temperature.Celsius: 25
    Console.WriteLine($"temperature.Fahrenheit: {temperature.Fahrenheit}"); // 输出: temperature.Fahrenheit: 77
    //temperature.Celsius = 30; // 错误： error CS0200: 无法为属性或索引器“Temperature.Celsius”赋值 - 它是只读的

}

#endregion

#region 按值传参与按引用传参
{

    // 默认情况下，方法调用时默认按照值传递（复制传入）
    static void DoubleValue(int value)
    {
        // 按值传递时，先将外部参数值复制给局部变量value
        // 因此，方法内部对局部变量value的修改不会影响外部参数
        value *= 2; // 只改变了局部变量value的值，原变量value未改变
    }

    // 可以使用ref关键字按照引用传递，这种情况下，方法内部可以改变原变量的值
    static void DoubleValueRef(ref int value)
    {
        value *= 2; // 改变了原变量value的值
    }

    int a = 10;
    DoubleValue(a); // a未改变
    Console.WriteLine($"a: {a}"); // 输出: a: 10

    // 传递a的引用
    DoubleValueRef(ref a); // a被改变了
    Console.WriteLine($"a: {a}"); // 输出: a: 20

    // struct是值类型，按照值传递方式传递，会把整个结构体复制给方法参数
    // 如果一个struct很大，会带来很大的复制开销。如果一个方法只需要读取结构体，而不会对其进行修改
    // 可以使用in参数，这种参数表示只读引用传递，这样既避免了复制开销，同时又能保证方法内部无法对
    // 结构体进行修改。
    void Calculate(in Point point1, in Point point2)
    {
        double x = point1.X + point2.X;
        double y = point1.Y + point2.Y;
        Console.WriteLine($"x: {x}, y: {y}");
    }
    Point point1 = new(1, 2);
    Point point2 = new(3, 4);
    // point2和point2作为引用传递Calculate方法，这样避免了按值传参时复制对象的开销
    Calculate(in point1, in point2);


}
#endregion


#region 自定义数据类型

// 自定义结构体
struct Point
{
    public double X;
    public double Y;

    // 结构体也可以有构造函数
    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    // 结构体可以有方法
    public void Move(double dx, double dy)
    {
        X += dx;
        Y += dy;
    }

    // readonly表示该方法内部不能修改结构体的字段
    public readonly double DistanceTo(Point other)
    {
        return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    }

    // 覆盖ToString方法
    public override readonly string ToString()
    {
        return $"Point(X={X}, Y={Y})";
    }

}

// readonly struct（C# 7.2及以上版本）
// readonly struct与struct类似，但其所有字段都是隐式readonly的，适合不可变对象

readonly struct Temperature
{
    public double Celsius { get; } // 隐式readonly，赋值（初始化）以后不能再被修改
    public double Fahrenheit => Celsius * 9 / 5 + 32; // 计算属性

    public Temperature(double celsius)
    {
        Celsius = celsius;
    }

    public override readonly string ToString()
    {
        return $"Temperature(Celsius={Celsius}, Fahrenheit={Fahrenheit})";
    }

}

#endregion


#region 值类型总结
/*
1.值类型变量直接存储数据本身，而引用类型变量存储的是数据的引用（地址）。

2.值类型在赋值、传参、函数返回等情况下具有复制语义。

3.值类型默认不可为null，除非声明为T?或Nullable<T>。

4.作为局部变量的值类型，通常存储在栈上，但不必然，作为引用类型的字段、装箱时、在数组中时，值类型也被分配到堆上。

5.struct、enum、tuple等都是值类型。

6.微软官方建议：如果 struct 超过 16 字节，在频繁作为参数传递时反而比引用类型慢（因为复制成本更高）。
实践中，坐标点（2-3 个 int/float）、颜色（4 个 byte）这类小数据适合 struct；
包含很多字段或者需要频繁作为参数传递的数据结构应考虑 class。

7.对于可变 struct，复制语义可能是陷阱：通过接口调用可变 struct 的方法时，CLR 会创建防御性副本，修改不会影响原变量，
这个行为有时令人困惑。使用 readonly struct 可以避免这个问题。

8.struct 总是有无参数构造函数（C# 10 之前隐式的默认无参构造函数，C# 10+ 允许显式定义）。
创建数组 new T[n] 时，每个元素都用默认构造函数初始化为零值。

9.值类型实现接口后，通过接口类型使用时会发生装箱（转为引用类型），失去值类型的性能优势。
如果需要在不装箱的情况下通过接口使用值类型，可以用泛型约束（where T : IMyInterface）。

10.in 参数（只读引用传递）可以避免大 struct 的复制开销，同时保证不被修改，是大 struct 方法参数的最佳实践。


总结：
值类型是 C# 类型系统的基础构件，核心特征是赋值时复制数据、变量直接存储值、不能为 null（除非使用 T?）。
所有内置数值类型、bool、char、struct、enum 都是值类型。
readonly struct 提供了不可变的值类型，适合表示不可变数据。
可空值类型 T? 为值类型提供了 null 支持，常用于数据库字段映射。
理解值类型的复制语义，以及它在不同上下文中实际的存储位置，是避免 C# 性能问题和逻辑错误的基础。
*/

#endregion