// Written by Joe Zachary for CS 3500, January 2017.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        [TestMethod]
        ///<summary>
        ///Construct a formula that works
        /// </summary>
        public void Construct4()
        {
            Formula f = new Formula("53");
            Assert.AreEqual(f.Evaluate(v => 0), 53, 1e-6);
        }

        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5 ()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        ///<summary>
        ///See if every exception message is present and correct
        ///</summary>
        [TestMethod]
        public void DoEverythingWrong()
        {
            String[] ruleBreaker =
            {
                "tea",
                "",
                "5)(",
                "(53",
                "+17",
                "35.3+",
                "7-(/3)",
                "(5-3)6"
            };
            String[] exception = { "Inexistent variable used", "No tokens found", "The number of right parentheses is larger than the number of left parentheses",
                "The number of left and right parentheses is uneven", "First character is improperly formatted", "Last character is improperly formatted",
                "Incorrect formatting of text after a parenthesis or operator", "Incorrect formatting of text" };
            for(int i=0;i<ruleBreaker.Length;i++)
            {
                try
                {
                    Formula f = new Formula(ruleBreaker[i]);
                    f.Evaluate(Lookup4);
                    Assert.Fail();
                }
                catch(Exception e)
                {
                    Assert.AreEqual(e.Message, exception[i]);
                }
            }
        }

        [TestMethod]
        ///<summary>
        ///Tries a large equation with lots of parentheses. The value calculated should be an approximation of e
        /// </summary>
        public void tryALargeEquation()
        {
            Formula f = new Formula("1+((x)+(x*x)/2+(x*(x*x))/(2*3))+(x*((x*x)*x))/(2*3*4)");
            Assert.AreEqual(f.Evaluate(v => 1),Math.E,0.1);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void DivideByZero()
        {
            Formula f = new Formula("5/0");
            f.Evaluate(v => 0);
        }

        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }
    }
}
