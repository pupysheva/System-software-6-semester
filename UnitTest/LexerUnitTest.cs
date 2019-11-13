using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lexer;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace UnitTest
{
    [TestClass]
    public class LexerUnitTest
    {
        LexerLang lang = new LexerLang();

#pragma warning disable IDE1006 // Стили именования
        /// <summary>
        /// Тестирование assign_op.txt
        /// </summary>
        [TestMethod]
        public void assign_op()
            => TestOnResourceCount(Resource1.assign_op, 3);

        [TestMethod]
        public void assign_op_multiline()
            => TestOnResourceCount(Resource1.assign_op_multiline, 6);

        [TestMethod]
        public void op()
        {
            TestOnResourceCount(Resource1.op, 11);
        }

        [TestMethod]
        public void while_kw(){
            TestOnResourceCount(Resource1._while, 16);
        }



        [TestMethod]
        public void print_kw()
        {
            List<Token> tokens = TestOnResourceCount(Resource1.print_kw, 1);

            Assert.AreEqual("PRINT_KW", tokens[0].Type.Name);

        }
        [TestMethod]
        public void condition()
        {
            TestOnResourceCount(Resource1.condition, 27);
        }
        [TestMethod]
        public void cycle_for()
        {
            TestOnResourceCount(Resource1.cycle_for, 29);
        }
#pragma warning restore IDE1006 // Стили именования
        /// <summary>
        /// Функция запускает тестирование на основание текста программы.
        /// Ожидается count терминалов, не считая терминалов CH_.
        /// </summary>
        /// <param name="text">Текст программы.</param>
        /// <param name="count">Количество ожидаемых терминалов.</param>
        public List<Token> TestOnResourceCount(string text, int count)
        {
            StreamReader input = StringToStream(text);
            List<Token> tokens = lang.SearchTokens(input);
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            input.Close();
            foreach (Token token in tokens)
                // Печатаем жетоны.
                Console.WriteLine(token);
            Assert.AreEqual(count, tokens.Count);
            return tokens;
        }

        public static StreamReader StringToStream(string resource)
        {
            return new StreamReader(
               new MemoryStream(
                   Encoding.UTF8.GetBytes(resource)
               ));
        }
    }
}
