using System;
using UnityEngine;

namespace TwinTower
{
    public class NotCutSceenCheckObject : MonoBehaviour
    {
        [SerializeField] private AudioClip bgm;
        public void Awake()
        {
            Debug.Log("싫행");
            StartCoroutine(UI_ScreenFader.FadeSceneIn());
            SoundManager.Instance.Play(bgm, Define.Sound.Bgm);
        }
    }
}