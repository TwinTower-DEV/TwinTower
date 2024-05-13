using System.Collections;
using UnityEngine;
/// <summary>
/// Fade IN,OUT을 구현한 코드입니다.
/// </summary>
namespace TwinTower
{
    public class UI_ScreenFader : MonoBehaviour
    {
        // Singleton 사용
        public static UI_ScreenFader Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;
                s_Instance = FindObjectOfType<UI_ScreenFader>();

                if (s_Instance != null)
                    return s_Instance;

                return s_Instance;

            }
        }

        protected static UI_ScreenFader s_Instance;


        public CanvasGroup FaderCanvasGroup;
        public float fadeDuration = 1f;

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            // 처음 시작할 땐 alpha(투명도) 0으로 시작
            Instance.FaderCanvasGroup.alpha = 0f;
            DontDestroyOnLoad(gameObject);
        }
        // 서서히 작동되게 하는 코드
        protected IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup, bool FadeCheck)
        {
            canvasGroup.blocksRaycasts = true;
            UIManager.Instance.FadeCheck = true;
            float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuration;
            while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha,
                    fadeSpeed * Time.deltaTime);
                yield return null;
            }
            canvasGroup.alpha = finalAlpha;
            canvasGroup.blocksRaycasts = false;

            if (!FadeCheck)
            {
                InputController.Instance.GainControl();
                UIManager.Instance.FadeCheck = false;
            }
        }
        // FadeIn 코드
        public static IEnumerator FadeSceneIn ()
        {
            CanvasGroup canvasGroup;
            canvasGroup = Instance.FaderCanvasGroup;
            yield return Instance.StartCoroutine(Instance.Fade(0f, canvasGroup, false));
        }
        // FadeOut 코드
        public static IEnumerator FadeScenOut()
        {
            InputController.Instance.ReleaseControl();
            CanvasGroup canvasGroup = Instance.FaderCanvasGroup;
            canvasGroup.gameObject.SetActive(true);
            yield return Instance.StartCoroutine(Instance.Fade(1f, canvasGroup, true));
            Debug.Log("Fade Out ");
        }
    }
}