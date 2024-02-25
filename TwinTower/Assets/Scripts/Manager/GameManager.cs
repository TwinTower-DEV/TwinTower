using System;
using UnityEngine;

namespace TwinTower
{
    /// <summary>
    /// GameManager 클래스입니다. 게임의 전반적인 진행을 관리합니다.
    /// </summary>
    public class GameManager : Manager<GameManager>
    {
        private TileFindManager _tileFindManager;
        [SerializeField] private Player _player;
        protected override void Awake()
        {
            _tileFindManager = TileFindManager.Instance;
            _player.transform.position =
                new Vector2(_tileFindManager.player1Map[0, 0].x + 0.5f, _tileFindManager.player1Map[0, 0].y + 0.5f);
            _player.pos = new Vector2(_tileFindManager.player1Map[0, 0].x + 0.5f, _tileFindManager.player1Map[0, 0].y + 0.5f);
        }

        public void Start()
        {
            
        }

        protected void FixedUpdate()
        {
            
        }
    }
}