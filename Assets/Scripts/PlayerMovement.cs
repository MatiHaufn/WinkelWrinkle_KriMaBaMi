using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float maxGroundSpeed;
    [SerializeField] float maxAirSpeed;
    [SerializeField] float jumpforce = 10f;
    [SerializeField] float groundAcceleration = 10f;
    [SerializeField] float groundDeceleration = 10f;
    [SerializeField] float airAcceleration = 10f;
    [SerializeField] float airDeceleration = 10f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float moveSpeed = 2; 
    [SerializeField] private GroundTest groundTest; 

    [SerializeField] Animator animator2D;
    [SerializeField] Animator animator3D;

    Animator currentAnimator;

    private bool isGrounded;

    private Rigidbody _rigidbody;
    float turnSmooooothVelocity;
    float horizontal;
    float vertical;

    float idleTimer = 0;
    float maxIdleTime = 20;
    int idleState = 0;

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    private void Start()
    {
        currentAnimator = animator3D;
    }

    private void Update()
    {
        Jumping();
        IdleAnimation();
        isGrounded = groundTest.IsGrounded(); 
    }
    private void FixedUpdate()
    {
        Movement();
    }

    public void TriggerLandAnimation()
    {
        currentAnimator.SetBool("JumpStart", false);
    }

    void Jumping()
    {
        if (isGrounded == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                currentAnimator.SetBool("JumpStart", true);
                _rigidbody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            }
        }
    }

    void IdleAnimation()
    {
        if (horizontal != 0 || vertical != 0)
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

        if(idleTimer >= maxIdleTime)
        {
            idleState = Random.Range(1, 3);
            idleTimer = 0; 
        }

        if(idleState == 1)
        {
            currentAnimator.SetTrigger("Idle2");
            idleTimer = 0;
            idleState = 0; 
        }
        else if(idleState == 2)
        {
            currentAnimator.SetTrigger("Idle3");
            idleTimer = 0;
            idleState = 0; 
        }
    }

    void Movement()
    {
        Vector3 characterScale = transform.localScale;
        
        if(GameManager.instance.playerFlach == true)
        {
            currentAnimator = animator2D;
            if (horizontal > 0)
            {
                characterScale.x = Mathf.Abs(transform.localScale.x);
            }
            else if(horizontal < 0)
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
        }

        if(horizontal != 0 || vertical != 0)
        {
           currentAnimator.SetBool("runActive", true);
        }
        else
        {
            currentAnimator.SetBool("runActive", false);
        }

        if (GameManager.instance.playerMoving)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            if (GameManager.instance.playerFlach == true)
            {
                vertical = 0f;
                transform.rotation = Quaternion.Euler(0f, -180f, 0f);
            }
            else
            {
                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

                if (direction.magnitude >= 0.1f)
                {   
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooooothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
            }

            Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }
   

    private void OnTriggerEnter(Collider other)
    {                
        //Respawn for Player!
        foreach (GameObject checkpoint in GameManager.instance.Checkpoints)
        {
            if (other.gameObject == checkpoint.gameObject)
            {
                GameManager.instance.lastCheckpoint = checkpoint;
            }
        }
        if (other.gameObject.tag == "Losezone")
        {
            GameManager.instance.Player.transform.position = GameManager.instance.lastCheckpoint.transform.position;
            GameManager.instance.playerFlach = false;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "MovingPlatform")
        {
            Debug.Log(this.gameObject.transform.parent);
            this.transform.SetParent(other.transform);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            Debug.Log(this.gameObject.transform.parent);
            transform.SetParent(null);
        }
    }

   
}






