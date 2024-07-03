namespace TwinTower
{
    /// <summary>
    /// Define 클래스입니다. enum들을 관리합니다.
    /// </summary>
    public class Define
    {
        public enum MoveDir
        {
            Up,
            Down,
            Left,
            Right,
            Die,
            None
        }
        public enum UIEvent
        {
            Click,
            Drag,
            EndDrag,
            Enter,
            Exit
        }
        public enum Sound
        {
            Bgm,
            Effect,
            MaxCount,
        }

        public struct Language
        {
            public string kor;
            public string eng;
        }
        public struct Resolution
        {
            public int width;
            public int height;
        }
    }
}