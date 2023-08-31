using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class CinemachineScript : MonoBehaviour
{
    CinemachineVirtualCamera vcam;

    public List<CinemachineVirtualCamera> CameraList = new List<CinemachineVirtualCamera>(); 
    public List<GameManager> TriggerForCams = new List<GameManager>();
   

    private void Start()
    {
        vcam = gameObject.GetComponent<CinemachineVirtualCamera>(); 
    }
    void Update()
    {
        if (GameManager.instance.playerFlach)
        {
            vcam.Priority = 10;
        }
        else
        { 
            vcam.Priority = 0; 
        }

    }

}
