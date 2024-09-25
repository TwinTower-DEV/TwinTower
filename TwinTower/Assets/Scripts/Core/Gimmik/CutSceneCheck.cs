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
                if (ManagerSet.Data.StageInfovalue.cutsceneflug != null && !ManagerSet.UI.iscutSceenCheck)
                {
                    ManagerSet.Sound.Play(audioClip, Define.Sound.Bgm);
                    
                    ManagerSet.UI.iscutSceenCheck = true;
                    ManagerSet.Data.Scripstvalue = ManagerSet.Data.ReadText(FileName);
                    //Time.timeScale = 0;
                    //InputController.Instance.ReleaseControl();
                    ManagerSet.UI.ShowNormalUI<UI_CutScene>();
                }
            }
        }

        public void CutSceneStart()
        {
            if (ManagerSet.Data.StageInfovalue.cutsceneflug != null && !ManagerSet.UI.iscutSceenCheck)
            {
                ManagerSet.UI.iscutSceenCheck = true;
                ManagerSet.Data.Scripstvalue = ManagerSet.Data.ReadText(FileName);
                //Time.timeScale = 0;
                //InputController.Instance.ReleaseControl();
                InputController.Instance.ReleaseControl();
                ManagerSet.UI.ShowNormalUI<UI_CutScene>();
            }
        }
    }
}