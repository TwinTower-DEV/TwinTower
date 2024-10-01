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

        public void Init()
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
                ManagerSet.Gamemanager.Restart();
            }
            else {
                return;
            }

            // if (ManagerSet.Gamemanager._player1.MoveCheck(moveDir) && ManagerSet.Gamemanager._player2.MoveCheck(moveDir)) {
                
                
            //     ManagerSet.Gamemanager._player1.DirectSetting(moveDir, false);
            //     ManagerSet.Gamemanager._player2.DirectSetting(moveDir, false);

            //     count--;
            //     ManagerSet.Gamemanager.UI_UpdateCount(count);
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

            if (!ManagerSet.Gamemanager.isClearCheck)
            {
                ManagerSet.Gamemanager.Restart();
            }
        }
        
        private async void Move(Define.MoveDir dir) 
        {
            islockMove = true;
            await UniTask.WhenAll(
                ManagerSet.Gamemanager._player1.OnReciveMove(dir),
                ManagerSet.Gamemanager._player2.OnReciveMove(dir)
            );
            Debug.Log("SetFalse");
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