using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Formulas
{
    [TestClass]
    public class GradingTests
    {
        // Tests of syntax errors detected by the constructor
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test1()
        {
            Formula f = new Formula("        ");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test2()
        {
            Formula f = new Formula("((2 + 5))) + 8");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test3()
        {
            Formula f = new Formula("2+5*8)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test4()
        {
            Formula f = new Formula("((3+5*7)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test5()
        {
            Formula f = new Formula("+3");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test6()
        {
            Formula f = new Formula("-y");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test7()
        {
            Formula f = new Formula("*7");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test8()
        {
            Formula f = new Formula("/z2x");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test9()
        {
            Formula f = new Formula(")");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test10()
        {
            Formula f = new Formula("(*5)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test11()
        {
            Formula f = new Formula("2 5");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test12()
        {
            Formula f = new Formula("x5 y");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test13()
        {
            Formula f = new Formula("((((((((((2)))))))))");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test14()
        {
            Formula f = new Formula("$");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test15()
        {
            Formula f = new Formula("x5 + x6 + x7 + (x8) +");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test15a()
        {
            Formula f = new Formula("x1 ++ y1");
        }

        // Simple tests that throw FormulaEvaluationExceptions
        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test16()
        {
            Formula f = new Formula("2+x");
            f.Evaluate(s => { throw new UndefinedVariableException(s); });
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test17()
        {
            Formula f = new Formula("5/0");
            f.Evaluate(s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test18()
        {
            Formula f = new Formula("(5 + x) / (y - 3)");
            f.Evaluate(s => 3);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test18a()
        {
            Formula f = new Formula("(5 + x) / (3 * 2 - 12 / 2)");
            f.Evaluate(s => 3);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test19()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(s => { if (s == "x") return 0; else throw new UndefinedVariableException(s); });
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test20()
        {
            Formula f = new Formula("x1 + x2 * x3 + x4 * x5 * x6 + x7");
            f.Evaluate(s => { if (s == "x7") throw new UndefinedVariableException(s); else return 1; });
        }

        // Simple formulas
        [TestMethod()]
        public void Test21()
        {
            Formula f = new Formula("4.5e1");
            Assert.AreEqual(45, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test21a()
        {
            Formula f = new Formula("4");
            Assert.AreEqual(4, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test22()
        {
            Formula f = new Formula("a05");
            Assert.AreEqual(10, f.Evaluate(s => 10), 1e-6);
        }

        [TestMethod()]
        public void Test22a()
        {
            Formula f = new Formula("a1b2c3d4e5f6g7h8i9j10");
            Assert.AreEqual(10, f.Evaluate(s => 10), 1e-6);
        }

        [TestMethod()]
        public void Test23()
        {
            Formula f = new Formula("5 + x");
            Assert.AreEqual(9, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test24()
        {
            Formula f = new Formula("5 - y");
            Assert.AreEqual(1, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test25()
        {
            Formula f = new Formula("5 * z");
            Assert.AreEqual(20, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test26()
        {
            Formula f = new Formula("8 / xx");
            Assert.AreEqual(2, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test27()
        {
            Formula f = new Formula("(5 + 4) * 2");
            Assert.AreEqual(18, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test28()
        {
            Formula f = new Formula("1 + 2 + 3 * 4 + 5");
            Assert.AreEqual(20, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test29()
        {
            Formula f = new Formula("(1 + 2 + 3 * 4 + 5) * 2");
            Assert.AreEqual(40, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test30()
        {
            Formula f = new Formula("((((((((((((3))))))))))))");
            Assert.AreEqual(3, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test31()
        {
            Formula f = new Formula("((((((((((((x))))))))))))");
            Assert.AreEqual(7, f.Evaluate(s => 7), 1e-6);
        }

        // Some more complicated formula evaluations
        [TestMethod()]
        public void Test32()
        {
            Formula f = new Formula("y*3-8/2+4*(8-9*2)/14*x");
            Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x") ? 1 : 4), 1e-9);
        }

        [TestMethod()]
        public void Test32a()
        {
            Formula f = new Formula("a + b * c - d + 3 * 3.0 - 3.0e0 / 0.003e3");
            Assert.AreEqual(17, (double)f.Evaluate(s => 3), 1e-9);
        }

        [TestMethod()]
        public void Test33()
        {
            Formula f = new Formula("a+(b+(c+(d+(e+f))))");
            Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
        }

        [TestMethod()]
        public void Test34()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
        }

        [TestMethod()]
        public void Test35()
        {
            Formula f = new Formula("a-a*a/a");
            Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
        }

        // Tests to make sure there can be more than one formula at a time
        [TestMethod()]
        public void Test36()
        {
            Formula f1 = new Formula("xx+3");
            Formula f2 = new Formula("xx-3");
            Assert.AreEqual(6, f1.Evaluate(s => 3), 1e-6);
            Assert.AreEqual(0, f2.Evaluate(s => 3), 1e-6);
        }

        [TestMethod()]
        public void Test37()
        {
            Test36();
        }

        [TestMethod()]
        public void Test38()
        {
            Test36();
        }

        [TestMethod()]
        public void Test39()
        {
            Test36();
        }

        [TestMethod()]
        public void Test40()
        {
            Test36();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test41()
        {
            Formula f = new Formula("(((((2+3*a)/(7e-5+b-c))*d+.0005e+92)-8.2)*3.14159) * ((e+3.1)-.00000000008)");
        }

        // Stress test for constructor, repeated five times to give it extra weight.
        [TestMethod()]
        public void Test42()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test43()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test44()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test45()
        {
            Test41();
        }
        [TestMethod]
        public void ZeroArg1()
        {
            new Formula();
        }

        [TestMethod]
        public void ZeroArg2()
        {
            Formula f = new Formula();
            Assert.AreEqual(0, f.GetVariables().Count);
        }

        [TestMethod]
        public void ZeroArg3()
        {
            Formula f = new Formula();
            Assert.AreEqual(0, f.Evaluate(s => 1));
        }

        [TestMethod]
        public void ZeroArg4()
        {
            Formula f1 = new Formula();
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(0, f2.Evaluate(s => 1));
        }

        [TestMethod]
        public void ThreeArg1()
        {
            Formula f = new Formula("x+y", s => s, s => true);
            Assert.AreEqual(3, f.Evaluate(s => (s == "x") ? 1 : 2));
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ThreeArg2()
        {
            Formula f = new Formula("x+y", s => "$", s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ThreeArg3()
        {
            Formula f = new Formula("x+y", s => s, s => false);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ThreeArg4()
        {
            Formula f = new Formula("x+y", s => s == "x" ? "z" : s, s => s != "z");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ThreeArg5()
        {
            Formula f = new Formula("$", s => "x", s => true);
        }

        [TestMethod]
        public void ThreeArg6()
        {
            Formula f = new Formula("1", s => "x", s => true);
            Assert.AreEqual(1.0, f.Evaluate(s => { throw new UndefinedVariableException(""); }), 1e-6);
        }

        [TestMethod]
        public void ThreeArg7()
        {
            Formula f = new Formula("y", s => "x", s => true);
            Assert.AreEqual(1.0, f.Evaluate(s => (s == "x") ? 1 : 0), 1e-6);
        }


        [TestMethod]
        public void ThreeArg8()
        {
            Formula f = new Formula("1e1 + e", s => "x", s => s == "x");
            Assert.AreEqual(12.0, f.Evaluate(s => (s == "x") ? 2 : 0), 1e-6);
        }

        [TestMethod]
        public void ThreeArg9()
        {
            Formula f = new Formula("xx+y", s => (s == "xx") ? "X" : "z", s => s.Length == 1);
            Assert.AreEqual(10.0, f.Evaluate(s => (s == "X") ? 7 : 3), 1e-6);
        }

        [TestMethod]
        public void ThreeArg10()
        {
            Formula f = new Formula("a + b + c + d", s => "x", s => true);
            Assert.AreEqual(4.0, f.Evaluate(s => (s == "x") ? 1 : 0), 1e-6);
        }

        [TestMethod]
        public void GetVars1()
        {
            Formula f = new Formula("0");
            var expected = new HashSet<string>();
            Assert.IsTrue(expected.SetEquals(f.GetVariables()));
        }

        [TestMethod]
        public void GetVars2()
        {
            Formula f = new Formula("x");
            var expected = new HashSet<string>();
            expected.Add("x");
            Assert.IsTrue(expected.SetEquals(f.GetVariables()));
        }

        [TestMethod]
        public void GetVars3()
        {
            Formula f = new Formula("a * b - c + d / e * 2.5e6");
            var expected = new HashSet<string>();
            expected.Add("a");
            expected.Add("b");
            expected.Add("c");
            expected.Add("d");
            expected.Add("e");
            var actual = f.GetVariables();
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod]
        public void GetVars4()
        {
            Formula f = new Formula("a * a + b * c - d * d");
            var expected = new HashSet<string>();
            expected.Add("a");
            expected.Add("b");
            expected.Add("c");
            expected.Add("d");
            var actual = f.GetVariables();
            Assert.AreEqual(4, actual.Count);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod]
        public void GetVars5()
        {
            Formula f = new Formula("x+y", s => s.ToUpper(), s => true);
            var expected = new HashSet<string>();
            expected.Add("X");
            expected.Add("Y");
            Assert.IsTrue(expected.SetEquals(f.GetVariables()));
        }

        [TestMethod]
        public void GetVars6()
        {
            Formula f = new Formula("x+y+z", s => s + s, s => true);
            var expected = new HashSet<string>();
            expected.Add("xx");
            expected.Add("yy");
            expected.Add("zz");
            Assert.IsTrue(expected.SetEquals(f.GetVariables()));
        }

        [TestMethod]
        public void ToString1()
        {
            Formula f1 = new Formula("7");
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(7.0, f2.Evaluate(s => 0), 1e-6);
        }

        [TestMethod]
        public void ToString2()
        {
            Formula f1 = new Formula("x");
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(8.0, f2.Evaluate(s => (s == "x") ? 8 : 0), 1e-6);
        }

        [TestMethod]
        public void ToString3()
        {
            Formula f1 = new Formula("x", s => s.ToUpper(), s => true);
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(8.0, f2.Evaluate(s => (s == "X") ? 8 : 0), 1e-6);
        }

        [TestMethod]
        public void ToString4()
        {
            Formula f1 = new Formula("a+b*(c-15)/2");
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(24.0, f2.Evaluate(s => char.IsLower(s[0]) ? 16 : 0), 1e-6);
        }

        [TestMethod]
        public void ToString5()
        {
            Formula f1 = new Formula("a+b*(c-15)/2", s => s, s => true);
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(24.0, f2.Evaluate(s => char.IsLower(s[0]) ? 16 : 0), 1e-6);
        }

        [TestMethod]
        public void ToString6()
        {
            Formula f1 = new Formula("a+b*(c-15)/2", s => s.ToUpper(), s => true);
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(24.0, f2.Evaluate(s => char.IsUpper(s[0]) ? 16 : 0), 1e-6);
        }
    }
}
