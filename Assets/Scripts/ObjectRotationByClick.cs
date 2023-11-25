using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotationByClick : MonoBehaviour
{
    [HideInInspector]
    public bool rotatingProcess = false;

    [Tooltip("Um einen wie großen Winkel soll er sich um die x-Achse drehen? (Negative Zahlen gehen auch)")]
    [SerializeField] float xAngle; 
    [Tooltip("Um einen wie großen Winkel soll er sich um die y-Achse drehen? (Negative Zahlen gehen auch)")]
    [SerializeField] float yAngle; 
    [Tooltip("Um einen wie großen Winkel soll er sich um die z-Achse drehen? (Negative Zahlen gehen auch)")]
    [SerializeField] float zAngle;
    [SerializeField] float rotationSpeed = 4;
    [SerializeField] bool animationTrigger = false;
    [SerializeField] Animator animator; 

    [Tooltip("Wird für immer in die gleiche Richtung gedreht, oder nicht")]
    [SerializeField] bool infiniteRotationInOneDirection;

    [Tooltip("Dinge die beim Rotieren zum Vorschein kommen, oder verschwinden sollen")]
    [SerializeField] List<GameObject> HiddenObjects = new List<GameObject>(); 

    List<float> angleList = new List<float>(); 
    Vector3 axis; 
    bool rotationDone = false; 

    int negativePositiveSwitch = 1;

    [SerializeField] bool switchObject = false; 
    
    [SerializeField] bool BackTimerActivated;
    [SerializeField] float maxTimerBack;
    float timerBack = 0;
    bool timeractive = false;
    
    private void Awake()
    {
        axis = new Vector3(xAngle, yAngle, zAngle);
        angleList.Add(xAngle); 
        angleList.Add(yAngle); 
        angleList.Add(zAngle); 
    }
  
    private void FixedUpdate()
    {
        ClickOnRotatingObject(); 

        if(HiddenObjects.Count > 0)
        {
            if(switchObject)
            {
                SwitchHiddenObject();
            }
            else
            {
                RevealHiddenObjects(); 
            }    
        }
    }

    void SwitchHiddenObject()
    {
        if (rotationDone)
        {
            if (HiddenObjects[0].activeSelf == false)
            {
                HiddenObjects[0].SetActive(true);
                HiddenObjects[1].SetActive(false);
            }
            else
            {
                HiddenObjects[0].SetActive(false);
                HiddenObjects[1].SetActive(true);
            }

            rotationDone = false;
        }
    }
    void RevealHiddenObjects()
    {
        foreach(GameObject hiddenObject in HiddenObjects)
        {
            if(rotationDone)
            {
                if (hiddenObject.activeSelf == false)
                    hiddenObject.SetActive(true);
                else
                    hiddenObject.SetActive(false);

                rotationDone = false; 
            }
        }
    }

    void ClickOnRotatingObject()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Rotation")))
            {
                if(hit.collider == this.GetComponent<Collider>())
                {
                    Debug.Log("hit");
                    if (animationTrigger && animator != null)
                    {
                        animator.SetBool("hochgeklappt", true); 
                    }
                    else
                    {

                        if (rotatingProcess == false)
                        {
                            rotatingProcess = true;
                            timeractive = true; 
                            StartCoroutine(RotateMe(axis * negativePositiveSwitch, 1f));

                       

                            if (infiniteRotationInOneDirection == false && BackTimerActivated == false) //Die Rotation soll sich hin- und her bewegen
                            {
                                foreach (float angle in angleList)
                                {
                                    if (angle > 0)
                                    {
                                        negativePositiveSwitch = -negativePositiveSwitch;
                                    }
                                    else if (angle < 0)
                                    {
                                        negativePositiveSwitch = -negativePositiveSwitch;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        if(BackTimerActivated == true && timeractive == true)
        {
            timerBack += Time.deltaTime; 
            
            if(timerBack >= maxTimerBack)
            {
                StartCoroutine(RotateMe(axis * -negativePositiveSwitch, 1f));
                timerBack = 0;
                timeractive = false;
            }
        }
    }

    IEnumerator RotateMe(Vector3 axis, float angle, float duration = 2f)
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        //transform.rotation = Quaternion.Slerp(from, to, rotationSpeed * Time.deltaTime);
        
        float timePassed = 0.0f;
        while (timePassed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, rotationSpeed * timePassed);  
            timePassed += Time.deltaTime;
            yield return null;
            transform.rotation = to;
        }
        rotationDone = true; 
        rotatingProcess = false;
    }
}


