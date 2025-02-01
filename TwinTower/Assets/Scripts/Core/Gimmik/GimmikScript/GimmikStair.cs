using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TwinTower;
using UnityEngine;

/// <summary>
/// 계단, 플레이어가 동시에 진입 시 다음 단계 진입.
/// OnPlayer로 플레이어가 계단 위에 있을 경우 다른 계단 확인 후 다음 단계 진입.
/// Manager 이용.
/// 보툥 계단 2개가 함께 이 스크립트를 저장함. 서로 Stair로 연결되어 있음.
/// </summary>
namespace TwinTower{

    public class GimmikStair : GimmikBase {

        public async override UniTask Active(MoveControl subject = null) 
        {
            await GameManager.Instance.ActiveStair();
        }

        public override void DeActive(MoveControl subject = null) 
        {
            GameManager.Instance.DeacitveStair();
        }
    }

}