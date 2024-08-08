using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 튜토리얼 표시 클래스
/// 튜토리얼마다 다른 스크립트를 보여주어야 하므로 TutorialObject로 생성한 튜토리얼을 받고, 이를 보여줌.
/// 추후 UI로 대체할 것임.
/// </summary>

namespace TwinTower
{
    public class TileTutorial : EXTile
    {
        [SerializeField] private string tutorialstring;
        [SerializeField] private string tutorialstring_eng;
        private UI_Tutorial uiTutorial;
        
        public override void Active()
        {
            uiTutorial = UIManager.Instance.ShowNormalUI<UI_Tutorial>();
            if(DataManager.Instance.UIGameDatavalue.langaugecursor == 0)
                uiTutorial.SetText(tutorialstring);
            else
                uiTutorial.SetText(tutorialstring_eng);
            uiTutorial.SetPosition(gameObject.transform.position, Vector3.up * 2.3f);
            uiTutorial.SetActives();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                uiTutorial.Close();
            }
        }
    }
}