namespace System.Reflection
{
    //
    // 摘要:
    //     指定控制绑定和由反射执行的成员和类型搜索方法的标志。
    public enum BindingFlags
    {
        //
        // 摘要:
        //     不指定绑定标志。
        Default = 0,
        //
        // 摘要:
        //     指定当绑定时不应考虑成员名的大小写。
        IgnoreCase = 1,
        //
        // 摘要:
        //     指定只应考虑在所提供类型的层次结构级别上声明的成员。 不考虑继承成员。
        DeclaredOnly = 2,
        //
        // 摘要:
        //     指定实例成员将包括在搜索中。
        Instance = 4,
        //
        // 摘要:
        //     指定静态成员将包括在搜索中。
        Static = 8,
        //
        // 摘要:
        //     指定公共成员将包括在搜索中。
        Public = 16,
        //
        // 摘要:
        //     指定非公共成员将包括在搜索中。
        NonPublic = 32,
        //
        // 摘要:
        //     指定应返回层次结构上的公共静态成员和受保护的静态成员。 不返回继承类中的私有静态成员。 静态成员包括字段、方法、事件和属性。 不返回嵌套类型。
        FlattenHierarchy = 64,
        //
        // 摘要:
        //     指定要调用一个方法。 它不能是构造函数或类型初始值设定项。
        InvokeMethod = 256,
        //
        // 摘要:
        //     指定“反射”应该创建指定类型的实例。 调用与给定参数匹配的构造函数。 忽略提供的成员名。 如果未指定查找类型，将应用 (Instance |Public)。
        //     调用类型初始值设定项是不可能的。
        CreateInstance = 512,
        //
        // 摘要:
        //     指定应返回指定字段的值。
        GetField = 1024,
        //
        // 摘要:
        //     指定应设置指定字段的值。
        SetField = 2048,
        //
        // 摘要:
        //     指定应返回指定属性的值。
        GetProperty = 4096,
        //
        // 摘要:
        //     指定应设置指定属性的值。 对于 COM 属性，指定此绑定标志与指定 PutDispProperty 和 PutRefDispProperty 是等效的。
        SetProperty = 8192,
        //
        // 摘要:
        //     指定应调用 COM 对象的 PROPPUT 成员。 PROPPUT 指定使用值的属性设置函数。 如果属性同时具有 PROPPUT 和 PROPPUTREF，而且需要区分调用哪一个，请使用
        //     PutDispProperty。
        PutDispProperty = 16384,
        //
        // 摘要:
        //     指定应调用 COM 对象的 PROPPUTREF 成员。 PROPPUTREF 指定使用引用而不是值的属性设置函数。 如果属性同时具有 PROPPUT 和
        //     PROPPUTREF，而且需要区分调用哪一个，请使用 PutRefDispProperty。
        PutRefDispProperty = 32768,
        //
        // 摘要:
        //     指定提供参数的类型必须与对应形参的类型完全匹配。 如果调用方提供一个非空 Binder 对象，则“反射”将引发异常，因为这意味着调用方正在提供的 BindToXXX
        //     实现将选取适当的方法。
        ExactBinding = 65536,
        //
        // 摘要:
        //     未实现。
        SuppressChangeType = 131072,
        //
        // 摘要:
        //     返回其参数计数与提供参数的数目匹配的成员集。 此绑定标志用于所带参数具有默认值的方法和带变量参数 (varargs) 的方法。 此标志应只与 System.Type.InvokeMember(System.String,System.Reflection.BindingFlags,System.Reflection.Binder,System.Object,System.Object[],System.Reflection.ParameterModifier[],System.Globalization.CultureInfo,System.String[])
        //     一起使用。
        OptionalParamBinding = 262144,
        //
        // 摘要:
        //     在 COM 互操作中用于指定可以忽略成员的返回值。
        IgnoreReturn = 16777216
    }
}
