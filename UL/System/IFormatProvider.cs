using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
