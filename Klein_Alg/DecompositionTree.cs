using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class DecompositionTree<T, U> : BinaryTree
                                        <DecompositionTreeItem, LeafDistTable>
    {
        Dictionary<int, int> depths
            = new Dictionary<int, int>();
        internal Dictionary<long, Leaf<DecompositionTreeItem, LeafDistTable>> subsets
            = new Dictionary<long, Leaf<DecompositionTreeItem, LeafDistTable>>();
        internal NodesToSeparatorPathsTable nodesToSeparatorPaths;

        public DecompositionTree
            (DecompositionTree<DecompositionTreeItem, LeafDistTable> sepTree,
            List<Leaf<DecompositionTreeItem, LeafDistTable>> leaves,
            Dictionary<long, Node> graphNodes)
        {
            root = sepTree.root;
            foreach (Leaf<DecompositionTreeItem, LeafDistTable> leaf in leaves)
            {
                leaf.table = new LeafDistTable(graphNodes, leaf.value.nodesSubset);
                foreach (long id in leaf.value.nodesSubset)
                    subsets.Add(id, leaf);
            }
            SetDepthAndDecompTreePath(root, 0, new List<bool>());
            nodesToSeparatorPaths = new NodesToSeparatorPathsTable(graphNodes);
            Get_xToSeparatorPaths(root, graphNodes);

        }

        internal void SetDepthAndDecompTreePath(
            BTNode<DecompositionTreeItem, LeafDistTable> parent,
            int depth, List<bool> decompTreePath)
        {
            parent.depth = depth;
            depths.Add(parent.id, depth);
            if (parent.inner)
            {
                SetDepthAndDecompTreePath(
                    ((InnerNode<DecompositionTreeItem, LeafDistTable>)parent)
                    .leftChild, depth + 1,
                    ExpandDecompTreePath(decompTreePath, false));
                SetDepthAndDecompTreePath(
                    ((InnerNode<DecompositionTreeItem, LeafDistTable>)parent)
                    .leftChild, depth + 1,
                    ExpandDecompTreePath(decompTreePath, true));
            }
            else
                ((Leaf<DecompositionTreeItem, LeafDistTable>)parent)
                    .decompRootPath = decompTreePath;

        }

        private List<bool> ExpandDecompTreePath(List<bool> decompTreePath,
            bool next)
        {
            List<bool> expandedPath = new List<bool>(decompTreePath);
            expandedPath.Add(next);
            return expandedPath;
        }

        internal BTNode<DecompositionTreeItem, LeafDistTable> Get_LCA
            (Leaf<DecompositionTreeItem, LeafDistTable> x,
             Leaf<DecompositionTreeItem, LeafDistTable> y)
        {

            int count = 0;
            int maxIndex = Math.Min(x.decompRootPath.Count(), y.decompRootPath.Count());
            while (count < maxIndex
                && x.decompRootPath.ElementAt(count) == y.decompRootPath.ElementAt(count))
                count++;
            List<bool> commonPath = x.decompRootPath.GetRange(0, count);
            return FollowPath(commonPath);
        }


        internal BTNode<DecompositionTreeItem, LeafDistTable>
            FollowPath(List<bool> decompRootPath)
        {
            BTNode<DecompositionTreeItem, LeafDistTable> x = root;
            for (int i = 0; i < decompRootPath.Count(); i++)
            {
                if (!decompRootPath.ElementAt(i))
                    x = ((InnerNode<DecompositionTreeItem, LeafDistTable>)x).leftChild;
                else
                    x = ((InnerNode<DecompositionTreeItem, LeafDistTable>)x).rightChild;
            }
            return x;
        }


        internal int Merge(List<int[]> ls0, List<int[]> ls1)
        {
            //to do: return value of est.
            return 0;
        }

        private void Get_xToSeparatorPaths(
            BTNode<DecompositionTreeItem, LeafDistTable> x,
            Dictionary<long, Node> graphNodes)
        {
            if (x.inner)
            {
                InnerNode<DecompositionTreeItem, LeafDistTable> xIn
                    = (InnerNode<DecompositionTreeItem, LeafDistTable>)x;
                Portals portals = new Portals(
                    xIn.value.separator.path0, xIn.value.separator.path1);
                portals.Create_PortalsDistanciesTableForSeparator(
                    x, graphNodes, // subset
                    nodesToSeparatorPaths);
                Get_xToSeparatorPaths(xIn.leftChild, graphNodes);
                Get_xToSeparatorPaths(xIn.rightChild, graphNodes);
            }

        }

    }

    class DecompositionTreeItem
    {
        internal List<long> nodesSubset;
        internal SeparatorItem separator;
        internal List<long> part0;
        internal List<long> part1;


        private DecompositionTreeItem(List<long> subset,
        SeparatorItem sep, List<long> p0, List<long> p1)
        {
            nodesSubset = subset;
            separator = sep;
            part0 = p0;
            part1 = p1;
        }
    }


    class SeparatorItem
    {
        internal List<Node> path0;  //pro testovani z pripravenych sep.
        internal List<Node> path1;
    }
}
