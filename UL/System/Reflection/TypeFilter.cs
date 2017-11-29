namespace System.Reflection
{
    public delegate bool TypeFilter(
        Type m,
        object filterCriteria
    );

    public delegate bool MemberFilter(MemberInfo m, object filterCriteria);
}
