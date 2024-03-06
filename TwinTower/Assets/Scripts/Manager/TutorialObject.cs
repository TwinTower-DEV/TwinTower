using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다음 단계 Tutorial 이름을 저장해주는 ScrriptableObject
/// </summary>
[CreateAssetMenu(fileName = "New Tutorial", menuName = "Tutorial/Create Tutorial")]
public class TutorialObject : ScriptableObject {
    public string TutorialString;
}
