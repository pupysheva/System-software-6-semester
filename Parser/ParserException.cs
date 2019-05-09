using Lexer;
using System;
using System.Collections.Generic;

namespace Parser
{
    public class ParserException : Exception
    {
        public ParserException(string message, object expected, object actual, IList<Token> listWhereProblem, int TokenIndex)
            : base($"{message} expected: {expected}, actual: {actual}, token at {TokenIndex} = {(listWhereProblem != null && 0 <= TokenIndex && TokenIndex < listWhereProblem.Count ? listWhereProblem[TokenIndex] : null)}")
        {
            if(listWhereProblem != null && 0 <= TokenIndex && TokenIndex < listWhereProblem.Count)
                this.TokenProblem = listWhereProblem[TokenIndex];
            this.TokenIndex = TokenIndex;
        }

        public ParserException(object expected, object actual, IList<Token> listWhereProblem, int TokenIndex)
            : base($"expected: {expected}, actual: {actual}, token at {TokenIndex} = {(listWhereProblem != null && 0 <= TokenIndex && TokenIndex < listWhereProblem.Count ? listWhereProblem[TokenIndex] : null)}")
        {
            if (listWhereProblem != null && 0 <= TokenIndex && TokenIndex < listWhereProblem.Count)
                this.TokenProblem = listWhereProblem[TokenIndex];
            this.TokenIndex = TokenIndex;
        }

        public ParserException(string messsage)
            : base(messsage) { }
        public ParserException()
            : base() { }

        /// <summary>
        /// Токен, в котором возникла проблема.
        /// </summary>
        public Token TokenProblem { get; } = null;

        /// <summary>
        /// Идентификатор токена с проблемой.
        /// </summary>
        public int TokenIndex { get; } = -1;
    }
}