using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;
//using Debug = System.Diagnostics.Debug;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// 이동 가능한 모든 오브젝트들이 상속 받는 클래스.
/// 다음칸에 이동 가능한지 확인과 이동 명령을 받는 함수 존재.
/// </summary>
namespace TwinTower
{
    public class MoveControl : MonoBehaviour
    {
        [SerializeField] public int _moveSpeed;
        [SerializeField] protected int Health;
        
        [SerializeField] protected LayerMask _layerMask;
        public bool isMove = false;
        private Vector3 destPos;
        
        // movedir방향으로 이동 가능한지 체크 - 이동 가능하다면 true반환
        // layermask를 통해 다음 칸에 있는 오브젝트에 따라 확인됨.
        public virtual bool MoveCheck(Vector3 movedir) {
            if (isMove) return false;
            if (movedir == Vector3.zero) return false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + movedir * 0.4f , movedir, 0.5f, _layerMask);
            if (hit.collider == null) return true;
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Box")) {
                MoveControl boxcontrol = hit.transform.gameObject.GetComponent<MoveControl>();
                if (boxcontrol.MoveCheck(movedir)) return true;
            }
            return false;
        }
        
        // 이동 하고자 하는 방향 설정과 이동 가능함을 표시
        public void DirectSetting(Vector3 movedir) {
            if (movedir == Vector3.zero) return;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + movedir * 0.5f , movedir, 0.5f, _layerMask);
            if (hit.collider != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Box")) {
                MoveControl boxcontrol = hit.transform.gameObject.GetComponent<MoveControl>();
                boxcontrol.DirectSetting(movedir);
            }
            isMove = true;
            destPos = transform.position + movedir;
        }

        public virtual void ReduceHealth()
        {
            InputController.Instance.ReleaseControl();
            StartCoroutine(ScreenManager.Instance.CurrentScreenReload());
            InputController.Instance.GainControl();
        }
        
        
        // 그 이동할 셀로 자연스럽게 이동하게 구현
        /*IEnumerator UpdateIsMoveing(Vector3 movedir)
        {
            isMove = true;
            Debug.Log("몇번들어옴?" + name);
            Vector3 destPos = transform.position + movedir;
            while (isMove) {
                Vector3 leftLength = destPos - transform.position;
                float dist = leftLength.magnitude;
                Debug.Log(destPos + " " + transform.position + " " + leftLength + " " + dist + " " + (dist < _moveSpeed * Time.deltaTime) + " " + movedir + " " + name);
                if (dist < _moveSpeed * Time.deltaTime)
                {
                    Debug.Log("여기는?");
                    transform.position = TileFindManager.Instance.gettileCentorLocation(destPos);
                    isMove = false;
                }
                else
                {
                    transform.position += movedir * _moveSpeed * Time.deltaTime;
                    isMove = true;
                }
                yield return new WaitForFixedUpdate();
            }
        }*/

        // 현재 이동 중인지 확인을 위함
        public bool getIsMove() {
            return isMove;
        }
        
        // 오브젝트의 현재 tilemap기준 Cell 위치 확인을 위함.
        protected virtual void Awake() {
            transform.position = TileFindManager.Instance.gettileCentorLocation(transform.position);
        }

        private void Update() {
            if(isMove) {
                Vector3 leftLength = destPos - transform.position;
                float dist = leftLength.magnitude;
                if (dist < _moveSpeed * Time.deltaTime) {
                    transform.position = TileFindManager.Instance.gettileCentorLocation(destPos);
                    isMove = false;
                }
                else {
                    transform.position += leftLength.normalized * _moveSpeed * Time.deltaTime;
                    isMove = true;
                }
            }
        }
    }
}