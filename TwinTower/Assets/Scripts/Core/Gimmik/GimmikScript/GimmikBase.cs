using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        public virtual void Rotate()
        {
            // 부모가 회전하고 있기 때문에 자식은 자기 자신의 원래 회전값으로 돌아가도록 회전해야함.
            transform.DORotate(new Vector3(0, 0, transform.rotation.z), 1);
        }
    }
}