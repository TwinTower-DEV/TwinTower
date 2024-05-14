using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif


[UnityEditor.CustomEditor(typeof(DispenserShoot))]

/// <summary>
/// 발사대 회전 시키면 자동으로 Sprite변경하게 하는 스크립트
/// </summary>
public class ArrowDispeserEditor : Editor {
    /*public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        DispenserShoot arrowDispenser = (DispenserShoot)target;
        SpriteRenderer targetSprite = arrowDispenser.GetComponent<SpriteRenderer>();
        targetSprite.sprite = arrowDispenser.GetSpriteOfDegree(arrowDispenser.transform.rotation.eulerAngles.z);
    }*/
}
