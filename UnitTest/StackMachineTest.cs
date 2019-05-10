using System;
using Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using static Parser.RuleOperator;

namespace UnitTest
{
    [TestClass]
    public class StackMachineTest
    {
        private readonly ParserLang EasyParserLang;
        private readonly LexerLang EasyLexerLang;

        public StackMachineTest()
        {
            EasyLexerLang = new LexerLang(new Terminal[]{
                new Terminal("ASSIGN_OP", "^=$"),
                new Terminal("VAR", "^[a-zA-Z]+$", uint.MaxValue),
                new Terminal("PRINT_KW", "^(print)|(PRINT)$",0),
                new Terminal("DIGIT", "^0|([1-9][0-9]*)$"),
                new Terminal("OP", "^\\+|-|\\*|/$"),

                new Terminal("CH_SPACE", "^ $"),
                new Terminal("CH_LEFTLINE", "^\r$"),
                new Terminal("CH_NEWLINE", "^\n$"),
                new Terminal("CH_TAB", "^\t$")
            });

            Nonterminal value = new Nonterminal("value", OR, "VAR", "DIGIT");
            Nonterminal stmt = new Nonterminal("stmt", AND, value, new Nonterminal("(OP value)*", ZERO_AND_MORE, "OP", value));
            Nonterminal assign_expr = new Nonterminal("assign_expr",
                    new ushort[] { 0, 2, 1 }, AND,
                    "VAR", "ASSIGN_OP", stmt);
            Nonterminal expr = new Nonterminal("expr",
                OR,
                assign_expr, "PRINT_KW");
            Nonterminal lang = new Nonterminal("lang", ZERO_AND_MORE, expr);
        }

        [TestMethod]
        public void TestMethod1()
        {
            
        }
    }
}
