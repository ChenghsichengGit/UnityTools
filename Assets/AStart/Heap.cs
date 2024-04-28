using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    // 使用堆來優化AStar系統，減少系統負擔
    public class Heap<T> where T : IHeapItem<T>
    {
        // 用於存儲堆中的元素的陣列
        T[] items;
        // 目前堆中的元素數量
        int currentItemCount;

        /// <summary>
        /// 初始化堆的容量
        /// </summary>
        public Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];
        }

        /// <summary>
        /// 將一個元素添加到堆中
        /// </summary>
        public void Add(T item)
        {
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(item);
            currentItemCount++;
        }

        /// <summary>
        /// 移除並返回堆中最小的元素
        /// </summary>
        public T RemoveFirst()
        {
            T firstItem = items[0];
            currentItemCount--;
            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;
            SortDown(items[0]);
            return firstItem;
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        // 堆中元素的數量
        public int Count
        {
            get
            {
                return currentItemCount;
            }
        }

        // 檢查堆中是否包含指定的元素
        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }

        // 向下排序操作，保證堆的結構仍然符合最小堆的要求
        void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < currentItemCount)
                {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < currentItemCount)
                    {
                        if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(item, items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        // 向上排序操作，保證堆的結構仍然符合最小堆的要求
        void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                {
                    break;
                }

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        // 交換兩個元素的位置
        void Swap(T itemA, T itemB)
        {
            items[itemA.HeapIndex] = itemB;
            items[itemB.HeapIndex] = itemA;
            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
    }

    // 讓繼承的腳本可以用list.Sort()排序
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }
}