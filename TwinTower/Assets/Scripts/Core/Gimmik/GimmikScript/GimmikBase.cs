using System;
using UnityEngine;

namespace TwinTower
{
    public class GimmikBase : MonoBehaviour
    {
        public MapType type;
        public int x;
        public int y;

        public GimmikBase linkedObject;

        // wall, door 등 이동불가능한 Gimmik인 경우 override하여 false로 설정해야 함. - 손창하
        public virtual bool IsWalkable { get; set; } = true;
        
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