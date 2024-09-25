using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;

/// <summary>
/// 발판을 밟을 경우 문을 회전시킴과 동시에 플레이어가 통과 가능하도록 변경해준다.
/// </summary>
public class DoorActivate : ActivateObject {
    private Animator animator;
    public void Awake() {
        animator = GetComponent<Animator>();
    }

    public override void Launch()
    {
        ManagerSet.Sound.Play("문여닫는소리(저작권 표시해야함)/Door_Open&Close_SFX");
        animator.Play("OpenDoor");
        gameObject.layer = LayerMask.NameToLayer("Rotatable");       // default로 변환
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    
    public override void UnLaunch()
    {
        ManagerSet.Sound.Play("문여닫는소리(저작권 표시해야함)/Door_Open&Close_SFX");
        animator.Play("CloseDoor");
        gameObject.layer = LayerMask.NameToLayer("Wall");       // wall 변환
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
