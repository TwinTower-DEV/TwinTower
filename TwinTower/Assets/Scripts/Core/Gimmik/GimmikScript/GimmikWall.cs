using System;
using UnityEngine;

namespace TwinTower
{
    public class GimmikWall : GimmikBase
    {
        public override bool IsWalkable { get; set; } = false;
        
        public virtual void Active()
        {
            LinkTile();
        }

        public virtual void DeActive()
        {

        }

        public virtual void LinkTile()
        {
            linkedObject?.Active();
        }
    }
}