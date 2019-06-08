using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using UnitTest;

namespace StackMachine.Test
{
    [TestClass]
    public class FinalLangTest
    {
        [TestMethod]
        public void LexerTest()
        {
            TestOnResurseCount(Resource1.LangExample, 154);
        }

        [TestMethod]
        public void ParserTest()
        {
            StreamReader input = StringToStream(Resource1.LangExample);
            List<Token> tokens = Lang.lexerLang.SearchTokens(input);
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            input.Close();
            ReportParser report = Lang.parserLang.Check(tokens);
            Console.WriteLine(string.Join("\n", report.Info));
            Assert.IsTrue(report.IsSuccess);
        }

        [TestMethod]
        public void CompileTest()
        {
            StreamReader input = StringToStream(Resource1.LangExample);
            List<Token> tokens = Lang.lexerLang.SearchTokens(input);
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            input.Close();
            Console.WriteLine(Lang.parserLang.Check(tokens).Compile);
            List<string> Polsk = Lang.parserLang.Compile(tokens);
            Console.WriteLine(string.Join("\n", Polsk));
        }

        /// <summary>
        /// Функция запускает тестирование на основание текста программы.
        /// Ожидается count терминалов, не считая терминалов CH_.
        /// </summary>
        /// <param name="text">Текст программы.</param>
        /// <param name="count">Количество ожидаемых терминалов.</param>
        public List<Token> TestOnResurseCount(string text, int count)
        {
            StreamReader input = StringToStream(text);
            List<Token> tokens = Lang.lexerLang.SearchTokens(input);
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            input.Close();
            foreach (Token token in tokens)
                // Печатаем токины.
                Console.WriteLine(token);
            Assert.AreEqual(count, tokens.Count);
            return tokens;
        }

        public static StreamReader StringToStream(string resurse)
        {
            return new StreamReader(
               new MemoryStream(
                   Encoding.UTF8.GetBytes(resurse)
               ));
        }
    }
}
