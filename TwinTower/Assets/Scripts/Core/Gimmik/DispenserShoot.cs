using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 함정 발동 발판을 밟을 시 화살이 나오는 구멍이다.
/// 이곳을 통해 화살이 발사된다.
/// </summary>
public class DispenserShoot : ActivateObject {
    public GameObject arrowPrefab;

    private bool haveArrow;                     // 한번만 발사
    void Start() {
        haveArrow = true;
    }

    public override void Launch() {
        if (!haveArrow) return;             // 이미 발사 된 경우 종료
        GameObject arrowObject = Instantiate(arrowPrefab, transform.position + transform.up * 0.3f, Quaternion.identity, transform);
		
        Arrow arrow = arrowObject.GetComponent<Arrow>();
        arrow.Launch(-transform.right);
        haveArrow = false;
    }
}
