using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 함정 발동 발판이다.
/// </summary>
public class PressurePlate : MonoBehaviour {
    public GameObject dispenser;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            DispenserShoot dispensershoot =  dispenser.GetComponent<DispenserShoot>();
            dispensershoot.Launch();
        }
    }
}
