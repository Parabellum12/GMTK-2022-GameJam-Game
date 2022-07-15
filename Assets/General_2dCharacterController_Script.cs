using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_2dCharacterController_Script : MonoBehaviour
{
    KeyCode MoveUp = KeyCode.W;
    KeyCode MoveDown = KeyCode.S;
    KeyCode MoveLeft = KeyCode.A;
    KeyCode MoveRight = KeyCode.D;


    public bool AllowMovement = true;

    public float TopSpeed = 10f;
    public float Acceleration = 5f;
    public float Deceleration = 10f;

    [SerializeField] Rigidbody2D CharacterRigidBody;

    float currentXSpeed = 0;
    float currentYSpeed = 0;
    public void Update()
    {
        float xmove = 0;
        float ymove = 0;
        if (Input.GetKey(MoveUp))
        {
            ymove += 1;
        }
        if (Input.GetKey(MoveDown))
        {
            ymove -= 1;
        }
        if (Input.GetKey(MoveLeft))
        {
            xmove -= 1;
        }
        if (Input.GetKey(MoveRight))
        {
            xmove += 1;
        }
        handleMove(xmove, ymove);
    }


    void handleMove(float xmove, float ymove)
    {


        if (xmove != 0)
        {
            currentXSpeed += xmove * Acceleration * Time.deltaTime;
        }
        else
        {
            if (currentXSpeed > 0)
            {
                currentXSpeed -= Deceleration * Time.deltaTime;
            }
            else if (currentXSpeed < 0)
            {
                currentXSpeed += Deceleration * Time.deltaTime;
            }

        }
        currentXSpeed = Mathf.Clamp(currentXSpeed, -TopSpeed, TopSpeed);
        if (Mathf.Abs(currentXSpeed) < .05)
        {
            currentXSpeed = 0;
        }

        if (ymove != 0)
        {
            currentYSpeed += ymove * Acceleration * Time.deltaTime;
        }
        else
        {
            if (currentYSpeed > 0)
            {
                currentYSpeed -= Deceleration * Time.deltaTime;
            }
            else if (currentYSpeed < 0)
            {
                currentYSpeed += Deceleration * Time.deltaTime;
            }
        }
        currentYSpeed = Mathf.Clamp(currentYSpeed, -TopSpeed, TopSpeed);
        if (Mathf.Abs(currentYSpeed) < .05)
        {
            currentYSpeed = 0;
        }


        float xSpeedToMoveBy = currentXSpeed;
        float ySpeedToMoveBy = currentYSpeed;
        if (currentXSpeed != 0 && currentYSpeed != 0)
        {
            xSpeedToMoveBy = currentXSpeed / 1.4f;
            ySpeedToMoveBy = currentYSpeed / 1.4f;
        }
        
        CharacterRigidBody.velocity = new Vector2(xSpeedToMoveBy, ySpeedToMoveBy);
        //Vector3 test = new Vector3(transform.position.x + xSpeedToMoveBy * Time.deltaTime, transform.position.y + ySpeedToMoveBy * Time.deltaTime, -1f);
        //transform.position =  test;

    }


}

