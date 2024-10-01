using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;

namespace TwinTower{

    public class GimmikPressurePlate : GimmikBase {

        bool isLaunch = false;

        public override void Active() 
        {
            if (!isLaunch)
            {
                isLaunch = true;

                ManagerSet.Sound.Play("Button_Click_SFX", Define.Sound.Effect);
                ManagerSet.Sound.Play("arrow_launch/Arrow_Launch_SFX");
            }
        }
    }

}
