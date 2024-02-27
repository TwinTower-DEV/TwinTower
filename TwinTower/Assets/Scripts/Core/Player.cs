using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// 플레이어 이동과 플레이어의 각종 이벤트 처리를 담당할 클래스입니다.
/// </summary>

namespace TwinTower
{
    public class Player: MonoBehaviour
    {
        [SerializeField] private int _moveSpeed;
        [SerializeField] private Grid maps;
        public Vector3 pos;
        public Define.MoveDir moveDir = Define.MoveDir.None;
        private bool isMove = false;
        [SerializeField]private Vector3Int cellPos = Vector3Int.zero;
        
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
        // 다음 이동할 셀을 지정해줌
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
                        check = Vector3.up;
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
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, check, 0.8f, LayerMask.GetMask("Wall"));
                    if (hit.collider == null)
                    {
                        cellPos = dest;
                        isMove = true;
                    }
                }
            }
        }
        // 그 이동할 셀로 자연스럽게 이동하게 구현
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
            //Vector3 pos = maps.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);
            //transform.position = pos;
        }

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            UpdateIsMoveing();
            Move();
            GroundedHorizontalMovement();
            
        }
}
    }