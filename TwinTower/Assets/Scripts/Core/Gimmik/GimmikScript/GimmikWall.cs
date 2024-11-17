using System;
using UnityEngine;

namespace TwinTower
{
    public class GimmikWall : GimmikBase
    {
        // GimmikWall은 Map에서 직접 생성해주고 있음. 따라서 생성자 이용하여 isWalkable 덮어쓰도록 구현함. Awake는 버그인지 동작 안해서 생성자로 바꿈
        public GimmikWall()
        {
            isWalkable = false;
        }
    }
}