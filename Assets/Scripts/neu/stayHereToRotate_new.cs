using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stayHereToRotate_new : MonoBehaviour
{
    [SerializeField] GameObject rotatingObject;
    [SerializeField] Material blinkingObjectMaterial;

    private void Start()
    {
        if (blinkingObjectMaterial != null)
        {
            blinkingObjectMaterial.SetFloat("_blinking", 0f);
        }
    }

    public void ActivatedRotationTrigger()
    {
        rotatingObject.GetComponent<RotatingObjectByClick_new>().SetAnimationBool();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (blinkingObjectMaterial != null)
                blinkingObjectMaterial.SetFloat("_blinking", true ? 1f : 0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (blinkingObjectMaterial != null)
                blinkingObjectMaterial.SetFloat("_blinking", false ? 1f : 0f);
        }
    }
}
