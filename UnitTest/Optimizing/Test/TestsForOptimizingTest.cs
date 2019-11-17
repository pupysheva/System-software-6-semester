using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;
using StackMachine;

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
        public void OptimizingSimple()
        {
            Assert.AreEqual("a = 1 + 1", Resources.OptimizeFirst);
        }
    }
}