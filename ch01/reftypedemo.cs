#region C#中的引用类型

/*
引用类型的变量不直接存储数据，而是存储一个指向堆内存中对象的引用（可以理解为"地址"）。
赋值时复制的是这个引用，而不是对象本身，因此两个变量可以指向同一个对象——通过任一变量修改对象，另一个变量也能看到变化。

C# 中的引用类型包括：
 - class
 - string
 - delegate：Action、Func<T>
 - interface
 - record class（C# 9+）。
 - dynamic
 - object

 引用类型的对象在堆上分配，由垃圾收集器（GC）管理其生命周期，不再被任何引用指向时会被 GC 回收。

引用类型的默认值是 null（没有指向任何对象）。
在启用可空引用类型检查（NRT）的项目中，编译器会对未经检查的引用访问发出警告，
帮助提前发现潜在的空引用异常。

*/

#endregion

#region 引用类型共享引用语义

{
    var person1 = new Person { Name = "Alice", Age = 30 };
    // 这里通过赋值语句复制的是指向对象的引用，而不是对象本身
    // 因此 person1 和 person2 指向同一个对象
    var person2 = person1;
    // 通过object.ReferenceEquals() 方法可以检查两个引用是否指向同一个对象
    Console.WriteLine(object.ReferenceEquals(person1, person2)); // 输出: True
    // 多个对象同时引用一个对象，通过任一一个变量修改对象属性，其他变量也能看到变化
    person1.Name = "Bob";
    Console.WriteLine(person2.Name); // 输出: Bob
    person2.Age = 25;
    Console.WriteLine(person1.Age); // 输出: 25

    // 让person2指向一个新对象，这不会影响person1
    person2 = new Person { Name = "Tom", Age = 15 };
    Console.WriteLine(object.ReferenceEquals(person1, person2)); // 输出: False
    Console.WriteLine(person1); // 输出: Person(Name=Bob, Age=25)
    Console.WriteLine(person2); // 输出: Person(Name=Tom, Age=15)
}

#endregion

#region null引用与NullReferenceException

{
    Person? person;
    // Console.WriteLine(person); // error CS0165: 使用了未赋值的局部变量“person”
    person = null;

    try
    {
        // 如果对一个null引用访问其成员，运行时会报NullReferenceException
        Console.WriteLine(person.Name); // 如果开启了null检查，这里会出现一个警告：warning CS8602: 解引用可能出现空引用。
    }
    catch (NullReferenceException e)
    {
        Console.WriteLine(e.Message); // 输出: Object reference not set to an instance of an object.
    }

    // 检查引用不为null的方式
    if (person is not null)
    {
        Console.WriteLine(person.Name);
    }
    else
    {
        Console.WriteLine("Person is null.");
    }

    // 检查一个是否引用是否为null
    if (person is null)
    {
        Console.WriteLine("Person is null.");
    }
    // is/is not语法是C# 7.0引入的模式匹配语法，建议使用
    // 旧的语法也可以使用 person == null 和 person != null

    // 空条件运算符：当引用不为null时，执行表达式，否则返回null
    string? name = person?.Name;
    // 等价于
    name = null;
    if (person is not null)
    {
        name = person.Name;
    }
    // 空合并运算符：当左侧表达式不为null时，返回左侧表达式，否则返回右侧表达式
    name = name ?? "Unknown";
    // 等价于
    if (name is null)
    {
        name = "Unknown";
    }

}

#endregion

#region 引用类型对象的拷贝
{
    // 与值类型不同，引用类型是共享引用语义
    // 因此如果确实要创建一个对象的副本，需要手动操作
    var person1 = new Person { Name = "Alice", Age = 30 };
    // 对于简单的对象，可以手动逐一复制属性
    var person2 = new Person { Name = person1.Name, Age = person1.Age };
    Console.WriteLine(object.ReferenceEquals(person1, person2)); // 输出: False

    // 这里Person类是一个简单对象，它的两个属性：
    // Name: string，是不可变引用类型，复制的话是安全的
    // Age: int，值类型，本身就是复制值本身，因此也是安全的
    // 如果是更复杂的对象，比如它的属性里有另一个可变引用类似，
    // 像上面那种简单的复制方式是不安全的，这种方式被称为浅拷贝。
    // 如果需要两个对象完全地独立，需要进行深拷贝，即递归地复制所有属性。

    // record class是不可变的类，可以通过with语句创建副本
    var record1 = new PersonRecord("Alice", 30);
    var record2 = record1 with { Name = "Bob" };
    Console.WriteLine(record1.Name); // 输出: Alice
    Console.WriteLine(record2.Name); // 输出: Bob
    Console.WriteLine(record1);
    Console.WriteLine(record2);
}
#endregion

#region 数组是一种引用类型
{
    // 需要注意的是，数组是一种引用类型
    int[] arr1 = [1, 2, 3];
    // 这里复制的是对于数组的引用，而不是数组本身
    // 现在，arr1和arr2指向同一个数组对象
    int[] arr2 = arr1;
    Console.WriteLine(object.ReferenceEquals(arr1, arr2)); // 输出: True
    arr1[0] = 10;
    Console.WriteLine($"arr1[0]: {arr1[0]}, arr2[0]: {arr2[0]}"); // 输出: arr1[0]: 10, arr2[0]: 10

    // 如果要创建数组的副本，可以使用Array.Clone()、Array.Copy()或asSpan().ToArray()方法
    // 需要注意的是，这些方法执行的都是浅拷贝，如果数组元素是可变引用类型，需要手动进行深拷贝。

    var arr3 = (int[])arr1.Clone();
    Console.WriteLine(object.ReferenceEquals(arr1, arr3)); // 输出: False

    var arr4 = new int[arr1.Length];
    // 第一个参数是源数组，第二个参数是目标数组，第三个参数是复制的元素个数
    Array.Copy(arr1, arr4, arr1.Length);
    Console.WriteLine(object.ReferenceEquals(arr1, arr4)); // 输出: False

    int[] arr5 = arr1.AsSpan().ToArray();
    Console.WriteLine(object.ReferenceEquals(arr1, arr5)); // 输出: False

    // 也可以用集合表达式+展开语法创建数组的副本
    var arr6 = (int[])[.. arr1];
    Console.WriteLine(object.ReferenceEquals(arr1, arr6)); // 输出: False


}
#endregion

