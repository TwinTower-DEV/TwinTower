using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Tilemaps;

/// <summary>
/// 타일의 중앙 위치로 쉽게 이동시켜주는 매니저.
/// </summary>

public class TileFindManager : MonoBehaviour {
    private Tilemap[] tileMaps = new Tilemap[2];
    
    private static TileFindManager instance;

    // 외부에서 싱글톤 인스턴스에 접근할 수 있는 프로퍼티
    public static TileFindManager Instance
    {
        get
        {
            // 인스턴스가 아직 없는 경우 새로 생성
            if (instance == null)
            {
                // 씬에서 싱글톤 게임 오브젝트를 찾음
                GameObject singletonObject = GameObject.Find("Map");

                // 싱글톤 게임 오브젝트가 있는 경우 해당 컴포넌트를 찾아 할당
                instance = singletonObject.GetComponent<TileFindManager>();
            }

            return instance;
        }
    }

    private void Awake() {
        FindTileMap();
    }
    
    public Vector3 gettileCentorLocation(Vector3 objectLocation) {
        if (tileMaps[0] == null) FindTileMap();
        for (int i = 0; i < tileMaps.Length; i++) {
            Vector3Int tilePosition = tileMaps[i].WorldToCell(objectLocation);
            Vector3 cellCenter = tileMaps[i].GetCellCenterWorld(tilePosition);
            if (tileMaps[i].GetTile(tilePosition) != null) return cellCenter;
        }

        return Vector3.zero;
        // throw new Exception("이상한 오브젝트가 들어옴 -> 타일맵 내에 존재하지 않는 오브젝트.");
    }
    
    public Tilemap getTileInArea(Vector3 objectLocation, bool isOpp) {
        if (tileMaps[0] == null) FindTileMap();
        Vector3Int tilePosition = tileMaps[0].WorldToCell(objectLocation);
        if (tileMaps[0].GetTile(tilePosition) != null) {
            if (isOpp) return tileMaps[1];
            return tileMaps[0];
        }
        if (isOpp) return tileMaps[0];
        return tileMaps[1];
    }

    private void FindTileMap() {
        tileMaps[0] = transform.GetChild(0).GetComponent<Tilemap>();
        tileMaps[1] = transform.GetChild(1).GetComponent<Tilemap>();
    }
}
