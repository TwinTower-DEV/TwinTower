using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        // FadeIn 코드
        public async UniTask FadeSceneIn()
        {
            CanvasGroup canvasGroup;
            canvasGroup = Instance.FaderCanvasGroup;
            await canvasGroup.DOFade(0f, fadeDuration).ToUniTask();

            InputController.Instance.GainControl();
            UIManager.Instance.FadeCheck = false;
        }

        // FadeOut 코드
        public async UniTask FadeSceneOut()
        {
            InputController.Instance.ReleaseControl();
            UIManager.Instance.FadeCheck = true;

            CanvasGroup canvasGroup = Instance.FaderCanvasGroup;
            canvasGroup.gameObject.SetActive(true);
            await canvasGroup.DOFade(1f, fadeDuration).ToUniTask();
        }

        public bool FadeCheck()
        {
            CanvasGroup canvasGroup;
            canvasGroup = Instance.FaderCanvasGroup;
            
            return canvasGroup.alpha > 0 ? true : false;
        }
    }
}