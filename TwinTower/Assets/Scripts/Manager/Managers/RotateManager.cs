using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinTower
{
    public class RotateManager : Manager<RotateManager>
    {
        private Queue<IEnumerator> _rotateQueue = new Queue<IEnumerator>();
        private bool runCheck = false;
        public void PushQueue(IEnumerator action)
        {
            _rotateQueue.Enqueue(action);
            if(!runCheck)
                Actions();
        }

        public void Actions()
        {
            if (_rotateQueue.Count > 0)
            {
                IEnumerator _action = _rotateQueue.Peek();
                _rotateQueue.Dequeue();
                runCheck = true;
                StartCoroutine(Enter(_action));
            }
        }

        public IEnumerator Enter(IEnumerator action)
        {
            yield return StartCoroutine(action);
            
            if (_rotateQueue.Count > 0)
            {
                IEnumerator aciond = _rotateQueue.Peek();
                _rotateQueue.Dequeue();

                yield return StartCoroutine(aciond);
            }

            runCheck = false;
        }
    }
}