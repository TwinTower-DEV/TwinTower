using System;
using System.IO;

namespace TwinTower
{
    public abstract class GameData
    {
        public string[] names;
        public string[] value;

        public virtual void Set()
        {
            
        }
    }
    
    public class UIGameData : GameData
    {
        public int bgmcoursor;
        public int secursor;
        public int displaymodecursor;
        public int displaycursor;
        public int langaugecursor;
        public UIGameData(int bgmcoursor, int secursor, int displaymodecursor, int displaycursor,
            int langaugecursor)
        {
            this.bgmcoursor = bgmcoursor;
            this.secursor = secursor;
            this.displaymodecursor = displaymodecursor;
            this.displaycursor = displaycursor;
            this.langaugecursor = langaugecursor;
            names = new string[5]
                { "BGMCursor", "SECursor", "DisplayModeCursor", "DisplayCursor", "LangaugeCursor" };
            value = new string[5]
            {
                this.bgmcoursor.ToString(), this.secursor.ToString(), this.displaymodecursor.ToString(),
                this.displaycursor.ToString(), this.langaugecursor.ToString()
            };
        }

        public override void Set()
        {
            bgmcoursor = Int32.Parse(value[0]);
            secursor = Int32.Parse(value[1]);
            displaymodecursor = Int32.Parse(value[2]);
            displaycursor = Int32.Parse(value[3]);
            langaugecursor = Int32.Parse(value[4]);
        }
    }

    public class StageInfo : GameData
    {
        public int nextStage;
        public int cutsceneflug;

        public StageInfo(int nextStage, int cutsceneflug)
        {
            this.nextStage = nextStage;
            this.cutsceneflug = cutsceneflug;
            names = new string[2]
                { "NextStage", "CutSceneFlug"};
            value = new string[2]
            {
                this.nextStage.ToString(), this.cutsceneflug.ToString()
            };
        }
        
        public override void Set()
        {
            nextStage = Int32.Parse(value[0]);
            cutsceneflug = Int32.Parse(value[1]);
        }
    }
}