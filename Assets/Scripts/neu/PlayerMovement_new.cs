using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement_new : MonoBehaviour
{
    public InputActionReference leftStick, rightStick, jump, interact, start, push; 
    public GameObject playerCollision2D;
    public GameObject playerCollision3D;

    [SerializeField] Animator animator3D;
    [SerializeField] Animator animator2D;

    int playerLayer = 6;
    int flatPlayerLayer = 10;

    [SerializeField] float maxJumpForce = 10f;
    [SerializeField] float jumpforce = 10f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float moveSpeed = 2;

    Animator currentAnimator;
    Rigidbody myRigidbody;
    Vector3 exit2DPosition; 

    //Push Box 
    [SerializeField] LayerMask box3DLayer;
    [SerializeField] LayerMask box2DLayer;
    LayerMask currentBoxLayerMask;
    bool grabbingBox = false; 

    Ray boxTrackRay; 
    GameObject boxToInteract;
    bool stayingAtColliderX;
    bool stayingAtColliderZ;
    bool rotationActivated = true;
    bool pushingInZ = false;
    bool pushingInX = false;

    //Plattmacher
    bool plattmacherTouched = false;

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
    [SerializeField] string[] tagsToStand;
    float raycastDistance; 
    public bool isGrounded;
    Ray groundRay; 

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        currentAnimator = animator3D;
        currentBoxLayerMask = box3DLayer;
    }

    private void Update()
    {
        IdleAnimation();
        GroundCheck();
        Movement();
    }

    void Set2DSettings()
    {
        playerCollision2D.SetActive(true);
        playerCollision3D.SetActive(false); 
        myRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        ChangeFlatnessSettings(flatPlayerLayer);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        currentAnimator = animator2D;
    }

    void Set3DSettings()
    {
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        playerCollision2D.SetActive(false);
        playerCollision3D.SetActive(true);
        myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        ChangeFlatnessSettings(playerLayer);
        currentAnimator = animator3D;
    }

    void ChangeFlatnessSettings(int layer)
    {
        gameObject.layer = layer;
    }

    void GroundCheck()
    {
        groundRay = new Ray(transform.position + (Vector3.up * 0.1f), Vector3.down);
        raycastDistance = 0.2f; 

        if (Physics.Raycast(groundRay, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.CompareTag(tagsToStand[0]) || hit.collider.CompareTag(tagsToStand[1]))
            {
                currentAnimator.SetBool("JumpStart", false);
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
        Debug.DrawRay(groundRay.origin, groundRay.direction * raycastDistance, isGrounded ? Color.green : Color.red);
    }
    void Movement()
    {   
        if (GameManager.instance.playerMoving)
        {
            playerMovement = leftStick.action.ReadValue<Vector2>();
            mouseMovement = rightStick.action.ReadValue<Vector2>();

            if (GameManager.instance.playerFlach == true)
            {
                playerMovement.y = 0f;

                if (grabbingBox == false)
                {
                    if (playerMovement.x < 0)
                    {
                        transform.rotation = Quaternion.Euler(0f, 0f, 0f); 
                    }
                    else if (playerMovement.x > 0)
                    {
                        transform.rotation = Quaternion.Euler(0f, 180f, 0f); 
                    }
                }
            }
            else
            {
                if (pushingInX)
                {
                    playerMovement.y = 0f;
                }
                if (pushingInZ)
                {
                    playerMovement.x = 0f;
                }
                
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
            if (jump.action.WasPressedThisFrame())
            {
                if(myRigidbody.velocity.magnitude < maxJumpForce)
                {
                    Debug.Log("Jump");
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

        if (GameManager.instance.playerFlach == false)
        {
            boxTrackRay = new Ray(transform.position + (transform.up * (transform.localScale.y / 2)), transform.forward);
            currentBoxLayerMask = box3DLayer; 
        }
        else
        {
            boxTrackRay = new Ray(transform.position + (transform.up * (transform.localScale.y / 2)), -transform.right);
            currentBoxLayerMask = box2DLayer; 
        }
        
        float boxTrackRayDistance = 1.5f; 
        Debug.DrawRay(boxTrackRay.origin, boxTrackRay.direction * boxTrackRayDistance, rotationActivated ? Color.blue : Color.red);

        if (boxToInteract != null)
        {
            if (Physics.Raycast(boxTrackRay, out RaycastHit hit, boxTrackRayDistance, currentBoxLayerMask))
            { 
                if (push.action.IsPressed())
                {
                    if(hit.collider.gameObject.tag == "Box" || hit.collider.gameObject.tag == "TwoDBox")
                    {
                        rotationActivated = false; 
                        if (stayingAtColliderZ)
                        {
                            grabbingBox = true;
                            pushingInZ = true; 
                            pushingInX = false; 
                            //Debug.Log("Collider Z"); 
                            boxToInteract.GetComponent<Box_new>().SetPositionLock(1, playerMovement, moveSpeed);
                        }
                        else
                        {
                            pushingInZ = false;
                            grabbingBox = false;
                        }

                        if (stayingAtColliderX)
                        {
                            grabbingBox = true; 
                            pushingInZ = false; 
                            pushingInX = true; 
                            //Debug.Log("Collider X");
                            if (GameManager.instance.playerFlach == false)
                            {
                                boxToInteract.GetComponent<Box_new>().SetPositionLock(2, playerMovement, moveSpeed);
                            }
                            else
                            {
                                boxToInteract.GetComponent<Box_new>().SetPositionLock(2, playerMovement, moveSpeed);
                            }
                        }
                        else
                        {
                            pushingInX = false;
                            grabbingBox = false; 
                        }
                    }
                }
            }
            else
            {
                grabbingBox = false;
                rotationActivated = true;
                boxToInteract.GetComponent<Box_new>().SetPositionLock(0, new Vector2(0f, 0f), 0f);
            }

            if(!stayingAtColliderX && !stayingAtColliderZ)
            {
                boxToInteract.GetComponent<Box_new>().SetPositionLock(0, new Vector2(0f, 0f), 0f);
            }
            if(push.action.WasReleasedThisFrame()) 
            {
                pushingInZ = false;
                pushingInX = false;
                rotationActivated = true;
                boxToInteract.GetComponent<Box_new>().SetPositionLock(0, new Vector2(0f, 0f), 0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Plattmacher")
        {
            plattmacherTouched = true; 
        }

        foreach (GameObject checkpoint in GameManager.instance.Checkpoints)
        {
            if (other.gameObject == checkpoint.gameObject)
            {
                GameManager.instance.lastCheckpoint = checkpoint;
            }
        }
        if (other.gameObject.tag == "Losezone")
        {
            //Debug.Log("Lose");
            GameManager.instance.playerFlach = false;
            Set3DSettings();
            gameObject.GetComponent<Plattmacher_new>().Get3D(false, GameManager.instance.lastCheckpoint.transform.position);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "StayHereToGetFlat")
        {
            objToStayActivate = other.gameObject;
            if (plattmacherTouched)
            {
                GameManager.instance.playerFlach = true;
                Set2DSettings(); 
                gameObject.GetComponent<Plattmacher_new>().Get2D(other.gameObject.GetComponent<StayHereToGetFlat_new>().wall2DStartPositionPlayer);
            }
        }

        if (other.gameObject.tag == "Exit2D")
        {
            if (interact.action.IsPressed()){
                GameManager.instance.playerFlach = false;
                Set3DSettings();
                exit2DPosition = other.gameObject.GetComponent<ExitFrom2DScript>().exitPlayer.position; 
                gameObject.GetComponent<Plattmacher_new>().Get3D(true, exit2DPosition); 
            }
        }

        //Box Collider X-Axis and Z-Axis 
        if (other.gameObject.tag == "ColliderZ")
        {
            stayingAtColliderZ = true;
            stayingAtColliderX = false;
            boxToInteract = other.gameObject.transform.parent.gameObject;
        }
        else if (other.gameObject.tag == "ColliderX")
        {
            stayingAtColliderZ = false;
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
        if (other.gameObject.tag == "Plattmacher")
        {
            plattmacherTouched = false;
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

        if (!GameManager.instance.playerFlach)
        {

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
}
