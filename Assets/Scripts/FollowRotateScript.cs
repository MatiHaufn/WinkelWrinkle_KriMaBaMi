using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotateScript : MonoBehaviour
{
  
    [SerializeField] Transform target;
    [SerializeField] float delay1;
    [SerializeField] float delay2;
    public bool isMoving = true;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentsInChildren<Rigidbody>();            {}
    }

    // Update is called once per frame
    void Update()
    {


        //Zielen
        Vector3 forward =  new Vector3(0, target.position.y, 0) - new Vector3(0, transform.position.y, 0);
        Quaternion q = Quaternion.LookRotation(forward, Vector3.up);

        //Bedingung für Arme unterschiedliche Geschw.

        if (isMoving)
        {
            if (this.tag == "Hinge1")
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, q, delay1 * Time.deltaTime);
            }

            if (this.tag == "Hinge2")
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, q, delay2 * Time.deltaTime);
            }
        }

    }
  
    

}