using System;
using System.IO;

namespace TwinTower
{
    public abstract class GamaData
    {
        public string[] names;
        public string[] value;

        public virtual void Set()
        {
            
        }
    }
    
    public class UIGameData : GamaData
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
            bgmcoursor = Int32.Parse(names[0]);
            secursor = Int32.Parse(names[1]);
            displaymodecursor = Int32.Parse(names[2]);
            displaycursor = Int32.Parse(names[3]);
            langaugecursor = Int32.Parse(names[4]);
        }
    }
}