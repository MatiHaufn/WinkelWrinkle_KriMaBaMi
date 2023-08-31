using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movingupdown : MonoBehaviour
{
    public float MoveDuration = 1;
    public float MoveSpeed = 1;
    float StartMoveSpeed; 

    public bool right = false;
    public bool top = false;
    public bool negativeUpDown = false;
    public bool negativeLeftRight = false;
    public bool infiniteOneWay = false;

    public bool standingIsTrue = false; 
    private Vector3 StartPos; 

    float Timer = 0;
    int MoveDirection = 1; 
    int updownDirection = 1; 
    int leftrightDirection = 1;

    public float stopTime = 2;
    float stopTimerDuration = 0; 

    private void Start()
    {
        StartPos = gameObject.transform.position;
        StartMoveSpeed = MoveSpeed; 
    }
    private void Update()
    {
        if(standingIsTrue == false)
            Moving(); 
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (standingIsTrue == true)
        {
            if (other.gameObject.tag == "Player")
            {
                Moving();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(standingIsTrue == true)
        {
            gameObject.transform.position = StartPos; 
        }
    }

    void Moving()
    {
        if (negativeUpDown == true)
        {
            updownDirection = -1;
        }
        else
        {
            updownDirection = 1;
        }

        if (negativeLeftRight == true)
        {
            leftrightDirection = -1;
        }
        else
        {
            leftrightDirection = 1;
        }

        if (infiniteOneWay == true)
        {
            if (top == true)
            {
                transform.Translate(new Vector3(0, MoveSpeed, 0) * MoveDirection * updownDirection * Time.deltaTime);
            }
            if (right == true)
            {
                transform.Translate(new Vector3(MoveSpeed, 0, 0) * MoveDirection * leftrightDirection * Time.deltaTime);
            }
        }
        else
        {
            if (top == true)
            {
                Timer += Time.deltaTime;

                if (Timer > MoveDuration)
                {
                    stopTimerDuration += Time.deltaTime;
                    MoveSpeed = 0; 
                    if(stopTimerDuration >= stopTime)
                    {
                        
                        MoveDirection = -MoveDirection;
                        MoveSpeed = StartMoveSpeed;
                        Timer = 0;
                        stopTimerDuration = 0; 
                    }
                }
                transform.Translate(new Vector3(0, MoveSpeed, 0) * MoveDirection * updownDirection * Time.deltaTime);
            }


            if (right == true)
            {
                Timer += Time.deltaTime;
                if (Timer > MoveDuration)
                {
                    stopTimerDuration += Time.deltaTime;
                    MoveSpeed = 0;
                    if (stopTimerDuration >= stopTime)
                    {

                        
                        MoveSpeed = StartMoveSpeed;
                        MoveDirection = -MoveDirection;
                        Timer = 0;
                        stopTimerDuration = 0;
                    }
                }
                transform.Translate(new Vector3(MoveSpeed, 0, 0) * MoveDirection * leftrightDirection * Time.deltaTime);
            }
        }
    }

}
