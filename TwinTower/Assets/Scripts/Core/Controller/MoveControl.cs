using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace TwinTower
{
    public class MoveControl : MonoBehaviour
    {
        [SerializeField] protected int _moveSpeed;
        [SerializeField] protected Grid maps;
        [SerializeField] private Vector3Int cellPos = Vector3Int.zero;
        [SerializeField] protected LayerMask _layerMask;
        public bool isMove = false;
        // 다음 이동할 셀을 지정해줌

        public void SetSpwnPoint(Vector3Int pos)
        {
            cellPos = pos;
        }

        public void DirectSetting(Vector3 movedir) {
            cellPos += Vector3Int.RoundToInt(movedir);
            isMove = true;
        }
        
        public virtual bool MoveCheck(Vector3 movedir) {
            if (isMove) return false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + movedir * 0.5f , movedir, 0.5f, _layerMask);
            if (hit.collider == null) return true;
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Box")) {
                MoveControl boxcontrol = hit.transform.gameObject.GetComponent<MoveControl>();
                if (boxcontrol.MoveCheck(movedir)) {
                    boxcontrol.DirectSetting(movedir);
                    return true;
                }
                return false;
            }
            return false;
        }

        /*protected Vector3 MDRToVec3(Define.MoveDir movedir) {
            switch (movedir)
            {
                case Define.MoveDir.Up:
                    return Vector3.up;
                case Define.MoveDir.Left:
                    return Vector3.left;
                case Define.MoveDir.Right:
                    return Vector3.right;
                case Define.MoveDir.Down:
                    return Vector3.down;
            }
            return Vector3.zero;
        }*/
        
        // 그 이동할 셀로 자연스럽게 이동하게 구현
        private void UpdateIsMoveing()
        {
            Vector3 destPos = maps.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);
            Vector3 moveDir = destPos - transform.position;
            
            float dist = moveDir.magnitude;
            if (dist < _moveSpeed * Time.deltaTime)
            {
                transform.position = destPos;
                isMove = false;
            }
            else
            {
                transform.position += moveDir.normalized * _moveSpeed * Time.deltaTime;
                isMove = true;
            }
        }

        protected void FixedUpdate()
        {
            if(isMove) UpdateIsMoveing();
        }
    }
}