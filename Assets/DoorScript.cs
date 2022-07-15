using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer thisSpriteRenderer;
    [SerializeField] Rigidbody2D thisrigidbody;
    [SerializeField] BoxCollider2D thisCollider;
    [SerializeField] RoomHandler_Script roomHandler;
    [SerializeField] int doorNum;
    public bool deactivated = false;
    // Start is called before the first frame update
    void Start()
    {
        thisSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    bool alreadyUpdated = false;
    // Update is called once per frame
    void Update()
    {
        alreadyUpdated = true;
        if ((roomHandler.Pos.x == 0 && doorNum == 2) || (roomHandler.Pos.y == 0 && doorNum == 1))
        {
            thisCollider.isTrigger = false;
            thisSpriteRenderer.color = Color.black;
        }
        else
        {
            if (thisSpriteRenderer.color.Equals(Color.red))
            {
                thisCollider.isTrigger = false;
            }
            else if (thisSpriteRenderer.color.Equals(Color.green))
            {
                thisCollider.isTrigger = true;
            }
        }
    }
    bool alreadyHit = false;
    float lastHit = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!alreadyUpdated || Time.realtimeSinceStartup - lastHit < .5f)
        {
            return;
        }
        lastHit = Time.realtimeSinceStartup;
        if (collision.gameObject.tag.Equals("Player") && !alreadyHit && !deactivated)
        {
            roomHandler.doorWasWalkedThroughNew(doorNum);
            alreadyHit = true;
            deactivated = true;
        }
        else if (deactivated)
        {
            roomHandler.doorWasWalkedThroughNotNew(doorNum);
        }
    }
}
