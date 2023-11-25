using UnityEngine;

public class Box_new : MonoBehaviour
{
    Rigidbody myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void SetPositionLock(int state, float xMovement, float zMovement, float moveSpeed)
    {
        moveSpeed *= 1.1f;
        if (state == 1)
        {
            myRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        }
        else if(state == 2) 
        {
            myRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation; 
        }
        else 
        {
            myRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        Vector3 moveDirection = new Vector3(xMovement, 0f, zMovement).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}
