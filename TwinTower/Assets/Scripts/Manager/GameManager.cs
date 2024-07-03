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
        public UI_FieldScene _FieldScene;
        public UI_FieldScene_ENG _FieldSceneEng;
        public bool isClearCheck = false;
        public bool isRotateCheck = false;
        protected override void Awake()
        {
            //UIManager.Instance.Clear();
            if (DataManager.Instance.UIGameDatavalue.langaugecursor == 0)
            {
                _FieldScene = UIManager.Instance.ShowNormalUI<UI_FieldScene>();
            }
            else
            {
                _FieldSceneEng = UIManager.Instance.ShowNormalUI<UI_FieldScene_ENG>();
            }
            FindPlayer();
            isClearCheck = false;
            
            InputManager.Instance.UpDateCount();
            
            if (DataManager.Instance.UIGameDatavalue.langaugecursor == 0)
                _FieldScene.CountUpdate(InputManager.Instance.GetCount());
            else
                _FieldSceneEng.CountUpdate(InputManager.Instance.GetCount());
        }


        public void UI_UpdateCount(int count)
        {
            if(DataManager.Instance.UIGameDatavalue.langaugecursor == 0)
                _FieldScene.CountUpdate(count);
            else
                _FieldSceneEng.CountUpdate(count);
        }
        public void Restart()
        {
            _player1.Dir = Define.MoveDir.Die;
            _player2.Dir = Define.MoveDir.Die;
            InputController.Instance.ReleaseControl();
            StartCoroutine(ScreenManager.Instance.CurrentScreenReload());
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
    }
}