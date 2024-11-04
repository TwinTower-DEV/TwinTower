using System;
using UnityEngine;

namespace TwinTower
{
    public class UIInputController: Manager<UIInputController>
    {
        private Action InputHandler;

        public void SetHandler(Action Handler)
        {
            InputHandler = Handler;
        }
        
        public void Update()
        {
            if (!Input.anyKey) return;
            if (InputHandler == null) return;
            
            InputHandler.Invoke();
        }
    }
}