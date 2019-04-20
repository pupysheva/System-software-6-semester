using Lexer;
using System;
using System.Collections.Generic;

namespace Parser
{
    public class ParserException : Exception
    {
        public ParserException(string messsage)
            : base(messsage) { }
        public ParserException()
            : base() { }

        public int EndexTerminalError { get; } = -1;
        //public List<Terminal> Expected = ;
    }
}