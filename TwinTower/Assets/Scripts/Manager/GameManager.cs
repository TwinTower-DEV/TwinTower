using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwinTower
{
    /// <summary>
    /// GameManager 클래스입니다. 게임의 전반적인 진행을 관리합니다.
    /// </summary>
    public class GameManager : Manager<GameManager>
    {
        private TileFindManager _tileFindManager;
        public Player _player1;
        public Player _player2;

        public List<MoveControl> _moveobjlist = new List<MoveControl>();
        protected override void Awake()
        {
            _tileFindManager = TileFindManager.Instance;
            FindPlayer();
        }

        public void FindPlayer() {
            _player1 = GameObject.Find("Dalia").GetComponent<Player>();
            _player2 = GameObject.Find("Irise").GetComponent<Player>();
        }
    }
}