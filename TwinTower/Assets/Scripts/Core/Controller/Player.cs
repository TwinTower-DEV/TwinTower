using System;
using System.Collections;
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
                if (dir == value) return;

                switch (value)
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
                    case Define.MoveDir.Die:
                        _animator.Play("Dead");
                        break;
                    case Define.MoveDir.None:
                        if (dir == Define.MoveDir.Up)
                        {
                            _animator.Play("Up_Idle");
                        }
                        else if (dir == Define.MoveDir.Left)
                        {
                            _animator.Play("Left_Idle");
                        }
                        else if (dir == Define.MoveDir.Right)
                        {
                            _animator.Play("Right_Idle");
                        }
                        else
                        {
                            _animator.Play("Down_Idle");
                        }
                        break;
                }
                dir = value;
            }
        }

        public override void ReduceHealth()
        {
            Dir = Define.MoveDir.Die;
            InputController.Instance.ReleaseControl();
            StartCoroutine(ScreenManager.Instance.CurrentScreenReload());
        }

        protected override void Awake()
        {
            base.Awake();
            InputManager.Create();
            _animator = GetComponent<Animator>();
            dir = Define.MoveDir.Down;
        }
        
    }
}