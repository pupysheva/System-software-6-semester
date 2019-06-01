using Lexer;
using Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackMachine
{
    public static class Lang
    {
        public static readonly LexerLang lexerLang;
        public static readonly ParserLang parserLang;
        public static readonly AbstractStackExecuteLang stackMachine;

        static Lang()
        {

        }
    }
}
