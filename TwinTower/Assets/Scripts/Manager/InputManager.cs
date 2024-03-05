using System;
using UnityEngine;

namespace TwinTower
{
    public class InputManager : Manager<InputManager> {
        public Vector3 moveDir = Vector3.zero;
        
        public bool islockMove = false;
        private void GroundedHorizontalMovement()
        {
            if (InputController.Instance.LeftMove.Down) {
                moveDir = Vector3.left;
            }
            else if (InputController.Instance.RightMove.Down)
            {
                moveDir = Vector3.right;
            }
            else if (InputController.Instance.UpMove.Down)
            {
                moveDir = Vector3.up;
            }
            else if (InputController.Instance.DownMove.Down)
            {
                moveDir = Vector3.down;
            }
            else
            {
                moveDir = Vector3.zero;
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