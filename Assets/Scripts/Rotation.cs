using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 4;
    [Tooltip("Um einen wie großen Winkel soll er sich um die x-Achse drehen? (Negative Zahlen gehen auch)")]
    [SerializeField] float xAngle;
    [Tooltip("Um einen wie großen Winkel soll er sich um die y-Achse drehen? (Negative Zahlen gehen auch)")]
    [SerializeField] float yAngle;
    [Tooltip("Um einen wie großen Winkel soll er sich um die z-Achse drehen? (Negative Zahlen gehen auch)")]
    [SerializeField] float zAngle;
    
    GameObject otherGameObject;
    
    Vector3 axis;
    
    bool rotatingProcess = false;

    private void Start()
    {
        axis = new Vector3(xAngle, yAngle, zAngle);
    }


    private void FixedUpdate()
    {
        if(rotatingProcess == true)
        {
            StartCoroutine(RotateMe(otherGameObject, axis, 2f));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            if (Input.GetKey(KeyCode.E))
            {
                otherGameObject = other.gameObject;
                rotatingProcess = true; 
            }
        }
    }

    IEnumerator RotateMe(GameObject box, Vector3 axis, float angle, float duration = 0.5f)
    {
        Quaternion from = box.transform.rotation;
        Quaternion to = box.transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float timePassed = 0.0f;
        
        while (timePassed < duration)
        {
            box.transform.rotation = Quaternion.Slerp(from, to, rotationSpeed * timePassed);
            timePassed += Time.deltaTime;
            yield return null;
            box.transform.rotation = to;
            rotatingProcess = false;
        }
    }
}