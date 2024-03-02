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
        public bool MoveCheck(Define.MoveDir movedir)
        {
            if(movedir == Define.MoveDir.None) return false;
            Vector3 check = Vector3.zero;
            moveDir = movedir;
            switch (movedir)
            {
                case Define.MoveDir.Up:
                    check = Vector3.up;
                    break;
                case Define.MoveDir.Left:
                    check = Vector3.left;
                    break;
                case Define.MoveDir.Right:
                    check = Vector3.right;
                    break;
                case Define.MoveDir.Down:
                    check = Vector3.down;
                    break;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position + check * 0.5f , check, 0.5f, _layerMask);
            if (hit.collider == null)
            {
                return true;
            }
            else
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Box"))
                {
                    
                    if (hit.transform.gameObject.GetComponent<MoveControl>().MoveCheck(movedir))
                    {
                        return true;
                    }
                    else
                    {
                        moveDir = Define.MoveDir.None;
                        return false;
                    }
                }
            }
            
            moveDir = Define.MoveDir.None;
            return false;
            
        }
        protected void Move()
        {
            if (!isMove)
            {
                switch (moveDir)
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

            moveDir = Define.MoveDir.None;
            isMove = true;
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
        
            
            if (InputManager.Instance.isSyncMove)
            {
                UpdateIsMoveing();
                Move();
                
            }
        }
    }
}