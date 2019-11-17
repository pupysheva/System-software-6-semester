using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using StackMachine;
using System.IO;
using Optimizing;
using System.Text;

namespace Optimizing.Test
{
    [TestClass]
    public class TestsForOptimizingTest
    {
        [TestMethod]
        public void TestResx()
        {
            Assert.AreEqual("Hey! It is work!", Resources.ResxTest);
        }


        [TestMethod]
        public void CheckOptimizingSimple()
        {
            Assert.AreEqual("a = 1 + 1", Resources.OptimizeFirst);
            var tokens = Lang.lexerLang.SearchTokens(StringToStream(Resources.OptimizeFirst));
            tokens.RemoveAll(t => t.Type.Name.StartsWith("CH_"));
            Console.WriteLine(string.Join("\n", tokens));
            var output = Lang.parserLang.Compile(
                tokens,
                Lang.parserLang.Check(tokens)
            );
            Console.WriteLine(string.Join(", ", output));
            CollectionAssert.AreEqual(new string[]{"a", "1", "1", "+", "="}, output);
        }

        [TestMethod]
        public void OptimizingSimple()
        {
            Assert.AreEqual("a = 1 + 1", Resources.OptimizeFirst);
            var tokens = Lang.lexerLang.SearchTokens(StringToStream(Resources.OptimizeFirst));
            tokens.RemoveAll(t => t.Type.Name.StartsWith("CH_"));
            var output = Lang.parserLang.Compile(
                tokens,
                Optimizing.SimpleOptimizing.Instance.Optimize(
                    Lang.parserLang.Check(tokens)
                )
            );
            Console.WriteLine(string.Join(", ", output));
            CollectionAssert.AreEqual(new string[]{"a", "2", "="}, output);
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