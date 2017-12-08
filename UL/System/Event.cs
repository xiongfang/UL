using System.Reflection;

namespace System
{
    public abstract class Delegate
    {
        object _target;
        //
        // 摘要:
        //     初始化一个委托，该委托对指定的类实例调用指定的实例方法。
        //
        // 参数:
        //   target:
        //     类实例，委托对其调用 method。
        //
        //   method:
        //     委托表示的实例方法的名称。
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     target 为 null。 - 或 - method 为 null。
        //
        //   T:System.ArgumentException:
        //     绑定到目标方法时出错。
        //[SecuritySafeCritical]
        //protected Delegate(object target, string method);
        //
        // 摘要:
        //     初始化一个委托，该委托从指定的类调用指定的静态方法。
        //
        // 参数:
        //   target:
        //     System.Type，它表示定义 method 的类。
        //
        //   method:
        //     委托表示的静态方法的名称。
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     target 为 null。 - 或 - method 为 null。
        //
        //   T:System.ArgumentException:
        //     target 不是 RuntimeType。 请参见 反射中的运行时类型。 - 或 - target 表示开放式泛型类型。
        //[SecuritySafeCritical]
        //protected Delegate(Type target, string method);
        //
        // 摘要:
        //     获取委托所表示的方法。
        //
        // 返回结果:
        //     描述委托所表示的方法的 System.Reflection.MethodInfo。
        //
        // 异常:
        //   T:System.MemberAccessException:
        //     调用方不能访问委托所表示的方法（例如，当该方法为私有时）。
        public MethodInfo Method { get { return GetMethodImpl(); } }
        //
        // 摘要:
        //     获取类实例，当前委托将对其调用实例方法。
        //
        // 返回结果:
        //     如果委托表示实例方法，则为当前委托对其调用实例方法的对象；如果委托表示静态方法，则为 null。
        public object Target
        {
            get { return _target; }
        }

        public static Delegate Combine(Delegate a, Delegate b)
        {
            if (a == null && b == null)
                return null;
            if (a == null)
                return b;
            if (b == null)
                return a;

            return a.CombineImpl(b);
        }

        public static Delegate Combine(params Delegate[] delegates)
        {
            Delegate left = null;
            for (int i=0;i<delegates.Length;i++)
            {
                left = Combine(left,delegates[i]);
            }

            return left;
        }
        //
        // 摘要:
        //     从一个委托的调用列表中移除另一个委托的最后一个调用列表。
        //
        // 参数:
        //   source:
        //     委托，将从中移除 value 的调用列表。
        //
        //   value:
        //     委托，它提供将从其中移除 source 的调用列表的调用列表。
        //
        // 返回结果:
        //     一个新委托，其调用列表的构成方法为：获取 source 的调用列表，如果在 source 的调用列表中找到了 value 的调用列表，则从中移除 value
        //     的最后一个调用列表。 如果 value 为 null，或在 source 的调用列表中没有找到 value 的调用列表，则返回 source。 如果 value
        //     的调用列表等于 source 的调用列表，或 source 为空引用，则返回空引用。
        //
        // 异常:
        //   T:System.MemberAccessException:
        //     调用方不能访问委托所表示的方法（例如，当该方法为私有时）。
        //
        //   T:System.ArgumentException:
        //     委托类型不匹配。
        public extern static Delegate Remove(Delegate source, Delegate value);

        public extern static Delegate RemoveAll(Delegate source, Delegate value);

        public object DynamicInvoke(params object[] args)
        {
            return DynamicInvokeImpl(args);
        }

        public extern virtual Delegate[] GetInvocationList();


        //
        // 摘要:
        //     将指定多路广播（可组合）委托和当前多路广播（可组合）委托的调用列表连接起来。
        //
        // 参数:
        //   d:
        //     多路广播（可组合）委托，其调用列表要追加到当前多路广播（可组合）委托的调用列表的结尾。
        //
        // 返回结果:
        //     新的多路广播（可组合）委托，其调用列表将当前多路广播（可组合）委托的调用列表和 d 的调用列表连接在一起；或者如果 d 为 null，则返回当前多路广播（可组合）委托。
        //
        // 异常:
        //   T:System.MulticastNotSupportedException:
        //     总是引发。
        protected extern virtual Delegate CombineImpl(Delegate d);
        //
        // 摘要:
        //     动态调用（后期绑定）由当前委托所表示的方法。
        //
        // 参数:
        //   args:
        //     属于传递给由当前委托表示的方法的参数的对象数组。-或- 如果当前委托表示的方法不需要参数，则为 null。
        //
        // 返回结果:
        //     委托所表示的方法返回的对象。
        //
        // 异常:
        //   T:System.MemberAccessException:
        //     调用方不具有访问由该委托表示的方法的权限（例如，如果方法是专用的）。-或- args 中列出的参数数量、命令或类型无效。
        //
        //   T:System.ArgumentException:
        //     对对象或类调用委托所表示的方法，但该对象或类不支持这种方法。
        //
        //   T:System.Reflection.TargetInvocationException:
        //     委托表示的方法是实例方法，并且目标对象是 null。-或- 某个封装的方法引发异常。
        //[SecuritySafeCritical]
        protected extern virtual object DynamicInvokeImpl(object[] args);
        //
        // 摘要:
        //     获取当前委托所表示的静态方法。
        //
        // 返回结果:
        //     描述当前委托表示的静态方法的 System.Reflection.MethodInfo。
        //
        // 异常:
        //   T:System.MemberAccessException:
        //     调用方不能访问委托所表示的方法（例如，当该方法为私有时）。
        //[SecuritySafeCritical]
        protected extern virtual MethodInfo GetMethodImpl();
        //
        // 摘要:
        //     从一个委托的调用列表中移除另一个委托的调用列表。
        //
        // 参数:
        //   d:
        //     委托，它提供要从当前委托的调用列表中移除的调用列表。
        //
        // 返回结果:
        //     一个新委托，其调用列表的构成方法为：获取当前委托的调用列表，如果在当前委托的调用列表中找到了 value 的调用列表，则从中移除 value 的调用列表。
        //     如果 value 为 null，或者在当前委托的调用列表中没有找到 value 的调用列表，则返回当前委托。 如果 value 的调用列表等于当前委托的调用列表，则返回
        //     null。
        //
        // 异常:
        //   T:System.MemberAccessException:
        //     调用方不能访问委托所表示的方法（例如，当该方法为私有时）。
        protected extern virtual Delegate RemoveImpl(Delegate d);

        //
        // 摘要:
        //     确定指定的委托是否相等。
        //
        // 参数:
        //   d1:
        //     要比较的第一个委托。
        //
        //   d2:
        //     要比较的第二个委托。
        //
        // 返回结果:
        //     如果 d1 等于 d2，则为 true；否则为 false。
        //[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public extern static bool operator ==(Delegate d1, Delegate d2);
        //
        // 摘要:
        //     确定指定的委托是否相等。
        //
        // 参数:
        //   d1:
        //     要比较的第一个委托。
        //
        //   d2:
        //     要比较的第二个委托。
        //
        // 返回结果:
        //     如果 d1 不等于 d2，则为 true；否则为 false。
        public extern static bool operator !=(Delegate d1, Delegate d2);


        public static Delegate operator +(Delegate d1, Delegate d2)
        {
            return Combine(d1, d2);
        }
        public static Delegate operator -(Delegate d1, Delegate d2)
        {
            return Remove(d1, d2);
        }
    }
}
