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
        public void TestMethod1()
        {
            Assert.IsNotNull(lang);
            StreamReader input = OpenFile(Resource1.assign_op);
            List<Token> tokens = lang.SearchTokens(input);
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            Assert.AreEqual(3, tokens.Count);
        }

        [TestMethod]
        public void assign_op_multiline()
        {
            StreamReader input = OpenFile(Resource1.assign_op_multiline);
            List<Token> tokens = lang.SearchTokens(input);
            tokens.RemoveAll((Token t) => t.Type.Name.Contains("CH_"));
            Assert.AreEqual(6, tokens.Count);
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
