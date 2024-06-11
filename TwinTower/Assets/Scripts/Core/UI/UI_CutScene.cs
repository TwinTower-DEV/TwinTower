using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TwinTower
{
    public class UI_CutScene : UI_Base
    {
        enum Images
        {
            Irise_Image,
            Dalia_Image,
            Chat
        }

        enum Texts
        {
            NameText,
            ChatText
        }   

        private List<string> scripts;
        private int script_idx;
        [SerializeField] private AudioClip BGM;
        private Animator _anim;
        
        // 0 : 기본, 1 : 놀람, 2 : 슬픔, 3 : 의문
        [SerializeField] private List<Sprite> _irisesprites;
        [SerializeField] private List<Sprite> _daliasprites;
        public override void Init()
        {
            SoundManager.Instance.SetReduceVolume();
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));

            //_anim = Get<Image>((int)Images.Chat).gameObject.GetComponent<Animator>();
            //_anim.SetBool("Start", true);
            UIManager.Instance.InputHandler += KeyInput;
            if (DataManager.Instance.StageInfovalue.cutsceneflug != null)
            {
                script_idx = 0;
                scripts = DataManager.Instance.Scripstvalue;
            }
            
            Canvas canvas = GetComponent<Canvas>();
            if (canvas != null)
                canvas.sortingOrder = 31;
            NextScript();
        }

        private void KeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (script_idx == scripts.Count)
                {
                    EndEvent();
                }
                else
                {
                    NextScript();
                }
            }
        }

        private void EndEvent()
        {
            UIManager.Instance.iscutSceenCheck = false;
            if (!UI_ScreenFader.FadeCheck())
            {
                UIManager.Instance.InputHandler -= KeyInput;
                //Time.timeScale = 1;
                SoundManager.Instance.SetReduceVolume();
                UIManager.Instance.CloseFieldCutSceneUI(this);
            }
            else
            {
                UIManager.Instance.InputHandler -= KeyInput;
                //Time.timeScale = 1;
                StartCoroutine(UI_ScreenFader.FadeSceneIn());
                SoundManager.Instance.SetReduceVolume();
                SoundManager.Instance.ChangeBGM(BGM);
                UIManager.Instance.CloseNormalUI(this);
            }
        }

        private void ActivateCheck(Image obj, float alpha)
        {
            if (obj.gameObject.activeSelf)
            {
                Color c = obj.color;
                c.a = alpha;
                obj.color = c;
            }
            else 
                obj.gameObject.SetActive(true);
        }
        
        private void NextScript()
        {
            string [] now = scripts[script_idx].Split(':');
            if (now[1].Length >= 2)
            {
                ActivateCheck(Get<Image>((int)Images.Irise_Image), 1f);
                ActivateCheck(Get<Image>((int)Images.Dalia_Image), 1f);
                Get<Image>((int)Images.Irise_Image).gameObject.GetComponent<Image>().sprite =
                    _irisesprites[int.Parse(now[2])];
                Get<Image>((int)Images.Dalia_Image).gameObject.GetComponent<Image>().sprite =
                    _daliasprites[int.Parse(now[2])];
            }
            else
            {
                if (now[1] == "1")
                {
                    ActivateCheck(Get<Image>((int)Images.Irise_Image), 1f);
                    Get<TextMeshProUGUI>((int)Texts.NameText).gameObject.GetComponent<TextMeshProUGUI>().text = "아이리스";
                    Get<Image>((int)Images.Irise_Image).gameObject.GetComponent<Image>().sprite =
                        _irisesprites[int.Parse(now[2])];

                    if (Get<Image>((int)Images.Dalia_Image).gameObject.GetComponent<Image>().color.a > 0)
                    {
                        Get<Image>((int)Images.Dalia_Image).gameObject.GetComponent<Image>().sprite =
                            _daliasprites[0];
                        ActivateCheck(Get<Image>((int)Images.Dalia_Image), 0.5f);
                    }
                }
                else if (now[1] == "2")
                {
                    ActivateCheck(Get<Image>((int)Images.Dalia_Image), 1f);

                    Get<TextMeshProUGUI>((int)Texts.NameText).gameObject.GetComponent<TextMeshProUGUI>().text = "달리아";
                    Get<Image>((int)Images.Dalia_Image).gameObject.GetComponent<Image>().sprite =
                        _daliasprites[int.Parse(now[2])];
                    if (Get<Image>((int)Images.Irise_Image).gameObject.GetComponent<Image>().color.a > 0)
                    {
                        Get<Image>((int)Images.Irise_Image).gameObject.GetComponent<Image>().sprite =
                            _irisesprites[0];
                        ActivateCheck(Get<Image>((int)Images.Irise_Image), 0.5f);
                    }
                }
                else
                {
                    Get<TextMeshProUGUI>((int)Texts.NameText).gameObject.GetComponent<TextMeshProUGUI>().text = "???";
                    if (Get<Image>((int)Images.Irise_Image).gameObject.GetComponent<Image>().color.a > 0)
                    {
                        Get<Image>((int)Images.Irise_Image).gameObject.GetComponent<Image>().sprite =
                            _irisesprites[0];
                        ActivateCheck(Get<Image>((int)Images.Irise_Image), 0.5f);
                    }

                    if (Get<Image>((int)Images.Dalia_Image).gameObject.GetComponent<Image>().color.a > 0)
                    {
                        Get<Image>((int)Images.Dalia_Image).gameObject.GetComponent<Image>().sprite =
                            _daliasprites[0];
                        ActivateCheck(Get<Image>((int)Images.Dalia_Image), 0.5f);
                    }
                }
            }

            Get<TextMeshProUGUI>((int)Texts.ChatText).gameObject.GetComponent<TextMeshProUGUI>().text = now[0];
            script_idx++;
        }
    }
}