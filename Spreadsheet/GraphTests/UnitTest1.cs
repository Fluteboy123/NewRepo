using System;
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
        }
        [TestMethod]
        public void TestForNoDuplicates()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("Cause", "Effect");
            dg.AddDependency("Cause", "Result");
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
    }
}
