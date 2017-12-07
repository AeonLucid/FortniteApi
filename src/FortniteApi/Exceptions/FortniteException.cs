using System;
using System.Runtime.Serialization;

namespace FortniteApi.Exceptions
{
    public class FortniteException : Exception
    {
        public FortniteException()
        {
        }

        protected FortniteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public FortniteException(string message) : base(message)
        {
        }

        public FortniteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
