using System;
using System.Collections;
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
        [SerializeField] private Vector3Int player1cellPos = Vector3Int.zero;
        [SerializeField] private Vector3Int player2cellPos = Vector3Int.zero;
        [SerializeField] private GameObject _player1;
        [SerializeField] private GameObject _player2;
        public Vector3 pos;
        public Define.MoveDir moveDir = Define.MoveDir.None;
        private bool isMove = false;
        public bool islockMove = false;
        
        
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
                Vector3Int player1dest = player1cellPos;
                Vector3Int player2dest = player2cellPos;
                Vector3 check = Vector3.zero;
                switch (moveDir)
                {
                    case Define.MoveDir.Up:
                        player1dest += Vector3Int.up;
                        player2dest += Vector3Int.up;
                        check = Vector3.up;
                        break;
                    case Define.MoveDir.Left:
                        player1dest += Vector3Int.left;
                        player2dest += Vector3Int.left;
                        check = Vector3.left;
                        break;
                    case Define.MoveDir.Right:
                        player1dest += Vector3Int.right;
                        player2dest += Vector3Int.right;
                        check = Vector3.right;
                        break;
                    case Define.MoveDir.Down:
                        player1dest += Vector3Int.down;
                        player2dest += Vector3Int.down;
                        check = Vector3.down;
                        break;
                }

                if (moveDir != Define.MoveDir.None)
                {
                    RaycastHit2D player1hit = Physics2D.Raycast(_player1.transform.position, check, 0.8f, LayerMask.GetMask("Wall"));
                    RaycastHit2D player2hit = Physics2D.Raycast(_player2.transform.position, check, 0.8f, LayerMask.GetMask("Wall"));

                    if (player1hit.collider == null && player2hit.collider == null)
                    {
                        player1cellPos = player1dest;
                        player2cellPos = player2dest;
                        isMove = true;
                    }
                }
            }
        }
        // 그 이동할 셀로 자연스럽게 이동하게 구현
        private void UpdateIsMoveing()
        {
            if (!isMove) return;

            Vector3 player1destPos = maps.CellToWorld(player1cellPos) + new Vector3(0.5f, 0.5f);
            Vector3 player2destPos = maps.CellToWorld(player2cellPos) + new Vector3(0.5f, 0.5f); 
            Vector3 player1moveDir = player1destPos - _player1.transform.position;
            Vector3 player2moveDir = player2destPos - _player2.transform.position;
            float player1dist = player1moveDir.magnitude;
            float player2dist = player2moveDir.magnitude;
            if (player1dist < _moveSpeed * Time.deltaTime && player2dist < _moveSpeed * Time.deltaTime)
            {
                _player1.transform.position = player1destPos;
                _player2.transform.position = player2destPos;
                isMove = false;
            }
            else
            {
                _player1.transform.position += player1moveDir.normalized * _moveSpeed * Time.deltaTime;
                _player2.transform.position += player2moveDir.normalized * _moveSpeed * Time.deltaTime;
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
            if (!islockMove)
            {
                GroundedHorizontalMovement();
            }

        }
}
    }