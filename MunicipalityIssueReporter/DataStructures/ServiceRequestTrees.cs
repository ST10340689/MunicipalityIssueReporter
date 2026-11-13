using MunicipalityIssueReporter.Models;
using System;
using System.Collections.Generic;

namespace MunicipalityIssueReporter.DataStructures
{
    /// <summary>
    /// 1. BASIC TREE (N-ary Tree):
    /// Organises municipal service requests hierarchically:
    /// Root ("Municipal Services") -> Department -> Category -> Service Requests.
    /// </summary>
    public class BasicTreeNode
    {
        public string Name { get; }
        public string NodeType { get; } // "Root", "Department", or "Category"
        public List<BasicTreeNode> Children { get; } = new List<BasicTreeNode>();
        public List<ServiceRequest> Requests { get; } = new List<ServiceRequest>();

        public BasicTreeNode(string name, string nodeType)
        {
            Name = name;
            NodeType = nodeType;
        }

        public BasicTreeNode GetOrAddChild(string childName, string childType)
        {
            foreach (BasicTreeNode child in Children)
            {
                if (string.Equals(child.Name, childName, StringComparison.OrdinalIgnoreCase))
                {
                    return child;
                }
            }

            BasicTreeNode created = new BasicTreeNode(childName, childType);
            Children.Add(created);
            return created;
        }

        /// <summary>
        /// Recursively collects all service requests stored under this tree node and its descendants.
        /// </summary>
        public List<ServiceRequest> CollectAllRequests()
        {
            List<ServiceRequest> result = new List<ServiceRequest>(Requests);
            foreach (BasicTreeNode child in Children)
            {
                result.AddRange(child.CollectAllRequests());
            }
            return result;
        }
    }

    /// <summary>
    /// Node for the Binary Tree / Binary Search Tree / Self-Balancing AVL Tree.
    /// </summary>
    public class AvlTreeNode
    {
        public int Key => Request.Id;
        public ServiceRequest Request { get; set; }
        public AvlTreeNode? Left { get; set; }
        public AvlTreeNode? Right { get; set; }
        public int Height { get; set; } = 1;

        public AvlTreeNode(ServiceRequest request)
        {
            Request = request;
        }
    }

    /// <summary>
    /// 2. BINARY TREE, BINARY SEARCH TREE (BST), AND SELF-BALANCING AVL TREE:
    /// Stores ServiceRequest items keyed by their unique integer Id.
    /// Automatically balances itself via Left and Right rotations after every insertion
    /// so lookup by unique identifier is guaranteed O(log n) even when IDs are inserted sequentially.
    /// </summary>
    public class ServiceRequestAvlTree
    {
        private AvlTreeNode? root;

        public int Count { get; private set; }

        public int TreeHeight => GetHeight(root);

        public void Clear()
        {
            root = null;
            Count = 0;
        }

        public void Insert(ServiceRequest request)
        {
            root = InsertRecursive(root, request);
        }

        /// <summary>
        /// Searches the AVL Binary Search Tree for a service request by its unique ID in O(log n) time.
        /// Also records the path of node IDs visited from the root to demonstrate tree traversal.
        /// </summary>
        public ServiceRequest? SearchById(int id, out List<int> visitedPath)
        {
            visitedPath = new List<int>();
            AvlTreeNode? current = root;

            while (current != null)
            {
                visitedPath.Add(current.Key);

                if (id == current.Key)
                {
                    return current.Request;
                }

                current = id < current.Key ? current.Left : current.Right;
            }

            return null;
        }

        /// <summary>
        /// Performs an In-Order traversal (Left -> Root -> Right) of the Binary Search Tree,
        /// returning all service requests sorted by their unique ID in ascending order.
        /// </summary>
        public List<ServiceRequest> InOrderTraversal()
        {
            List<ServiceRequest> result = new List<ServiceRequest>(Count);
            InOrderRecursive(root, result);
            return result;
        }

        private static void InOrderRecursive(AvlTreeNode? node, List<ServiceRequest> output)
        {
            if (node == null)
            {
                return;
            }

            InOrderRecursive(node.Left, output);
            output.Add(node.Request);
            InOrderRecursive(node.Right, output);
        }

