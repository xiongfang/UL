
namespace System.Reflection
{
    //
    // 摘要:
    //     获取有关成员属性的信息并提供对成员元数据的访问。
    public abstract class MemberInfo
    {
        //
        // 摘要:
        //     在派生类中重写时，获取一个 System.Reflection.MemberTypes 值，指示此成员的类型（方法、构造函数和事件等）。
        //
        // 返回结果:
        //     指示成员类型的 System.Reflection.MemberTypes 值。
        public abstract MemberTypes MemberType { get; }
        //
        // 摘要:
        //     获取当前成员的名称。
        //
        // 返回结果:
        //     包含此成员名称的 System.String。
        public abstract string Name { get; }
        //
        // 摘要:
        //     获取声明该成员的类。
        //
        // 返回结果:
        //     声明该成员的类的 Type 对象。
        public abstract Type DeclaringType { get; }
        //
        // 摘要:
        //     获取用于获取 MemberInfo 的此实例的类对象。
        //
        // 返回结果:
        //     Type 对象，通过它获取了该 MemberInfo 对象。
        public abstract Type ReflectedType { get; }
        ////
        //// 摘要:
        ////     获取包含此成员自定义特性的集合。
        ////
        //// 返回结果:
        ////     包含此成员的自定义特性的集合。
        //public virtual IEnumerable<CustomAttributeData> CustomAttributes { get; }
        //
        // 摘要:
        //     获取一个值，该值标识元数据元素。
        ////
        //// 返回结果:
        ////     一个值，与 System.Reflection.MemberInfo.Module 一起来唯一标识元数据元素。
        ////
        //// 异常:
        ////   T:System.InvalidOperationException:
        ////     当前 System.Reflection.MemberInfo 表示某数组类型的数组方法（如 Address），该数组类型的元素类型属于尚未完成的动态类型。
        ////     若要在这种情况下获取元数据标记，请将 System.Reflection.MemberInfo 对象传递给 System.Reflection.Emit.ModuleBuilder.GetMethodToken(System.Reflection.MethodInfo)
        ////     方法；或者使用 System.Reflection.Emit.ModuleBuilder.GetArrayMethodToken(System.Type,System.String,System.Reflection.CallingConventions,System.Type,System.Type[])
        ////     方法直接获取该标记，而不是首先使用 System.Reflection.Emit.ModuleBuilder.GetArrayMethod(System.Type,System.String,System.Reflection.CallingConventions,System.Type,System.Type[])
        ////     方法获取 System.Reflection.MethodInfo。
        //public virtual int MetadataToken { get; }
        ////
        //// 摘要:
        ////     获取一个模块，在该模块中已经定义一个类型，该类型用于声明由当前 System.Reflection.MemberInfo 表示的成员。
        ////
        //// 返回结果:
        ////     System.Reflection.Module，在其中已经定义一个类型，该类型用于声明由当前 System.Reflection.MemberInfo
        ////     表示的成员。
        ////
        //// 异常:
        ////   T:System.NotImplementedException:
        ////     此方法未实现。
        //public virtual Module Module { get; }

        //
        // 摘要:
        //     返回一个值，该值指示此实例是否与指定的对象相等。
        //
        // 参数:
        //   obj:
        //     与此实例进行比较的 object，或 null。
        //
        // 返回结果:
        //     如果 obj 等于此实例的类型和值，则为 true；否则为 false。
        //public override bool Equals(object obj);
        //
        // 摘要:
        //     在派生类中重写时，返回应用于此成员的所有自定义特性的数组。
        //
        // 参数:
        //   inherit:
        //     搜索此成员的继承链以查找这些属性，则为 true；否则为 false。 属性和事件中忽略此参数，请参见“备注”。
        //
        // 返回结果:
        //     一个包含应用于此成员的所有自定义特性的数组，在未定义任何特性时为包含零个元素的数组。
        //
        // 异常:
        //   T:System.InvalidOperationException:
        //     该成员属于加载到仅反射上下文的类型。 请参见如何：将程序集加载到仅反射上下文中。
        //
        //   T:System.TypeLoadException:
        //     未能加载自定义特性类型。
        //public abstract object[] GetCustomAttributes(bool inherit);
        //
        // 摘要:
        //     在派生类中重写时，返回应用于此成员并由 System.Type 标识的自定义特性的数组。
        //
        // 参数:
        //   attributeType:
        //     要搜索的特性类型。 只返回可分配给此类型的属性。
        //
        //   inherit:
        //     搜索此成员的继承链以查找这些属性，则为 true；否则为 false。 属性和事件中忽略此参数，请参见“备注”。
        //
        // 返回结果:
        //     应用于此成员的自定义特性的数组；如果未应用任何可分配给 attributeType 的特性，则为包含零个元素的数组。
        //
        // 异常:
        //   T:System.TypeLoadException:
        //     无法加载自定义特性类型。
        //
        //   T:System.ArgumentNullException:
        //     如果 attributeType 为 null。
        //
        //   T:System.InvalidOperationException:
        //     该成员属于加载到仅反射上下文的类型。 请参见如何：将程序集加载到仅反射上下文中。
        //public abstract object[] GetCustomAttributes(Type attributeType, bool inherit);
        //
        // 摘要:
        //     返回一个 System.Reflection.CustomAttributeData 对象列表，这些对象表示有关已应用于目标成员的特性的数据。
        //
        // 返回结果:
        //     System.Reflection.CustomAttributeData 对象的泛型列表，这些对象表示有关已应用于目标成员的特性的数据。
        //public virtual IList<CustomAttributeData> GetCustomAttributesData();
        //
        // 摘要:
        //     返回此实例的哈希代码。
        //
        // 返回结果:
        //     32 位有符号整数哈希代码。
        //public override int GetHashCode();
        //
        // 摘要:
        //     在派生类中重写时，指示是否将指定类型或其派生类型的一个或多个特性应用于此成员。
        //
        // 参数:
        //   attributeType:
        //     要搜索的自定义属性的类型。 该搜索包括派生类型。
        //
        //   inherit:
        //     搜索此成员的继承链以查找这些属性，则为 true；否则为 false。 属性和事件中忽略此参数，请参见“备注”。
        //
        // 返回结果:
        //     如果将 attributeType 或其任何派生类型的一个或多个实例应用于此成员，则为 true；否则为 false。
        //public abstract bool IsDefined(Type attributeType, bool inherit);

        //
        // 摘要:
        //     指示两个 System.Reflection.MemberInfo 对象是否相等。
        //
        // 参数:
        //   left:
        //     要与 right 进行比较的 System.Reflection.MemberInfo。
        //
        //   right:
        //     要与 left 进行比较的 System.Reflection.MemberInfo。
        //
        // 返回结果:
        //     如果 left 等于 right，则为 true；否则为 false。
        //public static bool operator ==(MemberInfo left, MemberInfo right);
        //
        // 摘要:
        //     指示两个 System.Reflection.MemberInfo 对象是否不相等。
        //
        // 参数:
        //   left:
        //     要与 right 进行比较的 System.Reflection.MemberInfo。
        //
        //   right:
        //     要与 left 进行比较的 System.Reflection.MemberInfo。
        //
        // 返回结果:
        //     如果 left 不等于 right，则为 true；否则为 false。
        //public static bool operator !=(MemberInfo left, MemberInfo right);
    }
}
