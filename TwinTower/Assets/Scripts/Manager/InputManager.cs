using System;
using UnityEngine;

namespace TwinTower
{
    /// <summary>
    /// InputManager 클래스입니다 사용자의 입력을 관리합니다.
    /// </summary>
    public class InputManager : Manager<InputManager> {
        public Vector3 moveDir = Vector3.zero;
        
        public bool islockMove = false;
        private void GroundedHorizontalMovement()
        {
            if (GameManager.Instance._player1.getIsMove() || GameManager.Instance._player2.getIsMove()) return;
            
            if (InputController.Instance.LeftMove.Down) {
                moveDir = Vector3.left;
                PlayerMove(Define.MoveDir.Left);
            }
            else if (InputController.Instance.RightMove.Down)
            {
                moveDir = Vector3.right;
                PlayerMove(Define.MoveDir.Right);
            }
            else if (InputController.Instance.UpMove.Down)
            {
                moveDir = Vector3.up;
                PlayerMove(Define.MoveDir.Up);
            }
            else if (InputController.Instance.DownMove.Down)
            {
                moveDir = Vector3.down;
                PlayerMove(Define.MoveDir.Down);
            }
            else if (InputController.Instance.ResetButton.Down)
            {
                
                StartCoroutine(ScreenManager.Instance.CurrentScreenReload());
                
            }
            else
            {
                moveDir = Vector3.zero;
                PlayerMove(Define.MoveDir.None);
                return;
            }

            if (GameManager.Instance._player1.MoveCheck(moveDir) && GameManager.Instance._player2.MoveCheck(moveDir)) {
                GameManager.Instance._player1.DirectSetting(moveDir);
                GameManager.Instance._player2.DirectSetting(moveDir);
            }
        }

        private void PlayerMove(Define.MoveDir dir)
        {
            GameManager.Instance._player1.Dir = dir;
            GameManager.Instance._player2.Dir = dir;
        }
        private void FixedUpdate()
        {
            if(!islockMove)
                GroundedHorizontalMovement();
        }
    }
}