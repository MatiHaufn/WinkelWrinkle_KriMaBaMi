using UnityEngine;

public class ObjectInTrigger_new : MonoBehaviour
{
    [SerializeField] GameObject triggerRotation;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Box")
        {
            triggerRotation.GetComponent<TriggerRotation_new>().ObjInPos(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Box")
        {
            triggerRotation.GetComponent<TriggerRotation_new>().ObjInPos(false);
        }
    }
}
