using UnityEngine;

namespace TwinTower
{
    public class Player : MoveControl
    {
        private Animator _animator;
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        
        
    }
}