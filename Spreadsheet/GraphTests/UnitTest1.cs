using System;
using System.Collections;
using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAssembly()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("Cause","Effect");
            dg.RemoveDependency("Cause", "Effect");
            Assert.AreEqual(dg.Size, 2);
        }
        [TestMethod]
        public void TestForNoDuplicates()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("Cause", "Effect");
            dg.AddDependency("Effect", "Action");
            Assert.AreEqual(dg.Size, 3);
        }
        [TestMethod]
        public void TestforListAccuracy()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("Cause", "Effect");
            Assert.AreEqual(dg.HasDependents("Cause"), true);
            Assert.AreEqual(dg.HasDependees("Cause"), false);
            Assert.AreEqual(dg.HasDependents("Effect"), false);
            Assert.AreEqual(dg.HasDependees("Effect"), true);
        }
        [TestMethod]
        public void TestTheLists()
        {
            String[] letters = { "A", "B", "C", "D", "E", "F" };
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A","B");
            dg.AddDependency("A", "C");
            dg.AddDependency("A", "D");
            dg.AddDependency("A", "E");
            dg.AddDependency("A", "F");
            dg.AddDependency("B", "F");
            dg.AddDependency("C", "F");
            dg.AddDependency("D", "F");
            dg.AddDependency("E", "F");
            IEnumerator dependents = dg.GetDependents("A").GetEnumerator();
            IEnumerator dependees = dg.GetDependees("F").GetEnumerator();
            for(int i = 0;i<5;i++)
            {
                dependents.MoveNext();
                Assert.AreEqual(dependents.Current, letters[i + 1]);
                dependees.MoveNext();
                Assert.AreEqual(dependees.Current, letters[i]);
            }
        }
        [TestMethod]
        public void TestNullValues()
        {
            byte numOfExceptions = 0;
            String s = null;
            DependencyGraph dg = new DependencyGraph();
            try
            {
                IEnumerator dependees = dg.GetDependees(s).GetEnumerator();
                dependees.MoveNext();
            }
            catch(ArgumentNullException)
            {
                numOfExceptions++;
            }
            try
            {
                IEnumerator dependents = dg.GetDependents(s).GetEnumerator();
                dependents.MoveNext();
            }
            catch (ArgumentNullException)
            {
                numOfExceptions+=2;
            }
            try
            {
                dg.HasDependents(s);
            }
            catch (ArgumentNullException)
            {
                numOfExceptions+=4;
            }
            try
            {
                dg.HasDependees(s);
            }
            catch (ArgumentNullException)
            {
                numOfExceptions+=8;
            }
            Assert.AreEqual(numOfExceptions, 15);
        }
        [TestMethod]
        public void TestNullCheck()
        {
            DependencyGraph dg = new DependencyGraph();
        }
        [TestMethod]
        public void TestLargeNumbers()
        {
            DependencyGraph dg = new DependencyGraph();
            String[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            String startPoint = letters[(new Random()).Next(8)];
            for (int i = 0; i < 10_000; i++)
            {
                String randomString = letters[(new Random()).Next(8)] + letters[(new Random()).Next(8)] + letters[(new Random()).Next(8)] + letters[(new Random()).Next(8)];
                dg.AddDependency(startPoint, randomString);
            }
        }
        [TestMethod]
        public void TestReplacements()
        {
            DependencyGraph dg = new DependencyGraph();
            String[] replacements = { "H", "I", "J" };
            dg.AddDependency("D", "A");
            dg.AddDependency("D", "B");
            dg.AddDependency("D", "C");
            dg.AddDependency("E", "D");
            dg.AddDependency("F", "D");
            dg.AddDependency("G", "D");
            dg.ReplaceDependents("D", replacements);
            dg.ReplaceDependees("D", replacements);
            Assert.AreEqual(dg.Size, 10);
        }
    }
}
