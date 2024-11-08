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
    public class GameManager
    {
        public Player _player1;
        public Player _player2;
        public Map leftMap;
        public Map rightMap;
        public UI_FieldScene _FieldScene;
        public bool isClearCheck = false;
        public bool isRotateCheck = false;
        
        
        public void Init()
        {
            //UIManager.Instance.Clear();
            //_FieldScene = ManagerSet.UI.ShowNormalUI<UI_FieldScene>();
            //FindPlayer();
            isClearCheck = false;
            
            //InputManager.Instance.UpDateCount();
            
            //_FieldScene.CountUpdate(InputManager.Instance.GetCount());

        }

        public void CurrentScnen(UI_FieldScene _scene)
        {
            _FieldScene = _scene;
        }

        public void UI_UpdateCount(int count)
        {
            _FieldScene.CountUpdate(count);
        }
        public void Restart()
        {
            _player1.Dir = Define.MoveDir.Die;
            _player2.Dir = Define.MoveDir.Die;
            InputController.Instance.ReleaseControl();
            ScreenManager.Instance.Reload();
        }
        public void FindPlayer() {
            if (GameObject.Find("Dalia").GetComponent<Player>() != null)
            {
                _player1 = GameObject.Find("Dalia").GetComponent<Player>();
                //InputController.Instance.GainControl();
            }

            if(GameObject.Find("Irise").GetComponent<Player>() != null)
                _player2 = GameObject.Find("Irise").GetComponent<Player>();
        }

        public void Player(Player _player)
        {
            if (_player.gameObject.name == "Dalia")
            {
                _player1 = _player;
            }
            else
            {
                _player2 = _player;
            }
        }

        public Map GetMap(MapType type)
        {
            if (type == MapType.Left)
            {
                return leftMap;
            }
            else
            {
                return rightMap;
            }
        }
    }
}