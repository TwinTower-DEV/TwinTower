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
    // Start is called before the first frame update
    void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction) {
        rigidbody2d.AddForce(direction * force * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Box box = other.GetComponent<Box>();            // 상자에 맞을 경우 체력 피격.
        if (box != null) {
            box.ReduceHealth();
        }
        Destroy(gameObject);
    }
}
