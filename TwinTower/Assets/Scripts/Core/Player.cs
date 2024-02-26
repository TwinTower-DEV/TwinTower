using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace TwinTower
{
    public class Player: MonoBehaviour
    {
        [SerializeField] private int _moveSpeed;
        [SerializeField] private Grid maps;
        private Rigidbody2D rigid;
        [SerializeField]private int currx = 0, curry = 0;
        public Vector3 pos;
        public Define.MoveDir moveDir = Define.MoveDir.None;
        private bool isMove = false;
        private Vector3Int cellPos = Vector3Int.zero;
        
        public void MoveToTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, _moveSpeed * Time.deltaTime);
        }
        // 키 입력을 통해 위, 아래, 왼쪽, 오른쪽 State를 정의함 
        private void GroundedHorizontalMovement()
        {
            if (InputController.Instance.LeftMove.Down)
            {
                moveDir = Define.MoveDir.Left;
            }
            else if (InputController.Instance.RightMove.Down)
            {
                moveDir = Define.MoveDir.Right;
            }
            else if (InputController.Instance.UpMove.Down)
            {
                moveDir = Define.MoveDir.Up;
            }
            else if (InputController.Instance.DownMove.Down)
            {
                moveDir = Define.MoveDir.Down;
            }
            else
            {
                moveDir = Define.MoveDir.None;
            }
        }

        private void Move()
        {
            if (!isMove)
            {
                Vector3Int dest = cellPos;
                Vector3 check = Vector3.zero;
                switch (moveDir)
                {
                    case Define.MoveDir.Up:
                        dest += Vector3Int.up;
                        check = Vector3.forward;
                        break;
                    case Define.MoveDir.Left:
                        dest += Vector3Int.left;
                        check = Vector3.left;
                        break;
                    case Define.MoveDir.Right:
                        dest += Vector3Int.right;
                        check = Vector3.right;
                        break;
                    case Define.MoveDir.Down:
                        dest += Vector3Int.down;
                        check = Vector3.down;
                        break;
                }

                if (moveDir != Define.MoveDir.None)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, check, 1.0f, LayerMask.GetMask("Wall"));
                    if (hit.collider == null)
                    {
                        cellPos = dest;
                        isMove = true;
                    }
                }
            }
        }

        private void UpdateIsMoveing()
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
            Vector3 pos = maps.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);
            transform.position = pos;
        }

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            Debug.Log(isMove);
            UpdateIsMoveing();
            Move();
            GroundedHorizontalMovement();
            
        }
}
    }