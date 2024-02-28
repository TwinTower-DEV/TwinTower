using UnityEngine;
namespace TwinTower
{
    public class PlayerManager: Manager<PlayerManager>
    {
        public Player _playerControl;
        public bool isPlayer1MoveCheck = false;
        public bool isPlayer2MoveCheck = false;
        protected override void Awake()
        {
            _playerControl = GameObject.Find("PlayerControl").GetComponent<Player>();
        }
    }
}