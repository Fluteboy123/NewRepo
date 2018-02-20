// Finished class by Samuel Englert for CS 3500, January 2018.

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

        public DependencyGraph(DependencyGraph dg)
        {
            head = CreateNewNode(dg.head);
        }

        private Node CreateNewNode(Node n)
        {
            if (n == null)
                return null;
            Node r = new Node(n.Name)
            {
                Left = CreateNewNode(n.Left),
                Right = CreateNewNode(n.Right)
            };
            return r;
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
            ArrayList d = new ArrayList();
            IEnumerator deps = n.Dependents().GetEnumerator();
            while (deps.MoveNext())
            {
                d.Add(deps.Current);
            }
            return d.Count != 0;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
                throw new ArgumentNullException();
            Node n = head == null ? (head = new Node(s)) : head.AddOrFindString(s);
            ArrayList d = new ArrayList();
            IEnumerator deps = n.Dependees().GetEnumerator();
            while (deps.MoveNext())
            {
                d.Add(deps.Current);
            }
            return d.Count != 0;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
                throw new ArgumentNullException();
            Node n = head == null ? (head = new Node(s)) : head.AddOrFindString(s);
            IEnumerator deps = n.Dependents().GetEnumerator();
            while (deps.MoveNext())
            {
                Node current = (Node)deps.Current;
                yield return current.Name;
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
            IEnumerator deps = n.Dependees().GetEnumerator();
            while (deps.MoveNext())
            {
                Node current = (Node)deps.Current;
                yield return current.Name;
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
            if (s == null || t == null)
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
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)//If a string is null, remove all recently added nodes, and put the new ones back
        {
            if (s == null)
                throw new ArgumentNullException();
            IEnumerator enumerator = newDependents.GetEnumerator();
            Node n = head == null ? (head = new Node(s)) : head.AddOrFindString(s);
            //If a string in newDependents is null, this array will be here to put the dependencies back
            ArrayList backup = new ArrayList();
            //Remove the existing dependents
            IEnumerator deps = n.Dependents().GetEnumerator();
            while (deps.MoveNext())
            {
                Node x = (Node)deps.Current;
                backup.Add(x);
            }
            foreach (Node x in backup)
            {
                n.RemoveDependent(x);
            }
            //Add the new ones
            while (enumerator.MoveNext())
            {
                if ((string)(enumerator.Current) == null)
                {
                    foreach (String x in newDependents)
                    {
                        try
                        {
                            n.RemoveDependent(head.AddOrFindString(x));
                        }
                        catch (Exception) { }
                    }
                    foreach (Node x in backup)
                        n.AddDependent(x);
                    throw new ArgumentNullException();
                }
                n.AddDependent(head.AddOrFindString((string)enumerator.Current));
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t == null)
                throw new ArgumentNullException();
            IEnumerator enumerator = newDependees.GetEnumerator();
            Node n = head == null ? (head = new Node(t)) : head.AddOrFindString(t);
            //If a string in newDependees is null, this array will be here to put the dependencies back
            ArrayList backup = new ArrayList();
            //Remove the existing dependees
            IEnumerator deps = n.Dependees().GetEnumerator();
            while (deps.MoveNext())
            {
                Node x = (Node)deps.Current;
                backup.Add(x);
            }
            foreach (Node x in backup)
            {
                x.RemoveDependent(n);
            }
            //Add the new ones
            while (enumerator.MoveNext())
            {
                if ((string)(enumerator.Current) == null)//If a string is null, remove all recently added nodes, and put the new ones back
                {
                    foreach (String x in newDependees)
                    {
                        try
                        {
                            head.AddOrFindString(x).RemoveDependent(n);
                        }
                        catch (Exception) { }
                    }
                    foreach (Node x in backup)
                        x.AddDependent(n);
                    throw new ArgumentNullException();
                }
                Node temp = head.AddOrFindString((string)enumerator.Current);
                temp.AddDependent(n);
            }
        }
        /// <summary>
        /// A Node in the DependencyGraph class contains a string, its dependents and its dependees. Nodes are stored in a BST organized alphabetically.
        /// No duplicates are allowed
        /// </summary>
        private class Node
        {
            //Variables
            private string name;
            public Node Left { get; set; }
            public Node Right { get; set; }
            private ArrayList dependents = new ArrayList(), dependees = new ArrayList();
            /// <summary>
            /// Creates a new node with the specified string. Used mostly in the class itself. Unless head is null, use head.AddOrFindString() to add a Node
            /// </summary>
            /// <param name="s">The new string in the Dependency Graph</param>
            public Node(String s)
            {
                name = s;
            }
            /// <summary>
            /// Returns the Node's name
            /// </summary>
            public String Name => name;
            /// <summary>
            /// Moves through the BST to find the node with key as its title. If no node is found with the title, a new one is made.
            /// </summary>
            /// <param name="key">The name of the node to be made or found</param>
            /// <returns>The node with the specified string as its title</returns>
            public Node AddOrFindString(String key)
            {
                if (name.Equals(key))
                    return this;
                if (name.CompareTo(key) > 0)
                    return Left == null ? (Left = new Node(key)) : Left.AddOrFindString(key);
                else
                    return Right == null ? (Right = new Node(key)) : Right.AddOrFindString(key);
            }
            /// <summary>
            /// Takes the node d and makes it a dependent of the current Node, if it isn't already a dependent. 
            /// The current node is also automatically added to the dependee list of d.
            /// </summary>
            /// <param name="d">The dependent to be added</param>
            public void AddDependent(Node d)
            {
                if (!dependents.Contains(d))
                {
                    dependents.Add(d);
                    d.AddDependee(this);
                }
            }
            /// <summary>
            /// Returns a list of dependents
            /// </summary>
            public IEnumerable<Node> Dependents()
            {
                foreach (Node x in dependents)
                {
                    yield return x;
                }
            }
            /// <summary>
            /// Takes the node d and removes it from the current Node's dependent list, if it is a dependent. 
            /// The current node is also automatically removed from the dependee list of d.
            /// </summary>
            /// <param name="d">The dependent to be removed</param>
            public void RemoveDependent(Node d)
            {
                int index = dependents.IndexOf(d);
                if (index >= 0)
                {
                    dependents.RemoveAt(index);
                    d.RemoveDependee(this);
                }
            }
            /// <summary>
            /// Takes the node d and makes it a dependee of the current Node, if it isn't already a dependee. 
            /// </summary>
            /// <param name="d">The dependee to be added</param>
            public void AddDependee(Node d)
            {
                if (!dependees.Contains(d))
                    dependees.Add(d);
            }
            /// <summary>
            /// Returns a list of dependees
            /// </summary>
            public IEnumerable<Node> Dependees()
            {
                foreach (Node x in dependees)
                {
                    yield return x;
                }
            }
            /// <summary>
            /// Takes the node d and removes it from the current Node's dependee list, if it is a dependee. 
            /// </summary>
            /// <param name="d">The dependee to be removed</param>
            public void RemoveDependee(Node d)
            {
                int index = dependees.IndexOf(d);
                if (index >= 0)
                    dependees.RemoveAt(index);
                else
                    throw new ArgumentException(message: "Dependee of " + name + " not found");
            }
            /// <summary>
            /// Returns the size of the tree starting from this node
            /// </summary>
            public int GetSize()
            {
                int size = dependents.Count;
                size += Left == null ? 0 : Left.GetSize();
                size += Right == null ? 0 : Right.GetSize();
                return size;
            }
        }
    }
}
