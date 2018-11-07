
namespace ul.System.Reflection.Metadata
{
    public class Type
    {
        string _name;
        string _namespace;
        string _comments;
        int _modifier;
        int _type;
        string _parent;
        bool _is_abstract;
        bool _is_generic_type_definition;

        public string Name { get { return _name; } }
        public string Namespace { get { return _namespace; } }
        public string Comments { get { return _comments; } }
        public int Modifier
        {
            get { return _modifier; }
        }
        public int TypeID { get { return _type; } }
        public string Parent { get { return _parent; } }
        public bool IsAbstract { get { return _is_abstract; } }
        public bool IsGenericTypeDefinition { get { return _is_generic_type_definition; } }

    }

    //public class TypeSyntax
    //{
    //    string _name;
    //    string _name_space;
    //    TypeSyntax[] args;
    //    bool isGenericType;
    //    bool isGenericTypeDefinition;
    //    bool isGenericParameter;

    //    public TypeSyntax(string n,string ns,bool a,bool b,bool c)

    //}

    public class Member
    {

    }
}
