using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TwinTower
{
    /// <summary>
    /// InputManager 클래스입니다 사용자의 입력을 관리합니다.
    /// 클리어 시간 측정하는거면 Time.timeScale써야 함
    /// </summary>
    public class InputManager : Manager<InputManager> {
        [SerializeField]private int count;
        private int[] stagecount = new[] { 19, 39, 24, 30, 30, 17, 12, 38, 22, 30, 29, 9, 17, 29 };

        public bool islockMove = false;
        private bool MoveFlag = false;

        public override void Init()
        {
            
        }
        
        private void GroundedHorizontalMovement() {
           
            if (InputController.Instance.LeftMove.Down) {
                Move(Define.MoveDir.Left);
            }
            else if (InputController.Instance.RightMove.Down) {
                Move(Define.MoveDir.Right);
            }
            else if (InputController.Instance.UpMove.Down) {
                Move(Define.MoveDir.Up);
            }
            else if (InputController.Instance.DownMove.Down) {
                Move(Define.MoveDir.Down);
            }
            else if (InputController.Instance.ResetButton.Down) {
                GameManager.Instance.Restart();
            }
            else {
                return;
            }

            // if (GameManager.Instance._player1.MoveCheck(moveDir) && GameManager.Instance._player2.MoveCheck(moveDir)) {
                
                
            //     GameManager.Instance._player1.DirectSetting(moveDir, false);
            //     GameManager.Instance._player2.DirectSetting(moveDir, false);

            //     count--;
            //     GameManager.Instance.UI_UpdateCount(count);
            //     if (count <= 0)
            //     {
            //         InputController.Instance.ReleaseControl();
            //         StartCoroutine(OverCount());
            //     }
            // }

            // if (moveDir != Vector3.zero && MoveFlag == false) {         //  막혔을 경우
            //     StartCoroutine(BlockMotion(moveDir));
            // }

            MoveFlag = false;
        }

        IEnumerator OverCount()
        {
            yield return new WaitForSeconds(1.5f);

            if (!GameManager.Instance.isClearCheck)
            {
                GameManager.Instance.Restart();
            }
        }
        
        private async void Move(Define.MoveDir dir) 
        {
            islockMove = true;
            Define.MoveDir player1Dir = GameManager.Instance._player1.map.GetCurrentDir(dir);
            Define.MoveDir player2Dir = GameManager.Instance._player2.map.GetCurrentDir(dir);
            
            bool canMove = GameManager.Instance._player1.CanMoveTile(player1Dir) && GameManager.Instance._player2.CanMoveTile(player2Dir);
            await UniTask.WhenAll(
                GameManager.Instance._player1.OnReciveMove(player1Dir, canMove),
                GameManager.Instance._player2.OnReciveMove(player2Dir, canMove)
            );
            islockMove = false;
        }

        private void Update() {
            if (!islockMove) GroundedHorizontalMovement();
        }

        public void UpDateCount()
        {
            count = stagecount[SceneManager.GetActiveScene().buildIndex - 1];
        }

        public int GetCount()
        {
            return count;
        }
        
        }
    
}