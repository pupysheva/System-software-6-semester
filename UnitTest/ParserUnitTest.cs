using System;
using Lexer;
using Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static UnitTest.LexerUnitTest;
using static Parser.RuleOperator;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class ParserUnitTest
    {
        /// <summary>
        /// Содержит нетерминалы среды тестирования.
        /// </summary>
        private readonly ParserLang parserLang;
        /// <summary>
        /// Содержит терминалы среды тестирования.
        /// </summary>
        private readonly LexerLang lexerLang;
        public ParserUnitTest()
        {
            lexerLang = new LexerLang(new Terminal[]
            {
                new Terminal("ASSIGN_OP", "^=$"),
                new Terminal("VAR", "^[a-zA-Z]+$",uint.MaxValue),
                new Terminal("DIGIT", "^0|([1-9][0-9]*)$"),
                new Terminal("OP", "^\\+|-|\\*|/$"),
                new Terminal("LOGICAL_OP", "^>|<|>=|<=|==$"),
                new Terminal("WHILE_KW", "^while$", 0),
                new Terminal("PRINT_KW", "^print$",0),
                new Terminal("FOR_KW", "^for$",0),
                new Terminal("IF_KW", "^if$",0),
                new Terminal("ELSE_KW", "^else$",0),
                new Terminal("L_QB", "^{$"),
                new Terminal("R_QB", "^}$"),
                new Terminal("L_B", "^\\($"),
                new Terminal("R_B", "^\\)$"),
                new Terminal("COMMA", "^;$"),
                new Terminal("COM", "^,$"),
                /*
                 Те терминалы, которые ниже, по-сути нужны парсеру.
                 Для того, чтобы проанализировать выражение:
                 a = "Привет, мир!",
                 чтобы не было так:
                 a="Привет,мир!".
                 */
                new Terminal("CH_SPACE", "^ $"),
                new Terminal("CH_LEFTLINE", "^\r$"),
                new Terminal("CH_NEWLINE", "^\n$"),
                new Terminal("CH_TAB", "^\t$")
            }
            );


            // Переменная lang используется в while_body, поэтому её надо объявить раньше остальных.
            Nonterminal lang = new Nonterminal(ZERO_AND_MORE);

            Nonterminal while_body = new Nonterminal(AND, "L_QB", lang, "R_QB");
            Nonterminal value = new Nonterminal(OR, "VAR", "DIGIT");
            Nonterminal while_condition = new Nonterminal(AND, value, "LOGICAL_OP", value);
            Nonterminal while_expr = new Nonterminal(AND, "WHILE_KW", while_condition, while_body);
            Nonterminal stmt = new Nonterminal(AND, value, new Nonterminal(ZERO_AND_MORE, "OP", value));
            Nonterminal assign_expr = new Nonterminal(AND, "VAR", "ASSIGN_OP", stmt);
            Nonterminal expr = new Nonterminal(OR, assign_expr, while_expr, "PRINT_KW");
            lang.Add(expr);

            parserLang = new ParserLang(lang);
        }

        [TestMethod]
        public void ParserOR_assign_op()
        {
            List<Token> tokens = lexerLang.SearchTokens(OpenFile(Resource1.ParserOR_assign_op));
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(
                ReportParser.SUCCESS,
                new ParserLang(new Nonterminal(OR, "ASSIGN_OP")).Check(tokens)
            );
        }
        [TestMethod]
        public void ParserOR_assign_op2()
        {
            List<Token> tokens = lexerLang.SearchTokens(OpenFile(Resource1.ParserOR_assign_op));
            Assert.AreEqual(1, tokens.Count);
            Assert.IsTrue(new ParserLang(new Nonterminal(OR, "абвгд", "ASSIGN_OP")).Check(tokens).IsSuccess);
        }

        [TestMethod]
        public void ParserONE_AND_MORE_OR__ASSIGN_OP__VAR()
        {
            List<Token> tokens = lexerLang.SearchTokens(OpenFile(Resource1.ParserONE_AND_MORE_OR__ASSIGN_OP__VAR));
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            Assert.AreEqual(21, tokens.Count);

            Nonterminal expr = new Nonterminal(OR, "ASSIGN_OP", "VAR");
            Nonterminal lang = new Nonterminal(ONE_AND_MORE, expr);
            ReportParser report = new ParserLang(lang).Check(tokens);
            Console.WriteLine(report);
            Assert.IsTrue(report.IsSuccess);
        }

        [TestMethod]
        public void ParserOR_assign_op3()
        {
            List<Token> tokens = lexerLang.SearchTokens(OpenFile(Resource1.ParserOR_assign_op));

            ReportParser output = parserLang.Check(tokens);
            Assert.AreEqual(1, output.Count);

        }

        [TestMethod]
        public void Parser_assign_op_full()
        {
            List<Token> tokens = lexerLang.SearchTokens(OpenFile(Resource1.Parser_assign_op_full));
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            Assert.AreEqual(17, tokens.Count);

            ReportParser report = parserLang.Check(tokens);
            Console.WriteLine(report);
            Assert.IsTrue(report.IsSuccess);
        }

        [TestMethod]
        public void _while()
        {
            List<Token> tokens = lexerLang.SearchTokens(OpenFile(Resource1._while));
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            Assert.AreEqual(16, tokens.Count);

            ReportParser report = parserLang.Check(tokens);
            Console.WriteLine(report);
            Assert.IsTrue(report.IsSuccess);
        }

        [TestMethod]
        public void Parser_var_op_while_print_var()
        {
            List<Token> tokens = lexerLang.SearchTokens(OpenFile(Resource1.Parser_var_op_while_print_var));
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            Assert.AreEqual(18, tokens.Count);

            ReportParser report = parserLang.Check(tokens);
            Console.WriteLine(report);
            Assert.IsFalse(report.IsSuccess);
        }
    }
}
