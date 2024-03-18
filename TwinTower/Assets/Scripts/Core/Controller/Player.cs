﻿using System;
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
                    case Define.MoveDir.None:
                        if (dir == Define.MoveDir.Down)
                        {
                            _animator.Play("Down_Idle");
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
                                _animator.Play("Up_Idle");
                        }
                        break;
                }
                dir = value;
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            InputManager.Create();
            Debug.Log("adadasd");
            _animator = GetComponent<Animator>();
        }
    }
}