using System;
using System.Collections.Generic;
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
        
    }
}