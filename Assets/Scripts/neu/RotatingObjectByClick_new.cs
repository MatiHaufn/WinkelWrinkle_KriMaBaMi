using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObjectByClick_new : MonoBehaviour
{
    [SerializeField] Animator animator;
    Collider myCollider;

    float colliderTimer = 0;
    float maxColliderTimer = 1; 
    bool animationActive = false;

    private void Start()
    {
        myCollider = GetComponent<Collider>();
        myCollider.enabled = false;
    }
    private void Update()
    {
        if (myCollider.enabled == true)
        {
            colliderTimer += Time.deltaTime; 
            if(colliderTimer > maxColliderTimer)
            {
                myCollider.enabled = false;
                animationActive = false;
                colliderTimer = 0; 
            }
        }        
    }

    public void TriggerAnimation()
    {
        if (animator != null && !animationActive)
        {
            animationActive = true; 
            myCollider.enabled = true; 
            animator.SetTrigger("AnimatorTrigger");
        }
    }
}
