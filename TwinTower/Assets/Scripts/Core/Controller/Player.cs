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
            dir = map.GetCurrentDir(dir);
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
                // case Define.MoveDir.Die:
                //     _animator.Play("Dead");
                //     SoundManager.Instance.Play("character_death/Character_die_SFX(넘어졌을때 사운드)");
                //     break;
                case Define.MoveDir.None:
                    // if (dir == Define.MoveDir.Up)
                    // {
                    //     _animator.Play("Up_Idle");
                    // }
                    // else if (dir == Define.MoveDir.Left)
                    // {
                    //     _animator.Play("Left_Idle");
                    // }
                    // else if (dir == Define.MoveDir.Right)
                    // {
                    //     _animator.Play("Right_Idle");
                    // }
                    // else
                    // {
                    //     _animator.Play("Down_Idle");
                    // }
                    break;
            }
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