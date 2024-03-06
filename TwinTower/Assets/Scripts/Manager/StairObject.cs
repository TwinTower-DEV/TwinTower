using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다음 단계 Scene의 이름을 저장해주는 ScrriptableObject
/// </summary>
[CreateAssetMenu(fileName = "New Stair", menuName = "Stair/Create Stair")]
public class StairObject : ScriptableObject {
    public string NextSceneString;
}