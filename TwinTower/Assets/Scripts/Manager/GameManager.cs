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
        public MoveControl _player1;
        public MoveControl _player2;
        public Vector3Int _player1spwnPoint;
        public Vector3Int _player2spwnPoint;
        public bool isMovecheck = true;
        protected override void Awake()
        {
            _tileFindManager = TileFindManager.Instance;
            _player1 = GameObject.Find("Player1").GetComponent<MoveControl>();
            _player2 = GameObject.Find("Player2").GetComponent<MoveControl>();
            _player1spwnPoint = new Vector3Int(-8, -1, 0);
            _player2spwnPoint = new Vector3Int(8, -1, 0);
            
            _player1.SetSpwnPoint(_player1spwnPoint);
            _player2.SetSpwnPoint(_player2spwnPoint);
        }

        public void Start()
        {
            
        }

        protected void FixedUpdate()
        {
            
        }
        
    }
}