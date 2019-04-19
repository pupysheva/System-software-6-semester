using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lexer;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LexerUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        Lang lang = new Lang();

        /// <summary>
        /// Тестирование assign_op.txt
        /// </summary>
        [TestMethod]
        public void assign_op()
            => TestOnResurseCount(Resource1.assign_op, 3);

        [TestMethod]
        public void assign_op_multiline()
            => TestOnResurseCount(Resource1.assign_op_multiline, 6);

        [TestMethod]
        public void op()
            => TestOnResurseCount(Resource1.op, 11);

        /// <summary>
        /// Функция запускает тестирование на основание текста программы.
        /// Ожидается count терминалов, не считая терминалов CH_.
        /// </summary>
        /// <param name="text">Текст программы.</param>
        /// <param name="count">Количество ожидаемых терминалов.</param>
        public void TestOnResurseCount(string text, int count)
        {
            StreamReader input = OpenFile(text);
            List<Token> tokens = lang.SearchTokens(input);
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            Assert.AreEqual(count, tokens.Count);
            input.Close();
            foreach (Token token in tokens)
                // Печатаем токины.
                Console.WriteLine(token);
        }

        public StreamReader OpenFile(string resurse)
        {
            return new StreamReader(
               new MemoryStream(
                   Encoding.UTF8.GetBytes(resurse)
               ));
        }
    }
}
