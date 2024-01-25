using UnityEngine;

public class ObjectInTrigger_new : MonoBehaviour
{
    [SerializeField] TriggerRotation_new triggerRotation;
    [SerializeField] UIButtonActivator_new UIButtonScript;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Box")
        {
            triggerRotation.ObjInPos(true);
            UIButtonScript.LBoxInPosition(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Box")
        {
            triggerRotation.ObjInPos(false);
            UIButtonScript.LBoxInPosition(false);
        }
    }
}
