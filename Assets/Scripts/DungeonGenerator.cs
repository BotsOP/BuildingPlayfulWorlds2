using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator: MonoBehaviour
{
    public enum Tile { Floor }

    [Header("Prefabs")]
    public GameObject FloorPrefab;
    public GameObject WallPrefab;


    [Header("Dungeon Settings")]
    public int amountRooms;
    public int width;
    public int height;
    public int minRoomSize;
    public int maxRoomSize;
    public int widthCorridor;

    private Dictionary<Vector2Int, Tile> dungeonDictionary = new Dictionary<Vector2Int, Tile>();
    private List<Room> roomList = new List<Room>();
    private List<GameObject> allSpawnedObjects = new List<GameObject>();
    void Start()
    {
        GenerateDungeon();
    }

    private void AllocateRooms()
    {
        int amountOfTries = 0;
        int amountOfMaxTries = 100;
        while(amountRooms > 1 || amountOfTries > amountOfMaxTries) {
            Room room = new Room()
            {
                position = new Vector2Int(Random.Range(0, width), Random.Range(0, height)),
                size = new Vector2Int(Random.Range(minRoomSize, maxRoomSize), Random.Range(minRoomSize, maxRoomSize))
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
        for (int xx = room.position.x; xx < room.position.x + room.size.x; xx++)
        {
            for (int yy = room.position.y; yy < room.position.y + room.size.y; yy++)
            {
                Vector2Int pos = new Vector2Int(xx, yy);
                dungeonDictionary.Add(pos, Tile.Floor);
            }
        }
        roomList.Add(room);
    }

    private bool CheckIfRoomFitsInDungeon(Room room)
    {
        for (int xx = room.position.x; xx < room.position.x + room.size.x; xx++)
        {
            for (int yy = room.position.y; yy < room.position.y + room.size.y; yy++)
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
            
            int extraX = Random.Range(minRoomSize/2, startRoom.size.x - (widthCorridor - 1));
            int extraY = Random.Range(minRoomSize/2, startRoom.size.y - (widthCorridor - 1));
            Debug.Log(extraX + "    " + extraY);

            for(int j = 0; j <= widthCorridor; j++)
            {
                int dirX = Mathf.RoundToInt(Mathf.Sign(otherRoom.position.x - startRoom.position.x));
                int dirY = Mathf.RoundToInt(Mathf.Sign(otherRoom.position.y - startRoom.position.y));

                Debug.Log(dirX + "    " + (startRoom.position.x + extraX) + "    " + (otherRoom.position.x + extraX));
                for(int x = startRoom.position.x + extraX; x != otherRoom.position.x + extraX; x += dirX)
                {
                    PlaceCorridor(new Vector2Int(x, startRoom.position.y + extraY + (j * dirY)));
                    if(x < -10 || x > 150)
                        break;
                }
    
                Debug.Log(dirY + "    " + (startRoom.position.y + extraY) + "    " + (otherRoom.position.y + extraY));
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
            GameObject floor = Instantiate(FloorPrefab, new Vector3Int(kv.Key.x, 0, kv.Key.y), Quaternion.identity);
            allSpawnedObjects.Add(floor);

            //SpawnWallsForTile(kv.Key);
        }
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

    // private void SpawnWallsForTile(Vector2Int position)
    // {
    //     for (int x = -1; x <= 1; x++)
    //     {
    //         for (int z = -1; z <= 1; z++)
    //         {
    //             if(Mathf.Abs(x) == Mathf.Abs(z)) { continue; }
    //             Vector2Int gridPos = position + new Vector2Int(x, z);
    //             if (!dungeonDictionary.ContainsKey(gridPos))
    //             {
    //                 //Spawn Wall
    //                 Vector3 direction = new Vector3(gridPos.x, 0, gridPos.y) - new Vector3(position.x, 0, position.y);
    //                 GameObject wall = Instantiate(WallPrefab, new Vector3(position.x, 0, position.y), Quaternion.LookRotation(direction));
    //                 allSpawnedObjects.Add(wall);
    //             }
    //         }
    //     }
    // }


    public void GenerateDungeon()
    {
        AddPrebuildRooms();
        AllocateRooms();
        AllocateCorridors();
        BuildDungeon();

        Debug.Log(roomList.Count);
    }

}

public class Room
{
    public Vector2Int position;
    public Vector2Int size;
}
