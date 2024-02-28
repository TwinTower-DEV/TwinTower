using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 발판을 밟을 경우 문을 회전시킴과 동시에 플레이어가 통과 가능하도록 변경해준다.
/// </summary>
public class DoorActivate : ActivateObject
{
    public override void Launch()
    {
        transform.localRotation = Quaternion.Euler(0 ,0, transform.rotation.eulerAngles.z - 90);
        gameObject.layer = LayerMask.NameToLayer("Default");       // default로 변환
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    
    public override void UnLaunch()
    {
        transform.localRotation = Quaternion.Euler(0 ,0, transform.rotation.eulerAngles.z + 90);
        gameObject.layer = LayerMask.NameToLayer("Wall");       // wall 변환
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
