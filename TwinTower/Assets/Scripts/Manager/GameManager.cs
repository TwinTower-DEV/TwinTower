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
    public class GameManager : MonoBehaviour {
        public static GameManager Instance;
        public Player _player1;
        public Player _player2;

        private void Awake() {
            Instance = this;
            _player1 = GameObject.Find("Dalia").GetComponent<Player>();
            _player2 = GameObject.Find("Irise").GetComponent<Player>();
        }
    }
}