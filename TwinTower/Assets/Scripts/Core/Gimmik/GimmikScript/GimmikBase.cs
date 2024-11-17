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

        protected Map map;

        public bool isWalkable = true;
        public bool isActivateByPlayer = true;
        
        private void Awake() 
        {
            map = GetComponentInParent<Map>();
        }

        public void OnActive()
        {
            if (isActivateByPlayer == true)
            {
                Active();
            }
        }
        
        public virtual void Active()
        {
            LinkTile();
        }

        public void OnDeactive()
        {
            if (isActivateByPlayer == true)
            {
                DeActive();
            }
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