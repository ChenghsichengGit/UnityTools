using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

namespace AStar
{
    // 網格系統中的一個節點
    public class Node : IHeapItem<Node>
    {
        // 節點是否可行走
        public bool walkable;
        // 節點在世界空間中的位置
        public Vector3 worldPosition;
        // 節點在網格中的 x 和 y 座標
        public int gridX;
        public int gridY;

        // 移動懲罰值，用於計算路徑時考慮地形和障礙物
        public int movementPanalty;

        // 路徑搜索中的成本值
        // 從起點到當前節點的實際代價
        public int gCost;
        // 從當前節點到目標節點的預估代價
        public int hCost;
        // 記錄從起點到當前節點的最短路徑的上一個節點
        public Node parent;
        // 用於堆排序的索引
        int heapIndex;

        public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _panaly)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
            gridX = _gridX;
            gridY = _gridY;
            movementPanalty = _panaly;
        }

        // 從起點到目標節點的總代價
        public int fCost
        {
            get { return gCost + hCost; }
        }

        // 堆排序所需的索引屬性
        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        // 比較兩個節點的 fCost，用於堆排序
        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }
    }
}
