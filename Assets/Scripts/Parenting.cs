using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parenting : MonoBehaviour
{
    public GameObject parent; 
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManager.instance.Player.transform.SetParent(parent.gameObject.transform);
        }
    }
}
