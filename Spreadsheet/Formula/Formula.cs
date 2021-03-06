﻿// Skeleton written by Joe Zachary for CS 3500, January 2017

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public struct Formula
    {
        //Variables
        public ArrayList Tokens { get; private set; }
        public delegate string Normalizer(string s);
        public delegate bool Validator(string s);
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula)
        {
            double temp;
            Tokens = new ArrayList();
            byte leftPar = 0, rightPar = 0;
            IEnumerator counter = GetTokens(formula).GetEnumerator();

            //Adds all tokens while checking for exceptions
            while (counter.MoveNext())
            {
                Tokens.Add(counter.Current);

                if (counter.Current.Equals("("))
                    leftPar++;
                if (counter.Current.Equals(")"))
                    rightPar++;
                if (rightPar > leftPar)//Rule #3
                    throw new FormulaFormatException("The number of right parentheses is larger than the number of left parentheses");
            }

            if (Tokens.Count == 0)//Rule #2
            {
                throw new FormulaFormatException("No tokens found");
            }

            if (!Tokens[0].Equals("(") && !new Regex(@"[a-zA-Z][0-9a-zA-Z]*").IsMatch((string)Tokens[0]) && !Double.TryParse((string)Tokens[0], out temp))//Rule #5
                throw new FormulaFormatException("First character is improperly formatted");

            if (leftPar != rightPar)//Rule #4
                throw new FormulaFormatException("The number of left and right parentheses is uneven");

            if (!Tokens[Tokens.Count - 1].Equals(")") && !new Regex(@"[a-zA-Z][0-9a-zA-Z]*").IsMatch((string)Tokens[Tokens.Count - 1]) && !Double.TryParse((string)Tokens[Tokens.Count - 1], out temp))//Rule #6
                throw new FormulaFormatException("Last character is improperly formatted");

            for (int i = 0; i < Tokens.Count - 1; i++)
            {
                if (Tokens[i].Equals("(") || new Regex(@"^[\+\-*/]$").IsMatch((string)Tokens[i]))//Rule #7
                {
                    String follower = (string)Tokens[i + 1];
                    if (!follower.Equals("(") && !Double.TryParse(follower, out temp) && !new Regex(@"[a-zA-Z][0-9a-zA-Z]*").IsMatch(follower))
                        throw new FormulaFormatException("Incorrect formatting of text after a parenthesis or operator");
                }
                else if (Double.TryParse((string)Tokens[i], out temp) || new Regex(@"[a-zA-Z][0-9a-zA-Z]*").IsMatch((string)Tokens[i]) || Tokens[i].Equals(")"))//Rule #8
                {
                    String follower = (string)Tokens[i + 1];
                    if (!follower.Equals(")") && !new Regex(@"^[\+\-*/]$").IsMatch(follower))
                        throw new FormulaFormatException("Incorrect formatting of text");
                }
            }
        }

        public Formula(String s, Normalizer N, Validator V)
        {
            double temp;
            Tokens = new ArrayList();
            byte leftPar = 0, rightPar = 0;
            IEnumerator counter = GetTokens(s).GetEnumerator();

            //Adds all tokens while checking for exceptions
            while (counter.MoveNext())
            {
                Tokens.Add(counter.Current);

                if (counter.Current.Equals("("))
                    leftPar++;
                if (counter.Current.Equals(")"))
                    rightPar++;
                if (rightPar > leftPar)//Rule #3
                    throw new FormulaFormatException("The number of right parentheses is larger than the number of left parentheses");
            }

            if (Tokens.Count == 0)//Rule #2
            {
                throw new FormulaFormatException("No tokens found");
            }

            if (!Tokens[0].Equals("(") && !new Regex(@"[a-zA-Z][0-9a-zA-Z]*").IsMatch((string)Tokens[0]) && !Double.TryParse((string)Tokens[0], out temp))//Rule #5
                throw new FormulaFormatException("First character is improperly formatted");

            if (leftPar != rightPar)//Rule #4
                throw new FormulaFormatException("The number of left and right parentheses is uneven");

            if (!Tokens[Tokens.Count - 1].Equals(")") && !new Regex(@"[a-zA-Z][0-9a-zA-Z]*").IsMatch((string)Tokens[Tokens.Count - 1]) && !Double.TryParse((string)Tokens[Tokens.Count - 1], out temp))//Rule #6
                throw new FormulaFormatException("Last character is improperly formatted");

            for (int i = 0; i < Tokens.Count - 1; i++)
            {
                if (Tokens[i].Equals("(") || new Regex(@"^[\+\-*/]$").IsMatch((string)Tokens[i]))//Rule #7
                {
                    String follower = (string)Tokens[i + 1];
                    if (!follower.Equals("(") && !Double.TryParse(follower, out temp) && !new Regex(@"[a-zA-Z][0-9a-zA-Z]*").IsMatch(follower))
                        throw new FormulaFormatException("Incorrect formatting of text after a parenthesis or operator");
                }
                else if (Double.TryParse((string)Tokens[i], out temp) || new Regex(@"[a-zA-Z][0-9a-zA-Z]*").IsMatch((string)Tokens[i]) || Tokens[i].Equals(")"))//Rule #8
                {
                    String follower = (string)Tokens[i + 1];
                    if (!follower.Equals(")") && !new Regex(@"^[\+\-*/]$").IsMatch(follower))
                        throw new FormulaFormatException("Incorrect formatting of text");
                }
                if(new Regex(@"[a-zA-Z][0-9a-zA-Z]*").IsMatch((string)Tokens[i]))
                {
                    if (!new Regex(@"[a-zA-Z][0-9a-zA-Z]*").IsMatch(N((string)Tokens[i])))
                        throw new FormulaFormatException("Improperly normalized variable");
                    else if(!V(N((string)(Tokens[i]))))
                        throw new FormulaFormatException("Invalid variable");
                    Tokens[i] = N((string)(Tokens[i]));
                }
            }
        }
        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            return Calculate(lookup, 0, Tokens.Count);
        }

        private double Calculate(Lookup lookup, int startIndex, int endIndex)
        {
            if (startIndex >= endIndex)
                return 0;
            double num = 0;
            ArrayList parens = new ArrayList();
            //Order of Operations
            if ((parens = ParenPairs(startIndex, endIndex)).Count != 0)//Parentheses
            {
                for (int i = 0; i < parens.Count; i += 2)
                {
                    double result = Calculate(lookup, (int)parens[i] + 1, (int)parens[i + 1]);
                    for (int j = (int)parens[i]; j <= (int)parens[i + 1]; j++)
                        Tokens[j] = "";
                    Tokens[(int)parens[i] + 1] = result.ToString();
                }
            }

            for (int i = startIndex; i < endIndex; i++)//Addition
            {
                if (Tokens[i].Equals("+"))
                {
                    num = Calculate(lookup, startIndex, i) + Calculate(lookup, i + 1, endIndex);
                    return num;
                }
            }

            for (int i = startIndex; i < endIndex; i++)//Subtraction
            {
                if (Tokens[i].Equals("-"))
                {
                    num = Calculate(lookup, startIndex, i) - Calculate(lookup, i + 1, endIndex);
                    return num;
                }
            }

            for (int i = startIndex; i < endIndex; i++)//Division
            {
                if (Tokens[i].Equals("/"))
                {
                    double denom = Calculate(lookup, i + 1, endIndex);
                    if (denom == 0)
                        throw new FormulaEvaluationException("Attempted Division By Zero");
                    num = Calculate(lookup, startIndex, i) / denom;
                    return num;
                }
            }

            for (int i = startIndex; i < endIndex; i++)//Multiplication
            {
                if (Tokens[i].Equals("*"))
                {
                    num = Calculate(lookup, startIndex, i) * Calculate(lookup, i + 1, endIndex);
                    return num;
                }
            }

            /*for (int i = startIndex; i < endIndex; i++)//Exponentials
            {
                if (tokens[i].Equals("^"))
                {
                    num = Math.Pow(Calculate(lookup, startIndex, i), Calculate(lookup, i + 1, endIndex));
                    return num;
                }
            }*/

            for (int i = startIndex; i < endIndex; i++)//Numbers/Variables
            {
                if (!Tokens[i].Equals(""))
                {
                    if (Double.TryParse((string)Tokens[i], out num))
                    {
                        return num;
                    }
                    else
                    {
                        try
                        {
                            
                            return lookup((string)(Tokens[i]));
                        }
                        catch (Exception)//Rule 1
                        {
                            throw new FormulaEvaluationException("Inexistent variable used");
                        }
                    }
                }
            }
            return 0;
        }

        private ArrayList ParenPairs(int startIndex, int endIndex)
        {
            ArrayList indexList = new ArrayList();
            int parenLevel = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (Tokens[i].Equals("("))
                {
                    parenLevel++;
                    if (parenLevel == 1)
                        indexList.Add(i);
                }
                if (Tokens[i].Equals(")"))
                {
                    parenLevel--;
                    if (parenLevel == 0)
                        indexList.Add(i);
                }
            }
            return indexList;
        }
        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens.
            // NOTE:  These patterns are designed to be used to create a pattern to split a string into tokens.
            // For example, the opPattern will match any string that contains an operator symbol, such as
            // "abc+def".  If you want to use one of these patterns to match an entire string (e.g., make it so
            // the opPattern will match "+" but not "abc+def", you need to add ^ to the beginning of the pattern
            // and $ to the end (e.g., opPattern would need to be @"^[\+\-*/]$".)
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // PLEASE NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.  This pattern is useful for 
            // splitting a string into tokens.
            String splittingPattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            // PLEASE NOTE:  Notice the second parameter to Split, which says to ignore embedded white space
            /// in the pattern.
            foreach (String s in Regex.Split(formula, splittingPattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string var);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    [Serializable]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
