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
        private Tilemap rotateTileMap;                         // 회전할 Tilemap
        private Tilemap wallRenderer;
        
        [SerializeField] public bool isOpp;
        [SerializeField] private float rotateSpeed = 100;                        // 회전 속도
        [SerializeField] private float transperencySpeed = 500f;                // 벽 투명해지는 속도
        
        private Vector2 boxSize = new Vector2(8f, 8f);
        private float distDegree;

        private bool isDegree45 = false;
        private Vector3 rotationDir = Vector3.back;
        private Vector2 rotationCenter;
        private List<Collider2D> rotatableObject;      // 같이 회전되는 오브젝트들(무빙워크, 발사대)
        private List<Collider2D> unRotatableObject;    // 회전되지 않는 오브젝트들
        
        private void Awake() {
            //rotateTileMap = TileFindManager.Instance.getTileInArea(transform.position, isOpp);
            rotationCenter = rotateTileMap.transform.position;
            wallRenderer = rotateTileMap.transform.GetChild(0).GetComponent<Tilemap>();
        }

        // 발사
        public void Launch() {

        }

        // 회전
        IEnumerator RotateStart() {
            distDegree = rotateTileMap.transform.rotation.eulerAngles.z - 90;
            InputController.Instance.ReleaseControl();                  // 키 입력 막기
            ManagerSet.Gamemanager.isRotateCheck = true;
            // 여기는 Wall 희미하게 변화해주는곳
            while (wallRenderer.color.a > 0) {
                Color color = wallRenderer.color;
                color.a -= transperencySpeed * 0.1f * Time.deltaTime;
                wallRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }
            
            // 45도에 회전률 변경을 위한 변수 설정
            float rotateAngle = 0;
            
            // 여기에서부터 회전 시작.
            while (true) {
                rotateAngle += rotateSpeed * Time.deltaTime;
                rotateTileMap.transform.RotateAround(rotationCenter, rotationDir, rotateSpeed * Time.deltaTime);
                foreach (var collider in rotatableObject) {
                    collider.transform.RotateAround(rotationCenter, rotationDir, rotateSpeed * Time.deltaTime);
                }
                foreach (var collider in unRotatableObject) {
                    collider.transform.RotateAround(rotationCenter, rotationDir, rotateSpeed * Time.deltaTime);
                }

                // 45도 정도 회전했을 때 FitRotateObject발동
                if (!isDegree45 && rotateAngle >= 45) FitRotateObject(); 
                
                // 90도 이상 되었을때 종료 및 정각(0, 90, 180, 270)을 유지
                if (rotateAngle >= 90) {
                    rotateTileMap.transform.rotation = Quaternion.Euler(0, 0, distDegree);
                    foreach (var collider in rotatableObject) {
                        collider.transform.rotation = Quaternion.Euler(collider.transform.rotation.eulerAngles.x, collider.transform.rotation.eulerAngles.y, FindClosestDegree(collider.transform.rotation.eulerAngles.z));
                    }
                    foreach (var collider in unRotatableObject) {
                        collider.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
            
            // 벽 투명화 풀기
            Color _color = wallRenderer.color;
            _color.a = 1;
            wallRenderer.color = _color;
            wallRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);

            // 타일의 정 중앙에 배치
            foreach (var collider in rotatableObject) {
                //collider.transform.position = TileFindManager.Instance.gettileCentorLocation(collider.transform.position);
            }
            foreach (var collider in unRotatableObject) {
                //collider.transform.position = TileFindManager.Instance.gettileCentorLocation(collider.transform.position);
            }

            isDegree45 = false;
            InputController.Instance.GainControl();             // 키 입력 풀기
            ManagerSet.Gamemanager.isRotateCheck = false;
        }

        // 45도가 되었을때 회전되면 안되는 오브젝트들은 원래의 회전률로 돌리기
        // 타일맵도 원상복구
        // 발사대의 경우 sprite의 변경 요청까지
        public void FitRotateObject() {
            isDegree45 = true;
            // 회전 되면 안되는 오브젝트들 회전(player, box, 버튼들)
            foreach (var collider in unRotatableObject) {
                Vector3 currDegree = collider.transform.rotation.eulerAngles;
                currDegree.z += 90; 
                collider.transform.rotation = Quaternion.Euler(currDegree);
            }

            // 발사대 회전
            /*foreach (var collider in rotatableObject) {
                DispenserShoot arrowdisepenser = collider.GetComponent<DispenserShoot>();
                if (arrowdisepenser != null) {
                    arrowdisepenser.prevDirection();
                }
            }*/
            
            // 타일맵 회전
            Quaternion rotation = Quaternion.Euler(0, 0, -distDegree); // 90도 회전
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);

            foreach (Vector3Int pos in rotateTileMap.cellBounds.allPositionsWithin) {
                if (!rotateTileMap.HasTile(pos)) continue;
                rotateTileMap.SetTransformMatrix(pos, matrix);
            }
        }
        
        // 0, 360, 180, 270 과 가까운 각도로 변환
        public float FindClosestDegree(float target) {
            float[] numbers = { 90, 180, 270, 360, 0 };
            float closestNumber = numbers[0];
            float minDifference = Math.Abs(target - closestNumber);

            foreach (float number in numbers) {
                float difference = Math.Abs(target - number);
                if (difference < minDifference) {
                    minDifference = difference;
                    closestNumber = number;
                }
            }

            return closestNumber;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            // MoveControl moveableController = other.GetComponent<MoveControl>();
            // if (moveableController != null && moveableController.getIsMove()) {
            //     ManagerSet.Sound.Play("Button_Click_SFX", Define.Sound.Effect);
            //     Launch();
            // }
        }
    }
}