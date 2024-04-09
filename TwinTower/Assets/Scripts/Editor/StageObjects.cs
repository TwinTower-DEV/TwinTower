using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using PlasticPipe.PlasticProtocol.Messages.Serialization;
using TwinTower;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 스테이지 내에 배치되어 있는 모든 오브젝트를 받아와 부모-자식 트리를 연결시켜
/// 잘못 배치된 것은 없는지 Debug 해주는 역할을 하는 클래스들임.
/// </summary>
public class StageObjects
{
    public HashSet<ObjectNode> rootNodes = new HashSet<ObjectNode>();   // 오브젝트들의 부모 노드들
    public MapNode mapNode;                                             // 맵

    public StageObjects() {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject _object in allObjects) {                    // 노드 연결 시작
            if (_object.transform.parent == null) LinkedRoot(_object.transform);
        }

        GameObject map = GameObject.Find("Map");                        // 맵 노드
        if (map == null) {
            throw new Exception("Map이름으로 되어있는 타일맵이 없습니다.");
        }

        mapNode = new MapNode(map);
    }

    public void Check() {                                               // 에러 체크 시작
        foreach (var rootNode in rootNodes) {
            rootNode.CheckError();
        }
        mapNode.CheckError();
    }
        
    // RootNode들 rootNodes에 연결
    private void LinkedRoot(Transform transform) {
        if (transform.GetComponent<SpriteRenderer>() == null &&
            transform.GetComponentInChildren<SpriteRenderer>() == null) {
            return;
        }
            
        ObjectNode newRootNode = LinkedFamily(transform);
        rootNodes.Add(newRootNode);
    }
        
    // 부모와 자식 노드 연결
    private ObjectNode LinkedFamily(Transform transform) {
        GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(transform.gameObject);
        if (prefab == null) {
            throw new Exception(transform.name + "이 오브젝트가 현재 Prefab이 적용되어 있지 않습니다.");
        }
        ObjectNode newNode = NodeFactory.createNode(prefab.name, transform.gameObject);
            
        for (int i = 0; i < transform.childCount; i++) {
            ObjectNode newChildNode = LinkedFamily(transform.GetChild(i));
            newChildNode.parentNode = newNode;
            newNode.childsObects.Add(newChildNode);
        }

        return newNode;
    }
}

/// <summary>
/// 대부분의 노드들이 상속받는 노드
/// 부모 노드, 자식 노드를 저장하며 체크가 이루어질때 자식들의 체크도 이루어진다.
/// 에러체크를 하는데 공통적으로 해야할 체크 사항들이 포함되어 있다.
/// (회전율 0, z위치 0, 프리팹 확인 등)
/// </summary>
public class ObjectNode {
    public GameObject Object;
    public ObjectNode parentNode;
    public List<ObjectNode> childsObects = new List<ObjectNode>();

    public ObjectNode(GameObject _object) {
        Object = _object;
    }
    
    public virtual void CheckError() {
        CheckBase();
        foreach (ObjectNode childObject in childsObects) {
            childObject.CheckError();
        }
    }

    // 기본 체크 사항
    private void CheckBase() {
        if(Object.transform.position.z != 0) Print("z위치 0으로 해주세요");
        bool checkRotation = false;
        for (int i = 0; i < 360; i += 90) {
            if (Object.transform.rotation.eulerAngles.z == i) checkRotation = true;
        }
        if(!checkRotation) Print("z회전값 확인해주세요");
        /*if(Object.transform.rotation.eulerAngles.x != 0) Print("x회전값 0으로 해주세요");
        if(Object.transform.rotation.eulerAngles.y != 0) Print("y회전값 0으로 해주세요");*/
        
        GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(Object.gameObject);
        if (prefab == null) {
            throw new Exception(Object.name + "이 오브젝트가 현재 Prefab이 적용되어 있지 않습니다.");
        }
        if(Object.layer != prefab.layer) Print("Layer 설정이 설정되어 있는 Prefab과 다릅니다.");
    }

