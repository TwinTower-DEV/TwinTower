using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[UnityEditor.CustomEditor(typeof(DispenserShoot))]
public class ArrowDispeserEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        DispenserShoot arrowDispenser = (DispenserShoot)target;
        SpriteRenderer targetSprite = arrowDispenser.GetComponent<SpriteRenderer>();
        targetSprite.sprite = arrowDispenser.GetSpriteOfDegree(arrowDispenser.transform.rotation.eulerAngles.z);
    }
}
