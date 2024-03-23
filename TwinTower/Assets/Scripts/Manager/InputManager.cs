using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace TwinTower
{
    /// <summary>
    /// InputManager 클래스입니다 사용자의 입력을 관리합니다.
    /// 클리어 시간 측정하는거면 Time.timeScale써야 함
    /// </summary>
    public class InputManager : Manager<InputManager> {
        public Vector3 moveDir = Vector3.zero;
        private bool isESC;

        public bool islockMove = false;

        private void GroundedHorizontalMovement() {
            if (GameManager.Instance._player1.getIsMove() || GameManager.Instance._player2.getIsMove()) return;

            if (InputController.Instance.LeftMove.Down) {
                moveDir = Vector3.left;
                PlayerMove(Define.MoveDir.Left);
            }
            else if (InputController.Instance.RightMove.Down) {
                moveDir = Vector3.right;
                PlayerMove(Define.MoveDir.Right);
            }
            else if (InputController.Instance.UpMove.Down) {
                moveDir = Vector3.up;
                PlayerMove(Define.MoveDir.Up);
            }
            else if (InputController.Instance.DownMove.Down) {
                moveDir = Vector3.down;
                PlayerMove(Define.MoveDir.Down);
            }
            else if (InputController.Instance.ResetButton.Down) {
                StartCoroutine(ScreenManager.Instance.CurrentScreenReload());
            }
            else {
                moveDir = Vector3.zero;
                PlayerMove(Define.MoveDir.None);
                return;
            }

            if (GameManager.Instance._player1.MoveCheck(moveDir) && GameManager.Instance._player2.MoveCheck(moveDir)) {
                GameManager.Instance._player1.DirectSetting(moveDir);
                GameManager.Instance._player2.DirectSetting(moveDir);
            }
        }

        private void PlayerMove(Define.MoveDir dir) {
            GameManager.Instance._player1.Dir = dir;
            GameManager.Instance._player2.Dir = dir;
        }

        private void FixedUpdate() {
            if (!islockMove) GroundedHorizontalMovement();
        }

        private void Update() {
            if (InputController.Instance.EscapeButton.Down) { // ESC
                if (!isESC) {
                    InputController.Instance.ReleaseControl();
                    InputController.Instance.EscapeButton.GainControl();
                    InputController.Instance.EnterButton.GainControl();
                    isESC = true;
                    Time.timeScale = 0;
                    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("UIMenus", UnityEngine.SceneManagement.LoadSceneMode.Additive);
                }
            }
        }
        
        public void UnPause() {
            if (Time.timeScale > 0)
                return;

            StartCoroutine(UnPauseCoroutine());
        }

        private IEnumerator UnPauseCoroutine() {
                Time.timeScale = 1;
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("UIMenus");
                InputController.Instance.GainControl();
                yield return new WaitForFixedUpdate();
                yield return new WaitForEndOfFrame();
                isESC = false;
            }
        }
}