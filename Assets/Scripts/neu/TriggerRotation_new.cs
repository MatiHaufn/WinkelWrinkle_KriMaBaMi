using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TriggerRotation_new : MonoBehaviour
{
    public GameObject _rotatingObject;

    [SerializeField] float rotationSpeed;

    InputActionReference interact;

    bool _playerIsNear = false;
    bool objectInPosition = false;
    bool isRotating = false;
    bool rotated = false; 

    private void Start()
    {
        interact = GameManager.instance.Player.GetComponent<PlayerMovement_new>().interact;
    }

    private void Update()
    {
        if (_playerIsNear && objectInPosition && !isRotating)
        {
            if (interact.action.IsPressed())
            {
                //Debug.Log("pressed");
                StartCoroutine(RotateObject(_rotatingObject));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            _playerIsNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerIsNear = false;
        }
    }

    public void ObjInPos(bool inPosition)
    {
        objectInPosition = inPosition; 
    }

    IEnumerator RotateObject(GameObject box)
    {
        rotated = !rotated;
        box.GetComponent<Box_new>().ColliderRotated(rotated); 
        
        isRotating = true;
        Quaternion targetRotation = box.transform.rotation * Quaternion.Euler(0, 90, 0);
        while (Quaternion.Angle(box.transform.rotation, targetRotation) > 0.01f)
        {
            box.transform.rotation = Quaternion.Slerp(box.transform.rotation, targetRotation, Time.deltaTime * 5); 
            yield return null;
        }
        box.transform.rotation = targetRotation;
        isRotating = false;

    }
}
