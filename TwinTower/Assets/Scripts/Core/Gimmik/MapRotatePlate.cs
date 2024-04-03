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
        private Tilemap wallRenderer;
        
        [SerializeField] private bool isOpp;
        [SerializeField] private float rotateSpeed = 40;
        [SerializeField] private float transperencySpeed = 500f;
        
        private Vector2 boxSize = new Vector2(8f, 8f);
        private float distDegree;

        private bool isDegree45 = false;
        private Vector3 rotationDir;
        private Vector2 rotationCenter;
        private List<Collider2D> rotatableObject = new List<Collider2D>();
        private List<Collider2D> unRotatableObject = new List<Collider2D>();

        private void Awake() {
            rotationCenter = rotateTileMap.transform.position;
            wallRenderer = rotateTileMap.transform.GetChild(0).GetComponent<Tilemap>();
            Collider2D[] collidersInArea = Physics2D.OverlapBoxAll(rotationCenter, boxSize, 0f);
            
            foreach(Collider2D collider in collidersInArea) {
                if (collider.GetComponent<Tilemap>() != null) continue;
                if(collider.GetComponent<MovingWalk>() != null) rotatableObject.Add(collider);
                else if(collider.GetComponent<DispenserShoot>() != null) rotatableObject.Add(collider);
                else unRotatableObject.Add(collider);
            }
            
            if (isOpp) rotationDir = Vector3.forward;
            else rotationDir = Vector3.back;
        }

        public void Launch() {
            if (!isOpp) distDegree = rotateTileMap.transform.rotation.eulerAngles.z - 90;
            else distDegree = rotateTileMap.transform.rotation.eulerAngles.z + 90;
            StartCoroutine(RotateStart());
        }

        IEnumerator RotateStart() {
            InputController.Instance.ReleaseControl();                  // 키 입력 막기

            // 여기는 Wall 희미하게 변화해주는곳
            while (wallRenderer.color.a > 0) {
                Color color = wallRenderer.color;
                color.a -= transperencySpeed * 0.1f * Time.deltaTime;
                wallRenderer.color = color;
                yield return new WaitForFixedUpdate();
            }
            
            float prevDegree = rotateTileMap.transform.rotation.eulerAngles.z;
            
            while (true) {
                rotateTileMap.transform.RotateAround(rotationCenter, rotationDir, rotateSpeed * Time.deltaTime);
                foreach (var collider in rotatableObject) {
                    collider.transform.RotateAround(rotationCenter, rotationDir, rotateSpeed * Time.deltaTime);
                }
                foreach (var collider in unRotatableObject) {
                    collider.transform.RotateAround(rotationCenter, rotationDir, rotateSpeed * Time.deltaTime);
                }
                float currDegree = rotateTileMap.transform.rotation.eulerAngles.z;

                float diffDegree = Mathf.Abs(currDegree - prevDegree);
                if (diffDegree >= 180f) diffDegree = 360 - diffDegree;

                if (!isDegree45 && diffDegree >= 45) {
                    FitRotateObject();
                }
                if (diffDegree >= 90) {
                    rotateTileMap.transform.rotation = Quaternion.Euler(0, 0, distDegree);
                    foreach (var collider in rotatableObject) {
                        collider.transform.rotation = Quaternion.Euler(0, 0, FindClosestDegree(collider.transform.rotation.eulerAngles.z));
                    }
                    foreach (var collider in unRotatableObject) {
                        collider.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
            
            Color _color = wallRenderer.color;
            _color.a = 1;
            wallRenderer.color = _color;
            wallRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);

            foreach (var collider in rotatableObject) {
                collider.transform.position = TileFindManager.Instance.gettileCentorLocation(collider.transform.position);
            }
            foreach (var collider in unRotatableObject) {
                collider.transform.position = TileFindManager.Instance.gettileCentorLocation(collider.transform.position);
            }

            isDegree45 = false;
            
            InputController.Instance.GainControl();             // 키 입력 풀기
        }

        public void FitRotateObject() {
            isDegree45 = true;
            foreach (var collider in unRotatableObject) {
                Vector3 currDegree = collider.transform.rotation.eulerAngles;
                if (isOpp) currDegree.z -= 90; 
                else currDegree.z += 90; 
                collider.transform.rotation = Quaternion.Euler(currDegree);
            }

            foreach (var collider in rotatableObject) {
                DispenserShoot arrowdisepenser = collider.GetComponent<DispenserShoot>();
                if (arrowdisepenser != null) {
                    if(!isOpp) arrowdisepenser.prevDirection();
                    else arrowdisepenser.NextDirection();
                }
            }
            
            Quaternion rotation = Quaternion.Euler(0, 0, -distDegree); // 90도 회전
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);

            foreach (Vector3Int pos in rotateTileMap.cellBounds.allPositionsWithin) {
                if (!rotateTileMap.HasTile(pos)) continue;
                rotateTileMap.SetTransformMatrix(pos, matrix);
            }
        }
        
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
            MoveControl moveableController = other.GetComponent<MoveControl>();
            if (moveableController != null) Launch();
        }
    }
}