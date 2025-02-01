using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

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
        public int hp;
        public Define.MoveControlType type;

        public async UniTask OnReciveMove(Define.MoveDir dir, bool canMove) 
        {
            (int movedX, int movedY) = map.GetCoordinates(dir, x, y);
            OnBeforeReciveMove(dir);
            
            if (canMove == true)
            {
                OnBeforeMove();
                await MoveMovedObject(dir, movedX, movedY);
                await Move(movedX, movedY);
                await OnAfterMove();
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
            GimmikBase gimmik = map.GetGimmik(x, y);
            
            if (gimmik != null)
            {
                gimmik.OnDeactive(this, type);
            }
        }

        protected async virtual UniTask Move(int x, int y)
        {
            this.x = x;
            this.y = y;
            Vector2 target = map.GetTilePosition(x, y);

            await transform.DOLocalMove(target, 0.1f).ToUniTask();
        }

        protected async virtual UniTask OnAfterMove()
        {
            GimmikBase gimmik = map.GetGimmik(x, y);
            
            if (gimmik != null)
            {
                await gimmik.OnActive(this, type);
            }
        }

#endregion

        protected async virtual UniTask BlockMotion(int x, int y) 
        {
            Vector2 target = map.GetTilePosition(x, y);
            await transform.DOLocalMove(new Vector2(target.x, target.y), 0.05f).SetLoops(2, LoopType.Yoyo).ToUniTask();
        }

        public bool CanMoveTile(Define.MoveDir dir)
        {
            (int nextX, int nextY) = map.GetCoordinates(dir, x, y);
            
            if (map.GetMovedObject(nextX, nextY) != null)
            {
                (nextX, nextY) = map.GetCoordinates(dir, nextX, nextY);
                if (map.GetMovedObject(nextX, nextY) != null)
                {
                    // box가 2개 이상인 경우는 움직일 수 없음.
                    return false;
                }
            }

            return map.CanMove(nextX, nextY);
        }

        private async UniTask MoveMovedObject(Define.MoveDir dir, int moveX, int moveY)
        {
            MoveControl movedObject = map.GetMovedObject(moveX, moveY);
            if (movedObject != null)
            {
                await movedObject.OnReciveMove(dir, true);
            }
        }

        protected virtual void MoveSoundStart()
        {
            
        }

        protected virtual void ReduceHealth()
        {

        }

        public async UniTask GetDamage(int damage)
        {
            hp -= damage;

            if (hp <= 0)
            {
                await Death();
            }
        }

        public virtual async UniTask Death()
        {
            
        }
    }
}