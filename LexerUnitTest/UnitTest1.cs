using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lexer;
using System.IO;
using System.Collections.Generic;

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
            BufferedStream input = new BufferedStream(
                new StreamReader(
                    Resource1.assign_op
                ).BaseStream);
            List<Token> tokens = lang.SearchTokens(input);
            Assert.AreEqual(tokens.Count, 3);
        }
    }
}
