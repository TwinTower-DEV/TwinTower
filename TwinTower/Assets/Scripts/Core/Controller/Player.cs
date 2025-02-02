using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TwinTower
{
    public class Player : MoveControl
    {
        private Animator _animator;
        public Define.MoveDir dir;
        
        public Define.MoveDir Dir
        {
            get { return dir; }
            set
            {
                
            }
        }

        protected void Awake()
        {
            GameManager.Instance.Player(this);
            InputManager.Create();
            _animator = GetComponent<Animator>();
            dir = Define.MoveDir.Down;
        }

        protected override void OnBeforeReciveMove(Define.MoveDir dir)
        {
            dir = map.GetOriginDir(dir);
            switch (dir)
            {
                case Define.MoveDir.Down:
                    _animator.Play("Down_Run");
                    break;
                case Define.MoveDir.Left:
                    _animator.Play("Left_Run");
                    break;
                case Define.MoveDir.Right:
                    _animator.Play("Right_Run");
                    break;
                case Define.MoveDir.Up:
                    _animator.Play("Up_Run");
                    break;
            }
        }

        protected async override UniTask OnAfterMove()
        {
            map.ShowInvisibleWalls(x, y);
            await base.OnAfterMove();
        }

        // public override void ReduceHealth()
        // {
        //     GameManager.Instance.Restart();
        // }

        protected override void MoveSoundStart()
        {
            SoundManager.Instance.Play("character_walk/Character_Walk_SFX");
        }

        public override async UniTask Death()
        {
            InputController.Instance.ReleaseControl();
            _animator.Play("Dead");
            SoundManager.Instance.Play("character_death/Character_die_SFX(넘어졌을때 사운드)");
            await UniTask.Delay(1000);
            GameManager.Instance.Restart();
        }
    }
}