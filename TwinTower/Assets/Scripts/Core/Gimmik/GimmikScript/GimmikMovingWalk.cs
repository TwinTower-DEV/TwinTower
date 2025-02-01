using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TwinTower;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GimmikMovingWalk : GimmikBase 
{
    public Define.MoveDir dir;

    public async override UniTask Active(MoveControl subject = null)
    {
        await Moving(subject);
    }

    public async UniTask Moving(MoveControl subject)
    {
        (int nextX, int nextY) = map.GetCoordinates(dir, x, y);
        if (map.CanMove(nextX, nextY) == true && map.GetMovedObject(nextX, nextY) == null)
        {
            await subject.OnReciveMove(dir, true);
        }
    }
}
