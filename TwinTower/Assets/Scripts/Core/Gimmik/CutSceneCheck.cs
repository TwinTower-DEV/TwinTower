using System;
using System.IO;
using UnityEngine;

namespace TwinTower
{
    public class CutSceneCheck : MonoBehaviour
    {
        [SerializeField] private string FileName;
        [SerializeField] private bool startCheck = true;
        [SerializeField] private AudioClip audioClip;
        public void Start()
        {
            if (startCheck)
            {
                if (DataManager.Instance.StageInfovalue.cutsceneflug != null && !UIManager.Instance.iscutSceenCheck)
                {
                    SoundManager.Instance.ChangeBGM(audioClip);
                    
                    UIManager.Instance.iscutSceenCheck = true;
                    DataManager.Instance.Scripstvalue = DataManager.Instance.ReadText(FileName);
                    //Time.timeScale = 0;
                    //InputController.Instance.ReleaseControl();
                    UIManager.Instance.ShowNormalUI<UI_CutScene>();
                }
            }
        }

        public void CutSceneStart()
        {
            if (DataManager.Instance.StageInfovalue.cutsceneflug != null && !UIManager.Instance.iscutSceenCheck)
            {
                UIManager.Instance.iscutSceenCheck = true;
                DataManager.Instance.Scripstvalue = DataManager.Instance.ReadText(FileName);
                //Time.timeScale = 0;
                //InputController.Instance.ReleaseControl();
                InputController.Instance.ReleaseControl();
                UIManager.Instance.ShowNormalUI<UI_CutScene>();
            }
        }
    }
}