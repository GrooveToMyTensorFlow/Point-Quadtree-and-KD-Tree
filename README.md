# Point Quadtree and KD-Tree

This project focuses on implementing two data structures that generalize the binary search tree for storing multi-dimensional keys: the point quadtree and the kd-tree. These data structures are used to store points in k-dimensional space.

## Classes

1. `Point`: Represents a point in k-dimensional space.
2. `PointQuadtree`: A tree-based data structure to store multi-dimensional points using the point quadtree algorithm.
3. `KDTree`: A tree-based data structure to store multi-dimensional points using the kd-tree algorithm.

## Methods

For both `PointQuadtree` and `KDTree` classes:

1. `public bool Insert(Point p)`: Insert point `p` into the data structure. Assume that duplicate points are not inserted. Returns `true` if successful, `false` otherwise.

2. `public bool Delete(Point p)`: Delete point `p` from the data structure. Returns `true` if successful, `false` otherwise.

3. `public bool Contains(Point p)`: Check if point `p` exists in the data structure. Returns `true` if found, `false` otherwise.

Note: Additional support methods such as `FindMin` for the KDTree may also be required.

## Requirements

1. Complete the equivalent `Point` class in C#.

2. Fully implement additional C# classes for `PointQuadtree` and `KDTree` with the above methods.

3. Test your program thoroughly for points in two dimensions only. Your code, however, should be general enough for points in any dimension.

4. In a separate document, comment on which implementation (point quadtree or kd-tree) you would choose to support the storage of multi-dimensional keys.

