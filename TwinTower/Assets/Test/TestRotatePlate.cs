using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestRotatePlate : MonoBehaviour {
    private Tilemap tilemap;

    private void Awake() {
        tilemap = GetComponent<Tilemap>();
        
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = new TileBase[bounds.size.x * bounds.size.y * bounds.size.z];

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int z = bounds.zMin; z < bounds.zMax; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);
                    allTiles[(x - bounds.xMin) + (y - bounds.yMin) * bounds.size.x + (z - bounds.zMin) * bounds.size.x * bounds.size.y] = tilemap.GetTile(pos);
                }
            }
        }
    }
    
    public void RotateAll()
    {
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++) {
            for (int y = bounds.yMin; y < bounds.yMax; y++) {
                Vector3Int tilePosition = new Vector3Int(x, y);
                TileBase tile = tilemap.GetTile(tilePosition);

                if (tile != null)
                {
                    // 타일을 회전시킴
                    Quaternion rotation = Quaternion.Euler(0, 0, 90); // 90도 회전
                    Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
                    tilemap.SetTransformMatrix(tilePosition, matrix);
                }
            }
        }
    }
}
