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

        public async UniTask OnReciveMove(Define.MoveDir dir, bool isMove) 
        {
            (int movedX, int movedY) = ConvertDirToPos(dir);
            OnBeforeMove(dir);

            if (isMove == true)
            {
                await Move(movedX, movedY);
            }
            else
            {
                await BlockMotion(movedX, movedY);
            }

            OnAfterMove();
        }

        protected virtual void OnBeforeMove(Define.MoveDir dir)
        {

        }

        protected async virtual UniTask Move(int x, int y)
        {
            this.x = x;
            this.y = y;
            Vector2 target = map.GetTilePosition(x, y);

            await transform.DOLocalMove(target, 0.1f).ToUniTask();
        }

        protected virtual void MoveSoundStart()
        {
            
        }

        protected virtual void ReduceHealth()
        {

        }

        protected async virtual UniTask BlockMotion(int x, int y) 
        {
            Vector2 target = map.GetTilePosition(x, y);
            await transform.DOLocalMove(new Vector2(target.x, target.y), 0.05f).SetLoops(2, LoopType.Yoyo).ToUniTask();
        }

        protected virtual void OnAfterMove()
        {

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
            return map.CanMove(nextX, nextY);
        }
    }
}