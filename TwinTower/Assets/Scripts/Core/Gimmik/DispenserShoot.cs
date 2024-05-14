using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;


/// <summary>
/// 함정 발동 발판을 밟을 시 화살이 나오는 구멍이다.
/// 이곳을 통해 화살이 발사된다.
/// </summary>
/// 
/*[System.Serializable]
public class DirectInfo {
    public Sprite dispenserSprite;
    public float angle;
}*/
public class DispenserShoot : ActivateObject {
    // [SerializeField] public DirectInfo[] directInfos;
    /*private SpriteRenderer spriterenderer;
    public int currDirIndex;*/

    private Rigidbody2D rigidbody2D;
    private Collider2D collider2d;
    [SerializeField] private float force = 90000; 

    void Start() {
        // spriterenderer = GetComponent<SpriteRenderer>();
        /*for (int i = 0; i < 4; i++) {
            if (transform.rotation.eulerAngles.z == directInfos[i].angle) currDirIndex = i;
        }*/
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    public override void Launch() {
        collider2d.enabled = true;
        rigidbody2D.AddForce(transform.up * force * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        MoveControl control = other.GetComponent<MoveControl>();

        if (control != null) {
            control.ReduceHealth();
            Destroy(gameObject);
        }
        DispenserShoot shooter = other.GetComponent<DispenserShoot>();
        if(shooter == null && other.gameObject.layer == LayerMask.NameToLayer("Wall")) Destroy(gameObject); // 화살대는 무시
        
    }

    // 회전 45도가 되었을때 스프라이트 변경을 위한 public 함수
    /*public void NextDirection() {
        currDirIndex = (currDirIndex + 1) % 4;
        spriterenderer.sprite = directInfos[currDirIndex].dispenserSprite;
    }*/
    
    /*public void prevDirection() {
        currDirIndex = (currDirIndex + 3) % 4;
        spriterenderer.sprite = directInfos[currDirIndex].dispenserSprite;
    }*/

    /*public Sprite GetSpriteOfDegree(float _angle) {
        for (int i = 0; i < 4; i++) {
            if (_angle == directInfos[i].angle) {
                currDirIndex = i;
                return directInfos[i].dispenserSprite;
            }
        }

        return null;
    }*/
    
}