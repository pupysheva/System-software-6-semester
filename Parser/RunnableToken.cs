using System;
using System.Collections.Generic;
using Lexer;

namespace Parser
{
    /// <summary>
    /// Функция должна преобразовывать входящие RunnableToken и Token в RunnableToken.
    /// </summary>
    /// <param name="RunnableTokens_or_and_Tokens">Последовательность жетонов и запускаемых жетонов.</param>
    /// <returns></returns>
    delegate StackMachine TokenTo(List<object> RunnableTokens_or_and_Tokens);

    /// <summary>
    /// 
    /// </summary>
    class StackMachine
    {
        public StackMachine()
        {

        }

        public void Run()
        {
            Stack<Token> memory = new Stack<Token>();
            run(memory);
        }

        private Action<Stack<Token>> run { get; }
    }
}
