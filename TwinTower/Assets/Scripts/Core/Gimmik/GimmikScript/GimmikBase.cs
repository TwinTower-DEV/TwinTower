using System;
using Cysharp.Threading.Tasks;
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

        public async UniTask OnActive(MoveControl subject)
        {
            if (isActivateByPlayer == true)
            {
                        Debug.LogError($"Active: {x}, {y}: {map.GetGimmik(x, y)?.GetType()}");
                await Active(subject);
            }
        }
        
        public async virtual UniTask Active(MoveControl subject = null)
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