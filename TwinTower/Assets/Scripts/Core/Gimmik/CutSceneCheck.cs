using System;
using System.IO;
using UnityEngine;

namespace TwinTower
{
    public class CutSceneCheck : MonoBehaviour
    {
        [SerializeField] private string FileName;
        public void Start()
        {
            if (DataManager.Instance.StageInfovalue.cutsceneflug != null && !UIManager.Instance.iscutSceenCheck)
            {
                UIManager.Instance.iscutSceenCheck = true;
                DataManager.Instance.Scripstvalue = DataManager.Instance.ReadText(FileName);
                //Time.timeScale = 0;
                InputController.Instance.ReleaseControl();
                UIManager.Instance.ShowNormalUI<UI_CutScene>();
            }
        }
    }
}