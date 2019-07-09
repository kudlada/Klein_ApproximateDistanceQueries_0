using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class Query
    {
        public static int GetDistanceEstimate(
            DecompositionTree<DecompositionTreeItem, LeafDistTable> decompTree,
            Node x, Node y)
        {
            Leaf<DecompositionTreeItem, LeafDistTable> xLeaf 
                = decompTree.subsets[x.id];
            Leaf<DecompositionTreeItem, LeafDistTable> yLeaf
                = decompTree.subsets[y.id];
            BTNode<DecompositionTreeItem, LeafDistTable> LCA
                = decompTree.Get_LCA(xLeaf, yLeaf);
            Dictionary <BTNode<DecompositionTreeItem, LeafDistTable>,
                Dictionary<int, List<int[]>>> xTable 
                = decompTree.nodesToSeparatorPaths.table[x.id];
            Dictionary<BTNode<DecompositionTreeItem, LeafDistTable>,
                Dictionary<int, List<int[]>>> yTable
                = decompTree.nodesToSeparatorPaths.table[y.id];
            int est0= decompTree.Merge(
                xTable[LCA][0],
                yTable[LCA][0]);
            int est1 = decompTree.Merge(
                xTable[LCA][1],
                yTable[LCA][1]);
            return Math.Min(est0, est1);

        }
    }
}
