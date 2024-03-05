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

public class TileFindManager : Manager<TileFindManager> {
    [SerializeField] private Tilemap tilemap;
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

        
        
    }

    public void Reset()
    {
        Init();
    }

    // 특정 위치와 가까운 tile의 가운데 좌표 반환해주는 함수
    // Input - 오브젝트의 위치
    // Output - Input을 기반으로 가까운 타일의 가운데 좌표 반환
    // 사용처 - tool/Ojbect Location Stereotyping
    public Vector3 gettileCentorLocation(Vector3 objectLocation) {
        Vector3Int tilePosition = tilemap.WorldToCell(objectLocation);
        Vector3 cellCentor = tilemap.GetCellCenterWorld(tilePosition);

        return cellCentor;
    }
}
