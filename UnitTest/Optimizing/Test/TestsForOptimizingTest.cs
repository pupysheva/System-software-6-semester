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
            Assert.AreEqual("Hey! It is work!", Resources.resxTest);
        }

        public void OptimizingSimple()
        {
            
        }
    }
}