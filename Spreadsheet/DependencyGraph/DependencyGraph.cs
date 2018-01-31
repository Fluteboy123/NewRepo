// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        //Head of the BST
        private Node head = null;
        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return head == null ? 0 : head.GetSize(); }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (s == null)
                throw new ArgumentNullException();
            Node n = head == null ? (head = new Node(s)) : head.AddOrFindString(s);
            return n.Dependents.Count!=0;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
                throw new ArgumentNullException();
            Node n = head == null ? (head = new Node(s)) : head.AddOrFindString(s);
            return n.Dependees.Count != 0;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
                throw new ArgumentNullException();
            Node n = head == null ? (head = new Node(s)) : head.AddOrFindString(s);
            ArrayList deps = n.Dependents;
            foreach(Node x in deps)
            {
                yield return x.Name;
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
                throw new ArgumentNullException();
            Node n = head == null ? (head = new Node(s)) : head.AddOrFindString(s);
            ArrayList deps = n.Dependees;
            foreach (Node x in deps)
            {
                yield return x.Name;
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s == null || t == null)
                throw new ArgumentNullException();
            Node nodeS = head == null ? (head = new Node(s)) : head.AddOrFindString(s);
            Node nodeT = head.AddOrFindString(t);
            nodeS.AddDependent(nodeT);
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s == null||t == null)
                throw new ArgumentNullException();
            Node nodeS = head == null ? (head = new Node(s)) : head.AddOrFindString(s);
            Node nodeT = head.AddOrFindString(t);
            nodeS.RemoveDependent(nodeT);
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
        }
        private class Node
        {
            private string name;
            private Node left, right;
            private ArrayList dependents, dependees;
            public Node(String s)
            {
                name = s;
            }
            public String Name => name;
            public Node AddOrFindString(String key)
            {
                if (name.Equals(key))
                    return this;
                if (name.CompareTo(key) > 0)
                    return left == null ? (left = new Node(key)) : left.AddOrFindString(key);
                else
                    return right == null ? (right = new Node(key)) : right.AddOrFindString(key);
            }
            public void AddDependent(Node d)
            {
                if (!dependents.Contains(d))
                {
                    dependents.Add(d);
                    d.AddDependee(this);
                }
            }
            public ArrayList Dependents => dependents;
            public void RemoveDependent(Node d)
            {
                int index = dependents.IndexOf(d);
                if (index >= 0)
                    dependents.Remove(index);
                d.RemoveDependee(this);
            }
            public void AddDependee(Node d)
            {
                if (!dependees.Contains(d))
                    dependees.Add(d);
            }
            public ArrayList Dependees => dependees;
            public void RemoveDependee(Node d)
            {
                int index = dependents.IndexOf(d);
                if (index >= 0)
                    dependents.Remove(index);
                else
                    throw new ArgumentException(message: "Dependee of " + name+" not found");
            }
            public int GetSize()
            {
                int size = 1;
                size += left == null? 0: left.GetSize();
                size += right == null ? 0 : right.GetSize();
                return size;
            }
        }
    }
}
