using System.Collections.Generic;
using UnityEngine;

public class SetObjectToPositionThroughTrigger_new : MonoBehaviour
{
    [SerializeField] GameObject ObjToSet;
    [SerializeField] List<GameObject> ObjsToDeactivate;
    [SerializeField] Transform targetPosition;
    bool positionSet = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Box" && !positionSet)
        {
            ObjToSet.transform.position = targetPosition.position;
            other.gameObject.tag = "Ground";
            positionSet = true;
            Debug.Log("setPosition");
        }
    }
}