    // 타일맵 내에 중앙에 존재하는지 확인 - ArrowTrap이나 DOorActivate는 가운데 확인이 필요 없기 떄문에
    // 선택사항으로 상속받은 노드가 사용을 결정
    protected void CheckLocation() {
        Tilemap[] tileMaps = GameObject.FindObjectsOfType<Tilemap>();
        bool isCenter = false;
    
        for (int i = 0; i < tileMaps.Length; i++) {
            if (tileMaps[i].transform.childCount == 0) continue;
            Vector3Int tilePosition = tileMaps[i].WorldToCell(Object.transform.position);
            Vector3 cellCenter = tileMaps[i].GetCellCenterWorld(tilePosition);
            if (tileMaps[i].GetTile(tilePosition) != null) {
                if (Object.transform.position == cellCenter) isCenter = true;
            }
        }
        
        if(!isCenter) Print("중앙에 배치되어 있지 않거나 타일맵 내에 배치되어 있지 않습니다.");
    }

    // 부모로 올라가면서 print
    protected void Print(string s) {
        if(parentNode != null) parentNode.Print(Object.name + " - " + s);
        else Debug.Log(Object.name + " - " + s);
    }
}

// 일반적인 노드들
// 위치 확인도 필요한 노드들은 이 클래스를 상속받는다.
public class NormalNode : ObjectNode { 
    public NormalNode(GameObject _object) : base(_object) { }
    public override void CheckError() {
        base.CheckError();
        CheckLocation();
    }
}

// Arrow Trap이 가지게 될 노드
// 발판과 발사대가 서로 연결되어 있는지 확인한다.
public class ArrowTrapNode : ObjectNode {
    public ArrowTrapNode(GameObject _object) : base(_object) { }
    
    public override void CheckError() {
        base.CheckError();
        DispenserShoot[] dispenserShoot = Object.GetComponentsInChildren<DispenserShoot>();
        PressurePlate[] pressurePlate = Object.GetComponentsInChildren<PressurePlate>();
        
        if(dispenserShoot.Length != 1) Print("발사대가 1개가 아닙니다.");
        if(pressurePlate.Length != 1) Print("발판이 1개가 아닙니다.");
        
        if(pressurePlate.Length == 0) Print("발사대가 없습니다.");
        else if(dispenserShoot.Length == 0) Print("발판이 없습니다.");
        else {
            if (pressurePlate[0].activateObject != dispenserShoot[0].gameObject) {
                Print("발사대와 발판이 이상한 것으로 연결되어 있습니다.");
            }
        }
        
        if(childsObects.Count != 2) Print("이상한 것이 들어가 있습니다.");
    }
}

// 발사대가 가지게 될 노드
// 회전율 별로 Sprite가 올바르게 적용되었는지 발판과 발사대가 서로 연결되어 있는지 확인해준다.
// Arrow 프리팹이 연결되어 있는지 확인한다.
public class DispenserNode : NormalNode {
    public DispenserNode(GameObject _object) : base(_object) { }
    public override void CheckError() {
        base.CheckError();
        CheckLocation();
        
        DispenserShoot dispenserShoot = Object.GetComponent<DispenserShoot>();
        if(dispenserShoot == null) Print("DispenserShoot 컴포넌트가 들어가 있지 않음 -> 개발자 호출 바람");
        else {
            if(dispenserShoot.arrowPrefab == null) Print("arrow prefab이 적용되어 있지 않음");
            if(dispenserShoot.directInfos.Length != 4) Print("방향 별 Sprite가 적용되어 있지 않음");
            else {
                foreach (DirectInfo info in dispenserShoot.directInfos) {
                    if (Object.transform.rotation.eulerAngles.z == info.angle) {
                        if (dispenserShoot.GetComponent<SpriteRenderer>().sprite !=
                            info.dispenserSprite) {
                            Print("각도에 따른 Sprite가 이상하게 적용되어 있습니다. 회전값을 새로고침 해주세요");
                        }
                    }
                }
            }
        }
    }
}

// DoorActivate 오브젝트가 체크해야 할 사항
// 문과 발판이 올바르게 연결되어 있는지 체크해준다.
public class DoorTrapNode : ObjectNode {
    public DoorTrapNode(GameObject _object) : base(_object) { }
    public override void CheckError() {
        base.CheckError();
        DoorActivate[] doorActivate = Object.GetComponentsInChildren<DoorActivate>();
        PressurePlate[] pressurePlate = Object.GetComponentsInChildren<PressurePlate>();
            
        if(doorActivate.Length != 1) Print("문이 1개가 아닙니다.");
        if(pressurePlate.Length != 1) Print("발판이 1개가 아닙니다.");
            
        if(pressurePlate.Length == 0) Print("문이 없습니다.");
        else if(doorActivate.Length == 0) Print("발판이 없습니다.");
        else {
            if (pressurePlate[0].activateObject != doorActivate[0].gameObject) {
                Print("문이 발판이 이상한 것으로 연결되어 있습니다.");
            }
        }
            
        if(childsObects.Count != 2) Print("이상한 것이 들어가 있습니다.");
    }
}

