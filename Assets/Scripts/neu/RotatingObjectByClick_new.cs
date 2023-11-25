using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObjectByClick_new : MonoBehaviour
{
    [SerializeField] bool animationTrigger = false;
    [SerializeField] int setVariablesInAnimation = 0;
    [SerializeField] Animator animator;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Rotation")))
            {
                if (hit.collider == this.GetComponent<Collider>())
                {
                    TriggerAnimation(); 
                }
            }
        }
    }

    public void TriggerAnimation()
    {
        if (animationTrigger && animator != null)
        {
            if (setVariablesInAnimation == 1)
            {
                animator.SetTrigger("AnimatorTrigger");
            }

            if (setVariablesInAnimation == 2)
            {
                animator.SetBool("AnimatorBool", true);
            }
        }
    }
}
