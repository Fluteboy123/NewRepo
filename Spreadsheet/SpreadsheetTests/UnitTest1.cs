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
            ss.SetCellContents("A2", 3.14);
            Assert.AreEqual(ss.GetCellContents("A2"), 3.14);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void ErrorTest1()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellContents("55");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void ErrorTest2()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetCellContents("Hi",2.42);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void ErrorTest3()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetCellContents("A55B","B3 + 7");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void ErrorTest4()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetCellContents("55",new Formulas.Formula("5*2/3"));
        }
    }
}
