using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;

namespace TwinTower{

    public class GimmikButton : GimmikBase {
        public bool isLaunchOnce;
        bool isLaunch = false;

        public override void Active() 
        {
            if (!isLaunch)
            {
                if (isLaunchOnce == true) // isLaunchOnce가 True이면 1번만 동작할 수 있는 것이므로 isLaunch를 true로 함. 아닌 경우에는 누를때마다 동작해야하므로 true 처리
                {
                    isLaunch = true;
                }
                linkedObject.Active();
            }
        }

        public override void DeActive() 
        {
            linkedObject.DeActive();
        }
    }

}
