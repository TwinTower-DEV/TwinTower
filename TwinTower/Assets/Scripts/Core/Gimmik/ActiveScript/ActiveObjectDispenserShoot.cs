using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;

/// <summary>
/// 함정 발동 발판을 밟을 시 화살이 나오는 구멍이다.
/// 이곳을 통해 화살이 발사된다.
/// </summary>
/// 
public class ActivateObjectDispenserShoot : ActivateObject {
    public GameObject Arrow;

    public override void Launch() {
        ShootArrow();
    }

    private void ShootArrow()
    {
        // 화살 발사 작업 필요
    }
}