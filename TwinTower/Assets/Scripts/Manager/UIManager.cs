﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TwinTower
{
    public class UIManager
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
        private Action Handler;
        public Action InputHandler
        {
            get
            {
                return Handler;
            }
            set
            {
                Handler = value;
                UIInputController.Instance.SetHandler(Handler);
            }
        }
        public bool iscutSceenCheck = false;
        public bool FadeCheck = false;
        public void Init()
        {
            InitLanguage(ManagerSet.Data.UIGameDatavalue.langaugecursor);
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

            GameObject go = ManagerSet.Resource.Instantiate($"UI/{name}");
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
            ManagerSet.Resource.Destroy(ui.gameObject);
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
            UI_Base _ui = _uistack.Peek();
            CloseNormalUI(_ui);

            string languageIdentifier;
            if (islenguage == 0)
            {
                languageIdentifier = "ko-KR";
            }
            else
            {
                languageIdentifier = "en-US";
            }

            LocaleIdentifier localeCode = new LocaleIdentifier(languageIdentifier);
            for(int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++) {
                Locale aLocale = LocalizationSettings.AvailableLocales.Locales[i];
                LocaleIdentifier anIdentifier = aLocale.Identifier;
                if(anIdentifier == localeCode) {
                    LocalizationSettings.SelectedLocale = aLocale;
                }
            }
        }

        public void InitLanguage(int language)
        {
            string languageIdentifier;
            if (language == 0)
            {
                languageIdentifier = "ko-KR";
            }
            else
            {
                languageIdentifier = "en-US";
            }

            LocaleIdentifier localeCode = new LocaleIdentifier(languageIdentifier);
            for(int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++) {
                Locale aLocale = LocalizationSettings.AvailableLocales.Locales[i];
                LocaleIdentifier anIdentifier = aLocale.Identifier;
                if(anIdentifier == localeCode) {
                    LocalizationSettings.SelectedLocale = aLocale;
                }
            }
        }
        
        public void CloseFieldCutSceneUI(UI_Base ui)
        {
            if (_normalUIs.Count == 0)
                return;
            if (ui == null) return;
            _normalUIs.Remove(ui);
            _uiNum = _uiNum - 1;
            ManagerSet.Resource.Destroy(ui.gameObject);
            //StartCoroutine(ScreenManager.Instance.NextSceneload());
        }

        public void Load(string loadscene)
        {
            //StartCoroutine(ScreenManager.Instance.NextSceneload(loadscene));
            //InputManager.Instance.UnPause();
        }
    }
}