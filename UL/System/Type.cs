using System.Reflection;
namespace System
{
    public class Type
    {
        public Type BaseType
        {
            get;
        }

        public Type DeclaringType
        {
            get;
        }

        public string FullName
        {
            get;
        }

        public bool IsAbstract
        {
            get;
        }

        public bool IsClass
        {
            get;
        }

        public bool IsEnum
        {
            get;
        }

        public bool IsGenericType
        {
            get;
        }

        public bool IsInterface
        {
            get;
        }

        public bool IsPublic
        {
            get;
        }

        public bool IsValueType
        {
            get;
        }

        public string Name
        {
            get;
        }

        public string Namespace
        {
            get;
        }

        public extern virtual Type[] FindInterfaces(
            TypeFilter filter,
            object filterCriteria
        );

        public extern virtual MemberInfo[] FindMembers(
            MemberTypes memberType,
            BindingFlags bindingAttr,
            MemberFilter filter,
            object filterCriteria
        );
    }
}