// OppRotatePlate의 에러 사항 체크
// IsOpp가 설정되어 있는지 확인해준다.
public class OppRotatePlateNode : NormalNode {
    public OppRotatePlateNode(GameObject _object) : base(_object) { }
    public override void CheckError() {
        base.CheckError();

        MapRotatePlate plate = Object.GetComponent<MapRotatePlate>();
        if (plate == null) {
            throw new Exception(Object.name + ": map Rotate Plate 컴포넌트가 없음.");
        }
        if(plate.rotateTileMap == null) Print("Rotate Tile Map이 지정되어 있지 않음 inspector창 확인 필요.");
        if(plate.isOpp == false) Print("isOpp가 설정되어 있지 않음 확인 필요");
    }
}

// RotatePlate의 에러 사항 체크
// IsOpp가 올바르게 설정되어 있는지 확인해준다.
public class RotatePlateNode : NormalNode {
    public RotatePlateNode(GameObject _object) : base(_object) { }
    public override void CheckError() {
        base.CheckError();

        MapRotatePlate plate = Object.GetComponent<MapRotatePlate>();
        if (plate == null) {
            throw new Exception(Object.name + ": map Rotate Plate 컴포넌트가 없음.");
        }
        if(plate.rotateTileMap == null) Print("Rotate Tile Map이 지정되어 있지 않음 inspector창 확인 필요.");
        if(plate.isOpp == true) Print("isOpp가 설정되어 있음 확인 필요(설정되면 안되는 오브젝트임)");
    }
}

// 맵이 보유하게 될 노드 - ObjectNode를 상속받지 않는다.
// 하라면 할 수 있는데.. 넘 귀찬.. 하라면 합니다 ㅠ
public class MapNode {
    private GameObject Object;
    private Tilemap LeftTile;
    private Tilemap RightTile;
    private Tilemap LeftWallTile;
    private Tilemap RightWallTile;
    public MapNode(GameObject _object) {
        Object = _object;

        LeftTile = Object.transform.GetChild(0).GetComponent<Tilemap>();
        RightTile = Object.transform.GetChild(1).GetComponent<Tilemap>();
        
        LeftWallTile = LeftTile.transform.GetChild(0).GetComponent<Tilemap>();
        RightWallTile = RightTile.transform.GetChild(0).GetComponent<Tilemap>();

        if (LeftTile == null || RightTile == null || LeftWallTile == null || RightWallTile == null) {
            Print("Map내에 있는 것들 제대로 확인 부탁. Wall, Tilemap위치 확인 -> 계속 안된다면 개발자 요청");
        }
    }

