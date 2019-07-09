using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class PriorityQueue<T>
    {
        internal class Item : IComparable<Item>
        {
            public int number;
            public T value;
            public int CompareTo(Item other)
            {
                return number.CompareTo(other.number);
            }
        }

        private Heap<Item> heap = new Heap<Item>();

        public void Add(int nr, T val)
        {
            heap.Add(new Item() { number = nr, value = val });
        }

        public T RemoveMin()
        {
            return heap.RemoveMin().value;
        }

        public void RemoveElement(int key)
        {
            Item element=null;
            for (int i=0;i<Count;i++)
            {
                if (heap.arr[i].number==key)
                {
                    element = heap.arr[i];
                    break;
                }
            }
            if (element!=null)
                heap.arr.Remove(element);
        }

        public T Peek()
        {
            return heap.arr[0].value;
        }

        public int Count
        {
            get
            {
                return heap.arr.Count;
            }
        }
    }

    class Heap<T> where T : IComparable<T>   //pridat peek atd pro jine pouziti nez v prior q.
    {
        public List<T> arr = new List<T>();

        public void Add(T val)
        {
            arr.Add(val);
            int i = arr.Count - 1;
            int parent = (i - 1) >> 1;
            while (i > 0 && arr[i].CompareTo(arr[parent]) < 0)
            {
                T cp = arr[i];
                arr[i] = arr[parent];
                i = parent;
                arr[parent] = cp;
                parent = (i - 1) >> 1;
            }
        }

        public T RemoveMin()
        {
            T result = arr[0];
            arr[0] = arr[arr.Count - 1];
            arr.RemoveAt(arr.Count - 1);
            
            int i = 0;
            while (i < arr.Count)
            {
                int min = i;
                if (2 * i + 1 < arr.Count && arr[2 * i + 1].CompareTo(arr[min]) == -1)
                    min = 2 * i + 1;
                if (2 * i + 2 < arr.Count && arr[2 * i + 2].CompareTo(arr[min]) == -1)
                    min = 2 * i + 2;

                if (min == i)
                    break;
                else
                {
                    T tmp = arr[i];
                    arr[i] = arr[min];
                    arr[min] = tmp;
                    i = min;
                }
            }

            return result;
        }
    }
}



