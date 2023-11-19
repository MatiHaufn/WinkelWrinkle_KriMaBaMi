using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Tooltip("Wenn sich das Objekt unterschiedlich schnell vor- und rückwärts bewegen soll, dann anhaken")]
    [SerializeField]bool differentBackSpeed = false; 
    [SerializeField] bool isRotating = false; 
    [Tooltip("Grundgeschwindigkeit")]
    [SerializeField] float forwardSpeed;
    [Tooltip("Nur, wenn er auf dem Rückweg eine andere Geschwindigkeit annehmen soll! (Dafür 'differentBackSpeed' ankreuzen)")]
    [SerializeField] float backwardSpeed;
    [SerializeField] float rotatingSpeed; 
    
    float currentSpeed;

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

            if (isRotating)
            {
                float rotationSpeed = Time.deltaTime * currentSpeed * rotatingSpeed;

                if (w == 0)
                {
                    gameObject.transform.Rotate(Vector3.forward, rotationSpeed);
                }
                else if (w == 1)
                {
                    gameObject.transform.Rotate(Vector3.back, rotationSpeed);
                }
            }
        }
    }
}
