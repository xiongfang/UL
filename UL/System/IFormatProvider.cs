namespace System
{
    public interface IFormatProvider
    {
        Object GetFormat(
            Type formatType
        );
    }

    public interface ICustomFormater
    {
        string Format(
            string format,
            Object arg,
            IFormatProvider formatProvider
        );
    }
}
