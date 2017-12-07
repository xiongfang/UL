using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    class Event
    {
        public extern static Event operator +(Event a, object b);
        public extern static Event operator -(Event a, object b);
    }
}
