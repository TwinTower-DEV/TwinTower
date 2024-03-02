using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Debug.Log("튜토리얼 보여주기");
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Debug.Log("튜토리얼 없애기");
        }
    }
}
