using Cinemachine;
using UnityEngine;

public class CinemachineScript : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam;

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
