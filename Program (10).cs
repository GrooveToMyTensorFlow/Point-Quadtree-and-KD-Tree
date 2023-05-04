using System;
using System.Linq;

namespace Assignment_3
{


    // Define a Point class to represent multi-dimensional points
    public class Point
    {
        // An array to store coordinates of the point
        public int[] Coordinates { get; set; }
        // Constructor to initialize the coordinates of the point
        public Point(int[] coordinates)
        {
            Coordinates = coordinates;
        }

        // Method to get the dimension of the point
        public int GetDimension()
        {
            return Coordinates.Length;
        }

        // Method to get the coordinate at a specific dimension
        public int GetCoordinate(int dimension)
        {
            return Coordinates[dimension];
        }

        // Override Equals to compare points based on their coordinates
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Point other = (Point)obj;
            return Coordinates.SequenceEqual(other.Coordinates);
        }

        // Override GetHashCode to generate a hash code based on the coordinates
        public override int GetHashCode()
        {
            return HashCode.Combine(Coordinates);
        }
    }

    // Define a PointQuadtree class to store multi-dimensional points
    public class PointQuadtree
    {
        // Define a Node class to represent a node in the quadtree
        private class Node
        {
            public Point Point { get; set; }
            public Node[] Children { get; set; }

            // Constructor to initialize the node with a point and create an array of child nodes
            public Node(Point point)
            {
                Point = point;
                Children = new Node[1 << point.GetDimension()];
            }
        }

        // Root node of the quadtree
        private Node root;
        // Dimension of the quadtree
        private int dimension;

        // Constructor to initialize the dimension of the quadtree
        public PointQuadtree(int dimension)
        {
            this.dimension = dimension;
        }

        // Method to insert a point into the quadtree
        public bool Insert(Point point)
        {
            if (point.GetDimension() != dimension)
                return false;

            root = Insert(root, point, 0);
            return true;
        }

        // Helper method to insert a point into the quadtree recursively
        private Node Insert(Node node, Point point, int depth)
        {
            if (node == null)
                return new Node(point);

            int direction = GetDirection(node.Point, point, depth);
            node.Children[direction] = Insert(node.Children[direction], point, depth + 1);
            return node;
        }

        // Method to delete a point from the quadtree
        public bool Delete(Point point)
        {
            return Delete(ref root, point, 0);
        }

        // Helper method to delete a point from the quadtree recursively
        private bool Delete(ref Node node, Point point, int depth)
        {
            if (node == null)
                return false;

            if (node.Point.Equals(point))
            {
                node = null;
                return true;
            }

            int direction = GetDirection(node.Point, point, depth);
            return Delete(ref node.Children[direction], point, depth + 1);
        }

        // Method to check if the quadtree contains a point
        public bool Contains(Point point)
        {
            return Contains(root, point, 0);
        }

        // Helper method to check if the quadtree contains a point recursively
        private bool Contains(Node node, Point point, int depth)
        {
            if (node == null)
                return false;

            if (node.Point.Equals(point))
                return true;

            int direction = GetDirection(node.Point, point, depth);
            return Contains(node.Children[direction], point, depth + 1);
        }
        // Determines the direction for the current dimension based on the coordinates of two points
        private int GetDirection(Point a, Point b, int depth)
        {
            int dimension = depth % a.GetDimension();
            return (a.GetCoordinate(dimension) < b.GetCoordinate(dimension)) ? 1 : 0;
        }
    }

    // KDTree class to store and manage multi-dimensional points.
    public class KDTree
    {
        // Nested Node class to represent a node in the KDTree.
        private class Node
        {
            // Properties to store the Point, Left and Right child nodes.
            public Point Point { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }

            // Node constructor that initializes the Point.
            public Node(Point point)
            {
                Point = point;
            }
        }

        // Root of the KDTree and the dimensionality of the points.
        private Node root;
        private int dimension;

        // KDTree constructor initializes the dimensionality.
        public KDTree(int dimension)
        {
            this.dimension = dimension;
        }

        // Insert a point into the KDTree.
        public bool Insert(Point point)
        {
            // Check if the point has the correct dimensionality.
            if (point.GetDimension() != dimension)
                return false;

            // Call the private recursive Insert method.
            root = Insert(root, point, 0);
            return true;
        }

        // Private recursive Insert method to insert the point into the KDTree.
        private Node Insert(Node node, Point point, int depth)
        {
            // If the node is null, create a new Node with the given point.
            if (node == null)
                return new Node(point);

            // Calculate the current dimension to split on.
            int currentDimension = depth % dimension;

            // Insert the point into the left or right subtree based on the current dimension.
            if (point.GetCoordinate(currentDimension) < node.Point.GetCoordinate(currentDimension))
                node.Left = Insert(node.Left, point, depth + 1);
            else
                node.Right = Insert(node.Right, point, depth + 1);

            // Return the updated node.
            return node;
        }

        // Delete a point from the KDTree.
        public bool Delete(Point point)
        {
            // Call the private recursive DeleteHelper method.
            return DeleteHelper(root, null, point, 0, true);
        }

        // Private recursive DeleteHelper method to delete the point from the KDTree.
        private bool DeleteHelper(Node currentNode, Node parentNode, Point point, int depth, bool isLeftChild)
        {
            // Base case: if the currentNode is null, the point is not in the KDTree.
            if (currentNode == null)
                return false;

            // Calculate the current dimension to compare points.
            int currentDimension = depth % dimension;
            bool isDeleted = false;

            // If the currentNode's Point matches the given point, delete it.
            if (currentNode.Point.Equals(point))
            {
                // Replace the currentNode's Point with the minimum point in the right subtree.
                if (currentNode.Right != null)
                {
                    currentNode.Point = FindMin(currentNode.Right, depth);
                    DeleteHelper(currentNode.Right, currentNode, currentNode.Point, depth + 1, false);
                }
                // Replace the currentNode's Point with the minimum point in the left subtree.
                else if (currentNode.Left != null)
                {
                    currentNode.Point = FindMin(currentNode.Left, depth);
                    DeleteHelper(currentNode.Left, currentNode, currentNode.Point, depth + 1, true);
                }
                // If currentNode has no children, delete it.
                else
                {
                    if (parentNode != null)
                    {
                        if (isLeftChild)
                            parentNode.Left = null;
                        else
                            parentNode.Right = null;
                    }
                    else
                    {
                        root = null;
                    }
                }

                isDeleted = true;
            }
            // If the point to be deleted is in the left subtree, recursively call DeleteHelper on the left subtree.
            else if (point.GetCoordinate(currentDimension) < currentNode.Point.GetCoordinate(currentDimension))
                isDeleted = DeleteHelper(currentNode.Left, currentNode, point, depth + 1, true);
            // If the point to be deleted is in the right subtree, recursively call DeleteHelper on the right subtree.
            else
                isDeleted = DeleteHelper(currentNode.Right, currentNode, point, depth + 1, false);

            // Return whether the point has been deleted.
            return isDeleted;
        }

        // Check if the KDTree contains a given point.
        public bool Contains(Point point)
        {
            // Call the private recursive Contains method.
            return Contains(root, point, 0);
        }

        // Private recursive Contains method to check if the KDTree contains a given point.
        private bool Contains(Node node, Point point, int depth)
        {
            // Base case: if the node is null, the point is not in the KDTree.
            if (node == null)
                return false;

            // If the node's Point matches the given point, the point is in the KDTree.
            if (node.Point.Equals(point))
                return true;

            // Calculate the current dimension to compare points.
            int currentDimension = depth % dimension;

            // Recursively call Contains on the left or right subtree based on the current dimension.
            if (point.GetCoordinate(currentDimension) < node.Point.GetCoordinate(currentDimension))
                return Contains(node.Left, point, depth + 1);

            return Contains(node.Right, point, depth + 1);
        }

        // Find the minimum point in the KDTree starting from a given node.
        private Point FindMin(Node node, int depth)
        {
            // Base case: if the node is null, return null.
            if (node == null)
                return null;

            // Calculate the current dimension to compare points.
            int currentDimension = depth % dimension;

            // If there is no left child, return the node's Point as the minimum.
            if (node.Left == null)
                return node.Point;

            // Recursively find the minimum points in the left and right subtrees.
            Point leftMin = FindMin(node.Left, depth + 1);
            Point rightMin = FindMin(node.Right, depth + 1);

            // Return the minimum point based on the current dimension.
            if (leftMin.GetCoordinate(currentDimension) < rightMin.GetCoordinate(currentDimension))
                return leftMin;

            return rightMin;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a KDTree with 2 dimensions
            KDTree kdTree = new KDTree(2);

            // Insert points into the KDTree
            kdTree.Insert(new Point(new int[] { 5, 8 }));
            kdTree.Insert(new Point(new int[] { 6, 1 }));
            kdTree.Insert(new Point(new int[] { 1, 5 }));
            kdTree.Insert(new Point(new int[] { 7, 9 }));
            kdTree.Insert(new Point(new int[] { 2, 3 }));

            Console.WriteLine("KDTree:");
            Console.WriteLine($"Contains (7, 9): {kdTree.Contains(new Point(new int[] { 7, 9 }))}");
            Console.WriteLine($"Contains (1, 5): {kdTree.Contains(new Point(new int[] { 1, 5 }))}");
            Console.WriteLine($"Contains (2, 3): {kdTree.Contains(new Point(new int[] { 2, 3 }))}");
            Console.WriteLine($"Delete (1, 5): {kdTree.Delete(new Point(new int[] { 1, 5 }))}");
            Console.WriteLine($"Contains (1, 5): {kdTree.Contains(new Point(new int[] { 1, 5 }))}");
            Console.WriteLine($"Contains (7, 9): {kdTree.Contains(new Point(new int[] { 7, 9 }))}");

            // Create a PointQuadtree with 2 dimensions
            PointQuadtree pointQuadtree = new PointQuadtree(2);

            // Insert points into the PointQuadtree
            pointQuadtree.Insert(new Point(new int[] { 5, 8 }));
            pointQuadtree.Insert(new Point(new int[] { 6, 1 }));
            pointQuadtree.Insert(new Point(new int[] { 1, 5 }));
            pointQuadtree.Insert(new Point(new int[] { 7, 9 }));
            pointQuadtree.Insert(new Point(new int[] { 2, 3 }));

            Console.WriteLine("\nPointQuadtree:");
            Console.WriteLine($"Contains (7, 9): {pointQuadtree.Contains(new Point(new int[] { 7, 9 }))}");
            Console.WriteLine($"Contains (1, 5): {pointQuadtree.Contains(new Point(new int[] { 1, 5 }))}");
            Console.WriteLine($"Contains (2, 3): {pointQuadtree.Contains(new Point(new int[] { 2, 3 }))}");
            Console.WriteLine($"Delete (1, 5): {pointQuadtree.Delete(new Point(new int[] { 1, 5 }))}");
            Console.WriteLine($"Contains (1, 5): {pointQuadtree.Contains(new Point(new int[] { 1, 5 }))}");
            Console.WriteLine($"Contains (7, 9): {pointQuadtree.Contains(new Point(new int[] { 7, 9 }))}");

            // Create a KDTree with 2 dimensions
            KDTree kdTree1 = new KDTree(2);

            // Insert points into the KDTree
            Console.WriteLine("\nKDTree:");
            Console.WriteLine($"Insert (3, 6): {kdTree1.Insert(new Point(new int[] { 3, 6 }))}");
            Console.WriteLine($"Insert (2, 4): {kdTree1.Insert(new Point(new int[] { 2, 4 }))}");
            Console.WriteLine($"Insert (9, 1): {kdTree1.Insert(new Point(new int[] { 9, 1 }))}");

            Console.WriteLine($"Contains (9, 1): {kdTree1.Contains(new Point(new int[] { 9, 1 }))}");
            Console.WriteLine($"Delete (9, 1): {kdTree1.Delete(new Point(new int[] { 9, 1 }))}");
            Console.WriteLine($"Contains (9, 1): {kdTree1.Contains(new Point(new int[] { 9, 1 }))}");
            Console.WriteLine($"Contains (3, 6): {kdTree1.Contains(new Point(new int[] { 3, 6 }))}");

            // Create a PointQuadtree with 2 dimensions
            PointQuadtree pointQuadtree1 = new PointQuadtree(2);

            // Insert points into the PointQuadtree
            Console.WriteLine("\nPointQuadtree:");
            Console.WriteLine($"Insert (3, 6): {pointQuadtree1.Insert(new Point(new int[] { 3, 6 }))}");
            Console.WriteLine($"Insert (2, 4): {pointQuadtree1.Insert(new Point(new int[] { 2, 4 }))}");
            Console.WriteLine($"Insert (9, 1): {pointQuadtree1.Insert(new Point(new int[] { 9, 1 }))}");

            Console.WriteLine($"Contains (9, 1): {pointQuadtree1.Contains(new Point(new int[] { 9, 1 }))}");
            Console.WriteLine($"Delete (9, 1): {pointQuadtree1.Delete(new Point(new int[] { 9, 1 }))}");
            Console.WriteLine($"Contains (9, 1): {pointQuadtree1.Contains(new Point(new int[] { 9, 1 }))}");
            Console.WriteLine($"Contains (2, 4): {pointQuadtree1.Contains(new Point(new int[] { 2, 4 }))}");
        }

    }
}


