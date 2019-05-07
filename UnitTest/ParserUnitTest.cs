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
        [TestMethod]
        public void ParserOR_assign_op()
        {
            List<Token> tokens = new LexerLang().SearchTokens(OpenFile(Resource1.ParserOR_assign_op));
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(
                ReportParser.SUCCESS,
                new ParserLang(new Nonterminal(OR, "ASSIGN_OP")).Check(tokens)
            );
        }
        [TestMethod]
        public void ParserOR_assign_op2()
        {
            List<Token> tokens = new LexerLang().SearchTokens(OpenFile(Resource1.ParserOR_assign_op));
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(
                ReportParser.SUCCESS,
                new ParserLang(new Nonterminal(OR, "абвгд", "ASSIGN_OP")).Check(tokens)
            );
        }

        [TestMethod]
        public void ParserONE_AND_MORE_OR__ASSIGN_OP__VAR()
        {
            List<Token> tokens = new LexerLang().SearchTokens(OpenFile(Resource1.ParserONE_AND_MORE_OR__ASSIGN_OP__VAR));
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            Assert.AreEqual(21, tokens.Count);

            Nonterminal expr = new Nonterminal(OR, "ASSIGN_OP", "VAR");
            Nonterminal lang = new Nonterminal(ONE_AND_MORE, expr);

            Assert.AreEqual(
                ReportParser.SUCCESS,
                new ParserLang(lang).Check(tokens)
            );
        }

    }
}
