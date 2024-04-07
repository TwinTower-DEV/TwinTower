using System;
using System.IO;
using UnityEngine;

namespace TwinTower
{
    public class CutSceneCheck : MonoBehaviour
    {
        [SerializeField] private string FileName;
        public void Awake()
        {
            if (DataManager.Instance.StageInfovalue.cutsceneflug != null)
            {
                DataManager.Instance.Scripstvalue = DataManager.Instance.ReadText(FileName);
                UIManager.Instance.ShowNormalUI<UI_CutScene>();
            }
        }
    }
}