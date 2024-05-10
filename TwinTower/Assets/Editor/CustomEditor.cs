using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using TwinTower;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

using UnityEditor.SceneManagement;
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
        SpriteRenderer[] ForCenterObjects = GameObject.FindObjectsOfType<SpriteRenderer>();
        
        foreach (SpriteRenderer obj in ForCenterObjects) {
            for (int i = 0; i < tileMaps.Length; i++) {
                Vector3Int tilePosition = tileMaps[i].WorldToCell(obj.transform.position);
                Vector3 cellCenter = tileMaps[i].GetCellCenterWorld(tilePosition);
                if (tileMaps[i].GetTile(tilePosition) != null) obj.transform.position = cellCenter;
            }
        }
    }
    
    /// <summary>
    /// 잘못 배치한 것이 없는지 확인하는 용도
    /// </summary>
    [MenuItem("Tools/Find Error Object")]
    public static void ErrorFind() {
        ClearConsole();
        StageObjects stageObjects = new StageObjects();
        stageObjects.Check();
    }

    // 콘솔 초기화
    public static void ClearConsole() {
        var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}



    // 기본 노드

    
