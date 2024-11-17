using System;
using UnityEngine;

namespace TwinTower
{
    public class GimmikWall : GimmikBase
    {
        private void Awake() {
            isWalkable = false;
        }
    }
}