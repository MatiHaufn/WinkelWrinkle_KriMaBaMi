using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIButtonActivator_new : MonoBehaviour
{
    [SerializeField] List<AnimationBool> _animationBool;
    [SerializeField] int maxTimer;
    [SerializeField] bool LBox; 

    InputActionReference button; 

    AnimationBool _currentAnimation; 
    GameObject Player; 

    float timer = 0;
    bool timerActive = false;
    bool onceActivated = false;
    bool inTriggerZone = false; 
    bool LBoxInPosition; 


    private void Start()
    {
        Player = GameManager.instance.Player; 
        foreach (AnimationBool animationBool in _animationBool)
        {
            if(animationBool.isActive == true) 
            { 
                _currentAnimation = animationBool;
            }
        }

        if(_currentAnimation.name == "RightButton" || _currentAnimation.name == "LBox")
        {
            button = Player.GetComponent<PlayerMovement_new>().interact; 
        }
        else
        {
            button = Player.GetComponent<PlayerMovement_new>().push;
        }

    }

    public void ObjInPos(bool inPosition)
    {
        LBoxInPosition = inPosition;
        
        if(LBoxInPosition) 
        { 
            _currentAnimation.name = "RightButton";
        }
        else
        {
            _currentAnimation.name = "LBox";
        }
    }



    private void Update()
    {
        if (timerActive && !onceActivated)
        {
            timer += Time.deltaTime; 
            if(timer >= maxTimer) 
            {
                Player.GetComponent<PlayerMovement_new>().ButtonAnimationSetup(true, _currentAnimation.name);
                timerActive = false;
                timer = 0; 
            }
        }

        if (button.action.IsPressed() && inTriggerZone && !LBox)
        {
            Player.GetComponent<PlayerMovement_new>().ButtonAnimationSetup(false, _currentAnimation.name);
            onceActivated = true;
        }
        
        if (button.action.IsPressed() && LBox && LBoxInPosition)
        {
            Player.GetComponent<PlayerMovement_new>().ButtonAnimationSetup(false, _currentAnimation.name);
            onceActivated = true;
        }
    }
        
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
            if (Player.GetComponent<PlayerMovement_new>() != null)
            {
                Player.GetComponent<PlayerMovement_new>().ButtonAnimationSetup(false, _currentAnimation.name);
            }
        }
    }
}

[System.Serializable]
public class AnimationBool
{
    public string name;
    public bool isActive; 
}
