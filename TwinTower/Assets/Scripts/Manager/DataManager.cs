using System;
using System.Reflection;
using UnityEngine;

namespace TwinTower
{
    public class DataManager : Manager<DataManager>
    {
        

        private UIGameData _uiGameData;

        public UIGameData UIGameDatavalue
        {
            get
            {
                if(_uiGameData != null)
                    return _uiGameData;
                else
                {
                    _uiGameData = new UIGameData(2, 2, 0, 2, 0);
                    return _uiGameData;
                }
            }
            set
            {
                _uiGameData = value;
                SaveData(_uiGameData);
            }
        }
        protected override void Awake()
        {
            base.Awake();
            InitDataSetting();
        }

        private void InitDataSetting()
        {
            _uiGameData = new UIGameData(2, 2, 0, 2, 0);  
            LoadData(_uiGameData);
        }

        private void SaveData<T>(T data) where T : GamaData
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].FieldType == typeof(int))
                {
                    PlayerPrefs.SetInt(data.names[i], Int32.Parse(data.value[i]));
                }
                else if (fields[i].FieldType == typeof(string))
                {
                    PlayerPrefs.SetString(data.names[i], data.value[i]);
                }
            }
        }

        private void LoadData<T>(T data) where T : GamaData
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            for (int i = 0; i < fields.Length; i++)
            {
                if (!PlayerPrefs.HasKey(data.names[i])) continue;
                
                if (fields[i].FieldType == typeof(int))
                {
                    data.names[i] = PlayerPrefs.GetInt(data.names[i]).ToString();
                }
                else if (fields[i].FieldType == typeof(string))
                {
                    data.names[i] = PlayerPrefs.GetString(data.names[i]);
                }
            }
            data.Set();
        }
    }
}