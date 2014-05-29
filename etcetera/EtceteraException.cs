namespace etcetera
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EtceteraException : Exception
    {
        public EtceteraException()
        {
        }

        public EtceteraException(string message) : base(message)
        {
        }

        public EtceteraException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EtceteraException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}