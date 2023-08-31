using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Tooltip("Grundgeschwindigkeit")]
    [SerializeField] float forwardSpeed;
    [Tooltip("Wenn sich das Objekt unterschiedlich schnell vor- und rückwärts bewegen soll, dann anhaken")]
    [SerializeField]bool differentBackSpeed = false; 
    [Tooltip("Nur, wenn er auf dem Rückweg eine andere Geschwindigkeit annehmen soll! (Dafür 'differentBackSpeed' ankreuzen)")]
    [SerializeField] float backwardSpeed;
    float currentSpeed;
    Rigidbody myRigidbody;
    [SerializeField] Quaternion targetRotation;
    [SerializeField] float angle; 


    [SerializeField] List<Transform> waypoints = new List<Transform>(); 
    int w = 0;
    float offset = 0.2f;

    [SerializeField] float forwardStay;
    [SerializeField] float backwardStay;
    float stayTimer = 0;
    float maxTimer;
    bool isMoving = false;
   
    void Start()
    {
        currentSpeed = forwardSpeed;
        if (this.gameObject.GetComponent<Rigidbody>() != null)
        {
            myRigidbody = GetComponent<Rigidbody>();
        }
    }
   
    void Update()
    {
        stayTimer += Time.deltaTime; 
       
        if(stayTimer >= maxTimer)
        {
            isMoving = true;
        }

        Moving(); 
    }

    void Moving()
    {
        float dist = Vector3.Distance(transform.position, waypoints[w].transform.position);
        if (dist < offset)
        {
            stayTimer = 0;
            isMoving = false;

            if(w == waypoints.Count - 1)
            {
                w = 0;
                maxTimer = forwardStay;
            }
            else
            {
                w++;
                maxTimer = backwardStay; 
            }
            
        }
        if(differentBackSpeed)
        {
            if(w == 0)
            {
                currentSpeed = forwardSpeed; 
            }
            else
            {
                currentSpeed = backwardSpeed;
            }
        }

        if (isMoving)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, waypoints[w].transform.position, Time.deltaTime * currentSpeed);
            /*
            if (this.gameObject.GetComponent<Rigidbody>() != null)
            {
                if(w == 0)
                {
                    gameObject.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, angle * Time.deltaTime * currentSpeed);
                    //myRigidbody.AddTorque(Vector3.forward); 
                }
                else if(w == 1)
                {
                   //myRigidbody.AddTorque(Vector3.back); 
                   //gameObject.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, angle);

                }
            }
        */
        }
        
    }
}
