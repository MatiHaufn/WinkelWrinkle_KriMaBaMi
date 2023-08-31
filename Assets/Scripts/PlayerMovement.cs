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
    [SerializeField] private bool IsGrounded;
    [SerializeField] private bool IsJumping;

    Animator currentAnimator;

    [SerializeField] Animator animator2D;
    [SerializeField] Animator animator3D;

    private Rigidbody _rigidbody;
    float turnSmooooothVelocity;
    float horizontal;
    float vertical;

    float idleTimer = 0;
    float maxIdleTime = 20;
    int idleState = 0;

    // => Zusammenfassen
    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    private void Start()
    {
        currentAnimator = animator3D;
    }

    private void Update()
    {
        Jumping();
        IdleAnimation();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    void Jumping()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded == true && IsJumping == false)
        {
            //FindObjectOfType<AudioManager>().Play("Theme");
            currentAnimator.SetBool("JumpStart", true);
            IsGrounded = false;
            IsJumping = true; 
            _rigidbody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }

        if (IsGrounded == true)
        {
            IsJumping = false;
            currentAnimator.SetBool("JumpStart", false);
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
            currentAnimator.SetFloat("speed", 0);

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

            // Wenn Player flach ist, soll er sich nur auf der X-Achse bewegen können und sich nicht rotieren
            if (GameManager.instance.playerFlach == false)
            {
                vertical = Input.GetAxisRaw("Vertical");
            }
            else
            {
                vertical = 0f;
                transform.rotation = Quaternion.Euler(0f, -180, 0f);
            }


            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; //normalized = eine Einheit von der Richtung

            if (direction.magnitude >= 0.1f )
            {
                Vector3 desiredSpeed;
                float acceleration;

                if (IsGrounded)
                {
                    desiredSpeed = direction * maxGroundSpeed;
                    acceleration = direction.magnitude != 0 ? groundAcceleration : groundDeceleration;
                }
                else
                {
                    desiredSpeed = direction * maxAirSpeed;
                    acceleration = direction.magnitude != 0 ? airAcceleration : airDeceleration;
                }

                Vector3 playerSpeed = Vector3.Lerp(_rigidbody.velocity, desiredSpeed, acceleration * Time.deltaTime); 

                _rigidbody.velocity = new Vector3(playerSpeed.x, _rigidbody.velocity.y, playerSpeed.z);

             

                if (GameManager.instance.playerFlach == false)
                {
                    // mit Atan2 Winkel berechnen und in Deg° angeben          
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    // funktion zur SmoothDampANgel damit ich nicht sofort in die Rotation Snap
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooooothVelocity, turnSmoothTime);
                    // eigentliche rotation
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
            }
        }

    }
   

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Ich steh aufmBoden");
        if(other.isTrigger == false)
        {
            IsGrounded = true;
        }
                
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






