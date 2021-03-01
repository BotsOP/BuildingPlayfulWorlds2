using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGenerator: MonoBehaviour
{
    public enum Tile { Floor }

    [Header("Prefabs")]
    public GameObject[] FloorPrefab;
    public GameObject[] WallPrefab;
    public GameObject TorchPrefab;
    public GameObject EnemyPrefab;
    public NavMeshSurface surface;


    [Header("Dungeon Settings")]
    public int amountRooms;
    public int width;
    public int height;
    public int minRoomSize;
    public int maxRoomSize;
    public int widthCorridor;
    public int roomAroundRooms;

    private Dictionary<Vector2Int, Tile> dungeonDictionary = new Dictionary<Vector2Int, Tile>();
    private List<Room> roomList = new List<Room>();
    private List<GameObject> allSpawnedObjects = new List<GameObject>();
    int torchAmount;
    void Start()
    {
        AddPrebuildRooms();
        AllocateRooms();
        AllocateCorridors();
        BuildDungeon();
        PlaceEnemies();

        Debug.Log(roomList.Count);
    }
    private void AddPrebuildRooms()
    {
        int widthRoom = 20;
        int heightRoom = 20;
        Room room = new Room()
        {
            position = new Vector2Int(width/2 - widthRoom/2, height/2 - heightRoom/2),
            size = new Vector2Int(widthRoom,heightRoom)
        };

        AddRoomToDungeon(room);
    }

    private void AllocateRooms()
    {
        int amountOfTries = 0;
        int amountOfMaxTries = 100;
        while(amountRooms > 1 || amountOfTries > amountOfMaxTries) {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);

            Room room = new Room()
            {
                position = new Vector2Int(Random.Range(0, width), Random.Range(0, height)),
                size = new Vector2Int(roomWidth, roomHeight),
                nonUseSize = new Vector2Int(roomWidth + roomAroundRooms * 2, roomHeight + roomAroundRooms * 2)
            };

            if (CheckIfRoomFitsInDungeon(room))
            {
                AddRoomToDungeon(room);
                amountRooms--;
            }

            amountOfTries++;
            if(amountOfTries > amountOfMaxTries)
                break;
        }
    }

    private void AddRoomToDungeon(Room room)
    {
        int i = 0;
        for (int xx = room.position.x; xx < room.position.x + room.size.x; xx++)
        {
            for (int yy = room.position.y; yy < room.position.y + room.size.y; yy++)
            {
                Vector2Int pos = new Vector2Int(xx, yy);
                i++;
                dungeonDictionary.Add(pos, Tile.Floor);
            }
        }
        roomList.Add(room);
    }

    private bool CheckIfRoomFitsInDungeon(Room room)
    {
        Vector2Int nonUsePos = new Vector2Int(room.position.x - (room.nonUseSize.x - room.size.x) / 2, room.position.y - (room.nonUseSize.y - room.size.y) / 2);
        for (int xx = nonUsePos.x; xx < nonUsePos.x + room.nonUseSize.x; xx++)
        {
            for (int yy = nonUsePos.y; yy < nonUsePos.y + room.nonUseSize.y; yy++)
            {
                Vector2Int pos = new Vector2Int(xx, yy);
                
                if (dungeonDictionary.ContainsKey(pos))
                {
                    return false;
                }
            }
        }

        return true;
    }


    private void AllocateCorridors()
    {

        for (int i = 1; i < roomList.Count; i++)
        {
            Room startRoom = roomList[i];
            Room otherRoom = roomList[0];
            
            int extraX = Random.Range(0, startRoom.size.x - (widthCorridor - 1));
            int extraY = Random.Range(0, startRoom.size.y - (widthCorridor - 1));

            for(int j = 0; j <= widthCorridor; j++)
            {
                int dirX = Mathf.RoundToInt(Mathf.Sign(otherRoom.position.x - startRoom.position.x));
                int dirY = Mathf.RoundToInt(Mathf.Sign(otherRoom.position.y - startRoom.position.y));

                for(int x = startRoom.position.x + extraX; x != otherRoom.position.x + extraX; x += dirX)
                {
                    PlaceCorridor(new Vector2Int(x, startRoom.position.y + extraY + (j * dirY)));
                    if(x < -10 || x > 150)
                        break;
                }

                for (int y = startRoom.position.y + extraY; y != otherRoom.position.y + extraY; y += dirY)
                {
                    PlaceCorridor(new Vector2Int(otherRoom.position.x + j + extraX, y));
                    if(y < -10 || y > 150)
                        break;
                }
            }
        }
    }

    private void PlaceCorridor(Vector2Int pos)
    {
        if (!dungeonDictionary.ContainsKey(pos))
            dungeonDictionary.Add(pos, Tile.Floor);
    }

    private void BuildDungeon()
    {
        foreach(KeyValuePair<Vector2Int, Tile> kv in dungeonDictionary)
        {
            GameObject floor = Instantiate(FloorPrefab[SelectRandomFloor()], new Vector3Int(kv.Key.x, 0, kv.Key.y), Quaternion.identity, GameObject.Find("Dungeon").transform);
            allSpawnedObjects.Add(floor);

            SpawnWallsForTile(kv.Key);
        }

        surface.BuildNavMesh();
    }

    private void PlaceEnemies()
    {
        for (int roomIndex = 1; roomIndex < roomList.Count; roomIndex++)
        {
            Vector2 placeEnemiesCorner = new Vector2(roomList[roomIndex].position.x + (roomList[roomIndex].size.x / 2.5f), roomList[roomIndex].position.y + (roomList[roomIndex].size.y / 2.5f));
                for (int x = 0; x < roomList[roomIndex].size.x / 4; x++)
                {
                    for (int y = 0; y < roomList[roomIndex].size.y / 4; y++)
                    {
                        Vector3 enemySpawnPos = new Vector3(placeEnemiesCorner.x + x, 0, placeEnemiesCorner.y + y);
                        Instantiate(EnemyPrefab, enemySpawnPos, Quaternion.identity);
                    }
                }
        }
    }

    private int SelectRandomFloor()
    {
        int chanceToSelectBaseFloor = 99;
        if(chanceToSelectBaseFloor > Random.Range(1, 101))
            return 0;
        else
            return Random.Range(1, FloorPrefab.Length);
    }

    
    private void SpawnWallsForTile(Vector2Int position)
    {
        int spawnTorchEverySoManyWalls = 5;
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if(Mathf.Abs(x) == Mathf.Abs(z)) { continue; }
                Vector2Int gridPos = position + new Vector2Int(x, z);
                if (!dungeonDictionary.ContainsKey(gridPos))
                {
                    //Spawn Wall
                    Vector3 direction = new Vector3(gridPos.x, 0, gridPos.y) - new Vector3(position.x, 0, position.y);
                    GameObject wall = Instantiate(WallPrefab[Random.Range(0, WallPrefab.Length)], new Vector3(position.x, 0, position.y), Quaternion.LookRotation(direction), GameObject.Find("Dungeon").transform);
                    allSpawnedObjects.Add(wall);
                    if(torchAmount % spawnTorchEverySoManyWalls == 0){
                        Instantiate(TorchPrefab, new Vector3(position.x, 0, position.y), Quaternion.LookRotation(direction), GameObject.Find("Dungeon").transform);
                    }
                    torchAmount++;
                }
            }
        }
    }

}

public class Room
{
    public Vector2Int position;
    public Vector2Int size;
    public Vector2Int nonUseSize;
}
