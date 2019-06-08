using System;
using System.Runtime.Serialization;

namespace Lexer
{
    [Serializable]
    public class LexerException : Exception
    {
        public LexerException() : base()
        {
        }

        public LexerException(string message) : base(message)
        {
        }

        public LexerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LexerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}