using UnityEngine;

namespace TwinTower
{
    public class ManagerSet: MonoBehaviour
    {
        static ManagerSet s_instance;
        static ManagerSet Instance { get { Init(); return s_instance; } }

        private DataManager _data = new DataManager();
        private GameManager _game = new GameManager();
        private ResourceManager _resource = new ResourceManager();
        private SoundManager _sound = new SoundManager();
        private UIManager _ui = new UIManager();
        
        public static DataManager Data { get { return Instance._data; } }
        public static ResourceManager Resource { get { return Instance._resource; } }
        public static SoundManager Sound { get { return Instance._sound; } }
        public static UIManager UI { get { return Instance._ui; } }
        public static GameManager Gamemanager { get { return Instance._game; } }
        static void Init()
        {
            if (s_instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<ManagerSet>();
                }
            
                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<ManagerSet>();
                s_instance._data.Init();
                s_instance._sound.Init();
                s_instance._ui.Init();
                s_instance._game.Init();
                //s_instance._gameManage.Init();
            }		
        }
        
    }
}