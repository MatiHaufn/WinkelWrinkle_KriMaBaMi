using UnityEngine;

public class GroundTest : MonoBehaviour
{

    [SerializeField] PlayerMovement_new player; 
    [SerializeField] bool grounded;
    [SerializeField] LayerMask[] layersToStand; 

    private void OnTriggerEnter(Collider other)
    {
        foreach(var layer in layersToStand)
        {
            if ((layer.value & (1 << other.gameObject.layer)) != 0 )
            {
                grounded = true;
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
