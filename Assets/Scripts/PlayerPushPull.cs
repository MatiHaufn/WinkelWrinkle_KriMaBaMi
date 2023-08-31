using UnityEngine;
using System.Collections;
using System;

public class PlayerPushPull : MonoBehaviour
{
    [SerializeField] float distance = 1f;
    [SerializeField] int boxMask = 1 << 8;

    GameObject box; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics.queriesHitBackfaces = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance))
        {
            Debug.Log("hitti");
            if(hit.collider != null && Input.GetKeyDown(KeyCode.E))
            {
                box = hit.collider.gameObject;

            }

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }
}
