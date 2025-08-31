using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using Mono.Cecil.Cil;

namespace AStar
{
    // 創建和管理網格系統
    public class Grid : MonoBehaviour
    {
        // 是否顯示網格Gizmos
        public bool displayGridGizmos;
        // 不可行走區域的LayerMask
        public LayerMask unwalkableMask;
        // 網格的世界大小
        public Vector2 gridWorldSize;
        // 節點半徑
        public float nodeRadius;
        // 可行走區域的地形類型和相應的代價
        public TerrainType[] walkableRegions;
        // 障礙物附近的代價
        public int obstacleProximityPenalty = 10;
        // 可行走區域的LayerMask
        LayerMask walkableMask;
        // 可行走區域的字典，用於存儲每個LayerMask對應的代價
        Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

        // 網格中的節點
        Node[,] grid;

        // 節點直徑、網格大小
        float nodeDiameter;
        int gridSizeX, gridSizeY;

        // 代價的最小值和最大值
        int penaltyMin = int.MaxValue;
        int penaltyMax = int.MinValue;

        private void Awake()
        {
            // 計算節點直徑和網格大小
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

            // 初始化可行走區域的LayerMask和對應的代價字典
            foreach (TerrainType region in walkableRegions)
            {
                walkableMask.value = walkableMask |= region.terrainMask.value;
                walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPanalty);
            }

            CreateGrid();
        }

        // 網格的最大大小
        public int MaxSize
        {
            get
            {
                return gridSizeX * gridSizeY;
            }
        }

        /// <summary>
        /// 創建網格
        /// </summary>
        private void CreateGrid()
        {
            // 創建一個新的二維 Node 陣列，用來存儲網格中的所有節點 [長,寬]
            grid = new Node[gridSizeX, gridSizeY];
            // 計算網格左下角的世界座標
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            // 計算每個節點的位置
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);

                    int movementPanalty = 0;

                    // 如果射線擊中了物體，則根據物體的圖層來設置移動懲罰值
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector2.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, walkableMask))
                    {
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPanalty);
                    }

                    if (!walkable)
                    {
                        movementPanalty += obstacleProximityPenalty;
                    }

                    // 設置網格中該節點的位置
                    grid[x, y] = new Node(walkable, worldPoint, x, y, movementPanalty);
                }
            }

            BlurPenaltyMap(3);
        }

        /// <summary>
        /// 計算節點權重
        /// </summary>
        void BlurPenaltyMap(int blurSize)
        {
            int kernelSize = blurSize * 2 + 1;
            int kernelExtents = (kernelSize - 1) / 2;

            int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
            int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

            for (int y = 0; y < gridSizeY; y++)
            {
                for (int x = -kernelExtents; x <= kernelExtents; x++)
                {
                    int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                    penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPanalty;
                }

                for (int x = 1; x < gridSizeX; x++)
                {
                    int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                    int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                    penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPanalty + grid[addIndex, y].movementPanalty;
                }
            }

            for (int x = 0; x < gridSizeY; x++)
            {
                for (int y = -kernelExtents; y <= kernelExtents; y++)
                {
                    int sampleY = Mathf.Clamp(x, 0, kernelExtents);
                    penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
                }

                int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
                grid[x, 0].movementPanalty = blurredPenalty;

                for (int y = 1; y < gridSizeX; y++)
                {
                    int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                    int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                    penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                    blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                    grid[x, y].movementPanalty = blurredPenalty;

                    if (blurredPenalty > penaltyMax)
                    {
                        penaltyMax = blurredPenalty;
                    }
                    if (blurredPenalty < penaltyMin)
                    {
                        penaltyMin = blurredPenalty;
                    }
                }
            }
        }

        /// <summary>
        /// 取得旁邊的節點
        /// </summary>
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        /// <summary>
        ///  根據世界座標獲取節點
        /// </summary>
        public Node GetNodeFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

            return grid[x, y];
        }

        private void OnDrawGizmos()
        {
            // 顯示範圍
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            // 想是每個節點是否可行走
            if (grid != null && displayGridGizmos)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPanalty));

                    Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * nodeDiameter);
                }
            }
        }

        // LayerMask類型
        [System.Serializable]
        public class TerrainType
        {
            public LayerMask terrainMask;
            public int terrainPanalty;
        }
    }
}