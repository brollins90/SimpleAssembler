namespace SimpleAssembler.Lexer
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class LexSyntaxException : Exception
    {
        public LexSyntaxException()
        {
        }

        public LexSyntaxException(string message) : base(message)
        {
        }

        public LexSyntaxException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LexSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}