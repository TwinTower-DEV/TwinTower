using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;

/// <summary>
/// 발판을 밟을 경우 문을 회전시킴과 동시에 플레이어가 통과 가능하도록 변경해준다.
/// </summary>
public class GimmikDoor : GimmikBase {
    private Animator animator;
    public void Awake() {
        animator = GetComponent<Animator>();
    }

    public override void Active()
    {
        isWalkable = true;
        ManagerSet.Sound.Play("문여닫는소리(저작권 표시해야함)/Door_Open&Close_SFX");
        animator.Play("OpenDoor");
    }
    
    public override void DeActive()
    {
        isWalkable = false;
        ManagerSet.Sound.Play("문여닫는소리(저작권 표시해야함)/Door_Open&Close_SFX");
        animator.Play("CloseDoor");
    }
}
