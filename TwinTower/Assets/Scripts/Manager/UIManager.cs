using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TwinTower
{
    public class UIManager : Manager<UIManager>
    {
        // sortingOrder을 관리하기 위한 변수
        int _order = 10;
        // Normal UI들을 관리하는 HashSet
        HashSet<UI_Base> _normalUIs = new HashSet<UI_Base>();
        private int _uiNum = 0;
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
            go.transform.SetParent(Root.transform);

            return ui;
        }
        public void CloseNormalUI(UI_Base ui)
        {
            if (_normalUIs.Count == 0)
                return;
            if (ui == null) return;
            _normalUIs.Remove(ui);
            _uiNum = _uiNum - 1;
            ResourceManager.Instance.Destroy(ui.gameObject);
        }
    }
}