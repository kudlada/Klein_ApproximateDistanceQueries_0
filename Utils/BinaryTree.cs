using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class BinaryTree<T,U>
    {
        public BTNode<T,U> root;       
    }

    abstract class BTNode<T,U>
    {
        public T value;
        public BTNode<T, U> parent;
        public int depth = 0;
        public int id = -1;
        public bool inner = false;

    }

    class InnerNode<T, U> : BTNode<T, U>
    {
        public BTNode<T, U> leftChild;
        public BTNode<T, U> rightChild;

        public InnerNode()
        {
            inner = true;
        }    
    }

    class Leaf<T,U> : BTNode<T,U>
    {
        public U table;
        public List<bool> decompRootPath;
        
    }
}
