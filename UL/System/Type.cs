using System.Reflection;
namespace System
{
    public abstract class Type
    {
        public abstract Type BaseType
        {
            get;
        }

        public abstract Type DeclaringType
        {
            get;
        }

        public abstract string FullName
        {
            get;
        }

        public abstract bool IsAbstract
        {
            get;
        }

        public abstract bool IsClass
        {
            get;
        }

        public abstract bool IsEnum
        {
            get;
        }

        public abstract bool IsGenericType
        {
            get;
        }

        public abstract bool IsInterface
        {
            get;
        }

        public abstract bool IsPublic
        {
            get;
        }

        public abstract bool IsValueType
        {
            get;
        }

        public abstract string Name
        {
            get;
        }

        public abstract string Namespace
        {
            get;
        }

        public abstract Type[] FindInterfaces(
            TypeFilter filter,
            object filterCriteria
        );

        public abstract MemberInfo[] FindMembers(
            MemberTypes memberType,
            BindingFlags bindingAttr,
            MemberFilter filter,
            object filterCriteria
        );


    }
}
