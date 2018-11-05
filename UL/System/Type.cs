namespace ul
{
    using ul.System.Reflection;
    namespace System
    {
        public class Type
        {
            ul.System.Reflection.Metadata.Type _type;

            public static extern Type GetType(string fullName);

            public Type BaseType
            {
                get
                {
                    return GetType(_type.Namespace + "." + _type.Name);
                }
            }

            public string FullName
            {
                get
                {
                    return _type.Namespace + "." + _type.Name;
                }
            }

            public bool IsAbstract
            {
                get
                {
                    return _type.IsAbstract; 
                }
            }

            public bool IsClass
            {
                get
                {
                    return _type.TypeID == 2;

                }
            }

            public bool IsEnum
            {
                get
                {
                    return _type.TypeID == 3;
                }
            }

            public bool IsGenericType
            {
                get
                {
                    return _type.IsGenericTypeDefinition;
                }
            }

            public bool IsInterface
            {
                get
                {
                    return _type.TypeID == 1;
                }
            }

            public bool IsPublic
            {
                get
                {
                    return _type.Modifier == 0;
                }
            }

            public bool IsValueType
            {
                get
                {
                    return _type.TypeID == 0;
                }
            }

            public string Name
            {
                get
                {
                    return _type.Name;
                }
            }

            public string Namespace
            {
                get
                {
                    return _type.Namespace;
                }
            }

            public bool IsChildOf(Type type)
            {
                Type baseType = BaseType;
                while (baseType !=null)
                {
                    if (baseType == type)
                        return true;
                    baseType = baseType.BaseType;
                }

                return false;
            }
        }
    }
}