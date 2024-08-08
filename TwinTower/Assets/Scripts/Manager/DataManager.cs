using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TwinTower
{
    public class DataManager : Manager<DataManager>
    {
        

        private UIGameData _uiGameData;
        private StageInfo _stageInfo;
        private SaveLoadController _saveloadcontroller;
        private List<string> scripts;
        public UIGameData UIGameDatavalue
        {
            get
            {
                return _uiGameData;
            }
            set
            {
                _uiGameData = value;
                SaveData(_uiGameData);
            }
        }
        
        public List<string> Scripstvalue
        {
            get
            {
                return scripts;
            }
            set
            {
                scripts = value;
            }
        }
        
        public StageInfo StageInfovalue
        {
            get
            {
                return _stageInfo;
            }
            set
            {
                _stageInfo = value;
            }
        }

        public SaveLoadController saveload {
            get {
                return _saveloadcontroller;
            }
            set {
                _saveloadcontroller = value;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            List<Tiles.Map> list = new List<Tiles.Map>();
            if (MapManager.Instance.mapInfo.TryGetValue(1, out list) != null)
            {
            }
            foreach (Tiles.Map VARIABLE in list)
            {
                Debug.Log(VARIABLE.x + " " + VARIABLE.y + " " + VARIABLE.ActiveNumber);
            }
            InitDataSetting();
        }

        private void InitDataSetting()
        {
            _uiGameData = new UIGameData(2, 2, 0, 2, 0);
            _stageInfo = new StageInfo(0, "testtxt");            _saveloadcontroller = new SaveLoadController();
            LoadData(_uiGameData);
            LoadData(_stageInfo);
        }

        private void SaveData<T>(T data) where T : GameData
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0, k = 0; i < fields.Length - 2; i++,k++)
            {
                if (fields[i].FieldType == typeof(int))
                {
                    PlayerPrefs.SetInt(data.names[k], Int32.Parse(data.value[k]));
                }
                else if (fields[i].FieldType == typeof(string))
                {
                    PlayerPrefs.SetString(data.names[k], data.value[k]);
                }
            }
        }

        private void LoadData<T>(T data) where T : GameData
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            for (int i = 0, k = 0; i < fields.Length - 2; i++,k++)
            {

                if (!PlayerPrefs.HasKey(data.names[k])) continue;

                if (fields[i].FieldType == typeof(int))
                {
                    data.value[k] = PlayerPrefs.GetInt(data.names[k]).ToString();
                }
                else if (fields[i].FieldType == typeof(string))
                {
                    data.value[k] = PlayerPrefs.GetString(data.names[k]);
                }
            }
            data.Set();
        }
        
        public List<string> ReadText(string s)
        {
            List<string> script = new List<string>();
                
            TextAsset textfile = Resources.Load(s) as TextAsset;
            StringReader stringReader = new StringReader(textfile.text);

            while (true)
            {
                string line = stringReader.ReadLine();
                if (line == null) break;
                
                script.Add(line);
            }

            return script;
        }
    }
}