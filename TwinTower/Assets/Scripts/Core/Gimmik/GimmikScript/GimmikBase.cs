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

        public void OnActive(MoveControl subject)
        {
            if (isActivateByPlayer == true)
            {
                Active(subject);
            }
        }
        
        public virtual void Active(MoveControl subject = null)
        {
            LinkTile();
        }

        public void OnDeactive(MoveControl subject)
        {
            if (isActivateByPlayer == true)
            {
                DeActive(subject);
            }
        }

        public virtual void DeActive(MoveControl subject = null)
        {

        }

        public virtual void LinkTile()
        {
            linkedObject?.Active();
        }
    }
}