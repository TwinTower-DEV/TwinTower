using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 함정 발동 발판을 밟을 시 화살이 나오는 구멍이다.
/// 이곳을 통해 화살이 발사된다.
/// </summary>
/// 
[System.Serializable]
public class DirectInfo {
    public Sprite dispenserSprite;
    public float angle;
}
public class DispenserShoot : ActivateObject {
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private DirectInfo[] directInfos;
    private SpriteRenderer spriterenderer;
    public int currDirIndex;

    private bool haveArrow;                     // 한번만 발사
    void Start() {
        haveArrow = true;
        spriterenderer = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 4; i++) {
            if (transform.rotation.eulerAngles.z == directInfos[i].angle) currDirIndex = i;
        }
    }

    public override void Launch() {
        if (!haveArrow) return;             // 이미 발사 된 경우 종료
        float Angle = Vector3.SignedAngle(Vector3.left, transform.up, Vector3.forward);
        GameObject arrowObject = Instantiate(arrowPrefab, transform.position + transform.up * 0.3f, 
            Quaternion.Euler(0, 0, Angle), transform);
		
        Arrow arrow = arrowObject.GetComponent<Arrow>();
        arrow.Launch(transform.up);
        haveArrow = false;
    }

    // 회전 45도가 되었을때 스프라이트 변경을 위한 public 함수
    public void NextDirection() {
        currDirIndex = (currDirIndex + 1) % 4;
        spriterenderer.sprite = directInfos[currDirIndex].dispenserSprite;
    }
    
    public void prevDirection() {
        currDirIndex = (currDirIndex + 3) % 4;
        spriterenderer.sprite = directInfos[currDirIndex].dispenserSprite;
    }

    public Sprite GetSpriteOfDegree(float _angle) {
        for (int i = 0; i < 4; i++) {
            if (_angle == directInfos[i].angle) {
                currDirIndex = i;
                return directInfos[i].dispenserSprite;
            }
        }

        return null;
    }
    
}