using System;
using UnityEngine;

namespace TwinTower
{
    public class NotCutSceenCheckObject : MonoBehaviour
    {
        [SerializeField] private AudioClip bgm;
        public void Start()
        {
            StartCoroutine(UI_ScreenFader.FadeSceneIn());
            SoundManager.Instance.ChangeBGM(bgm);
        }
    }
}