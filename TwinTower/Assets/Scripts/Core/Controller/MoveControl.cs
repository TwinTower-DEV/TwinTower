using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/// <summary>
/// 이동 가능한 모든 오브젝트들이 상속 받는 클래스.
/// 다음칸에 이동 가능한지 확인과 이동 명령을 받는 함수 존재.
/// </summary>
namespace TwinTower
{
    public class MoveControl : MonoBehaviour
    {
        public Map map;
        public int x;
        public int y;

        public async UniTask OnReciveMove(Define.MoveDir dir, bool canMove) 
        {
            (int movedX, int movedY) = ConvertDirToPos(dir);
            OnBeforeReciveMove(dir);
            
            if (canMove == true)
            {
                OnBeforeMove();
                MoveMovedObject(dir, movedX, movedY);
                await Move(movedX, movedY);
                OnAfterMove();
            }
            else
            {
                await BlockMotion(movedX, movedY);
            }
        }

        // Move, BlockMotion 모두에서 실행되어야 하는 메서드는 아래에 override
        protected virtual void OnBeforeReciveMove(Define.MoveDir dir)
        {

        }

#region Move

        protected virtual void OnBeforeMove()
        {
            map.GetGimmik(x, y)?.DeActive();
            Debug.LogError($"DeActive: {x}, {y}: {map.GetGimmik(x, y)?.GetType()}");
        }

        protected async virtual UniTask Move(int x, int y)
        {
            this.x = x;
            this.y = y;
            Vector2 target = map.GetTilePosition(x, y);

            await transform.DOLocalMove(target, 0.1f).ToUniTask();
        }

        protected virtual void OnAfterMove()
        {
            map.GetGimmik(x, y)?.Active();
            Debug.LogError($"Active: {x}, {y}: {map.GetGimmik(x, y)?.GetType()}");
        }

#endregion

        protected async virtual UniTask BlockMotion(int x, int y) 
        {
            Vector2 target = map.GetTilePosition(x, y);
            await transform.DOLocalMove(new Vector2(target.x, target.y), 0.05f).SetLoops(2, LoopType.Yoyo).ToUniTask();
        }

        private (int, int) ConvertDirToPos(Define.MoveDir dir)
        {
            int movedX = x;
            int movedY = y;
            switch (dir)
            {
                case Define.MoveDir.Up:
                    movedY += 1;
                    break;
                case Define.MoveDir.Down:
                    movedY -= 1;
                    break;
                case Define.MoveDir.Right:
                    movedX += 1;
                    break;
                case Define.MoveDir.Left:
                    movedX -= 1;
                    break;
                default:
                    Debug.LogError($"적절하지 않은 Move값: {dir}");
                    break;
            }

            return (movedX, movedY);
        }

        public bool CanMoveTile(Define.MoveDir dir)
        {
            (int nextX, int nextY) = ConvertDirToPos(dir);
            
            while (map.GetMovedObject(nextX, nextY) != null)
            {
                Debug.Log($"{nextX}, {nextY}에 장애물이 있습니다.");
                switch (dir)
                {
                    case Define.MoveDir.Up:
                        nextY += 1;
                        break;
                    case Define.MoveDir.Down:
                        nextY -= 1;
                        break;
                    case Define.MoveDir.Right:
                        nextX += 1;
                        break;
                    case Define.MoveDir.Left:
                        nextX -= 1;
                        break;
                }
            }

            return map.CanMove(nextX, nextY);
        }

        private void MoveMovedObject(Define.MoveDir dir, int moveX, int moveY)
        {
            map.GetMovedObject(moveX, moveY)?.OnReciveMove(dir, true);
        }

        protected virtual void MoveSoundStart()
        {
            
        }

        protected virtual void ReduceHealth()
        {

        }
    }
}