        private AvlTreeNode InsertRecursive(AvlTreeNode? node, ServiceRequest request)
        {
            if (node == null)
            {
                Count++;
                return new AvlTreeNode(request);
            }

            if (request.Id < node.Key)
            {
                node.Left = InsertRecursive(node.Left, request);
            }
            else if (request.Id > node.Key)
            {
                node.Right = InsertRecursive(node.Right, request);
            }
            else
            {
                node.Request = request;
                return node;
            }

            UpdateHeight(node);
            int balance = GetBalanceFactor(node);

            // Left-Left (LL) Case -> Right Rotation
            if (balance > 1 && node.Left != null && request.Id < node.Left.Key)
            {
                return RotateRight(node);
            }

            // Right-Right (RR) Case -> Left Rotation
            if (balance < -1 && node.Right != null && request.Id > node.Right.Key)
            {
                return RotateLeft(node);
            }

            // Left-Right (LR) Case -> Left Rotation on Left Child, then Right Rotation
            if (balance > 1 && node.Left != null && request.Id > node.Left.Key)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            // Right-Left (RL) Case -> Right Rotation on Right Child, then Left Rotation
            if (balance < -1 && node.Right != null && request.Id < node.Right.Key)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        private static int GetHeight(AvlTreeNode? node) => node?.Height ?? 0;

        private static void UpdateHeight(AvlTreeNode node)
        {
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

        private static int GetBalanceFactor(AvlTreeNode? node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

        private static AvlTreeNode RotateRight(AvlTreeNode y)
        {
            AvlTreeNode x = y.Left!;
            AvlTreeNode? t2 = x.Right;

            x.Right = y;
            y.Left = t2;

            UpdateHeight(y);
            UpdateHeight(x);

            return x;
        }

        private static AvlTreeNode RotateLeft(AvlTreeNode x)
        {
            AvlTreeNode y = x.Right!;
            AvlTreeNode? t2 = y.Left;

            y.Left = x;
            x.Right = t2;

            UpdateHeight(x);
            UpdateHeight(y);

            return y;
        }
    }

    /// <summary>
    /// 3. RED-BLACK TREE (Left-Leaning Red-Black Binary Search Tree):
    /// Maintains service requests ordered chronologically by (DateSubmitted, Id)
    /// using red/black colour links and tree rotations for guaranteed O(log n) operations.
    /// </summary>
    public class ServiceRequestRedBlackTree
    {
        private sealed class RbNode
        {
            public ServiceRequest Request { get; set; }
            public RbNode? Left { get; set; }
            public RbNode? Right { get; set; }
            public bool IsRed { get; set; } = true;

            public RbNode(ServiceRequest request)
            {
                Request = request;
            }
        }

        private RbNode? root;

        public void Clear()
        {
            root = null;
        }

        public void Insert(ServiceRequest request)
        {
            root = InsertRecursive(root, request);
            if (root != null)
            {
                root.IsRed = false;
            }
        }

        /// <summary>
        /// Returns all service requests ordered chronologically by submission date (newest first).
        /// </summary>
        public List<ServiceRequest> GetNewestFirst()
        {
            List<ServiceRequest> result = new List<ServiceRequest>();
            ReverseInOrder(root, result);
            return result;
        }

        private static void ReverseInOrder(RbNode? node, List<ServiceRequest> output)
        {
            if (node == null)
            {
                return;
            }

            ReverseInOrder(node.Right, output);
            output.Add(node.Request);
            ReverseInOrder(node.Left, output);
        }

        private static int CompareKeys(ServiceRequest a, ServiceRequest b)
        {
            int cmp = a.DateSubmitted.CompareTo(b.DateSubmitted);
            return cmp != 0 ? cmp : a.Id.CompareTo(b.Id);
        }

        private static bool IsRed(RbNode? node) => node != null && node.IsRed;

        private static RbNode RotateLeft(RbNode h)
        {
            RbNode x = h.Right!;
            h.Right = x.Left;
            x.Left = h;
            x.IsRed = h.IsRed;
            h.IsRed = true;
            return x;
        }

        private static RbNode RotateRight(RbNode h)
        {
            RbNode x = h.Left!;
            h.Left = x.Right;
            x.Right = h;
            x.IsRed = h.IsRed;
            h.IsRed = true;
            return x;
        }

        private static void FlipColors(RbNode h)
        {
            h.IsRed = !h.IsRed;
            if (h.Left != null) h.Left.IsRed = !h.Left.IsRed;
            if (h.Right != null) h.Right.IsRed = !h.Right.IsRed;
        }

        private RbNode InsertRecursive(RbNode? h, ServiceRequest request)
        {
            if (h == null)
            {
                return new RbNode(request);
            }

            int cmp = CompareKeys(request, h.Request);
            if (cmp < 0)
            {
                h.Left = InsertRecursive(h.Left, request);
            }
            else if (cmp > 0)
            {
                h.Right = InsertRecursive(h.Right, request);
            }
            else
            {
                h.Request = request;
            }

            // Fix right-leaning red link
            if (IsRed(h.Right) && !IsRed(h.Left))
            {
                h = RotateLeft(h);
            }

            // Fix two consecutive left-leaning red links
            if (IsRed(h.Left) && IsRed(h.Left?.Left))
            {
                h = RotateRight(h);
            }

            // Split 4-node
            if (IsRed(h.Left) && IsRed(h.Right))
            {
                FlipColors(h);
            }

            return h;
        }
    }
}
