using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement_new : MonoBehaviour
{
    public GameObject playerCollision3D;
    public GameObject playerCollision2D;

    [SerializeField] GroundTest groundTest;

    [SerializeField] Animator animator3D;
    [SerializeField] Animator animator2D;

    [SerializeField] float maxJumpForce = 10f;
    [SerializeField] float jumpforce = 10f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float moveSpeed = 2;
    
    [SerializeField] InputActionReference leftStick, rightStick, jump, interact, start, push; 
    Animator currentAnimator;
    Rigidbody myRigidbody;

    //Push Box 
    [SerializeField] LayerMask boxLayer; 
    GameObject boxToInteract;
    bool stayingAtColliderX;
    bool stayingAtColliderZ;
    bool rotationActivated = true;

    //floats for Controls 
    GameObject objToStayActivate;
    Vector2 mouseMovement; 
    Vector2 playerMovement; 
    float turnSmoothVelocity;

    //Timer for Idle Animations 
    float idleTimer = 0;
    float maxIdleTime = 20;
    int idleState = 0;

    //Ground Bool 
    [SerializeField] LayerMask[] layersToStand;
    float raycastDistance = 0.1f; 
    public bool isGrounded;
    Ray groundRay; 

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        currentAnimator = animator3D; 
    }

    private void Update()
    {
        isGrounded = groundTest.IsGrounded();
        IdleAnimation();
        GroundCheck();
        Movement();
    }
    void GroundCheck()
    {
        groundRay = new Ray(transform.position + (Vector3.up * 0.1f), Vector3.down);

        if (Physics.Raycast(groundRay, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                currentAnimator.SetBool("JumpStart", false);
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        Debug.DrawRay(groundRay.origin, groundRay.direction * raycastDistance, isGrounded ? Color.green : Color.red);
    }
    void Movement()
    {
        /*
         Vector3 characterScale = transform.localScale;

         if (GameManager.instance.playerFlach == true)
         {
             currentAnimator = animator2D;
             if (horizontal > 0)
             {
                 characterScale.x = Mathf.Abs(transform.localScale.x);
             }
             else if (horizontal < 0)
             {
                 characterScale.x = -Mathf.Abs(transform.localScale.x);
             }
             transform.localScale = characterScale;
         }
         else
         {
             characterScale.x = Mathf.Abs(transform.localScale.x);
             transform.localScale = characterScale;
             currentAnimator = animator3D;
         }*/

      
        if (GameManager.instance.playerMoving)
        {
            playerMovement = leftStick.action.ReadValue<Vector2>();
            mouseMovement = rightStick.action.ReadValue<Vector2>();

            if (GameManager.instance.playerFlach == true)
            {
                playerMovement.y = 0f;
                transform.rotation = Quaternion.Euler(0f, -180f, 0f);
            }
            else
            {
                Vector3 direction = new Vector3(playerMovement.x, 0f, playerMovement.y).normalized;

                if (direction.magnitude >= 0.1f && rotationActivated)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
            }

            Vector3 moveDirection = new Vector3(playerMovement.x, 0f, playerMovement.y).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        //Jump
        if (isGrounded == true)
        {
            if (jump.action.IsPressed())
            {
                if(myRigidbody.velocity.magnitude < maxJumpForce)
                {
                    currentAnimator.SetBool("JumpStart", true);
                    myRigidbody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
                }
            }
        }

        //Interact to get flat
        if (interact.action.IsPressed() && objToStayActivate != null)
        {
            objToStayActivate.GetComponent<StayHereToGetFlat_new>().PressedButton(); 
        }

        Ray boxTrackRay = new Ray(transform.position + (transform.up * (transform.localScale.y / 2)), transform.forward);
        float boxTrackRayDistance = 0.5f; 
        Debug.DrawRay(boxTrackRay.origin, boxTrackRay.direction * boxTrackRayDistance, rotationActivated ? Color.blue : Color.red);

        if (boxToInteract != null)
        {
            if (Physics.Raycast(boxTrackRay, out RaycastHit hit, boxTrackRayDistance, boxLayer))
            {
                Debug.Log(hit.collider.gameObject.name); 
                if (push.action.IsPressed())
                {
                    if(hit.collider.gameObject.tag == "Box")
                    {
                        if (stayingAtColliderZ)
                        { 
                            rotationActivated = false; 
                            Debug.Log("Collider Z"); 
                            boxToInteract.GetComponent<Box_new>().SetPositionLock(1, 0, playerMovement.y, moveSpeed);
                        }
                        else if (stayingAtColliderX)
                        {
                            rotationActivated = false; 
                            Debug.Log("Collider X"); 
                            boxToInteract.GetComponent<Box_new>().SetPositionLock(2, playerMovement.x, 0, moveSpeed);
                        }
                        else
                        {
                            rotationActivated = true;
                            Debug.Log("NONE"); 
                            boxToInteract.GetComponent<Box_new>().SetPositionLock(0, 0, 0, 0);
                        }
                    }
                }
                else
                {
                    boxToInteract.GetComponent<Box_new>().SetPositionLock(0, 0, 0, 0);
                }
            }
        }
        else
        {
            rotationActivated = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "StayHereToGetFlat")
        {
            objToStayActivate = other.gameObject;
        }
        if (other.gameObject.tag == "ColliderZ")
        {
            stayingAtColliderZ = true;
            boxToInteract = other.gameObject.transform.parent.gameObject;
        }
        if (other.gameObject.tag == "ColliderX")
        {
            stayingAtColliderX = true;
            boxToInteract = other.gameObject.transform.parent.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ColliderZ")
        {
            stayingAtColliderZ = false;
        }
        if (other.gameObject.tag == "ColliderX")
        {
            stayingAtColliderX = false;
        }
    }

    //Animation 
    void IdleAnimation()
    {
        if (playerMovement != Vector2.zero)
        {
            currentAnimator.SetFloat("speed", 1);
            idleTimer = 0;
            idleState = 0;
        }
        else
        {
            currentAnimator.SetFloat("speed", 0);
        }

        idleTimer += Time.deltaTime;

        if (idleTimer >= maxIdleTime)
        {
            idleState = Random.Range(1, 3);
            idleTimer = 0;
        }

        if (idleState == 1)
        {
            currentAnimator.SetTrigger("Idle2");
            idleTimer = 0;
            idleState = 0;
        }
        else if (idleState == 2)
        {
            currentAnimator.SetTrigger("Idle3");
            idleTimer = 0;
            idleState = 0;
        }
    }
}
