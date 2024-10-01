using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TwinTower
{
    /// <summary>
    /// InputManager 클래스입니다 사용자의 입력을 관리합니다.
    /// 클리어 시간 측정하는거면 Time.timeScale써야 함
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public Vector3 moveDir = Vector3.zero;
        [SerializeField]private int count;
        private int[] stagecount = new[] { 19, 39, 24, 30, 30, 17, 12, 38, 22, 30, 29, 9, 17, 29 };

        private void GroundedHorizontalMovement() 
        {
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
                GameManager.Instance.Restart();
            }
            else {
                moveDir = Vector3.zero;
                PlayerMove(Define.MoveDir.None);
                return;
            }

            if (GameManager.Instance._player1.MoveCheck(moveDir) && GameManager.Instance._player2.MoveCheck(moveDir)) 
            {
                count--;
                GameManager.Instance.UI_UpdateCount(count);
                if (count <= 0)
                {
                    InputController.Instance.ReleaseControl();
                    StartCoroutine(OverCount());
                }
                MoveFlag = true;
            }
        }

        IEnumerator OverCount()
        {
            yield return new WaitForSeconds(1.5f);

            if (!GameManager.Instance.isClearCheck)
            {
                GameManager.Instance.Restart();
            }
        }
        
        private void PlayerMove(Define.MoveDir dir) {
            if(GameManager.Instance._player1.Dir != Define.MoveDir.Die)
                GameManager.Instance._player1.Dir = dir;
            
            if(GameManager.Instance._player2.Dir != Define.MoveDir.Die)
                GameManager.Instance._player2.Dir = dir;
        }

        private void Update() {
            GroundedHorizontalMovement();
        }

        IEnumerator BlockMotion(Vector3 movedir) {
            Collider2D collider2dP1 = GameManager.Instance._player1.GetComponent<Collider2D>();
            Collider2D collider2dP2 = GameManager.Instance._player2.GetComponent<Collider2D>();
            collider2dP1.enabled = false;
            collider2dP2.enabled = false;
            
            InputController.Instance.ReleaseControl();

            Transform transformP1 = GameManager.Instance._player1.GetComponent<Transform>();
            Transform transformP2 = GameManager.Instance._player2.GetComponent<Transform>();
            
            Vector3 originalPos = transformP1.position;
            Vector3 originalPosP2 = transformP2.position;
            Vector3 destPos = transformP1.position + movedir * 0.5f;
            Vector3 leftLength = destPos - transformP1.position;
            float dist = leftLength.magnitude;

            float moveSpeed = GameManager.Instance._player1._moveSpeed;
            
            while (dist >= moveSpeed * Time.deltaTime) {
                transformP1.position += movedir * moveSpeed * Time.deltaTime;
                transformP2.position += movedir * moveSpeed * Time.deltaTime;
                leftLength = destPos - transformP1.position;
                dist = leftLength.magnitude;

                yield return new WaitForFixedUpdate();
            }

            destPos = originalPos;
            leftLength = destPos - transformP1.position;
            dist = leftLength.magnitude;
            
            while (dist >= moveSpeed * Time.deltaTime) {
                transformP1.position += (-1f * movedir) * moveSpeed * Time.deltaTime;
                transformP2.position += (-1f * movedir) * moveSpeed * Time.deltaTime;
                leftLength = destPos - transformP1.position;
                dist = leftLength.magnitude;
                
                yield return new WaitForFixedUpdate();
            }
            
            transformP1.position = TileFindManager.Instance.gettileCentorLocation(destPos);
            transformP2.position = TileFindManager.Instance.gettileCentorLocation(originalPosP2);
            
            collider2dP1.enabled = true;
            collider2dP2.enabled = true;
            
            InputController.Instance.GainControl();
        }
    }
}