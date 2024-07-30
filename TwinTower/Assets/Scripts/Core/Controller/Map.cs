using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace TwinTower
{
    public class Map : MonoBehaviour
    {
        public MapType type;

        public Tilemap objectMap;
        private TileBase[,] map;

        private void Start() 
        {
            GetTiles();
        }

        private void GetTiles()
        {
            BoundsInt bounds = objectMap.cellBounds;
            TileBase[] allTiles = objectMap.GetTilesBlock(bounds);

            map = new TileBase[bounds.size.x, bounds.size.y];

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];

                    if (tile != null)
                    {
                        map[x, y] = tile;
                    }
                }
            }
        }
    }
}