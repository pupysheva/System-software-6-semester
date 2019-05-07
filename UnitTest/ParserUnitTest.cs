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

        [TestMethod]
        public void ParserOR_assign_op3()
        {
            List<Token> tokens = new LexerLang().SearchTokens(OpenFile(Resource1.ParserOR_assign_op));

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

            ReportParser output = new ParserLang(lang).Check(tokens);
            Assert.AreEqual(1, output.Count);

        }

    }
}
