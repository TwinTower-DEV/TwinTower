using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 함정 발동 발판을 밟을 시 화살이 나오는 구멍이다.
/// 이곳을 통해 화살이 발사된다.
/// </summary>
public class DispenserShoot : MonoBehaviour {
    public GameObject arrowPrefab;
    public Vector2 lookDirection;

    private Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        lookDirection = new Vector2(-1, 0);
    }

    public void Launch() {
        GameObject arrowObject = Instantiate(arrowPrefab, rigidbody2d.position + lookDirection, Quaternion.identity);

        Arrow arrow = arrowObject.GetComponent<Arrow>();
        arrow.Launch(lookDirection);
    }
}
