using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using StackMachine;
using System.IO;
using Optimizing;
using System.Text;
using System.Collections.Generic;

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

        [DataTestMethod]
        [DataRow("OptimizeFirst", "a 2 =")]
        [DataRow("VarInVar", "a 3 = b 6 =")]
        [DataRow("VarVarInVar", "a 7 = b 14 =")]
        [DataRow("If", "1 2 + 3 ==")] // TODO
        public void OptimizingSimple(string resourceName, string expect)
        {
            var output = CompileAndOptimizing(Resources.GetString(resourceName));
            CollectionAssert.AreEqual(expect.Split(' '), output);
        }

        private static List<string> CompileAndOptimizing(string resourceBody)
        {
            var tokens = Lang.lexerLang.SearchTokens(StringToStream(resourceBody));
            tokens.RemoveAll(t => t.Type.Name.StartsWith("CH_"));
            Console.WriteLine($"tokens:\n{string.Join('\n', tokens)}");
            var checkedTokens = Lang.parserLang.Check(tokens);
            Console.WriteLine($"preLang:\n{string.Join(", ", checkedTokens)}");
            var output = Lang.parserLang.Compile(
                tokens,
                Optimizing.SimpleOptimizing.Instance.Optimize(
                    checkedTokens
                )
            );
            Console.WriteLine($"optimizing:\n{string.Join(", ", output)}");
            return output;
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