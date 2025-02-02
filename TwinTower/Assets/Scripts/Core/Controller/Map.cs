using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TwinTower
{
    public enum MapType
    {
        None = -1,
        Left,
        Right
    }

    public class Map : MonoBehaviour
    {
        public MapType type;
        public Tilemap walls;
        private GimmikBase[] gimmiks;
        public List<MoveControl> movedObjects;
        private GimmikBase[,] map;
        public Transform rotateParent;
        private Define.MoveDir currentRotation = Define.MoveDir.Up;

        private List<GimmikInvisibleWall> invisibleWalls = new List<GimmikInvisibleWall>();

        private void Start() 
        {
            GetWalls();
            GetGimmiks();
            ShowGimmik();
        }

        private void GetWalls()
        {
            BoundsInt bounds = walls.cellBounds;
            TileBase[] allTiles = walls.GetTilesBlock(bounds);

            map = new GimmikBase[bounds.size.x, bounds.size.y];

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    if (allTiles[x + y * bounds.size.x] != null)
                    {
                        map[x, y] = new GimmikWall();
                    }
                }
            }
        }

        private void GetGimmiks()
        {
            gimmiks = GetComponentsInChildren<GimmikBase>();
            //gimmiks.ForEach(gimmik => map[gimmik.x, gimmik.y] = gimmik);

            foreach(GimmikBase gimmik in gimmiks)
            {
                if (IsInMap(gimmik.x, gimmik.y) == true)
                {
                    map[gimmik.x, gimmik.y] = gimmik;
                }

                if (gimmik.GetType() == typeof(GimmikInvisibleWall))
                {
                    invisibleWalls.Add(gimmik as GimmikInvisibleWall);
                }
            }
        }

        private void ShowGimmik()
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Debug.Log($"({x},{y}): {map[x,y]?.GetType()}");
                }
            }
        }

        public bool CanMove(int x, int y)
        {
            // map에는 바깥 벽은 포함되지 않으므로, -1 or map보다 큰 값이면 무조건 움직이지 못해야 함. - 손창하
            if (x < 0 || x > map.GetLength(0) - 1)
            {
                return false;
            }
            
            if (y < 0 || y > map.GetLength(0) - 1)
            {
                return false;
            }

            return map[x, y]?.isWalkable ?? true;
        }

        public GimmikBase GetGimmik(int x, int y)
        {
            return map[x, y];
        }

        public MoveControl GetMovedObject(int x, int y)
        {
            return movedObjects.Find(obj => obj.x == x && obj.y == y);
        }

        public Vector2 GetTilePosition(int x, int y)
        {
            Vector3 cellSize = walls.cellSize; // Wall이나 Floor나 Cell Size는 같으므로 wall로 해도 문제 없음
            Vector3 origin = walls.origin;

            Vector2 target = new Vector2(origin.x + x + cellSize.x / 2, origin.y + y + cellSize.y / 2);

            return target;
        }

        // 해당 좌표로부터 해당하는 방향의 좌표 반환
        public (int, int) GetCoordinates(Define.MoveDir dir, int x, int y)
        {
            int movedX = x;
            int movedY = y;

            switch (dir)
            {
                case Define.MoveDir.Up:
                    movedY += 1;
                    break;
                case Define.MoveDir.Right:
                    movedX += 1;
                    break;
                case Define.MoveDir.Down:
                    movedY -= 1;
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

        public bool IsInMap(int x, int y)
        {
            if (x < 0 || x > map.GetLength(0) - 1)
            {
                return false;
            }
            
            if (y < 0 || y > map.GetLength(0) - 1)
            {
                return false;
            }

            return true;
        }

        public async UniTask Rotate()
        {
            RotateTiles();
            RotateMovedObjects();
            currentRotation += 1;
            await rotateParent.DORotate(new Vector3(0, 0, -90 * ((int)currentRotation % 4)), 1);
        }

        public Define.MoveDir GetCurrentDir(Define.MoveDir dir)
        {
            int result = ((dir - currentRotation) % 4 + 4) % 4;
            return (Define.MoveDir)result;
        }

        public Define.MoveDir GetOriginDir(Define.MoveDir dir)
        {
            int result = (((int)dir + (int)currentRotation) % 4 + 4) % 4;
            return (Define.MoveDir)result;
        }

        private void RotateTiles()
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x,y]?.Rotate();
                }
            }
        }

        private void RotateMovedObjects()
        {
            movedObjects.ForEach(obj => obj.Rotate());
        }

        public void ShowInvisibleWalls(int x, int y)
        {
            invisibleWalls.ForEach(wall => wall.SetInvisible(x, y));
        }
    }
}