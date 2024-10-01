using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TwinTower
{
    public class UI_SettingScene : UI_Base
    {
        private const int BUTTON_COUNT = 6;
        private const int Sound_BUTTON_COUNT = 5;
        [SerializeField] private Sprite noSelect_noClick;
        [SerializeField] private Sprite noSelect_Click;
        [SerializeField] private Sprite select_noCliCK;
        [SerializeField] private Sprite select_Click;

        private Dictionary<int, int> bgmbuttonCount = new Dictionary<int, int>();
        private Dictionary<int, int> sebuttonCount = new Dictionary<int, int>();
        private Dictionary<int, int> mainCategory = new Dictionary<int, int>();
        
        private int bgmcursor;
        private int seccursor;
        private int currentcoursor;
        private int displaymodecursor;
        private int displaycoursor;
        private int langaugecoursor;
        
        private Action[] _actions = new Action[BUTTON_COUNT];
        private string[] displayModes = new string[2] { "전체화면", "창 화면" };

        private string[] displays = new string[5]
            { "800 X 600", "1280 X 720", "1920 X 1080", "2560 X 1440", "3840 X 2160" };
        
        private int[] height = new int[5]
            { 600, 720, 1080, 1440, 2160 };

        private int[] width = new int[5]
            { 800, 1280, 1920, 2560, 3840 };
        private float[] soundvolume = new float[5]
            { 0.0f, 0.25f, 0.5f, 0.75f, 1.0f };
        private string[] langauges = new string[2] { "한국어", "ENGLISH" };
        enum Images
        {
            BGM_Button,
            SE_Button,
            DisplayMode_Button,
            //Display_Button,
            Language_Button,
            Creadit,
            Apply,
            BGM_Select,
            SE_Select,
            DisplayMode_Select,
            //Display_Select,
            Language_Select,
            Creadit_Select,
            Apply_Select,
            Audio_Setting,
            Display_Setting,
            Language_Setting,
            BGM_Button1,
            BGM_Button2,
            BGM_Button3,
            BGM_Button4,
            BGM_Button5,
            SE_Button1,
            SE_Button2,
            SE_Button3,
            SE_Button4,
            SE_Button5,
        }

        enum Texts
        {
            Audio_Setting_txt,
            Display_Setting_txt,
            Language_Setting_txt,
            DisplayMode_txt,
            Display_txt,
            Language_txt,
            Creadit_txt,
            Applay_txt,
            Lgo_txt
        }
        public override void Init()
        {
            Bind<Image>(typeof(Images));

            ManagerSet.UI.InputHandler -= KeyInput;
            ManagerSet.UI.InputHandler += KeyInput;
            bgmcursor = ManagerSet.Data.UIGameDatavalue.bgmcoursor;
            seccursor = ManagerSet.Data.UIGameDatavalue.secursor;
            currentcoursor = 0;
            displaymodecursor = ManagerSet.Data.UIGameDatavalue.displaymodecursor;
            displaycoursor = ManagerSet.Data.UIGameDatavalue.displaycursor;
            langaugecoursor = ManagerSet.Data.UIGameDatavalue.langaugecursor;
            
            EventBind();
            InitBgmButtonCount();
            InitSeButtonCount();
            InitMainCategory();
            InitSetting();
            
        }

        private void EventBind()
        {
            Get<Image>((int)Images.BGM_Button).gameObject.BindEvent(()=>ChangeCoursor((int)Images.BGM_Button), Define.UIEvent.Click);
            Get<Image>((int)Images.SE_Button).gameObject.BindEvent(()=>ChangeCoursor((int)Images.SE_Button), Define.UIEvent.Click);
            Get<Image>((int)Images.DisplayMode_Button).gameObject.BindEvent(()=>ChangeCoursor((int)Images.DisplayMode_Button), Define.UIEvent.Click);
            //Get<Image>((int)Images.Display_Button).gameObject.BindEvent(()=>ChangeCoursor((int)Images.Display_Button), Define.UIEvent.Click);
            Get<Image>((int)Images.Language_Button).gameObject.BindEvent(()=>ChangeCoursor((int)Images.Language_Button), Define.UIEvent.Click);
            Get<Image>((int)Images.Creadit).gameObject.BindEvent(Credit);
            Get<Image>((int)Images.Apply).gameObject.BindEvent(Apply);
            
            Get<Image>((int)Images.BGM_Button1).gameObject.BindEvent(() => PushSoundButton(Images.BGM_Button, Images.BGM_Button1), Define.UIEvent.Click);
            Get<Image>((int)Images.BGM_Button2).gameObject.BindEvent(() => PushSoundButton(Images.BGM_Button, Images.BGM_Button2), Define.UIEvent.Click);
            Get<Image>((int)Images.BGM_Button3).gameObject.BindEvent(() => PushSoundButton(Images.BGM_Button, Images.BGM_Button3), Define.UIEvent.Click);
            Get<Image>((int)Images.BGM_Button4).gameObject.BindEvent(() => PushSoundButton(Images.BGM_Button, Images.BGM_Button4), Define.UIEvent.Click);
            Get<Image>((int)Images.BGM_Button5).gameObject.BindEvent(() => PushSoundButton(Images.BGM_Button, Images.BGM_Button5), Define.UIEvent.Click);
            
            Get<Image>((int)Images.SE_Button1).gameObject.BindEvent(() => PushSoundButton(Images.SE_Button, Images.SE_Button1), Define.UIEvent.Click);
            Get<Image>((int)Images.SE_Button2).gameObject.BindEvent(() => PushSoundButton(Images.SE_Button, Images.SE_Button2), Define.UIEvent.Click);
            Get<Image>((int)Images.SE_Button3).gameObject.BindEvent(() => PushSoundButton(Images.SE_Button, Images.SE_Button3), Define.UIEvent.Click);
            Get<Image>((int)Images.SE_Button4).gameObject.BindEvent(() => PushSoundButton(Images.SE_Button, Images.SE_Button4), Define.UIEvent.Click);
            Get<Image>((int)Images.SE_Button5).gameObject.BindEvent(() => PushSoundButton(Images.SE_Button, Images.SE_Button5), Define.UIEvent.Click);

            _actions[0] = (() => 
                PushSoundButton((Images)currentcoursor, 0, (bgmcursor + 1) % Sound_BUTTON_COUNT));
            _actions[1] = (() =>
                PushSoundButton((Images)currentcoursor, 0, (seccursor + 1 + Sound_BUTTON_COUNT) % Sound_BUTTON_COUNT));
            _actions[2] = (() =>
                SidePushButton((Images)currentcoursor, true));
            _actions[3] = (() =>
                SidePushButton((Images)currentcoursor, true));
            _actions[4] = Credit;
            _actions[5] = Apply;

        }

        private void Test()
        {
            
        }
        
        private void KeyInput()
        {
            if (!Input.anyKey) return;
            
            if (_uiNum != ManagerSet.UI.UINum)
                return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UI_ClickSoundEffect();
                ManagerSet.Sound.CancelSetting();
                ManagerSet.UI.InputHandler -= KeyInput;
                ManagerSet.UI.CloseNormalUI(this);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if(currentcoursor == 0)
                    PushSoundButton((Images)currentcoursor, 0, (bgmcursor - 1 + Sound_BUTTON_COUNT) % Sound_BUTTON_COUNT);
                else if(currentcoursor == 1)
                    PushSoundButton((Images)currentcoursor, 0, (seccursor - 1 + Sound_BUTTON_COUNT) % Sound_BUTTON_COUNT);
                else if(currentcoursor == 4 || currentcoursor == 5)
                    SideEnterCoursor();
                else
                    SidePushButton((Images)currentcoursor, false);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if(currentcoursor == 0)
                    PushSoundButton((Images)currentcoursor, 0, (bgmcursor + 1) % Sound_BUTTON_COUNT);
                else if(currentcoursor == 1)
                    PushSoundButton((Images)currentcoursor, 0, (seccursor + 1) % Sound_BUTTON_COUNT);
                else if(currentcoursor == 4 || currentcoursor == 5)
                    SideEnterCoursor();
                else
                    SidePushButton((Images)currentcoursor, true);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                ChangeCoursor((currentcoursor - 1 + BUTTON_COUNT) % BUTTON_COUNT);
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                ChangeCoursor((currentcoursor + 1) % BUTTON_COUNT);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                _actions[currentcoursor].Invoke();
            }
        }

        
        private void InitBgmButtonCount()
        {
            bgmbuttonCount.Add(0, (int)Images.BGM_Button1);
            bgmbuttonCount.Add(1, (int)Images.BGM_Button2);
            bgmbuttonCount.Add(2, (int)Images.BGM_Button3);
            bgmbuttonCount.Add(3, (int)Images.BGM_Button4);
            bgmbuttonCount.Add(4, (int)Images.BGM_Button5);
            
            bgmbuttonCount.Add((int)Images.BGM_Button1, 0);
            bgmbuttonCount.Add((int)Images.BGM_Button2, 1);
            bgmbuttonCount.Add((int)Images.BGM_Button3, 2);
            bgmbuttonCount.Add((int)Images.BGM_Button4, 3);
            bgmbuttonCount.Add((int)Images.BGM_Button5, 4);
        }

        private void InitSeButtonCount()
        {
            sebuttonCount.Add(0, (int)Images.SE_Button1);
            sebuttonCount.Add(1, (int)Images.SE_Button2);
            sebuttonCount.Add(2, (int)Images.SE_Button3);
            sebuttonCount.Add(3, (int)Images.SE_Button4);
            sebuttonCount.Add(4, (int)Images.SE_Button5);
            
            sebuttonCount.Add((int)Images.SE_Button1, 0);
            sebuttonCount.Add((int)Images.SE_Button2, 1);
            sebuttonCount.Add((int)Images.SE_Button3, 2);
            sebuttonCount.Add((int)Images.SE_Button4, 3);
            sebuttonCount.Add((int)Images.SE_Button5, 4);
        }

        private void InitMainCategory()
        {
            mainCategory.Add(0, (int)Images.Audio_Setting);
            mainCategory.Add(1, (int)Images.Display_Setting);
            mainCategory.Add(2, (int)Images.Language_Setting);
        }
        private void SetColor(GameObject gameObject, string color, bool isImageCheck)
        {
            Color newcolor;
            if (ColorUtility.TryParseHtmlString(color, out newcolor))
            {
                if (!isImageCheck)
                    gameObject.GetComponentInChildren<TextMeshProUGUI>().color = newcolor;
                else
                    Util.FindChild<Image>(gameObject).color = newcolor;
            }
        }

        private void SelectSoundButton(int isCheck)
        {
            for (int i = 0; i < 5; i++)
            {
                if (isCheck == 0)
                {
                    if(i == bgmcursor)
                        Get<Image>(bgmbuttonCount[i]).gameObject.GetComponent<Image>().sprite = select_Click;
                    else
                        Get<Image>(bgmbuttonCount[i]).gameObject.GetComponent<Image>().sprite = select_noCliCK;
                    
                }
                else
                {
                    if(i == seccursor)
                        Get<Image>(sebuttonCount[i]).gameObject.GetComponent<Image>().sprite = select_Click;
                    else
                        Get<Image>(sebuttonCount[i]).gameObject.GetComponent<Image>().sprite = select_noCliCK;
                }
            }
        }
        private void NoSelectSoundButton(int isCheck)
        {
            for (int i = 0; i < 5; i++)
            {
                if (isCheck == 0)
                {
                    if(i == bgmcursor)
                        Get<Image>(bgmbuttonCount[i]).gameObject.GetComponent<Image>().sprite = noSelect_Click;
                    else
                        Get<Image>(bgmbuttonCount[i]).gameObject.GetComponent<Image>().sprite = noSelect_noClick;
                    
                }
                else
                {
                    if(i == seccursor)
                        Get<Image>(sebuttonCount[i]).gameObject.GetComponent<Image>().sprite = noSelect_Click;
                    else
                        Get<Image>(sebuttonCount[i]).gameObject.GetComponent<Image>().sprite = noSelect_noClick;
                }
            }
        }
        private void InitSetting()
        {
            SetColor(Get<Image>((int)Images.Audio_Setting).gameObject, "#846E62", false);
            SetColor(Get<Image>((int)Images.Audio_Setting).gameObject, "#846E62", true);
            SetColor(Get<Image>(currentcoursor).gameObject, "#846E62", false);
            SelectSoundButton(0);
            NoSelectSoundButton(1);
            //Util.FindChild<Image>(Get<Image>((int)Images.Display_Button).gameObject).GetComponentInChildren<TextMeshProUGUI>().text = displays[displaycoursor];
            Util.FindChild<Image>(Get<Image>((int)Images.DisplayMode_Button).gameObject).GetComponentInChildren<TextMeshProUGUI>().text = displayModes[displaymodecursor];
            Util.FindChild<Image>(Get<Image>((int)Images.Language_Button).gameObject).GetComponentInChildren<TextMeshProUGUI>().text = langauges[langaugecoursor];
            Get<Image>((int)Images.SE_Select).gameObject.SetActive(false);
            Get<Image>((int)Images.DisplayMode_Select).gameObject.SetActive(false);
            //Get<Image>((int)Images.Display_Select).gameObject.SetActive(false);
            Get<Image>((int)Images.Language_Select).gameObject.SetActive(false);
            Get<Image>((int)Images.Creadit_Select).gameObject.SetActive(false);
            Get<Image>((int)Images.Apply_Select).gameObject.SetActive(false);
            
        }

        private void PushSoundButton(Images selectsound, Images soundbutton, int nextidx = -1)
        {
            
            NoSelectSoundButton(1);
            NoSelectSoundButton(0);
            
            if (selectsound == Images.BGM_Button)
            {
                bgmcursor = nextidx == -1 ? bgmbuttonCount[(int)soundbutton] : nextidx;
                SelectSoundButton(0);
                ManagerSet.Sound.PreviewVolume_BGM(bgmcursor);
                UI_SoundEffect();
            }
            else
            {
                seccursor = nextidx == -1 ? sebuttonCount[(int)soundbutton] : nextidx;
                SelectSoundButton(1);
                ManagerSet.Sound.PreviewVolume_SE(seccursor);
                UI_SoundEffect();
            }

            currentcoursor = (int)selectsound;
        }

        private void SidePushButton(Images selectMain, bool check)
        {
            UI_SoundEffect();

            /*if (selectMain == Images.Display_Button)
            {
                if (check)
                    displaycoursor = (displaycoursor + 1) % BUTTON_COUNT;
                else
                    displaycoursor = (displaycoursor - 1 + BUTTON_COUNT) % BUTTON_COUNT;

                Util.FindChild<Image>(Get<Image>((int)selectMain).gameObject).GetComponentInChildren<TextMeshProUGUI>().text = displays[displaycoursor];
            }*/
           if (selectMain == Images.DisplayMode_Button)
            {
                if (check)
                    displaymodecursor = (displaymodecursor + 1) % 2;
                else
                    displaymodecursor = (displaymodecursor - 1 + 2) % 2;
                Util.FindChild<Image>(Get<Image>((int)selectMain).gameObject).GetComponentInChildren<TextMeshProUGUI>().text = displayModes[displaymodecursor];
            }
            else if (selectMain == Images.Language_Button)
            {
                if (check)
                    langaugecoursor = (langaugecoursor + 1) % 2;
                else
                    langaugecoursor = (langaugecoursor - 1 + 2) % 2;
                Util.FindChild<Image>(Get<Image>((int)selectMain).gameObject).GetComponentInChildren<TextMeshProUGUI>().text = langauges[langaugecoursor];

            }
            
        }
        
        private void ChangeSoundCoursor(int next)
        {
            
        }

        private void EnterCoursor(int nextidx)
        {
            if (currentcoursor == 2 || currentcoursor == 3)
            {
                SetColor(Get<Image>(mainCategory[currentcoursor - 1]).gameObject, "#FFFFFF", true);
                SetColor(Get<Image>(mainCategory[currentcoursor - 1]).gameObject, "#FFFFFF", false);
            }
            else
            {
                if (mainCategory.ContainsKey(currentcoursor / 2))
                {
                    SetColor(Get<Image>(mainCategory[currentcoursor / 2]).gameObject, "#FFFFFF", true);
                    SetColor(Get<Image>(mainCategory[currentcoursor / 2]).gameObject, "#FFFFFF", false);
                }
            }

            SetColor(Get<Image>(currentcoursor).gameObject, "#FFFFFF", false);
            Get<Image>(currentcoursor + BUTTON_COUNT).gameObject.SetActive(false);
            if(currentcoursor != 0 && currentcoursor != 1 && currentcoursor != 4 && currentcoursor != 5)
                SetColor(Get<Image>(currentcoursor).gameObject, "#FFFFFF", true);

            if (currentcoursor == 0 || currentcoursor == 1)
            {
                NoSelectSoundButton(currentcoursor);
            }

            if ((currentcoursor == 4 || currentcoursor == 5) && currentcoursor < nextidx)
                currentcoursor = 0;
            else if ((currentcoursor == 4 || currentcoursor == 5) && currentcoursor > nextidx && nextidx != 0)
                currentcoursor = 3;
            else 
                currentcoursor = nextidx;
            Get<Image>(currentcoursor + BUTTON_COUNT).gameObject.SetActive(true);
            if (currentcoursor == 2 || currentcoursor == 3)
            {
                SetColor(Get<Image>(mainCategory[currentcoursor - 1]).gameObject, "#FFFFFF", true);
                SetColor(Get<Image>(mainCategory[currentcoursor - 1]).gameObject, "#FFFFFF", false);
            }
            else
            {
                if (mainCategory.ContainsKey(currentcoursor / 2))
                {
                    SetColor(Get<Image>(mainCategory[currentcoursor / 2]).gameObject, "#FFFFFF", true);
                    SetColor(Get<Image>(mainCategory[currentcoursor / 2]).gameObject, "#FFFFFF", false);
                }
            }

            SetColor(Get<Image>(currentcoursor).gameObject, "#846E62", false);
            if (currentcoursor != 0 && currentcoursor != 1 && currentcoursor != 4 && currentcoursor != 5)
            {
                SetColor(Get<Image>(currentcoursor).gameObject, "#846E62", true);
                Debug.Log(currentcoursor);
            }
            if (currentcoursor == 0 || currentcoursor == 1)
            {
                SelectSoundButton(currentcoursor);
            }
        }

        private void SideEnterCoursor()
        {
            UI_SoundEffect();
            SetColor(Get<Image>(currentcoursor).gameObject, "#FFFFFF", false);
            Get<Image>(currentcoursor + BUTTON_COUNT).gameObject.SetActive(false);

            currentcoursor = currentcoursor == 4 ? 5 : 4;
            GetImage(currentcoursor + BUTTON_COUNT).gameObject.SetActive(true);
            SetColor(Get<Image>(currentcoursor).gameObject, "#846E62", false);
        }

        private void Credit()
        {
            ManagerSet.UI.ShowNormalUI<UI_Creadit>();
        }

        private void Apply()
        {
            UI_SoundEffect();
            UI_Setting_SaveCheck settingSaveCheck = ManagerSet.UI.ShowNormalUI<UI_Setting_SaveCheck>();
            settingSaveCheck.saveAction += SaveData;
        }

        private void SaveData()
        {
            LanguageApply();
            SoundApply();
            DisplayApply();
            DisplayModeApply();
            ManagerSet.Data.UIGameDatavalue = new UIGameData(bgmcursor, seccursor, displaymodecursor,
                displaycoursor, langaugecoursor);
        }

        private void SoundApply()
        {
            ManagerSet.Sound.ApplySoundVolume();
            // sf 는 이번주 회의에서 물어보기
        }

        private void DisplayApply()
        {
            Define.Resolution resolution;
            resolution.height = height[displaycoursor];
            resolution.width = width[displaycoursor];
            ManagerSet.UI.Resolution = resolution;
        }

        private void LanguageApply()
        {
            if(ManagerSet.Data.UIGameDatavalue.langaugecursor != langaugecoursor)
                ManagerSet.UI.ChangingLanguage(langaugecoursor);
        }

        private void DisplayModeApply()
        {
            if (displaymodecursor == 0)
                Screen.fullScreen = true;
            else
                Screen.fullScreen = false;
        }

        private void ExitCoursor(int idx)
        {
            
        }

        private void ChangeCoursor(int nextidx)
        {
            UI_SoundEffect();
            EnterCoursor(nextidx);      
        }
    }
}