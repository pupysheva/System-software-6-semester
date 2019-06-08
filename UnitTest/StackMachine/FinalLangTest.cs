using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;

namespace StackMachine.Test
{
    [TestClass]
    public class FinalLangTest
    {
        [TestMethod]
        public void LexerTest()
        {
            TestOnResurseCount(Resource1.LangExample, 148);
        }

        [TestMethod]
        public void ParserTest()
        {
            throw new NotImplementedException();
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
