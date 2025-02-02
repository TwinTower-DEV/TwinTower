using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TwinTower
{
    public class GimmikTeleport : GimmikBase
    {
        public async override UniTask Active(MoveControl subject = null) 
        {
            subject.Teleport(linkedObject.x, linkedObject.y);
        }
    }
}