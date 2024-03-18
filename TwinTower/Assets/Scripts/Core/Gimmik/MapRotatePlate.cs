using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 맵 회전 기믹을 정의한 클래스입니다.
/// </summary>
namespace TwinTower
{
    public class MapRotatePlate: MonoBehaviour
    {
        
        public class TileInfo
        {
            public Vector3Int pos;
            public TileBase _tile;

            public TileInfo(Vector3Int pos, TileBase _tile)
            {
                this.pos = pos;
                this._tile = _tile;
            }
        }       
        
        [SerializeField] public GameObject maprotatecenter; 
        [SerializeField] public Tilemap maprotateObj;
        
        private bool onPlayer = false;
        private void Awake()
        {
            GameManager.Instance._moveobjlist.Add(gameObject);
        }

        private void FixedUpdate()
        {
            
        }
        // 페이드 인, 아웃 효과와 함께 맵 회전 실행
        IEnumerator RotateStart()
        {
            
            // 플레이어 Animation Idle로 바꾸기
            GameManager.Instance._player1.Dir = Define.MoveDir.None;
            GameManager.Instance._player2.Dir = Define.MoveDir.None;

            yield return StartCoroutine(UI_ScreenFader.FadeScenOut());
            
            // 맵 회전
            maprotateObj.gameObject.transform.RotateAround(maprotatecenter.gameObject.transform.position, Vector3.forward, -90);
            
            Quaternion rotation = Quaternion.Euler(0, 0, -maprotateObj.transform.rotation.eulerAngles.z); // 90도 회전
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);

            foreach (Vector3Int pos in maprotateObj.cellBounds.allPositionsWithin) {
                if (!maprotateObj.HasTile(pos)) continue;
                maprotateObj.SetTransformMatrix(pos, matrix);
            }
            
            // MoveControl을 가진 오브젝트들 cellpos 재설정
            foreach (GameObject now in GameManager.Instance._moveobjlist)
            {
                if(now == null) continue;


                if (now.GetComponent<MoveControl>() != null)
                {
                    now.GetComponent<MoveControl>().SetCellPos(now.gameObject.transform.position);
                }

                now.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            
            yield return StartCoroutine(UI_ScreenFader.FadeSceneIn());
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