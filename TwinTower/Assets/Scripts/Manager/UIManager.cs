using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TwinTower
{
    public class UIManager : Manager<UIManager>
    {
        // sortingOrder을 관리하기 위한 변수
        // Normal UI들을 관리하는 HashSet
        HashSet<UI_Base> _normalUIs = new HashSet<UI_Base>();
        private Stack<UI_Base> _uistack = new Stack<UI_Base>();
        private Stack<string> _uienterstack = new Stack<string>();
        private int _uiNum = 0;
        public bool isClearUICheck = false;
        Define.Resolution _resolution = new Define.Resolution() { width = 1080, height = 1920 };
        bool _isWindowMode;

        int _deviceWidth = Screen.width;
        int _deviceHeight = Screen.height;
        
        public Action InputHandler;
        public bool iscutSceenCheck = false;
        public bool FadeCheck = false;
        protected override void Awake()
        {
            base.Awake();
        }

        public int UINum
        {
            get
            {
                return _uiNum;
            }
            private set
            {
                _uiNum = value;
            }
        }

        public void Update()
        {

            Brodcast();
        }

        private void Brodcast()
        {
            if (!Input.anyKey) return;
            if (InputHandler == null) return;
            
            InputHandler.Invoke();
            
        }
        public Define.Resolution Resolution
        {
            get
            {
                return _resolution;
            }
            set
            {
                _resolution = value;
                Screen.SetResolution(_resolution.width, 
                    (int)(((float)_deviceHeight / _deviceWidth) * _resolution.width), !_isWindowMode);
                if ((float)_resolution.width / _resolution.height < (float)_deviceWidth / _deviceHeight) // 기기의 해상도 비가 더 큰 경우
                {
                    float newWidth = ((float)_resolution.width / _resolution.height) / ((float)_deviceWidth / _deviceHeight); // 새로운 너비
                    Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
                }
                else // 게임의 해상도 비가 더 큰 경우
                {
                    float newHeight = ((float)_deviceWidth / _deviceHeight) / ((float)_resolution.width / _resolution.height); // 새로운 높이
                    Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
                }
            }
        }
        public bool IsWindowMode
        {
            get
            {
                return _isWindowMode;
            }
            set
            {
                _isWindowMode = value;
                Screen.SetResolution(_resolution.width,
                    (int)(((float)_deviceHeight / _deviceWidth) * _resolution.width), !_isWindowMode);
                if ((float)_resolution.width / _resolution.height < (float)_deviceWidth / _deviceHeight) // 기기의 해상도 비가 더 큰 경우
                {
                    float newWidth = ((float)_resolution.width / _resolution.height) / ((float)_deviceWidth / _deviceHeight); // 새로운 너비
                    Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
                }
                else // 게임의 해상도 비가 더 큰 경우
                {
                    float newHeight = ((float)_deviceWidth / _deviceHeight) / ((float)_resolution.width / _resolution.height); // 새로운 높이
                    Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
                }
            }
        }
        
        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find("@UI_Root");
                if (root == null)
                    root = new GameObject { name = "@UI_Root" };
                return root;
            }
        }
        
        public T ShowNormalUI<T>(string name = null) where T : UI_Base
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            _uiNum = _uiNum + 1;

            GameObject go = ResourceManager.Instance.Instantiate($"UI/{name}");
            T ui = Util.GetOrAddComponent<T>(go);
            _normalUIs.Add(ui);
            _uistack.Push(ui);
            go.transform.SetParent(Root.transform);
            
            return ui;
        }
        public void CloseNormalUI(UI_Base ui)
        {
            if (!_normalUIs.Contains(ui))
                return;
            
            if (_normalUIs.Count == 0)
                return;
            if (ui == null) return;
            _normalUIs.Remove(ui);
            _uistack.Pop();
            _uiNum = _uiNum - 1;
            ResourceManager.Instance.Destroy(ui.gameObject);
        }

        public void Clear()
        {
            while (_uistack.Count > 0)
            {
                UI_Base _ui = _uistack.Peek();
                _uistack.Pop();
            }
            
        }

        public void ChangingLanguage(int islenguage)
        {
            Debug.Log("Language Change");
            ScreenManager.Instance.FadeInOut();
            while (_uistack.Count > 0)
            {
                UI_Base _ui = _uistack.Peek();
                _uienterstack.Push(_ui.name);
                CloseNormalUI(_ui);
            }

            InputHandler = null;

            string name = _uienterstack.Peek();
            _uienterstack.Clear();
            if (islenguage == 0)
            {
                name = name.Substring(0, name.Length - 4);
            }
            else
            {
                name = name + "_ENG";
            }
            
            _uiNum = _uiNum + 1;
            GameObject go = ResourceManager.Instance.Instantiate($"UI/{name}");

            if (name.Contains("UI_MainScene") == true)
            {
                if (islenguage == 0)
                {
                    UI_MainScene ui = Util.GetOrAddComponent<UI_MainScene>(go);
                    _normalUIs.Add(ui);
                    _uistack.Push(ui);

                }
                else
                {
                    UI_MainScene_ENG ui = Util.GetOrAddComponent<UI_MainScene_ENG>(go);
                    _normalUIs.Add(ui);
                    _uistack.Push(ui);
                }
            }
            else if (name.Contains("UI_FieldScene") == true)
            {
                Time.timeScale = 1.0f;
                InputController.Instance.GainControl();
                if (islenguage == 0)
                {
                    UI_FieldScene ui = Util.GetOrAddComponent<UI_FieldScene>(go);
                    _normalUIs.Add(ui);
                    _uistack.Push(ui);

                }
                else
                {
                    UI_FieldScene_ENG ui = Util.GetOrAddComponent<UI_FieldScene_ENG>(go);
                    _normalUIs.Add(ui);
                    _uistack.Push(ui);
                }
            }
            go.transform.SetParent(Root.transform);

        }

        public void CloseFieldCutSceneUI(UI_Base ui)
        {
            if (_normalUIs.Count == 0)
                return;
            if (ui == null) return;
            _normalUIs.Remove(ui);
            _uiNum = _uiNum - 1;
            ResourceManager.Instance.Destroy(ui.gameObject);
            StartCoroutine(ScreenManager.Instance.NextSceneload());
        }

        public void Load(string loadscene)
        {
            StartCoroutine(ScreenManager.Instance.NextSceneload(loadscene));
            //InputManager.Instance.UnPause();
        }
    }
}