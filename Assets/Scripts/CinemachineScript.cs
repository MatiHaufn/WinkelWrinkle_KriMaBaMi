using Cinemachine;
using UnityEngine;

public class CinemachineScript : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam2D_1;
    [SerializeField] CinemachineVirtualCamera vcam2D_2;

    void Update()
    {
        if (GameManager.instance.playerFlach)
        {
            if(GameManager.instance.secondCameraView)
            {
                vcam2D_2.Priority = 10;
                vcam2D_1.Priority = 0; 
            }
            else
                vcam2D_1.Priority = 10; 
        }
        else
        {
            vcam2D_1.Priority = 0;
            vcam2D_2.Priority = 0;
        }

    }

}
