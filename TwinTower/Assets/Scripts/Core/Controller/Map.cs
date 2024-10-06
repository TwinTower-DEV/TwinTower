using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TwinTower
{
    public enum MapType
    {
        None = -1,
        Left,
        Right
    }

    public class Map : MonoBehaviour
    {
        public MapType type;
        public Tilemap walls;
        public List<GimmikBase> gimmiks;
        private GimmikBase[,] map;

        private void Start() 
        {
            GetWalls();
            GetGimmiks();
            //ShowGimmik();
        }

        private void GetWalls()
        {
            BoundsInt bounds = walls.cellBounds;
            TileBase[] allTiles = walls.GetTilesBlock(bounds);

            map = new GimmikBase[bounds.size.x, bounds.size.y];

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    if (allTiles[x + y * bounds.size.x] != null)
                    {
                        map[x, y] = new GimmikWall();
                    }
                }
            }
        }

        private void GetGimmiks()
        {
            gimmiks.ForEach(gimmik => map[gimmik.x, gimmik.y] = gimmik);
        }

        private void ShowGimmik()
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Debug.Log($"({x},{y}): {map[x,y]}");
                }
            }
        }

        public bool CanMove(int x, int y)
        {
            // map에는 바깥 벽은 포함되지 않으므로, -1 or map보다 큰 값이면 무조건 움직이지 못해야 함. - 손창하
            if (x < 0 || x > map.GetLength(0) - 1)
            {
                return false;
            }
            
            if (y < 0 || y > map.GetLength(0) - 1)
            {
                return false;
            }

            return map[x, y]?.IsWalkable ?? true;
        }

        public GimmikBase GetGimmik(int x, int y)
        {
            return map[x, y];
        }

        public Vector2 GetTilePosition(int x, int y)
        {
            Vector3 cellSize = walls.cellSize; // Wall이나 Floor나 Cell Size는 같으므로 wall로 해도 문제 없음
            Vector3 origin = walls.origin;

            Vector2 target = new Vector2(origin.x + x + cellSize.x / 2, origin.y + y + cellSize.y / 2);

            return target;
        }
    }
}