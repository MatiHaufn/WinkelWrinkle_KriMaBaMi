using UnityEngine;
using UnityEngine.VFX;

public class Plattmacher_new : MonoBehaviour
{
    [SerializeField] GameObject object3D;
    [SerializeField] GameObject object2D;

    [SerializeField] VisualEffect dimensionsSwitchVfx;

    public void Get2D(Transform inWallPosition)
    {
        object3D.SetActive(false);
        object2D.SetActive(true);
        transform.position = inWallPosition.position;
        dimensionsSwitchVfx.enabled = true; 
        dimensionsSwitchVfx.Play();
    }

    public void Get3D(bool vfxActivated, Vector3 in3DPosition)
    {
        object3D.SetActive(true);
        object2D.SetActive(false);

        transform.position = in3DPosition; 

        if (vfxActivated)
        {
            dimensionsSwitchVfx.enabled = true; 
            dimensionsSwitchVfx.Play();
        }
    }



}
