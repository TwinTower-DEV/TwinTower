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

public class TileFindManager : Manager<TileFindManager> {
    // 특정 위치와 가까운 tile의 가운데 좌표 반환해주는 함수
    // Input - 오브젝트의 위치, 가운데로 위치시킬 타일맵
    // Output - Input을 기반으로 가까운 타일의 가운데 좌표 반환
    // 사용처 - tool/Ojbect Location Stereotyping
    public Vector3 gettileCentorLocation(Vector3 objectLocation, Tilemap Tile) {
        Vector3Int tilePosition = Tile.WorldToCell(objectLocation);
        Vector3 cellCentor = Tile.GetCellCenterWorld(tilePosition);

        return cellCentor;
    }
}
