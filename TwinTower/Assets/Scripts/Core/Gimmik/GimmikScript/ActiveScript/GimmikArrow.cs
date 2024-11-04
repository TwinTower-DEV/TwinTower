using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;

/// <summary>
/// 함정 발동 발판을 밟을 시 화살이 나오는 구멍이다.
/// 이곳을 통해 화살이 발사된다.
/// </summary>
/// 
public class GimmikArrow : GimmikBase {
    public Define.MoveDir dir;
    public override void Active() {
        ShootArrow();
    }

    private void ShootArrow()
    {
        Map map = ManagerSet.Gamemanager.GetMap(type);
    }
}