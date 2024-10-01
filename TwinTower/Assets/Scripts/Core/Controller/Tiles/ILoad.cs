using System.Collections.Generic;

namespace TwinTower.Tiles
{
    public interface ILoad<Key, Value>
    {
        public Dictionary<Key, Value> MakeDic();
    }
}