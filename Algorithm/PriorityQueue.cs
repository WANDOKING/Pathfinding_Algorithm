using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algorithm
{
    class PriorityQueue<T> where T : IComparable<T>
    {
        public void Add(T data)
        {
            mHeap.Add(data);

            int currentIndex = Count - 1;
            while (currentIndex > 0)
            {
                int parentIndex = (currentIndex - 1) / 2;

                if (mHeap[currentIndex].CompareTo(mHeap[parentIndex]) < 0)
                {
                    T temp = mHeap[currentIndex];
                    mHeap[currentIndex] = mHeap[parentIndex];
                    mHeap[parentIndex] = temp;

                    currentIndex = parentIndex;
                }
                else
                {
                    break;
                }
            }
        }

        public T Pop()
        {
            T ret = mHeap[0];

            mHeap[0] = mHeap[Count - 1];
            mHeap.RemoveAt(Count - 1);

            if (Count == 0)
            {
                return ret;
            }

            int currentIndex = 0;
            while (true)
            {
                int leftChildIndex = currentIndex * 2 + 1;
                int rightChildIndex = currentIndex * 2 + 2;

                T min = mHeap[currentIndex];
                if (leftChildIndex <= Count - 1 && mHeap[leftChildIndex].CompareTo(min) < 0)
                {
                    min = mHeap[leftChildIndex];
                }
                if (rightChildIndex <= Count - 1 && mHeap[rightChildIndex].CompareTo(min) < 0)
                {
                    min = mHeap[rightChildIndex];
                }

                if (min.CompareTo(mHeap[currentIndex]) == 0)
                {
                    break;
                }
                else if (min.CompareTo(mHeap[leftChildIndex]) == 0)
                {
                    T temp = mHeap[currentIndex];
                    mHeap[currentIndex] = mHeap[leftChildIndex];
                    mHeap[leftChildIndex] = temp;

                    currentIndex = leftChildIndex;
                }
                else
                {
                    T temp = mHeap[currentIndex];
                    mHeap[currentIndex] = mHeap[rightChildIndex];
                    mHeap[rightChildIndex] = temp;

                    currentIndex = rightChildIndex;
                }
            }

            return ret;
        }

        public int Count { get { return mHeap.Count; } }

        private List<T> mHeap = new List<T>();
    }

}
