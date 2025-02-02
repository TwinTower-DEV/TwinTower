using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TwinTower
{
    public class GimmikInvisibleWall : GimmikBase
    {
        public SpriteRenderer sprite;
        public void SetInvisible(int x, int y)
        {
            switch (GetDistance(x, y))
            {
                case 0:
                    sprite.color = new Color(1, 1, 1, 0.25f);
                    break;
                case 1:
                    sprite.color = new Color(1, 1, 1, 0.5f);
                    break;
                default:
                    sprite.color = new Color(1, 1, 1, 1);
                    break;
            }
        }

        public int GetDistance(int x, int y)
        {
            int deltaX = Mathf.Abs(x - this.x);
            int deltaY = Mathf.Abs(y - this.y);
            return Mathf.Max(deltaX, deltaY);
        }
    }
}