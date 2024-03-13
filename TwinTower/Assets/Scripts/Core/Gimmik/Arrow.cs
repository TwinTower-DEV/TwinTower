using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 생성된 화살을 발사시키는 스크립트
/// </summary>
public class Arrow : MonoBehaviour {
    private Rigidbody2D rigidbody2d;

    [SerializeField] private float force;
    void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction) {
        rigidbody2d.AddForce(direction * force * Time.deltaTime);
    }

    // 어차피 무조건 부딪히게 되어있음 따로 피격 안해도 될듯(사용자가 못나가게 벽 만들거니까)
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            Box box = other.GetComponent<Box>(); 
            if(box != null) box.ReduceHealth();
        }
        // if (other.gameObject.layer == LayerMask.NameToLayer("Player"))  플레이어 체력 감소 코드 필요.
        Destroy(gameObject);        // 
    }
}
