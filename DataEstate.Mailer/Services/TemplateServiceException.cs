using System;
using System.Runtime.Serialization;

namespace DataEstate.Mailer.Services
{
    [Serializable]
    internal class TemplateServiceException : Exception
    {
        public TemplateServiceException()
        {
        }

        public TemplateServiceException(string message) : base(message)
        {
        }

        public TemplateServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TemplateServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}