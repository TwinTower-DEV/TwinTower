using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TwinTower
{
    public class NotCutSceenCheckObject : MonoBehaviour
    {
        [SerializeField] private AudioClip bgm;
        public void Awake()
        {
            Debug.Log("싫행");
            UI_ScreenFader.Instance.FadeSceneIn().Forget();
            SoundManager.Instance.Play(bgm, Define.Sound.Bgm);
        }
    }
}