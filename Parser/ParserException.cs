using System;

namespace Parser
{
    public class ParserException : Exception
    {
        public ParserException(object expected, object actual) :
            base($"expected: {expected}, actual: {actual}") { }
        public ParserException(string messsage, int IndexTerminalError = -1)
            : base(messsage + $"IndexTerminalError = {IndexTerminalError}") => this.IndexTerminalError = IndexTerminalError;
        public ParserException()
            : base() { }

        public int IndexTerminalError { get; } = -1;
        //public List<Terminal> Expected = ;
    }
}