using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TwinTower
{
    public class MoveControl : MonoBehaviour
    {
        [SerializeField] protected int _moveSpeed;
        [SerializeField] protected Grid maps;
        [SerializeField] private Vector3Int cellPos = Vector3Int.zero;
        [SerializeField] private LayerMask _layerMask;
        public Define.MoveDir moveDir = Define.MoveDir.None;
        public bool isMove = false;
        [SerializeField] protected bool isMapcheck;
        // 다음 이동할 셀을 지정해줌

        public void SetSpwnPoint(Vector3Int pos)
        {
            cellPos = pos;
        }

        public void SetMoveDir(Define.MoveDir dir)
        {
            moveDir = dir;
        }
        public bool MoveCheck(Define.MoveDir movedir) {
            if (isMove) return false;
            if(movedir == Define.MoveDir.None) return false;
            Vector3 nextDir = MDRToVec3(movedir);
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position + nextDir*0.5f, nextDir, 0.5f, _layerMask);
            if (hit.collider == null)
            {
                return true;
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wall")) return false;
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Box")) {
                MoveControl MoveableObject = hit.transform.gameObject.GetComponent<MoveControl>();
                if(MoveableObject == null) Debug.Log("오류입니당");
                if (MoveableObject.MoveCheck(movedir))
                {
                    MoveableObject.DstIsMDR(movedir);
                    return true;
                }

                return false;
            }
            return false;
        }

        Vector3 MDRToVec3(Define.MoveDir movedir) {
            switch (movedir)
            {
                case Define.MoveDir.Up:
                    return Vector3.up;
                    break;
                case Define.MoveDir.Left:
                    return Vector3.left;
                    break;
                case Define.MoveDir.Right:
                    return Vector3.right;
                    break;
                case Define.MoveDir.Down:
                    return Vector3.down;
                    break;
            }

            return Vector3.zero;
        }

        public void DstIsMDR(Define.MoveDir movedir) {
            isMove = true;
            switch (movedir)
            {
                case Define.MoveDir.Up:
                    cellPos += Vector3Int.up;
                    break;
                case Define.MoveDir.Left:
                    cellPos += Vector3Int.left;
                    break;
                case Define.MoveDir.Right:
                    cellPos += Vector3Int.right;
                    break;
                case Define.MoveDir.Down:
                    cellPos += Vector3Int.down;
                    break;
            }
        }
        
        // 그 이동할 셀로 자연스럽게 이동하게 구현
        protected void UpdateIsMoveing()
        {
            if (!isMove) return;
            
            
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
        private void Awake()
        {
            //Vector3 pos = maps.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);
            //transform.position = pos;
        }

        private void Start()
        {
            
        }

        protected void FixedUpdate()
        {
            UpdateIsMoveing();
        }
    }
}