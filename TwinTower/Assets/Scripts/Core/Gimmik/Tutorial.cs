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
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private string tutorialstring;
        private UI_Tutorial uiTutorial;

        private void Start()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, 0);
        }
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log(tutorialstring);
                uiTutorial = UIManager.Instance.ShowNormalUI<UI_Tutorial>();
                uiTutorial.SetText(tutorialstring);
                uiTutorial.SetPosition(gameObject.transform.position, Vector3.up * 2.3f);
                uiTutorial.SetActives();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                uiTutorial.Close();
                Debug.Log("튜토리얼 없애기");
            }
        }
    }
}