using System;
using Lexer;
using Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static UnitTest.UnitTest1;
using static Parser.RuleOperator;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class LexerUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<Token> tokens = new LexerLang().SearchTokens(OpenFile(Resource1.ParserOR_assign_op));
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(
                ReportParser.SUCCESS,
                new ParserLang(new Nonterminal(OR, "ASSIGN_OP")).Check(tokens)
            );
        }
        [TestMethod]
        public void TestMethod2()
        {
            List<Token> tokens = new LexerLang().SearchTokens(OpenFile(Resource1.ParserOR_assign_op));
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(
                ReportParser.SUCCESS,
                new ParserLang(new Nonterminal(OR, "абвгд", "ASSIGN_OP")).Check(tokens)
            );
        }

    }
}
