using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float moveSpeed;
    private float dirX;
    private bool facingRight = true;
    private Vector3 localScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
        moveSpeed = 3f;
    }

    private void Update()
    {
        dirX = Input.GetAxis("Horizontal") * moveSpeed;

        if (Input.GetButtonDown("Jump") && rb.velocity.y > -0.1 && rb.velocity.y < 0.1)
            rb.AddForce(Vector2.up * 500f);

        if (Mathf.Abs(dirX) > 0 && rb.velocity.y > -0.1 && rb.velocity.y < 0.1)
        {
            anim.SetBool("isRunning", true);
        }
        else
            anim.SetBool("isRunning", false);

        if (rb.velocity.y > -0.1 && rb.velocity.y < 0.1)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }

        if (rb.velocity.y > 0.1)
            anim.SetBool("isJumping", true);


        if (rb.velocity.y < -0.1)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    private void LateUpdate()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

}
