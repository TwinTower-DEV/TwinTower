using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace TwinTower.Tiles
{
    [Serializable]
    public class Map
    {
        public int x;
        public int y;
        public int ActiveNumber;
        public int spareX;
        public int spareY;
    }
    [Serializable]
    public class MapInfo : ILoad<int, List<Map>>
    {
        public List<Map> _mapInfolist = new List<Map>();

        public Dictionary<int, List<Map>> MakeDic()
        {
            Dictionary<int, List<Map>> _dictionary = new Dictionary<int, List<Map>>();
            Debug.Log(_mapInfolist.Count);
            _dictionary.Add(1, _mapInfolist);

            return _dictionary;
        }
    }
}