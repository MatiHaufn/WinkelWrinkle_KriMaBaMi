using UnityEngine;

public class StayHereToGetFlat_new : MonoBehaviour
{
    public Transform wall2DStartPositionPlayer;
    public Transform wall2DStartPositionBox; 

    [SerializeField] GameObject rotatingObject;
    [SerializeField] Material blinkingObjectMaterial;
    bool stayingInPosition = false;


    private void Update()
    {
        if (GameManager.instance.playerFlach)
            stayingInPosition = false; 
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            blinkingObjectMaterial.SetFloat("_blinking", true ? 1f : 0f);
            stayingInPosition = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            blinkingObjectMaterial.SetFloat("_blinking", false ? 1f : 0f);
            stayingInPosition = false;
        }
    }

    //used in PlayerMovement_new 
    public void PressedButton()
    {
        if (stayingInPosition)
        {
            rotatingObject.GetComponent<RotatingObjectByClick_new>().TriggerAnimation();
        }
    }
}
