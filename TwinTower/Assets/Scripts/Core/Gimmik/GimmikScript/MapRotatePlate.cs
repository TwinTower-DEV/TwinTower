using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

/// <summary>
/// 맵 회전 기믹을 정의한 클래스입니다.
/// </summary>
namespace TwinTower
{
    public class MapRotatePlate: GimmikBase {
        public Map rotateMap;                         // 회전할 Tilemap

        public async override UniTask Active(MoveControl subject = null) 
        {
            await rotateMap.Rotate();
        }

        public override void DeActive(MoveControl subject = null) 
        {
            
        }
    }
}