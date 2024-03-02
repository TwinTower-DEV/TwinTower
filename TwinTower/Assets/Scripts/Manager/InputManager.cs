using System;
using UnityEngine;

namespace TwinTower
{
    public class InputManager : Manager<InputManager>
    {
        public Define.MoveDir moveDir = Define.MoveDir.None;
        
        public bool isSyncMove = false;
        public bool islockMove = false;
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
                return;
            }

            if (GameManager.Instance._player1.MoveCheck(moveDir) && GameManager.Instance._player2.MoveCheck(moveDir))
            {
                isSyncMove = true;
                GameManager.Instance._player1.DirectSetting(moveDir);
                GameManager.Instance._player2.DirectSetting(moveDir);
            }
            else
            {
                isSyncMove = false;
            }
        }

        private void FixedUpdate()
        {
            if(!islockMove)
                GroundedHorizontalMovement();
        }
    }
}