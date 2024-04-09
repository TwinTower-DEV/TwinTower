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

        protected override void Awake()
        {
            UIManager.Instance.ShowNormalUI<UI_FieldScene>();
            FindPlayer();
        }
        public void FindPlayer() {
            if (GameObject.Find("Dalia").GetComponent<Player>() != null)
            {
                _player1 = GameObject.Find("Dalia").GetComponent<Player>();
                InputController.Instance.GainControl();
            }

            if(GameObject.Find("Irise").GetComponent<Player>() != null)
                _player2 = GameObject.Find("Irise").GetComponent<Player>();
        }
    }
}