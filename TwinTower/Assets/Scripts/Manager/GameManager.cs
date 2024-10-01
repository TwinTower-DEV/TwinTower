using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TwinTower
{
    /// <summary>
    /// GameManager 클래스입니다. 게임의 전반적인 진행을 관리합니다.
    /// 이거 OnLoad에 안되게 할거면 Player 직접 배치로 해도 괜찮을듯?
    /// </summary>
    public class GameManager : Manager<GameManager>
    {
        public Player _player1;
        public Player _player2;
        public bool isClearCheck = false;
        public bool isRotateCheck = false;
        protected override void Awake()
        {
            isClearCheck = false;
        }

        public void MoveStage() 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void Restart()
        {
            _player1.Dir = Define.MoveDir.Die;
            _player2.Dir = Define.MoveDir.Die;
            InputController.Instance.ReleaseControl();
            StartCoroutine(ScreenManager.Instance.CurrentScreenReload());
        }
    }
}