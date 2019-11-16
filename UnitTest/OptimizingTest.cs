using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;

namespace Optimizing.Test
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void StartTest()
        {
            Console.WriteLine(Resources.Greeting);
            Assert.Fail();
        }
    }
}