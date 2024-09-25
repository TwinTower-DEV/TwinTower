using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;

public class ArrowPressurePlate : PressurePlate {
    private SpriteRenderer sprite;
    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        activateObject.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (sprite.color == new Color(140f/255f, 140f/255f, 140f/255f)) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") ||
            other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            ManagerSet.Sound.Play("Button_Click_SFX", Define.Sound.Effect);
            activateObject.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            ActivateObject active = activateObject.GetComponent<ActivateObject>();
            active.Launch();
            ManagerSet.Sound.Play("arrow_launch/Arrow_Launch_SFX");
            sprite.color = new Color(140f/255f, 140f/255f, 140f/255f);
        }
    }

    // 발판과 연결되어 있는 activateObject를 UnLaunch시킴.(문 닫기)
    protected override void OnTriggerExit2D(Collider2D other) {
        return;
    }
}
