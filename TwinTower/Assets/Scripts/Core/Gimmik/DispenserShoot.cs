using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 함정 발동 발판을 밟을 시 화살이 나오는 구멍이다.
/// 이곳을 통해 화살이 발사된다.
/// </summary>
public class DispenserShoot : ActivateObject {
    public GameObject arrowPrefab;

    private Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public override void Launch() { 
        GameObject arrowObject = Instantiate(arrowPrefab, transform.position + transform.up * 0.3f, Quaternion.identity, transform);
		
        Arrow arrow = arrowObject.GetComponent<Arrow>();
        arrow.Launch(transform.up);
    }
}
