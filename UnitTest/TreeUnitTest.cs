using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser.Tree;

namespace UnitTest
{
    [TestClass]
    public class TreeUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            TreeNode<string> tree = new TreeNode<string>("lol")
            {
                "lal",
                "kek",
                new TreeNode<string>("cheburek")
                {
                    "Капуста",
                    "Мясо"
                }
            };
            Console.Write(tree.ToString(TreeNode<string>.StringFormat.NewLine));
            Assert.AreEqual("lol:\n\tlal\n\tkek\n\tcheburek:\n\t\tКапуста\n\t\tМясо",
                tree.ToString(TreeNode<string>.StringFormat.NewLine));
        }
    }
}
