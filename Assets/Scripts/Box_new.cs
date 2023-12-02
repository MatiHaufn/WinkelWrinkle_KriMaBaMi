using System.Collections.Generic;
using UnityEngine;

public class Box_new : MonoBehaviour
{
    [SerializeField] GameObject[] pushColliders; 
    [SerializeField] bool boxFlach = false;

    Rigidbody myRigidbody;
    Vector3 startPosition;
    Vector3 exit2DPosition; 

    int BoxLayer = 12; 
    int flatBoxLayer = 13;

    bool stayingInExit = false;
    bool plattmacherTouched = false;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }


    private void Update()
    {
        if (GameManager.instance.Player.GetComponent<PlayerMovement_new>().interact.action.IsPressed())
        {
            if (stayingInExit)
            {
                Debug.Log("Player doing Exit"); 
                boxFlach = false;
                Set3DSettings(exit2DPosition, true);
            }
        }
    }
    void Set2DSettings()
    {
        gameObject.layer = flatBoxLayer;

        foreach(GameObject obj in pushColliders)
        {
            if(obj.tag == "ColliderZ")
            {
                obj.SetActive(false); 
            }
            else if (obj.tag == "ColliderX")
            {
                obj.layer = flatBoxLayer;
            }
        }
    }

    void Set3DSettings(Vector3 spawnPosition, bool vfxActive)
    {
        gameObject.layer = BoxLayer;

        gameObject.GetComponent<Plattmacher_new>().Get3D(vfxActive, spawnPosition);
        foreach (GameObject obj in pushColliders)
        {
            if (obj.tag == "ColliderZ")
            {
                obj.SetActive(true);
            }
            else if (obj.tag == "ColliderX")
            {
                obj.layer = BoxLayer;
            }
        }
    }

    // While pushing in one direction, other directions are locked
    public void SetPositionLock(int state, Vector2 playerMovement, float moveSpeed)
    {
        Vector3 moveDirection;

        if (state == 1) //if Z Axis 
        {
            moveDirection = new Vector3(0f, 0f, playerMovement.y).normalized;
            myRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        }
        else if(state == 2) //if X Axis 
        {
            moveDirection = new Vector3(playerMovement.x, 0f, 0f).normalized;
            myRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation; 
        }
        else 
        {
            moveDirection = new Vector3(0f, 0f, 0f).normalized;
            myRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Plattmacher" && boxFlach == false)
        {
            plattmacherTouched = true;
        }

        if (other.gameObject.tag == "Losezone")
        {
            Debug.Log("Box Lose");
            Vector3 newStartPosition = startPosition + Vector3.up; 
            Set3DSettings(newStartPosition, false);
            boxFlach = false;
        }

        if (other.gameObject.tag == "Exit2D")
        {
            stayingInExit = true;
            exit2DPosition = other.gameObject.GetComponent<ExitFrom2DScript>().exitObject.position; 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Exit2D")
        {
            stayingInExit = false;
        }
        if (other.gameObject.tag == "Plattmacher" || boxFlach == true)
        {
            plattmacherTouched = false;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "StayHereToGetFlat")
        {
            if (plattmacherTouched && boxFlach == false)
            {
                gameObject.GetComponent<Plattmacher_new>().Get2D(other.gameObject.GetComponent<StayHereToGetFlat_new>().wall2DStartPositionBox);
                Set2DSettings(); 
                boxFlach = true;
            }
        }
    }
}