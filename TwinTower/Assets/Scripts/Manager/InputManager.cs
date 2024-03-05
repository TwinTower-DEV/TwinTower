using System;
using UnityEngine;

namespace TwinTower
{
    public class InputManager : Manager<InputManager> {
        public Vector3 moveDir = Vector3.zero;
        
        public bool islockMove = false;
        private void GroundedHorizontalMovement()
        {
            if (GameManager.Instance._player1.isMove || GameManager.Instance._player2.isMove) return;
            
            if (InputController.Instance.LeftMove.Down) {
                moveDir = Vector3.left;
                GameManager.Instance._player1.Dir = Define.MoveDir.Left;
                GameManager.Instance._player2.Dir = Define.MoveDir.Left;
            }
            else if (InputController.Instance.RightMove.Down)
            {
                moveDir = Vector3.right;
                GameManager.Instance._player1.Dir = Define.MoveDir.Right;
                GameManager.Instance._player2.Dir = Define.MoveDir.Right;
            }
            else if (InputController.Instance.UpMove.Down)
            {
                moveDir = Vector3.up;
                GameManager.Instance._player1.Dir = Define.MoveDir.Up;
                GameManager.Instance._player2.Dir = Define.MoveDir.Up;
            }
            else if (InputController.Instance.DownMove.Down)
            {
                moveDir = Vector3.down;
                GameManager.Instance._player1.Dir = Define.MoveDir.Down;
                GameManager.Instance._player2.Dir = Define.MoveDir.Down;
            }
            else
            {
                moveDir = Vector3.zero;
                GameManager.Instance._player1.Dir = Define.MoveDir.None;
                GameManager.Instance._player2.Dir = Define.MoveDir.None;
                return;
            }

            if (GameManager.Instance._player1.MoveCheck(moveDir) && GameManager.Instance._player2.MoveCheck(moveDir)) {
                GameManager.Instance._player1.DirectSetting(moveDir);
                GameManager.Instance._player2.DirectSetting(moveDir);
            }
        }

        private void FixedUpdate()
        {
            if(!islockMove)
                GroundedHorizontalMovement();
        }
    }
}