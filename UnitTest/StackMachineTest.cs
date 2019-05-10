using System;
using System.Collections.Generic;
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
                new Terminal("DIGIT", "^0|([1-9][0-9]*)$"),
                new Terminal("OP", "^\\+|-|\\*|/$"),
                new Terminal("WHILE_KW", "^while$", 0),
                new Terminal("PRINT_KW", "^print$", 0),
                new Terminal("L_QB", "^{$"),
                new Terminal("R_QB", "^}$"),
                new Terminal("L_B", "^\\($"),
                new Terminal("R_B", "^\\)$"),
                new Terminal("COM", "^,$"),

                new Terminal("CH_SPACE", "^ $"),
                new Terminal("CH_LEFTLINE", "^\r$"),
                new Terminal("CH_NEWLINE", "^\n$"),
                new Terminal("CH_TAB", "^\t$")
            });

            Nonterminal lang = new Nonterminal("lang", ZERO_AND_MORE);
            Nonterminal value = new Nonterminal("value", OR, "VAR", "DIGIT");
            Nonterminal stmt = new Nonterminal("stmt", AND, value, new Nonterminal("(OP value)*", ZERO_AND_MORE, "OP", value));
            Nonterminal assign_expr = new Nonterminal("assign_expr",
                    new ushort[] { 0, 2, 1 }, AND,
                    "VAR", "ASSIGN_OP", stmt);
            Nonterminal while_expr = new Nonterminal("while_expr", AND, "L_B", stmt, "R_B", "L_QB", lang, "R_QB");
            Nonterminal expr = new Nonterminal("expr",
                (IEnumerator<object> a, Stack<Token> mem) =>
                {
                    if(a.Current is Terminal)

                    //mem.
                }, OR,
                assign_expr, while_expr, "PRINT_KW");
            lang.Add(expr);
        }

        [TestMethod]
        public void TestMethod1()
        {
            
        }
    }
}
