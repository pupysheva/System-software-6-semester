using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyTypes.Tests
{
    [TestClass]
    public class MyHashSetTests
    {
        [TestMethod]
        public void MyHashSetTest()
        {
            MyHashSet<string> myHashSet = new MyHashSet<string>();
            Assert.AreEqual(0, myHashSet.Count);
        }

        [TestMethod]
        public void AddTest()
        {
            MyHashSet<string> myHashSet = new MyHashSet<string>()
            {
                "abs"
            };
            CollectionAssert.AreEquivalent(new string[] { "abs" }, myHashSet);
            Assert.IsTrue(myHashSet.Add("sss"));
            CollectionAssert.AreEquivalent(new string[] { "abs", "sss" }, myHashSet);
            Assert.IsFalse(myHashSet.Add("sss"));
            CollectionAssert.AreEquivalent(new string[] { "abs", "sss" }, myHashSet);
        }

        [TestMethod]
        public void ClearTest()
        {
            MyHashSet<string> myHashSet = new MyHashSet<string>()
            {
                "abs"
            };
            CollectionAssert.AreEquivalent(new string[] { "abs" }, myHashSet);
            myHashSet.Add("sss");
            CollectionAssert.AreEquivalent(new string[] { "abs", "sss" }, myHashSet);
            myHashSet.Clear();
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
        }

        [TestMethod]
        public void BalancingTest()
        {
            MyHashSet<int> myHashSet = new MyHashSet<int>(2);
            Assert.AreEqual(0, myHashSet.Balancing);
            myHashSet.Add(0);
            Assert.AreEqual(1, myHashSet.Balancing);
            myHashSet.Add(1);
            Assert.AreEqual(0, myHashSet.Balancing);
            myHashSet.Add(2);
            Assert.AreEqual(1.0/3.0, myHashSet.Balancing, double.Epsilon);
        }

        [TestMethod]
        public void ContainsTest()
        {
            Assert.IsFalse(new MyHashSet<string>().Contains(null));
            MyHashSet<string> myHashSet = new MyHashSet<string>()
            {
                "abs",
                "aaa",
                "bbb",
                "asdfsad"
            };
            CollectionAssert.AreEquivalent(new string[] { "abs", "aaa", "bbb", "asdfsad" }, myHashSet);
            Assert.IsTrue(myHashSet.Contains("abs"));
            Assert.IsTrue(myHashSet.Contains("aaa"));
            Assert.IsTrue(myHashSet.Contains("asdfsad"));
            Assert.IsTrue(myHashSet.Contains("bbb"));
            Assert.IsFalse(myHashSet.Contains(""));
            Assert.IsFalse(myHashSet.Contains(null));
            Assert.IsFalse(myHashSet.Contains("ork2p3okr"));
            Assert.IsFalse(myHashSet.Contains("\0"));
        }

        [TestMethod]
        public void RemoveTest()
        {
            MyHashSet<string> myHashSet = new MyHashSet<string>()
            {
                "abs",
                "aaa",
                "bbb",
                "asdfsad"
            };
            CollectionAssert.AreEquivalent(new string[] { "abs", "aaa", "bbb", "asdfsad" }, myHashSet);
            Assert.IsFalse(myHashSet.Remove(""));
            CollectionAssert.AreEquivalent(new string[] { "abs", "aaa", "bbb", "asdfsad" }, myHashSet);
            Assert.IsFalse(myHashSet.Remove(null));
            CollectionAssert.AreEquivalent(new string[] { "abs", "aaa", "bbb", "asdfsad" }, myHashSet);
            Assert.IsFalse(myHashSet.Remove("ork2p3okr"));
            CollectionAssert.AreEquivalent(new string[] { "abs", "aaa", "bbb", "asdfsad" }, myHashSet);
            Assert.IsFalse(myHashSet.Remove("\0"));
            CollectionAssert.AreEquivalent(new string[] { "abs", "aaa", "bbb", "asdfsad" }, myHashSet);
            Assert.IsTrue(myHashSet.Remove("abs"));
            CollectionAssert.AreEquivalent(new string[] { "aaa", "bbb", "asdfsad" }, myHashSet);
            Assert.IsTrue(myHashSet.Remove("aaa"));
            CollectionAssert.AreEquivalent(new string[] { "bbb", "asdfsad" }, myHashSet);
            Assert.IsTrue(myHashSet.Remove("asdfsad"));
            CollectionAssert.AreEquivalent(new string[] { "bbb" }, myHashSet);
            Assert.IsTrue(myHashSet.Remove("bbb"));
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
            Assert.IsFalse(myHashSet.Remove(""));
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
            Assert.IsFalse(myHashSet.Remove(null));
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
            Assert.IsFalse(myHashSet.Remove("ork2p3okr"));
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
            Assert.IsFalse(myHashSet.Remove("\0"));
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
            Assert.IsFalse(myHashSet.Remove("abs"));
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
            Assert.IsFalse(myHashSet.Remove("aaa"));
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
            Assert.IsFalse(myHashSet.Remove("asdfsad"));
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
            Assert.IsFalse(myHashSet.Remove("bbb"));
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
            Assert.IsTrue(myHashSet.Add("bbb"));
            CollectionAssert.AreEquivalent(new string[] { "bbb" }, myHashSet);
            Assert.IsTrue(myHashSet.Remove("bbb"));
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
            Assert.IsFalse(myHashSet.Remove("bbb"));
            CollectionAssert.AreEquivalent(new string[] { }, myHashSet);
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            MyHashSet<string> myHashSet = new MyHashSet<string>()
            {
                "abs",
                "aaa",
                "bbb",
                "asdfsad"
            };
            List<string> list = new List<string>();
            foreach(string str in myHashSet)
            {
                list.Add(str);
            }
            CollectionAssert.AreEquivalent(new string[] { "abs", "aaa", "bbb", "asdfsad" }, list);
            CollectionAssert.AreEquivalent(new string[] { "abs", "aaa", "bbb", "asdfsad" }, myHashSet);
        }

        /*
        [TestMethod]
        public void CopyToTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void CopyToTest1()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void CopyToTest2()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ExceptWithTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void UnionWithTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void IntersectWithTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SymmetricExceptWithTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void IsSubsetOfTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void IsSupersetOfTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void IsProperSupersetOfTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void IsProperSubsetOfTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void OverlapsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SetEqualsTest()
        {
            throw new NotImplementedException();
        }
        */
    }
}