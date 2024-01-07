using System.Threading;
using UnityEngine;

public class StayHereToGetFlat_new : MonoBehaviour
{
    public Transform wall2DStartPositionPlayer;
    public Transform wall2DStartPositionBox; 

    [SerializeField] GameObject rotatingObject;
    [SerializeField] Material blinkingObjectMaterial;
    [SerializeField] bool triggerAnimation = false;

    GameObject currentBox; 
    bool boxStayingInPosition = false;
    bool stayingInPosition = false;
    bool erzwungenPlatt = false; 
    
    private void Update()
    {
        if(GameManager.instance.playerFlach == true)
        {
            stayingInPosition = false;
        }

        if(erzwungenPlatt)
        {
            if(stayingInPosition)
            {
                GameManager.instance.Player.GetComponent<PlayerMovement_new>().Get2DPublic(this.gameObject);
            }
            if (boxStayingInPosition && currentBox != null)
            {
                currentBox.GetComponent<Box_new>().Get2DPublic(this.gameObject);
            }
        }
    }


    private void Start()
    {
        if(blinkingObjectMaterial != null)
        {
            blinkingObjectMaterial.SetFloat("_blinking", 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ErzwungenPlatt")
        {
            erzwungenPlatt = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stayingInPosition = true;
            if (blinkingObjectMaterial != null)
                blinkingObjectMaterial.SetFloat("_blinking", true ? 1f : 0f);
        }
        if(other.gameObject.tag == "Box")
        {
            currentBox = other.gameObject; 
            boxStayingInPosition = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stayingInPosition = false;
            if (blinkingObjectMaterial != null)
                blinkingObjectMaterial.SetFloat("_blinking", false ? 1f : 0f);
        }
        if(other.gameObject.tag == "Box")
        {
            boxStayingInPosition = false;
        }
        if (other.gameObject.tag == "ErzwungenPlatt")
        {
            erzwungenPlatt = false;
        }
    }

    //used in PlayerMovement_new 
    public void PressedButton()
    {
        if (stayingInPosition && !erzwungenPlatt)
        {
            if (triggerAnimation)
            {
                rotatingObject.GetComponent<RotatingObjectByClick_new>().TriggerAnimation();
            }
            else
            {
                rotatingObject.GetComponent<RotatingObjectByClick_new>().SetAnimationBool();
            }
        }
    }


}