    public void CheckError() {
        // 위치와 회전값 확인.
        if(Object.transform.position.x != 0) Print("x위치 0으로 해주세요");
        if(Object.transform.position.y != 0) Print("y위치 0으로 해주세요");
        if(Object.transform.position.z != 0) Print("z위치 0으로 해주세요");
        if(Object.transform.rotation.eulerAngles.x != 0) Print("x회전값 0으로 해주세요");
        if(Object.transform.rotation.eulerAngles.y != 0) Print("y회전값 0으로 해주세요");
        if(Object.transform.rotation.eulerAngles.z != 0) Print("z회전값 0으로 해주세요");
        
        if(LeftTile.transform.rotation.eulerAngles.x != 0) Print("LeftTile x회전값 0으로 해주세요");
        if(LeftTile.transform.rotation.eulerAngles.y != 0) Print("LeftTile y회전값 0으로 해주세요");
        if(LeftTile.transform.rotation.eulerAngles.z != 0) Print("LeftTile z회전값 0으로 해주세요");

        if(RightTile.transform.rotation.eulerAngles.x != 0) Print("RightTile x회전값 0으로 해주세요");
        if(RightTile.transform.rotation.eulerAngles.y != 0) Print("RightTile y회전값 0으로 해주세요");
        if(RightTile.transform.rotation.eulerAngles.z != 0) Print("RightTile z회전값 0으로 해주세요");

        if(RightWallTile.transform.rotation.eulerAngles.x != 0) Print("RightWallTile x회전값 0으로 해주세요");
        if(RightWallTile.transform.rotation.eulerAngles.y != 0) Print("RightWallTile y회전값 0으로 해주세요");
        if(RightWallTile.transform.rotation.eulerAngles.z != 0) Print("RightWallTile z회전값 0으로 해주세요");

        if(LeftWallTile.transform.rotation.eulerAngles.x != 0) Print("LeftWallTile x회전값 0으로 해주세요");
        if(LeftWallTile.transform.rotation.eulerAngles.y != 0) Print("LeftWallTile y회전값 0으로 해주세요");
        if(LeftWallTile.transform.rotation.eulerAngles.z != 0) Print("LeftWallTile z회전값 0으로 해주세요");

        
        if (Object.transform.childCount != 2) Print("맵 안에 이상한 것이 들어가 있습니다.");
        for (int i = 0; i < Object.transform.childCount; i++) {
            if(Object.transform.GetChild(i).GetComponent<Tilemap>() == null) Print("맵 안에 이상한 것이 들어가 있습니다.");
        }
        
        // 각 타일별 위치 확인
        if (LeftTile.transform.position != new Vector3(-5.666667f, -0.8666667f, 0f)) {
            Print("LeftTile 위치 확인 필요(-5.666667, -0.8666667, 0) 으로 맞춰야함.");
        }
        if (RightTile.transform.position != new Vector3(5.666667f, -0.8666667f, 0f)) {
            Print("LeftTile 위치 확인 필요(5.666667, -0.8666667, 0) 으로 맞춰야함.");
        }

        if (LeftWallTile.transform.position != new Vector3(-5.666667f, -0.8666667f, 0f)) {
            Print("LeftTile밑에 있는 Wall위치 (0, 0, 0)으로 맞춰줘야 함.");
        }
        if (RightWallTile.transform.position !=new Vector3(5.666667f, -0.8666667f, 0f)) {
            Print("RightWallTile에 있는 Wall위치 (0, 0, 0)으로 맞춰줘야 함.");
        }

        // 각 타일의 영역을 벗어났는지 확인
        BoundsInt LeftTileBounds = LeftTile.cellBounds;
        Vector3Int minPosition = LeftTile.WorldToCell(LeftTile.GetCellCenterWorld(LeftTileBounds.max));
        Vector3Int maxPosition = LeftTile.WorldToCell(LeftTile.GetCellCenterWorld(LeftTileBounds.min));

        foreach (Vector3Int pos in LeftTileBounds.allPositionsWithin) {
            if (LeftTile.HasTile(pos)) {
                minPosition.x = Mathf.Min(minPosition.x, pos.x);
                minPosition.y = Mathf.Min(minPosition.y, pos.y);
                maxPosition.x = Mathf.Max(maxPosition.x, pos.x);
                maxPosition.y = Mathf.Max(maxPosition.y, pos.y);
            }
        }

        if (LeftTile.GetCellCenterWorld(minPosition) != new Vector3(-9.1666667f, -4.3666667f, 0f) ||
            LeftTile.GetCellCenterWorld(maxPosition) != new Vector3(-2.1666667f, 2.63333333f, 0f)) {
            Print("LeftTile이 이상한 곳에 배치되어 있음 확인 필요.(Wall에 끼어있거나 RightTile에 숨어있을수도 있음)");
        }
        
        BoundsInt RightTileBounds = RightTile.cellBounds;
        minPosition = RightTile.WorldToCell(RightTile.GetCellCenterWorld(RightTileBounds.max));
        maxPosition = RightTile.WorldToCell(RightTile.GetCellCenterWorld(RightTileBounds.min));

        foreach (Vector3Int pos in RightTileBounds.allPositionsWithin) {
            if (RightTile.HasTile(pos)) {
                minPosition.x = Mathf.Min(minPosition.x, pos.x);
                minPosition.y = Mathf.Min(minPosition.y, pos.y);
                maxPosition.x = Mathf.Max(maxPosition.x, pos.x);
                maxPosition.y = Mathf.Max(maxPosition.y, pos.y);
            }
        }

        if (RightTile.GetCellCenterWorld(minPosition) != new Vector3(2.16666667f, -4.3666667f, 0f) ||
            RightTile.GetCellCenterWorld(maxPosition) != new Vector3(9.16666667f, 2.6333333f, 0f)) {
            Print("RightTile이 이상한 곳에 배치되어 있음 확인 필요.(Wall에 끼어있거나 LeftTile에 숨어있을수도 있음)");
        }
        
        BoundsInt RightWallTileBounds = RightWallTile.cellBounds;
        minPosition = RightWallTile.WorldToCell(RightWallTile.GetCellCenterWorld(RightWallTileBounds.max));
        maxPosition = RightWallTile.WorldToCell(RightWallTile.GetCellCenterWorld(RightWallTileBounds.min));

        foreach (Vector3Int pos in RightWallTileBounds.allPositionsWithin) {
            if (RightWallTile.HasTile(pos)) {
                minPosition.x = Mathf.Min(minPosition.x, pos.x);
                minPosition.y = Mathf.Min(minPosition.y, pos.y);
                maxPosition.x = Mathf.Max(maxPosition.x, pos.x);
                maxPosition.y = Mathf.Max(maxPosition.y, pos.y);
            }
        }

        if (RightWallTile.GetCellCenterWorld(minPosition) != new Vector3(1.16666667f, -5.3666667f, 0f) ||
            RightWallTile.GetCellCenterWorld(maxPosition) != new Vector3(10.16666667f, 3.6333333f, 0f)) {
            Print("RightTile - Wall이 이상한 곳에 배치되어 있음 확인 필요.(RightTile 숨어있을수도 있음)");
        }
        
        BoundsInt LeftWallTileBounds = LeftWallTile.cellBounds;
        minPosition = LeftWallTile.WorldToCell(LeftWallTile.GetCellCenterWorld(LeftWallTileBounds.max));
        maxPosition = LeftWallTile.WorldToCell(LeftWallTile.GetCellCenterWorld(LeftWallTileBounds.min));

        foreach (Vector3Int pos in LeftWallTileBounds.allPositionsWithin) {
            if (LeftWallTile.HasTile(pos)) {
                minPosition.x = Mathf.Min(minPosition.x, pos.x);
                minPosition.y = Mathf.Min(minPosition.y, pos.y);
                maxPosition.x = Mathf.Max(maxPosition.x, pos.x);
                maxPosition.y = Mathf.Max(maxPosition.y, pos.y);
            }
        }

        if (LeftWallTile.GetCellCenterWorld(minPosition) != new Vector3(-10.1666667f, -5.3666667f, 0f) ||
            LeftWallTile.GetCellCenterWorld(maxPosition) != new Vector3(-1.1666667f, 3.63333333f, 0f)) {
            Print("LeftTile - Wall이 이상한 곳에 배치되어 있음 확인 필요.(LeftTile에 숨어있을수도 있음)");
        }
   
        // 타일이 설치 된 개수 확인
        if(CountTiles(LeftWallTile) != 36) Print("LeftWallTile이 설치된 Tile개수가 36개가 아닙니다.");
        if(CountTiles(RightWallTile) != 36) Print("RightWallTile이 설치된 Tile개수가 36개가 아닙니다.");
        if(CountTiles(LeftTile) != 64) Print("LeftTile이 설치된 Tile개수가 64개가 아닙니다.(Wall 포함 100개)");
        if(CountTiles(RightTile) != 64) Print("LeftTile이 설치된 Tile개수가 64개가 아닙니다.(Wall 포함 100개)");
        
        GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(Object.gameObject);
        if (prefab == null) {
            throw new Exception(Object.name + "이 오브젝트가 현재 Prefab이 적용되어 있지 않습니다.");
        }
    }

    private void Print(string s) {
        Debug.Log(Object.name + " - " + s);
    }
    
    private int CountTiles(Tilemap tilemap) {
        int count = 0;

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        foreach (TileBase tile in allTiles) {
            if (tile != null) count++;
        }

        return count;
    }
}

// 노드 type별 생성 노드를 반환해주는 클래스.
public class NodeFactory {
    public static ObjectNode createNode(string type, GameObject _object) {
        switch (type) {
            case "ArrowDispenser":
                return new DispenserNode(_object);
            case "ArrowTrap":
                return new ArrowTrapNode(_object);
            case "DoorActivate":
                return new DoorTrapNode(_object);
            case "OppRotatePlate":
                return new OppRotatePlateNode(_object);
            case "RotatePlate":
                return new RotatePlateNode(_object);
            case "Stairs":
                return new ObjectNode(_object);
            default:
                return new NormalNode(_object);  // 상자, 무빙워크, 발판, 표지판, 플레이어, 뮨
        }
    }
}
