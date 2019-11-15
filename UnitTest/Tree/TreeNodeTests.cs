using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MyTypes.Tree.Tests
{
    [TestClass]
    public class TreeNodeTests
    {

        [TestMethod]
        public void ToStringTest()
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
        public void ToStringDeep()
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
            tree.AddTreeNode(tree);
            Console.Write(tree.ToString(StringFormat.NewLine));
            Assert.AreEqual("lol:\n\tlal\n\tkek\n\tcheburek:\n\t\tКапуста\n\t\tМясо\n\tcur: lol, deep: ...",
                tree.ToString(StringFormat.NewLine));
        }

        /// <summary>
        /// <see cref="TreeNode{T}.TreeNode(T)"/>
        /// </summary>
        [TestMethod]
        public void TreeNodeTest()
        {
            object test = new object();
            ITreeNode<object> tree = new TreeNode<object>(test);
            Console.Write(tree);
            Assert.AreEqual(test, tree.Current);
            Assert.IsNull(new TreeNode<object>().Current);
            Assert.AreEqual(3, new TreeNode<object>() { 1, 3, 4 }.Count);
        }

        /// <summary>
        /// <see cref="TreeNode{T}.Add(T)"/>
        /// </summary>
        [TestMethod]
        public void AddTest()
        {
            TreeNode<double> tree = new TreeNode<double>(1);
            Console.WriteLine(tree);
            Assert.AreEqual(1.0, tree.Current, 0.0);
            tree.Add(2);
            Console.WriteLine(tree);
            Assert.AreEqual(2.0, tree[0].Current, 0.0);
            tree[0].Add(3);
            Console.WriteLine(tree);
            Assert.AreEqual(3.0, tree[0][0].Current, 0.0);
            tree[0].Add(3.1);
            Console.WriteLine(tree);
            Assert.AreEqual(3.1, tree[0][1].Current, 0.0);
            Assert.AreEqual(4L, tree.GetCountAll());
        }

        [TestMethod]
        public void AddTest1()
        {
            TreeNode<string> tree1 = new TreeNode<string>("base1")
            {
                "child1"
            };
            TreeNode<string> tree2 = new TreeNode<string>("base2")
            {
                "child2"
            };
            Console.WriteLine(tree1);
            Console.WriteLine(tree2);
            tree1[0].AddTreeNode(tree2);
            Console.Write(tree1);
            Assert.AreEqual(tree2, tree1[0][0]);
            Assert.AreEqual(4, tree1.GetCountAll());
        }

        [TestMethod]
        public void ClearTest()
        {
            TreeNode<int> tree = new TreeNode<int>(0);
            for(int x = 0; x < 10; x++)
            {
                tree.Add(x * 10);
                for(int y = 0; y < 10; y++)
                {
                    tree[x].Add(x * 10 + y * 1);
                }
            }
            Console.WriteLine(tree);
            Assert.AreEqual(111, tree.GetCountAll());
            tree.Clear();
            Console.WriteLine(tree);
            Assert.AreEqual(1, tree.GetCountAll());
        }
        /*
        [TestMethod]
        public void ContainsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void CopyToTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void IndexOfTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void InsertTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void RemoveTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void RemoveAtTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetEnumerableOnlyNeighborsTest()
        {
            throw new NotImplementedException();
        }
        */
    }
}