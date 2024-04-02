using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

/// <summary>
/// 맵 회전 기믹을 정의한 클래스입니다.
/// </summary>
namespace TwinTower
{
    public class MapRotatePlate: MonoBehaviour {
        [SerializeField] private Tilemap rotateTileMap;
        
        [SerializeField] private bool isLeft;
        [SerializeField] private bool isOpp;
        [SerializeField] private float rotateSpeed = 40;
        [SerializeField] private float transperencySpeed = 500f;
        
        private Vector2 boxSize = new Vector2(7.5f, 7.5f);
        private float distDegree;

        private bool isDegree45 = false;
        
        public void Launch() {
            if (!isOpp) distDegree = rotateTileMap.transform.rotation.eulerAngles.z - 90;
            else distDegree = rotateTileMap.transform.rotation.eulerAngles.z + 90;
            StartCoroutine(RotateStart());
        }

        IEnumerator RotateStart() {
            InputController.Instance.ReleaseControl();                  // 키 입력 막기

            // 여기는 Wall 희미하게 변화해주는곳
            Tilemap wallRenderer = rotateTileMap.transform.GetChild(0).GetComponent<Tilemap>();
            while (wallRenderer.color.a > 0) {
                Color color = wallRenderer.color;
                color.a -= transperencySpeed * 0.1f * Time.deltaTime;
                wallRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }

            Vector2 rotationCenter = rotateTileMap.transform.position;
            Collider2D[] collidersInArea = Physics2D.OverlapBoxAll(rotationCenter, boxSize, 0f);
            
            Vector3 rotationDir;
            if (isOpp) rotationDir = Vector3.forward;
            else rotationDir = Vector3.back;

            float prevDegree = rotateTileMap.transform.rotation.eulerAngles.z;
            
            while (true) {
                rotateTileMap.transform.RotateAround(rotationCenter, rotationDir, rotateSpeed * Time.deltaTime);
                foreach (var collider in collidersInArea) {
                    collider.transform.RotateAround(rotationCenter, rotationDir, rotateSpeed * Time.deltaTime);
                }
                float currDegree = rotateTileMap.transform.rotation.eulerAngles.z;

                float diffDegree = Mathf.Abs(currDegree - prevDegree);
                if (diffDegree >= 180f) diffDegree = 360 - diffDegree;

                if (!isDegree45 && diffDegree >= 45) FitRotateObject(collidersInArea);
                if (diffDegree >= 90) {
                    rotateTileMap.transform.rotation = Quaternion.Euler(0, 0, distDegree);
                    foreach (var collider in collidersInArea) {
                        if (collider.gameObject.layer == LayerMask.NameToLayer("Rotatable")
                            || collider.gameObject.layer == LayerMask.NameToLayer("Wall")) {
                            collider.transform.rotation = Quaternion.Euler(0, 0, distDegree);
                        }
                        else collider.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
            
            Color _color = wallRenderer.color;
            _color.a = 1;
            wallRenderer.color = _color;

            isDegree45 = false;
            
            InputController.Instance.GainControl();             // 키 입력 풀기
        }

        public void FitRotateObject(Collider2D[] collidersInArea) {
            isDegree45 = true;
            Debug.Log("들어옴?");
            foreach (var collider in collidersInArea) {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Rotatable")
                    || collider.gameObject.layer == LayerMask.NameToLayer("Wall")) {
                    if (collider.GetComponent<DispenserShoot>()) {
                        
                    }
                }
                else {
                    Vector3 currDegree = collider.transform.rotation.eulerAngles;
                    if (isOpp) currDegree.z -= 90; 
                    else currDegree.z += 90; 
                    collider.transform.rotation = Quaternion.Euler(currDegree);
                }
            }
            Quaternion rotation = Quaternion.Euler(0, 0, -distDegree); // 90도 회전
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);

            foreach (Vector3Int pos in rotateTileMap.cellBounds.allPositionsWithin) {
                if (!rotateTileMap.HasTile(pos)) continue;
                rotateTileMap.SetTransformMatrix(pos, matrix);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            MoveControl moveableController = other.GetComponent<MoveControl>();
            if (moveableController != null) Launch();
        }
    }
}