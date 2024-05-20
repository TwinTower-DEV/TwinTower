using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;

/// <summary>
/// 발판을 밟을 경우 문을 회전시킴과 동시에 플레이어가 통과 가능하도록 변경해준다.
/// </summary>
public class DoorActivate : ActivateObject {
    [SerializeField] private AudioClip audio;
    private Animator animator;
    public void Awake() {
        animator = GetComponent<Animator>();
    }

    public override void Launch()
    {
        animator.Play("OpenDoor");
        SoundManager.Instance.PlayEffect(audio);
        gameObject.layer = LayerMask.NameToLayer("Rotatable");       // default로 변환
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    
    public override void UnLaunch()
    {
        animator.Play("CloseDoor");
        gameObject.layer = LayerMask.NameToLayer("Wall");       // wall 변환
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
