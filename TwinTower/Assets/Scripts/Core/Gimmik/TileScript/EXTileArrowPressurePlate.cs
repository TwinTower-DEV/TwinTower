using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;

public class EXTileArrowPressurePlate : EXTile {

    public ActivateObject dispenser;

    bool isLaunch = false;

    public override void Active() 
    {
        if (!isLaunch)
        {
            isLaunch = true;

            SoundManager.Instance.Play("Button_Click_SFX", Define.Sound.Effect);
            dispenser.Launch();
            SoundManager.Instance.Play("arrow_launch/Arrow_Launch_SFX");
            color = new Color(140f/255f, 140f/255f, 140f/255f);
        }
    }
}
