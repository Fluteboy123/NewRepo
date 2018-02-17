using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;

namespace SpreadsheetTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
        }

        [TestMethod]
        public void TestCells()
        {
            Spreadsheet ss = new Spreadsheet();
            Assert.AreEqual(ss.GetCellContents("A2"), null);
        }
        
    }
}
