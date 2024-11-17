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

    public override void Active(MoveControl subject = null)
    {
        Moving(subject);
    }

    private async void Moving(MoveControl subject)
    {
        await subject.OnReciveMove(dir, true);
    }
}
