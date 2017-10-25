
namespace System
{
    public class Exception
    {
        private string _msg;
        public Exception(string msg) { _msg = msg; }
        public Exception() { _msg = ""; }

        public string Message
        {
            get { return _msg; }
        }
    }
}
