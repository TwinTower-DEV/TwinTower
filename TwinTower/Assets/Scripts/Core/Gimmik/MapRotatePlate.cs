using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 맵 회전 기믹을 정의한 클래스입니다.
/// </summary>
namespace TwinTower
{
    public class MapRotatePlate: MonoBehaviour
    {
        [SerializeField] private bool isplayermapCheck; // Player1, 2 어떤 맵에 있는지 체크
        [SerializeField] private bool isOppositionCheck; // 반대편 맵을 돌려야 하는지 체크
        [SerializeField] public GameObject player1maprotatecenter; // 맵을 돌릴 때 중심점 재설정
        [SerializeField] public GameObject player2maprotatecenter; 
        [SerializeField] public GameObject player1maprotateObj; // 돌릴 맵 오브젝트 설정
        [SerializeField] public GameObject player2maprotateObj;
        
        public List<MoveControl> _player1mapMoveobj;
        public List<MoveControl> _player2mapMoveobj;
        private bool onPlayer = false;
        private void Awake()
        {
            
        }

        private void FixedUpdate()
        {
            
        }
        // 페이드 인, 아웃 효과와 함께 맵 회전 실행
        IEnumerator RotateStart()
        {
            GameObject rotateObj;
            GameObject rotatecenter;
            // 기획서에 적힌 대로 이 기믹이 플레이어 1 맵에 있는지, 2 맵에 있는지와, 반대 맵을 회전시키는지를 체크하는 코드
            if (!isplayermapCheck)
            {
                if (isOppositionCheck)
                {
                    rotateObj = player2maprotateObj;
                    rotatecenter = player2maprotatecenter;
                }
                else
                {
                    rotateObj = player1maprotateObj;
                    rotatecenter = player1maprotatecenter;
                }
            }
            else
            {
                if (isOppositionCheck)
                {
                    rotateObj = player1maprotateObj;
                    rotatecenter = player1maprotatecenter;
                }
                else
                {
                    rotateObj = player2maprotateObj;
                    rotatecenter = player2maprotatecenter;
                }
            }
            // 플레이어 Animation Idle로 바꾸기
            InputManager.Instance.islockMove = true;
            GameManager.Instance._player1.Dir = Define.MoveDir.None;
            GameManager.Instance._player2.Dir = Define.MoveDir.None;

            yield return StartCoroutine(UI_ScreenFader.FadeScenOut());
            
            // 맵 회전
            rotateObj.transform.RotateAround(rotatecenter.transform.position, Vector3.forward, -90);
            
            // MoveControl을 가진 오브젝트들 cellpos 재설정
            foreach (MoveControl now in GameManager.Instance._moveobjlist)
            {
                if(now == null) continue;
                
                now.SetCellPos(now.gameObject.transform.position);
                now.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            yield return StartCoroutine(UI_ScreenFader.FadeSceneIn());
            InputManager.Instance.islockMove = false;
        }
        // Player가 밟았을 때 실행
        private void OnTriggerStay2D(Collider2D other)
        {
            if (onPlayer) return;
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !other.gameObject.GetComponent<Player>().getIsMove())
            {
                onPlayer = true;
                StartCoroutine(RotateStart());
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            onPlayer = false;
        }

    }
}