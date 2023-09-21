using System;
using System.Runtime.Serialization;

namespace OrderService.Domain.Exceptions
{
    [Serializable]
    internal class OrderingDomainException : Exception
    {
        public OrderingDomainException()
        {
        }

        public OrderingDomainException(string message) : base(message)
        {
        }

        public OrderingDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OrderingDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}