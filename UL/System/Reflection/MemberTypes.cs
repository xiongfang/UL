
namespace System.Reflection
{
    //
    // 摘要:
    //     标记每个已定义为 MemberInfo 的派生类的成员类型。
    public enum MemberTypes
    {
        //
        // 摘要:
        //     指定该成员是一个构造函数，表示 System.Reflection.ConstructorInfo 成员。 0x01 的十六进制值。
        Constructor = 1,
        //
        // 摘要:
        //     指定该成员是一个事件，表示 System.Reflection.EventInfo 成员。 0x02 的十六进制值。
        Event = 2,
        //
        // 摘要:
        //     指定该成员是一个字段，表示 System.Reflection.FieldInfo 成员。 0x04 的十六进制值。
        Field = 4,
        //
        // 摘要:
        //     指定该成员是一个方法，表示 System.Reflection.MethodInfo 成员。 0x08 的十六进制值。
        Method = 8,
        //
        // 摘要:
        //     指定该成员是一个属性，表示 System.Reflection.PropertyInfo 成员。 0x10 的十六进制值。
        Property = 16,
        //
        // 摘要:
        //     指定该成员是一种类型，表示 System.Reflection.MemberTypes.TypeInfo 成员。 0x20 的十六进制值。
        TypeInfo = 32,
        //
        // 摘要:
        //     指定该成员是一个自定义成员类型。 0x40 的十六进制值。
        Custom = 64,
        //
        // 摘要:
        //     指定该成员是一个嵌套类型，可扩展 System.Reflection.MemberInfo。
        NestedType = 128,
        //
        // 摘要:
        //     指定所有成员类型。
        All = 191
    }
}
