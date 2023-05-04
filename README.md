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

# Comment on which implementation (point quadtree or kd-tree) would you choose to support the 
storage of multi-dimensional keys:
Although both Point Quadtree and KD-Tree are viable data structures for storing multi-dimensional keys, 
choosing one over the other is contingent upon the specific use case and requirements. Nonetheless, a 
comprehensive analysis reveals that KD-Tree stands out as the more robust choice for supporting multi-
dimensional keys.
The KD-Tree, a versatile and adaptable structure, is specifically designed to manage multi-dimensional 
data. By implementing a binary space partitioning scheme, it efficiently organizes data points in a 
hierarchical manner, dynamically splitting the space into distinct regions. This sophisticated partitioning 
system is conducive to rapid query execution, notably outpacing the Point Quadtree in scenarios with 
high-dimensional data. With every node in the KD-Tree dividing the space along a single dimension, the 
structure exhibits greater resilience against data imbalances, which is an inherent advantage when 
handling multi-dimensional keys.
Conversely, the Point Quadtree is a simple, yet effective, data structure that excels in low-dimensional 
spaces, such as two-dimensional or three-dimensional data sets. Each node in the Point Quadtree is 
divided into four quadrants (or eight octants in 3D), rendering it especially adept at spatial queries and 
proximity searches. However, this quadtree structure's design does not lend itself as gracefully to higher 
dimensions, as the number of partitions grows exponentially with each additional dimension. 
Consequently, Point Quadtree performance may suffer in multi-dimensional scenarios, as traversing the 
numerous regions can result in excessive computational overhead.
When considering the impact of the "curse of dimensionality" on these data structures, KD-Tree 
emerges as the superior choice. As the number of dimensions increases, the space becomes increasingly 
sparse, rendering distance-based queries and search algorithms less effective. Point Quadtree, in 
particular, is susceptible to this phenomenon, as it struggles to maintain balance and efficiency with 
growing dimensionality. In contrast, KD-Tree's partitioning methodology is better equipped to handle 
the sparsity induced by higher dimensions, thereby mitigating the impact of the curse of dimensionality 
to a degree.