#region 引用类型作为参数时的行为

#endregion
{
    // 引用类型作为参数时，默认按引用传递，因此在函数内部对于参数的修改会反映在调用者中
    static void ModifyPerson(Person arg)
    {
        arg.Name = $"{arg.Name} Modified";
    }
    var person = new Person { Name = "Alice", Age = 30 };
    ModifyPerson(person);
    Console.WriteLine(person.Name); // 输出: Alice Modified

    // 需要注意的是，函数端的形参和调用端的实参实际上是两个不同的变量
    // 它们只是共同引用了同一个对象，如果在函数体内给形参赋值
    // 实际上相当于让形参指向一个新对象，这不会影响调用端的实参
    static void TryReplacePerson(Person arg)
    {
        // 这里只是修改了形参的引用，不会影响调用端的实参
        arg = new Person { Name = "Bob", Age = 25 };
    }
    person = new Person { Name = "Alice", Age = 30 };
    TryReplacePerson(person);
    Console.WriteLine(person.Name); // 输出: Alice

    // 如果需要允许在函数内部修改实参的引用，需要传入引用的引用
    // 类比C语言中指针的指针
    static void ReallyReplacePerson(ref Person arg)
    {
        // ref 传递引用的引用，调用方的变量被替换
        arg = new Person { Name = "Bob", Age = 25 };
    }
    person = new Person { Name = "Alice", Age = 30 };
    // 函数让person指向了一个新对象
    // 如果其原先指向的对象没有被其他变量引用，这个对象就永远无法被访问到的了
    // 因此它所占用的内存会在合适的时机被垃圾收集器回收。
    ReallyReplacePerson(ref person);
    Console.WriteLine(person.Name); // 输出: Bob

}
#region 自定义类型，top-level下必须放在文件最底部

public class Person
{
    public string Name { get; set; } = "Unknown";
    public int Age { get; set; } = 0;

    public override string ToString()
    {
        return $"Person(Name={Name}, Age={Age})";
    }
}

record PersonRecord(string Name, int Age);


#endregion

#region 总结和说明
/*
常见问题:
数组是引用类型，那 int[] 中的 int 元素也存在堆上吗？
是的。数组对象在堆上，数组的元素作为数组对象的一部分也存储在堆上。
即使元素是值类型（int），只要它们是数组元素，就存在堆上。
这与"值类型存储在栈上"的说法并不矛盾——那个说法只适用于局部变量，不适用于作为其他对象字段/元素的情况。

string 是引用类型，为什么它的赋值语义看起来像值类型？
string 是不可变的引用类型，所有"修改"操作都创建新字符串。
所以 s2 = s1; s1 = s1.ToUpper(); 之后，s2 不变
因为 ToUpper() 返回了新字符串，s1 被指向了新对象，而 s2 还指向原来的字符串对象。
字符串的"值语义外观"来自于它的不可变性，而非真正的值类型复制。

== 对引用类型比较的是引用还是内容？
默认比较引用（ReferenceEquals），但很多类型重载了==，比如：
string 比较内容，record class 比较内容（自动生成），
其他大多数类型除非显式重载，否则比较引用。
不确定时用 ReferenceEquals 明确比较引用，用 Equals() 明确比较值（如果重载了的话）。

适用场景
- 复杂数据模型：用 class 建模业务实体（用户、订单、产品），需要行为和状态的组合。
- 共享可变状态：多个组件需要读写同一个对象时，引用语义天然支持共享（缓存、上下文对象）。
- 大型数据结构：结构很大时，引用类型传参只传引用（通常 8 字节），而值类型传参要复制全部数据。
- 需要继承和多态：值类型不支持继承（struct 不能继承其他 struct 或 class），需要继承时必须用 class。
- 集合元素：List<T> 本身是引用类型，存储在堆上。

注意事项
- 引用类型的默认值是 null，未初始化的字段自动为 null。在启用 NRT 的项目中，对非可空引用类型字段的不当初始化会有编译警告。
- 在多线程场景中，引用类型的共享状态是并发问题的根源。多个线程同时读写同一个对象需要加锁或使用线程安全的集合。
- 大量创建短期引用类型对象会给 GC 造成压力（GC 需要追踪、扫描和回收）。高频创建小对象的场景，可以考虑使用值类型（struct）或对象池（ObjectPool<T>）。
- 对象的浅复制（只复制字段值）和深复制（递归复制所有引用的对象）是两个不同的概念。MemberwiseClone()（Object 的方法）提供浅复制，
深复制需要手动实现或用序列化/反序列化方式完成。

总结
引用类型是 C# 中的另一大类型基础，变量持有的是对象的引用，赋值和传参时复制引用而非数据，多个变量可以共享同一个对象。
class、string、array、interface 是最常见的引用类型，对象在堆上分配并由 GC 管理。
null 是引用类型的默认值，空引用异常是 C# 中最常见的运行时错误，启用 NRT 并养成防御性 null 检查习惯是防范它的有效手段。
理解引用赋值语义（共享对象）与值赋值语义（独立副本）的区别，是处理复杂数据流和避免意料外副作用的关键。
*/
#endregion