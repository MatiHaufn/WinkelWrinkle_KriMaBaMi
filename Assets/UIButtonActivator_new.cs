using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIButtonActivator_new : MonoBehaviour
{
    [SerializeField] string _animationName;
    [SerializeField] int maxTimer;
    [SerializeField] bool LBox; 

    InputActionReference button; 

    GameObject Player; 

    float timer = 0;
    bool timerActive = false;
    bool inTriggerZone = false; 
    bool _lBoxInPosition;
    bool alreadyActivated = false;


    private void Start()
    {
        Player = GameManager.instance.Player; 
      
        if (_animationName == "RightButton" || _animationName == "LBox")
            button = Player.GetComponent<PlayerMovement_new>().interact; 
        else
            button = Player.GetComponent<PlayerMovement_new>().push;
    }

    public void LBoxInPosition(bool inPosition)
    {
        _lBoxInPosition = inPosition;
        
        if(_lBoxInPosition) 
            _animationName = "RightButton";
        else
            _animationName = "LBox";
    }

    private void Update()
    {   
        if(alreadyActivated)
        {
            if (timerActive && inTriggerZone)
            {
                timer += Time.deltaTime; 
                if(timer >= maxTimer) 
                {
                    Player.GetComponent<PlayerMovement_new>().ButtonAnimationSetup(true, _animationName);
                    timerActive = false;
                    timer = 0; 
                }
            }
        
            if (!inTriggerZone)
            {
                Player.GetComponent<PlayerMovement_new>().ButtonAnimationSetup(false, _animationName);
            }
        
            if (button.action.IsPressed() && inTriggerZone && !LBox)
            {
                Debug.Log("Deactivate");
                Player.GetComponent<PlayerMovement_new>().ButtonAnimationSetup(false, _animationName);
                gameObject.SetActive(false);
            }
        
            if (button.action.IsPressed() && LBox && _lBoxInPosition)
            {
                Player.GetComponent<PlayerMovement_new>().ButtonAnimationSetup(false, _animationName);
                gameObject.SetActive(false);    
            }
        }
    }
        
    public void PlayerInTrigger(bool inTrigger)
    {
        inTriggerZone = inTrigger;
        timerActive = inTrigger;
        if (!alreadyActivated)
            alreadyActivated = inTrigger; 
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            inTriggerZone = true; 
            timerActive = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTriggerZone = false;
            timerActive = false;
        }
    }*/
}
