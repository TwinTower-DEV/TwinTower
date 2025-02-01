using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TwinTower;
using UnityEngine;

/// <summary>
/// 함정 발동 발판을 밟을 시 화살이 나오는 구멍이다.
/// 이곳을 통해 화살이 발사된다.
/// </summary>
/// 
public class GimmikArrow : GimmikBase {
    public Define.MoveDir dir;
    public override async UniTask Active(MoveControl subject = null) {
        await ShootArrow();
    }

    private async UniTask ShootArrow()
    {
        (int nextX, int nextY) = map.GetCoordinates(dir, x, y);

        while (map.IsInMap(nextX, nextY) == true)
        {
            Vector2 target = map.GetTilePosition(nextX, nextY);

            await transform.DOLocalMove(target, 0.1f).ToUniTask();

            MoveControl nextMovedObject = map.GetMovedObject(nextX, nextY);

            if (nextMovedObject != null)
            {
                transform.gameObject.SetActive(false);
                await nextMovedObject.GetDamage(1);
                break;
            }
            
            (nextX, nextY) = map.GetCoordinates(dir, nextX, nextY);
        }
    }
}