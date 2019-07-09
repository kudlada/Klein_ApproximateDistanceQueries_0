using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class NodesToSeparatorPathsTable
    {
        internal Dictionary<long,
                    Dictionary<BTNode<DecompositionTreeItem, LeafDistTable>,
                        Dictionary<int, List<int[]>>>>
                table = new Dictionary<long,
                    Dictionary<BTNode<DecompositionTreeItem, LeafDistTable>,
                        Dictionary<int, List<int[]>>>>();

        internal NodesToSeparatorPathsTable(Dictionary<long, Node> nodes)
        {
            foreach (Node x in nodes.Values)
                table.Add(x.id,
                    new Dictionary<BTNode<DecompositionTreeItem, LeafDistTable>,
                        Dictionary<int, List<int[]>>>());
        }
        
    }
}
