using UnityEngine;

public class RotatingObjectByClick_new : MonoBehaviour
{
    [SerializeField] Animator animator;
    Collider myCollider;

    float colliderTimer = 0;
    float maxColliderTimer = 1; 
    bool animationActive = false;
    bool boolActive = false;

    float cooldownTimer = 0; 
    float maxCooldownTimer = 2;
    bool readyToMove = true; 

    
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

        if(!readyToMove)
        {
            cooldownTimer += Time.deltaTime; 
            if(cooldownTimer > maxCooldownTimer )
            {
                readyToMove = true;
                cooldownTimer = 0; 
            }
        }

    }

    public void TriggerAnimation()
    {
        if (animator != null && !animationActive)
        {
            animator.SetTrigger("AnimatorTrigger");
            animationActive = true; 
            myCollider.enabled = true; 
        }
    }

    public void SetAnimationBool()
    {
        if (readyToMove)
        {
            boolActive = !boolActive;
            readyToMove = false;
        }
        if (animator != null && !animationActive)
        {
            animator.SetBool("activated", boolActive);
        }
    }
}
