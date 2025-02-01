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
        public Define.MoveControlType activeType;
        
        private void Awake() 
        {
            map = GetComponentInParent<Map>();
        }

        public async UniTask OnActive(MoveControl subject, Define.MoveControlType moveControlType)
        {
            if (IsMoveControlActiveType(moveControlType) == true)
            {
                await Active(subject);
            }
        }
        
        public async virtual UniTask Active(MoveControl subject = null)
        {

        }

        public void OnDeactive(MoveControl subject, Define.MoveControlType moveControlType)
        {
            if (IsMoveControlActiveType(moveControlType) == true)
            {
                DeActive(subject);
            }
        }

        public virtual void DeActive(MoveControl subject = null)
        {

        }

        public bool IsMoveControlActiveType(Define.MoveControlType moveControlType)
        {
            return activeType.HasFlag(moveControlType);
        }
    }
}