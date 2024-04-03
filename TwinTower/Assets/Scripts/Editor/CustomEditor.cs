using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

/// <summary>
/// 에디터에서 필요한 도구를 설정해주는 Editor
/// </summary>
public class CustomEditor : Editor {
    /// <summary>
    /// 오브젝트를 선택하고 Tool바에 있는 Ojbect Location Stereotyping Select를 선택하면 해당 오브젝트와 가장 가까운 타일의 중앙에 배치시킴.
    /// 주로 맵을 변경하거나 설정할 때 유용하게 사용됨.
    /// </summary>
    [MenuItem("Tools/Object Location Stereotyping")]
    public static void ObjectLocationStereotyping() {
        Tilemap[] tileMaps = GameObject.FindObjectsOfType<Tilemap>();
        
        foreach (GameObject obj in Selection.gameObjects) {
            for (int i = 0; i < tileMaps.Length; i++) {
                if (obj.GetComponent<Grid>() != null) throw new Exception("타일맵도 포함되었음 조심");
                Vector3Int tilePosition = tileMaps[i].WorldToCell(obj.transform.position);
                Vector3 cellCenter = tileMaps[i].GetCellCenterWorld(tilePosition);
                if (tileMaps[i].GetTile(tilePosition) != null) obj.transform.position = cellCenter;
            }
        }
    }
}
