using System;
using System.Runtime.Serialization;

namespace MagicCardTracker.Pwa.Cache
{
    internal class CacheMissException : Exception
    {
        public CacheMissException()
        {
        }

        public CacheMissException(string message) : base(message)
        {
        }

        public CacheMissException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected CacheMissException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
