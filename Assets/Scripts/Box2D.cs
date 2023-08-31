using System.Collections.Generic;
using UnityEngine;

public class Box2D : MonoBehaviour
{
    public GameObject plaettenManager;
    [SerializeField] Transform movePoint;
    [SerializeField] float moveSpeed;

    [SerializeField] float xSchieben;

    List<Vector3> rays = new List<Vector3>();

    bool grab = false; 
    bool movingBox = false;
    float rayLength = 0f;
    bool wallCollision = false;
    float horizontal;
    int direction;
    int lastdirection;
    Vector3 lastPushedDirection;

    void Update()
    {
        MovingBox();
        //GetDistanceRay(); 
    }

    void MovingBox()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (grab)
        {
            GameManager.instance.Player.transform.SetParent(this.transform);
            movingBox = true;

            if (Input.GetMouseButtonUp(0))
            {
                grab = false;
            }
        }

        if (movingBox == false)
        {
            if (horizontal != 0)
            {
                direction = 1;
                lastdirection = direction;
            }
            else if (horizontal == 0 && grab == false)
            {
                direction = 0;
            }

            GameManager.instance.playerMoving = true;

            PushDirection(direction);
        }
        else if (movingBox == true)
        {
            GameManager.instance.playerMoving = false;
            if (Vector3.Distance(transform.position, movePoint.position) >= 0.1f && wallCollision == false)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, movePoint.position.x, moveSpeed * Time.deltaTime), transform.position.y, Mathf.Lerp(transform.position.z, movePoint.position.z, moveSpeed * Time.deltaTime));
                
                if (Mathf.Abs(transform.position.y - movePoint.position.y) >= 0.1f)
                {
                    movePoint.position = transform.position;
                }
            }
            else
            {
                if (wallCollision)
                {
                    movePoint.position = lastPushedDirection;
                    wallCollision = false;
                }
                transform.position = movePoint.position;

                if (Input.GetMouseButton(0))
                {
                    movingBox = true;
                    PushDirection(lastdirection);
                }
                else
                    movingBox = false;
            }
        }
    }

    void PushDirection(int directionHorizontal)
    {
        if (directionHorizontal == 1)
        {
            if (horizontal > 0.5f)
            {
                movePoint.transform.position = transform.position + Vector3.right * xSchieben;
                lastPushedDirection = movePoint.transform.position - Vector3.right * xSchieben;
            }
            else if (horizontal < -0.5f)
            {
                movePoint.transform.position = transform.position + Vector3.left * xSchieben;
                lastPushedDirection = movePoint.transform.position - Vector3.left * xSchieben;
            }
        }
        else if (directionHorizontal == 0)
        {
            movePoint.transform.position = transform.position;
            direction = 0;
        }
    }
    
    void GetDistanceRay()
    {
        RaycastHit hit;

        // Die Länge des Raycasts ist genau so groß und breit wie das Objekt
        if (direction == 1)
        {
            rayLength = transform.GetComponent<BoxCollider>().size.x * 1.5f;
            rays.Clear();

            rays.Add(new Vector3(0, 0, 0));
            rays.Add(new Vector3(0, xSchieben));
            rays.Add(new Vector3(0, -xSchieben));
        }

        foreach (Vector3 vectors in rays)
        {
            Ray ray = new Ray(transform.position - vectors, -(transform.position - movePoint.transform.position).normalized);

            Color debugCol = Color.green;
            //Debug.Log("Nothing");

            if (Physics.Raycast(ray, out hit, rayLength) && hit.collider.tag == "TwoDWall")
            {
                //Debug.Log("hit wall");
                lastdirection = 0;
                direction = 0;
                PushDirection(0);
                debugCol = Color.magenta;
            }
            Debug.DrawRay(ray.origin, ray.direction * rayLength, debugCol);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("TwoDBox")))
                {
                    if (hit.collider == gameObject.GetComponent<BoxCollider>())
                    {
                        grab = true;
                    }
                }
            }
        }

        if (other.gameObject.tag == "Exit2D" && Input.GetKey(KeyCode.E))
        {
            GameManager.instance.Player.transform.SetParent(null);
            plaettenManager.GetComponent<PlaettenBox>().box3D.transform.position = other.gameObject.transform.position + (Vector3.back * 1.5f);
            plaettenManager.GetComponent<PlaettenBox>().boxFlach = false;
            transform.position = transform.parent.position + Vector3.up * xSchieben;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Losezone")
        {
            GameManager.instance.Player.transform.SetParent(null);
            transform.position = transform.parent.position + Vector3.up;
            movePoint.position = transform.parent.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && grab == true)
        {
            GameManager.instance.Player.transform.SetParent(null);
            grab = false;
        }
        if (other.gameObject.tag == "Wall")
        {
            wallCollision = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Wall")
        {
            wallCollision = true;
        }
    }
}
