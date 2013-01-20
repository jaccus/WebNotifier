using System;

namespace WebNotifier
{
    internal class WebNotifierException : Exception
    {
        public WebNotifierException(string msg) : base(msg)
        {
        }

        public WebNotifierException(string msg, Exception e) : base(msg, e)
        {
        }
    }
}