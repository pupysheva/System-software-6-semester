using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser.Tree;

namespace UnitTest
{
    [TestClass]
    public class TreeUnitTest
    {
        [TestMethod]
        new public void ToString()
        {
            ITreeNode<string> tree = new TreeNode<string>("lol")
            {
                "lal",
                "kek",
                new TreeNode<string>("cheburek")
                {
                    "Капуста",
                    "Мясо"
                }
            };
            Console.Write(tree.ToString(StringFormat.NewLine));
            Assert.AreEqual("lol:\n\tlal\n\tkek\n\tcheburek:\n\t\tКапуста\n\t\tМясо",
                tree.ToString(StringFormat.NewLine));
        }

        [TestMethod]
        public void Deep()
        {
            ITreeNode<string> tree = new TreeNode<string>("lol")
            {
                "lal",
                "kek",
                new TreeNode<string>("cheburek")
                {
                    "Капуста",
                    "Мясо"
                }
            };
            tree.Add(tree);
            Console.Write(tree.ToString(StringFormat.NewLine));
            Assert.AreEqual("lol:\n\tlal\n\tkek\n\tcheburek:\n\t\tКапуста\n\t\tМясо\n\tcur: lol, deep: ...",
                tree.ToString(StringFormat.NewLine));
        }
    }
}
