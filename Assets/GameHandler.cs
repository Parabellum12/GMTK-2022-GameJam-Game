using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> EnemyPrefabsEasyToHard = new List<GameObject>();
    [SerializeField] GameObject roomPrefab;
    [SerializeField] Cinemachine.CinemachineVirtualCamera cam;
    [SerializeField] PlayerController_Script player;
    List<RoomHandler_Script> rooms = new List<RoomHandler_Script>();

    Vector2Int CurrentActiveRoom = Vector2Int.zero;
    List<List<bool>> Knowngrid = new List<List<bool>>();

    int difficultyLevel = 0;
    int numOfEnemiesToSpawn = 2;
    bool firstRoom = true;
    private void Start()
    {
        Knowngrid.Add(new List<bool>());
        Knowngrid[0].Add(false);
        numOfEnemiesToSpawn = 0;
        handleNextRoom(Vector2Int.zero, Vector2Int.zero);
        numOfEnemiesToSpawn = 2;
        firstRoom = false;
    }

    public bool[] getNextOpenDoors(Vector2Int room)
    {
        //0 = top, 1 = bottom, 2 = left, 3 = right
        bool[] returner = new bool[4] { true, true, true, true};

        returner[0] = Knowngrid[room.x][room.y + 1] == false;
        returner[3] = Knowngrid[room.x+1][room.y] == false;


        if (room.x == 0)
        {
            returner[2] = false;
        }
        else
        {
            returner[2] = Knowngrid[room.x - 1][room.y] == false;
        }

        if (room.y == 0)
        {
            returner[1] = false;
        }
        else
        {
            returner[1] = Knowngrid[room.x][room.y-1] == false;
        }


        return returner;
    }

    public GameObject[] getEnemiesToSpawn()
    {
        if (EnemyPrefabsEasyToHard.Count == 0)
        {
            return new GameObject[0];
        }
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < numOfEnemiesToSpawn; i++)
        {
            enemies.Add(EnemyPrefabsEasyToHard[Mathf.Clamp(difficultyLevel, 0, EnemyPrefabsEasyToHard.Count)]);
        }
        return enemies.ToArray();
    }

    public void handleNextRoom(Vector2Int NewRoomPos, Vector2Int CurrentRoomPos)
    {
        extendGrid(NewRoomPos);
        if (Knowngrid[NewRoomPos.x][NewRoomPos.y] == true)
        {
            return;
        }
        Knowngrid[NewRoomPos.x][NewRoomPos.y] = true;
        RoomHandler_Script newRoom = Instantiate(roomPrefab).GetComponent<RoomHandler_Script>();
        newRoom.Pos = NewRoomPos;
        newRoom.gameHandler = this;
        if (!firstRoom)
        {
            newRoom.lockRoom();
            newRoom.SpawnEnemies();
        }
        else
        {
            newRoom.UnLockRoom();
            newRoom.deactivateDoors(getNextOpenDoors(NewRoomPos));
        }
        CurrentActiveRoom = NewRoomPos;
        cam.LookAt = newRoom.transform;
        cam.Follow = newRoom.transform;
        float xmoveBy = 29;
        float yMoveBy = 16;
        newRoom.transform.position = new Vector2(xmoveBy * NewRoomPos.x, yMoveBy * NewRoomPos.y);

        rooms.Add(newRoom);

        if (NewRoomPos.x > CurrentRoomPos.x)
        {
            //leftdoor
            player.transform.position += new Vector3(1.5f, 0,0);
        }
        else if(NewRoomPos.x < CurrentRoomPos.x)
        {
            //RightDoor
            player.transform.position += new Vector3(-1.5f, 0, 0);
        }
        else if (NewRoomPos.y > CurrentRoomPos.y)
        {
            //bottomDoor;
            player.transform.position += new Vector3(0, 1.5f, 0);
        }
        else if (NewRoomPos.y < CurrentRoomPos.y)
        {
            //topDoor
            player.transform.position += new Vector3(0, -1.5f, 0);
        }


    }

    public void handleNextRoomOld(Vector2Int NewRoomPos, Vector2Int CurrentRoomPos)
    {
        if (CurrentActiveRoom != CurrentRoomPos)
        {
            return;
        }
        RoomHandler_Script newRoom = null;

        Debug.Log(NewRoomPos + " From:" + CurrentRoomPos);

        foreach (RoomHandler_Script scr in rooms)
        {
            if (scr.Pos.Equals(NewRoomPos))
            {
                newRoom = scr;
                break;
            }
        }
        if (newRoom == null)
        {
            Debug.Log("handleNextRoomOld HOW");
            return;
        }

        cam.LookAt = newRoom.transform;
        cam.Follow = newRoom.transform;
        if (NewRoomPos.x > CurrentRoomPos.x)
        {
            //leftdoor
            player.transform.position += new Vector3(1.5f, 0, 0);
        }
        else if (NewRoomPos.x < CurrentRoomPos.x)
        {
            //RightDoor
            player.transform.position += new Vector3(-1.5f, 0, 0);
        }
        else if (NewRoomPos.y > CurrentRoomPos.y)
        {
            //bottomDoor;
            player.transform.position += new Vector3(0, 1.5f, 0);
        }
        else if (NewRoomPos.y < CurrentRoomPos.y)
        {
            //topDoor
            player.transform.position += new Vector3(0, -1.5f, 0);
        }
    }

    int highestX = 0;
    int highesty = 0;
    public void extendGrid(Vector2Int nextPoint)
    {
        if (highestX < nextPoint.x+1)
        {
            highestX = nextPoint.x + 1;
        }
        if (highesty < nextPoint.y + 1)
        {
            highesty = nextPoint.y + 1;
        }

        while (Knowngrid.Count - 1 < highestX)
        {
            Knowngrid.Add(new List<bool>());
        }

        for (int i = 0; i < Knowngrid.Count; i++)
        {
            while (Knowngrid[i].Count - 1 < highesty)
            {
                Knowngrid[i].Add(false);
            }
        }

    }
}
