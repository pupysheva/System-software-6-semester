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
        [TestMethod]
        public void TestMethod1()
        {
            Lang lang = new Lang();
            Assert.IsNotNull(lang);
            StreamReader input = new StreamReader(
                new MemoryStream(
                    Encoding.UTF8.GetBytes(Resource1.assign_op.ToCharArray())
                ));
            List<Token> tokens = lang.SearchTokens(input);
            Assert.AreEqual(tokens.Count, 3);
        }
    }
}
