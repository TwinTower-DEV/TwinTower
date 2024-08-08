using System;
using System.Collections.Generic;
using NUnit.Framework;
using TwinTower.Tiles;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TwinTower
{
    public enum MapType
    {   
        None = -1,
        Left = 0,
        Right,
        All // 추후 왼,오른쪽이 없는 경우가 추가될 것을 대비해서 추가함.
    }

    public class MapManager : Manager<MapManager>
    {
        public Dictionary<int, List<Tiles.Map>> mapInfo = new Dictionary<int, List<Tiles.Map>>();

        protected override void Awake()
        {
            mapInfo = LoadJson<MapInfo, int, List<Tiles.Map>>("TileInformation").MakeDic();
        }

        Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoad<Key, Value>
        {
            TextAsset textAsset = ResourceManager.Instance.Load<TextAsset>($"Data/{path}");
            return JsonUtility.FromJson<Loader>(textAsset.text);
        }
    }
}