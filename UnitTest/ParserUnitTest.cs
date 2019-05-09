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
        private readonly ParserLang parserLang = new ParserLang();
        /// <summary>
        /// Содержит терминалы среды тестирования.
        /// </summary>
        private readonly LexerLang lexerLang = new LexerLang();

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

            ReportParser report = parserLang.Check(tokens);
            Console.WriteLine(report);
            Assert.IsFalse(report.IsSuccess); // Мол, есть ошибки

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
            Assert.IsTrue(report.IsSuccess);
        }
    }
}
