using UnityEngine;

namespace TwinTower
{
    public class ManagerSet: MonoBehaviour
    {
        static ManagerSet s_instance; // 유일성이 보장된다
        static ManagerSet Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

        private DataManager _data = new DataManager();
        private GameManager _game = new GameManager();
        private InputManager _input = new InputManager();
        private ResourceManager _resource = new ResourceManager();
        private SoundManager _sound = new SoundManager();
        private UIManager _ui = new UIManager();
        
        public static DataManager Data { get { return Instance._data; } }
        public static ResourceManager Resource { get { return Instance._resource; } }
        public static SoundManager Sound { get { return Instance._sound; } }
        public static UIManager UI { get { return Instance._ui; } }
        public static InputManager Input { get { return Instance._input; } }
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
                s_instance._input = new InputManager();
                s_instance._input.Init();
                s_instance._sound.Init();
                s_instance._ui.Init();
                //s_instance._gameManage.Init();
            }		
        }
        
    }
}