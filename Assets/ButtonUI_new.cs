using UnityEngine;

public class ButtonUI_new : MonoBehaviour
{
    Animator _myAnimator;
    SpriteRenderer _mySpriteRenderer;
   
    private void Start()
    {
        if(GetComponent<Animator>() != null)
        {
            _myAnimator = GetComponent<Animator>();   
        }
        _mySpriteRenderer = GetComponent<SpriteRenderer>(); 
    }
    
    void Update()
    {
        Transform cameraTransform = Camera.main.transform;
        transform.LookAt(cameraTransform.position, Vector3.up);
    }
    public void SetAnimationBool(bool activated, string boolName)
    {
        _myAnimator.SetBool(boolName, activated);
    }
}
