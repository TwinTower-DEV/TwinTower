﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TwinTower
{
    /// <summary>
    /// ScreenManager 클래스입니다.  게임 씬을 관리하는 클래스입니다.
    /// </summary>
    public class ScreenManager : Manager<ScreenManager>
    {
        public IEnumerator CurrentScreenReload()
        {
            yield return StartCoroutine(UI_ScreenFader.FadeScenOut());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            yield return StartCoroutine(UI_ScreenFader.FadeSceneIn());
            ManagerSet.Gamemanager.FindPlayer();
        }

        public void Reload()
        {
            StartCoroutine(CurrentScreenReload());
        }

        public IEnumerator FadeInOut()
        {
            yield return StartCoroutine(UI_ScreenFader.FadeScenOut());
            yield return StartCoroutine(UI_ScreenFader.FadeSceneIn());
        }
        public IEnumerator NextSceneload(string s = null)
        {
            ManagerSet.UI.Clear();
            ManagerSet.UI.iscutSceenCheck = false;
            yield return StartCoroutine(UI_ScreenFader.FadeScenOut());

            if (s == null)
            {
                if (SceneManager.GetActiveScene().buildIndex + 1 >= 15)
                {
                    InputManager.Destroys();
                    SceneManager.LoadScene("MainScene");
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    ManagerSet.Data.saveload.ChangeCurrSaveSlot(0);
                    ManagerSet.Data.saveload.Save(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name);
                }
            }
            else
                SceneManager.LoadScene(s);

            if (s == "MainScene" || SceneManager.GetActiveScene().buildIndex + 1 >= 15)
            {
                Debug.Log("asdasdw");
                yield return StartCoroutine(UI_ScreenFader.FadeSceneIn());
                
            }
            //GameManager.Instance.FindPlayer();
        }
    }
}