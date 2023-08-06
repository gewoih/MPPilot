using System.Runtime.Serialization;

namespace MPPilot.Domain.Exceptions
{
    public class InvalidApiKeyException : Exception
    {
        public InvalidApiKeyException()
        {
        }

        public InvalidApiKeyException(string? message) : base(message)
        {
        }

        public InvalidApiKeyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidApiKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
