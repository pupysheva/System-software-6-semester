using Lexer;
using System;

namespace Parser
{
    public class ParserException : Exception
    {
        public ParserException(string message, object expected, object actual, Token TokenProblem = null)
            : base($"{message}: expected: {expected}, actual: {actual}") => this.TokenProblem = TokenProblem;
        public ParserException(object expected, object actual, Token TokenProblem = null)
            : base($"expected: {expected}, actual: {actual}") => this.TokenProblem = TokenProblem;
        public ParserException(string messsage, Token TokenProblem = null)
            : base(messsage + $"IndexTerminalError = {TokenProblem}") => this.TokenProblem = TokenProblem;
        public ParserException()
            : base() { }

        /// <summary>
        /// Токен, в котором возникла проблема.
        /// </summary>
        public Token TokenProblem { get; } = null;
    }
}