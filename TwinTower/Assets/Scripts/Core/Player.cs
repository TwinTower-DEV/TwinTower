using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// 플레이어 이동과 플레이어의 각종 이벤트 처리를 담당할 클래스입니다.
/// </summary>

namespace TwinTower
{
    public class Player: MoveControl
    {
        private void Awake()
        {
            //Vector3 pos = maps.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);
            //transform.position = pos;
        }

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            
        }
}
    }