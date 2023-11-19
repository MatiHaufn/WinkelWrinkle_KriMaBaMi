using UnityEngine;

public class GroundTest : MonoBehaviour
{

    [SerializeField] PlayerMovement player; 
    [SerializeField] bool grounded;
    [SerializeField] LayerMask[] layersToStand; 

    private void OnTriggerEnter(Collider other)
    {
        foreach(var layer in layersToStand)
        {
            if ((layer.value & (1 << other.gameObject.layer)) != 0 )
            {
                grounded = true;
                player.TriggerLandAnimation(); 
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (var layer in layersToStand)
        {
            if ((layer.value & (1 << other.gameObject.layer)) != 0)
            {
                grounded = false;
            }
        }
    }
    
    public bool IsGrounded()
    {
        return grounded; 
    }
}
