using System;
using UnityEngine;

namespace TwinTower
{
    public class Player: MonoBehaviour
    {
        [SerializeField] private int _moveSpeed;
        private Rigidbody2D rigid;
        [SerializeField]private int currx = 0, curry = 0;
        public Vector3 pos;
        public void MoveToTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, _moveSpeed * Time.deltaTime);
        }

        private float CentorPos(int k)
        {
            return k + 0.5f;
        }
        public void GroundedHorizontalMovement()
        {
            if (InputController.Instance.LeftMove.Down)
            {
                if (curry - 1 >= 0)
                {
                    curry--;
                    pos = new Vector3(CentorPos(TileFindManager.Instance.player1Map[currx, curry].x),
                        CentorPos(TileFindManager.Instance.player1Map[currx, curry].y), 0);
                }
                else
                {
                    
                }
                Debug.Log("왼쪽으로 이동!");

            }
            else if (InputController.Instance.RightMove.Down)
            {
                if (curry + 1 < 8)
                {
                    curry++;
                    Debug.Log(TileFindManager.Instance.player1Map[currx, curry].x + " " +TileFindManager.Instance.player1Map[currx, curry].y );
                    pos = new Vector3(CentorPos(TileFindManager.Instance.player1Map[currx, curry ].x),
                        CentorPos(TileFindManager.Instance.player1Map[currx, curry].y), 0);
                }
                else
                {
                    
                }
                Debug.Log("오른쪽으로 이동!");

            }
            else if (InputController.Instance.UpMove.Down)
            {
                if (currx -1 >= 0)
                {
                    currx--;
                    pos = new Vector3(CentorPos(TileFindManager.Instance.player1Map[currx, curry].x),
                        CentorPos(TileFindManager.Instance.player1Map[currx, curry].y), 0);
                }
                else
                {
                    
                }
                Debug.Log("위쪽으로 이동!");
            }
            else if (InputController.Instance.DownMove.Down)
            {
                if (currx + 1 < 8)
                {
                    currx++;
                    pos = new Vector3(CentorPos(TileFindManager.Instance.player1Map[currx, curry].x),
                        CentorPos(TileFindManager.Instance.player1Map[currx, curry].y), 0);
                }
                Debug.Log("아래쪽으로 이동!");
            }
        }
        private void Awake()
        {
            
        }

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            GroundedHorizontalMovement();
            MoveToTarget();
        }
}
    }