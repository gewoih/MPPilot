using System.Runtime.Serialization;

namespace MPPilot.Domain.Exceptions
{
    public class APIKeyAlreadyExistsException : Exception
    {
        public APIKeyAlreadyExistsException()
        {
        }

        public APIKeyAlreadyExistsException(string? message) : base(message)
        {
        }

        public APIKeyAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected APIKeyAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
