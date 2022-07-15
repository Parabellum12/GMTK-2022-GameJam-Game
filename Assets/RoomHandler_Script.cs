using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler_Script : MonoBehaviour
{
    [SerializeField] GameObject DoorUp;
    [SerializeField] GameObject DoorDown;
    [SerializeField] GameObject DoorLeft;
    [SerializeField] GameObject DoorRight;

    public Vector2Int Pos;
    public GameHandler gameHandler;
    List<GameObject> enemies = new List<GameObject>();
    bool isRoomClear = false;
    public void lockRoom()
    {
        DoorUp.GetComponent<SpriteRenderer>().color = Color.red;
        DoorDown.GetComponent<SpriteRenderer>().color = Color.red;
        DoorLeft.GetComponent<SpriteRenderer>().color = Color.red;
        DoorRight.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void SpawnEnemies()
    {
        GameObject[] enemyPrefabsToSpawn = gameHandler.getEnemiesToSpawn();
        foreach (GameObject go in enemyPrefabsToSpawn)
        {
            enemies.Add(Instantiate(go, transform.position, Quaternion.identity));
        }
    }

    public void deactivateDoors(bool[] DoorStatus)
    {

        DoorUp.GetComponent<DoorScript>().deactivated = !DoorStatus[0];
        DoorDown.GetComponent<DoorScript>().deactivated = !DoorStatus[1];
        DoorLeft.GetComponent<DoorScript>().deactivated = !DoorStatus[2];
        DoorRight.GetComponent<DoorScript>().deactivated = !DoorStatus[3];
    }

    public void UnLockRoom()
    {
        bool[] doors = gameHandler.getNextOpenDoors(Pos);
        if (doors[0])
        {
            DoorUp.GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (doors[1])
        {
            DoorDown.GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (doors[2])
        {
            DoorLeft.GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (doors[3])
        {
            DoorRight.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void doorWasWalkedThroughNew(int door)
    {
        Vector2Int roomNum = Pos;
        switch (door)
        {
            case 0:
                roomNum.y++;
                break;
            case 1:
                roomNum.y--;
                break;
            case 2:
                roomNum.x--;
                break;
            case 3:
                roomNum.x++;
                break;
        }




        gameHandler.handleNextRoom(roomNum, Pos);
    }

    public void doorWasWalkedThroughNotNew(int door)
    {
        Vector2Int roomNum = Pos;
        switch (door)
        {
            case 0:
                roomNum.y++;
                break;
            case 1:
                roomNum.y--;
                break;
            case 2:
                roomNum.x--;
                break;
            case 3:
                roomNum.x++;
                break;
        }




        gameHandler.handleNextRoomOld(roomNum, Pos);
    }

    private void Update()
    {
        if (enemies.Count == 0)
        {
            if (!isRoomClear)
            {
                isRoomClear = true;
                UnLockRoom();
            }
        }
    }
}
