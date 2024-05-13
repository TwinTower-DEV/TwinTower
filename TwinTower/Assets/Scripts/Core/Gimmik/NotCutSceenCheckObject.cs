using System;
using UnityEngine;

namespace TwinTower
{
    public class NotCutSceenCheckObject : MonoBehaviour
    {
        public void Start()
        {
            StartCoroutine(UI_ScreenFader.FadeSceneIn());
        }
    }
}