using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwinTower
{
    public class TileFindTestCode: MonoBehaviour
    {
        public void Start()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Debug.Log(TileFindManager.Instance.player1Map[i,j].x + " " + TileFindManager.Instance.player1Map[i,j].y);
                }
            }
        }

        public void FixedUpdate()
        {
            
        }
    }
}