using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MyTypes.LinkedList.Tests
{
    [TestClass]
    public class MyLinkedListTests
    {
        [TestMethod]
        public void AddAfterTest()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            CollectionAssert.AreEqual(new string[] { }, list);
            var node = list.AddLast("abs");
            CollectionAssert.AreEqual(new string[] { "abs" }, list);
            list.AddAfter(node, "zz");
            CollectionAssert.AreEqual(new string[]{ "abs", "zz" }, list);
        }

        [TestMethod]
        public void FirstAndLastEqualsTest()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            list.AddLast("abs");
            Assert.AreEqual(list.Last, list.First);
            list.Clear();
            list.AddFirst("aaffa");
            Assert.AreEqual(list.First, list.Last);
        }

        [TestMethod]
        public void AddBeforeTest()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            CollectionAssert.AreEqual(new string[] { }, list);
            var node = list.AddLast("abs");
            CollectionAssert.AreEqual(new string[] { "abs" }, list);
            list.AddBefore(node, "zz");
            UnitTest.Writer.WriteAll<string>(list);
            CollectionAssert.AreEqual(new string[] { "zz", "abs" }, list);
        }

        [TestMethod]
        public void AddFirstTest()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            CollectionAssert.AreEqual(new string[] { }, list);
            list.AddFirst("abs");
            CollectionAssert.AreEqual(new string[] { "abs" }, list);
            list.AddFirst("zzz");
            CollectionAssert.AreEqual(new string[] { "zzz", "abs" }, list);
            list.AddFirst("ыыыы");
            CollectionAssert.AreEqual(new string[] { "ыыыы", "zzz", "abs" }, list);
        }

        [TestMethod]
        public void AddLastTest()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            CollectionAssert.AreEqual(new string[] { }, list);
            list.AddLast("abs");
            CollectionAssert.AreEqual(new string[] { "abs" }, list);
            list.AddLast("zzz");
            CollectionAssert.AreEqual(new string[] { "abs", "zzz" }, list);
            list.AddLast("ыыыы");
            CollectionAssert.AreEqual(new string[] { "abs", "zzz", "ыыыы" }, list);
        }

        [TestMethod]
        public void ClearTest()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            list.AddLast("abs");
            list.AddLast("zzz");
            list.AddLast("ыыыы");
            CollectionAssert.AreEqual(new string[] { "abs", "zzz", "ыыыы" }, list);
            list.Clear();
            CollectionAssert.AreEqual(new string[] { }, list);
        }

        [TestMethod]
        public void ContainsTest()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            list.AddLast("abs");
            list.AddLast("zzz");
            list.AddLast("ыыыы");
            Assert.IsTrue(list.Contains("abs"));
            Assert.IsTrue(list.Contains("zzz"));
            Assert.IsTrue(list.Contains("ыыыы"));
            Assert.IsFalse(list.Contains("iqewojf   2"));
            Assert.IsFalse(list.Contains(""));
            Assert.IsFalse(list.Contains(null));
        }

        [TestMethod]
        public void FindTest()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            MyLinkedListNode<string>[] nodes = {
                list.AddLast("abs"),
                list.AddLast("zzz"),
                list.AddLast("ыыыы"),
                list.AddLast("abs")
            };
            Assert.AreEqual(nodes[0], list.Find("abs"));
            Assert.AreEqual(nodes[1], list.Find("zzz"));
            Assert.AreEqual(nodes[2], list.Find("ыыыы"));
            Assert.AreNotEqual(nodes[3], list.Find("abs"));
            Assert.IsNull(list.Find("iqewojf   2"));
            Assert.IsNull(list.Find(""));
            Assert.IsNull(list.Find(null));
        }

        [TestMethod]
        public void FindLastTest()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            MyLinkedListNode<string>[] nodes = {
                list.AddLast("abs"),
                list.AddLast("zzz"),
                list.AddLast("ыыыы"),
                list.AddLast("abs")
            };
            Assert.AreNotEqual(nodes[0], list.FindLast("abs"));
            Assert.AreEqual(nodes[1], list.FindLast("zzz"));
            Assert.AreEqual(nodes[2], list.FindLast("ыыыы"));
            Assert.AreEqual(nodes[3], list.FindLast("abs"));
            Assert.IsNull(list.FindLast("iqewojf   2"));
            Assert.IsNull(list.FindLast(""));
            Assert.IsNull(list.FindLast(null));
        }

        [TestMethod]
        public void CopyToTest()
        {
            string[] test = new string[5];
            MyLinkedList<string> list = new MyLinkedList<string>();
            list.AddLast("abs");
            list.AddLast("zzz");
            list.AddLast("ыыыы");
            list.CopyTo(test, 1);
            CollectionAssert.AreEqual(new string[5] { null, "abs", "zzz", "ыыыы", null }, test);
        }

        [TestMethod]
        public void CopyToTest1()
        {
            Array test = new string[5];
            MyLinkedList<string> list = new MyLinkedList<string>();
            list.AddLast("abs");
            list.AddLast("zzz");
            list.AddLast("ыыыы");
            list.CopyTo(test, 1);
            CollectionAssert.AreEqual(new string[5] { null, "abs", "zzz", "ыыыы", null }, test);
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            list.AddLast("abs");
            list.AddLast("zzz");
            list.AddLast("ыыыы");
            int i = 0;
            string[] expected = new string[] { "abs", "zzz", "ыыыы" };
            foreach (string a in list)
            {
                Assert.AreEqual(expected[i++], a);
            }
            try
            {
                foreach (string a in list)
                {
                    list.AddLast("abs");
                }
                Assert.Fail();
            }
            catch (InvalidOperationException) { }
        }

        [TestMethod]
        public void RemoveTest()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            CollectionAssert.AreEqual(new string[] { }, list);
            list.AddLast("abs");
            CollectionAssert.AreEqual(new string[] { "abs" }, list);
            Assert.IsFalse(list.Remove("zzz"));
            CollectionAssert.AreEqual(new string[] { "abs" }, list);
            Assert.IsFalse(list.Remove("zzz"));
            CollectionAssert.AreEqual(new string[] { "abs" }, list);
            list.AddLast("zzz");
            CollectionAssert.AreEqual(new string[] { "abs", "zzz" }, list);
            list.AddLast("ыыыы");
            CollectionAssert.AreEqual(new string[] { "abs", "zzz", "ыыыы" }, list);
            Assert.IsTrue(list.Remove("zzz"));
            CollectionAssert.AreEqual(new string[] { "abs", "ыыыы" }, list);
            Assert.IsFalse(list.Remove("zzz"));
            CollectionAssert.AreEqual(new string[] { "abs", "ыыыы" }, list);
            Assert.IsFalse(list.Remove("zzz"));
            CollectionAssert.AreEqual(new string[] { "abs", "ыыыы" }, list);
            Assert.IsTrue(list.Remove("abs"));
            CollectionAssert.AreEqual(new string[] { "ыыыы" }, list);
            Assert.IsTrue(list.Remove("ыыыы"));
            CollectionAssert.AreEqual(new string[] { }, list);
            Assert.IsFalse(list.Remove("abs"));
            CollectionAssert.AreEqual(new string[] { }, list);
            Assert.IsFalse(list.Remove("ыыыы"));
            CollectionAssert.AreEqual(new string[] { }, list);
            list.AddLast("sss");
            CollectionAssert.AreEqual(new string[] { "sss" }, list);
            Assert.IsTrue(list.Remove("sss"));
            CollectionAssert.AreEqual(new string[] { }, list);
        }

        [TestMethod]
        public void RemoveTest1()
        {
            MyLinkedList<string> list = new MyLinkedList<string>();
            list.AddLast("abs");
            var node = list.AddLast("zzz");
            list.AddLast("ыыыы");

            Assert.IsTrue(list.Contains("zzz"));
            list.Remove(node);
            Assert.IsFalse(list.Contains("zzz"));
        }
    }
}