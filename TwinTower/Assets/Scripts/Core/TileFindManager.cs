using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Tilemaps;

/// <summary>
/// 맵 스테이지가 초기화 될 때마다 스테이지안의 Tile들의 좌표를 저장하는 매니저입니다.(한 번만 실핼됨)
/// </summary>

public class TileFindManager : Manager<TileFindManager>
{
    public class Info
    {
        public int x, y;
        public bool isobstacle;

        public Info(int x, int y, bool isobstacle)
        {
            this.x = x;
            this.y = y;
            this.isobstacle = isobstacle;
        }
    };
    
    
   public Info [,] player1Map = new Info[8, 8];
   public Info [,] player2Map = new Info[8, 8];

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        GameObject go = GameObject.Find("Map");

        if (go == null)
            return;

        Tilemap player1map = go.transform.GetChild(0).GetComponent<Tilemap>();
        Tilemap player2map = go.transform.GetChild(1).GetComponent<Tilemap>();

        int i = 0, j = 0;
        for (int y = player1map.cellBounds.yMax; y >= player1map.cellBounds.yMin;y--)
        {
            for (int x = player1map.cellBounds.xMin; x <= player1map.cellBounds.xMax; x++)
            {
                TileBase tile = player1map.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    player1Map[i, j] = new Info(x, y, false);
                    j++;
                    if (j > 7)
                    {
                        i++;
                        j = 0;
                    }
                }
            }
        }

        i = 0; j = 0;
        for (int y = player2map.cellBounds.yMax; y >= player2map.cellBounds.yMin;y--)
        {
            for (int x = player2map.cellBounds.xMin; x <= player2map.cellBounds.xMax; x++)
            {
                TileBase tile = player2map.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    player2Map[i, j] = new Info(x, y, false);
                    j++;
                    if (j > 7)
                    {
                        i++;
                        j = 0;
                    }
                }
            }
        }
        
    }

    public void Reset()
    {
        Init();
    }
}
