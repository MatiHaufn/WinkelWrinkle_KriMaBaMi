using UnityEngine;


public class HighlightObjects : MonoBehaviour
{
    public Material highlightMaterial;
    Material originalMaterial;
    GameObject lastHighlightedObject;

    void HighlightObject(GameObject gameObject)
    {
        if (lastHighlightedObject != gameObject)
        {
            ClearHighlighted();
            originalMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = highlightMaterial;
            lastHighlightedObject = gameObject;
        }

    }

    void ClearHighlighted()
    {
        if (lastHighlightedObject != null)
        {
            lastHighlightedObject.GetComponent<MeshRenderer>().sharedMaterial = originalMaterial;
            lastHighlightedObject = null;
        }
    }

    void HighlightObjectInCenterOfCam()
    {
        float rayDistance = 1000.0f;
        // Ray from the center of the viewport.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        // Check if we hit something.
        if (Physics.Raycast(ray, out rayHit, rayDistance, LayerMask.GetMask("Highlight")))
        {
            //Debug.Log("Highlighthit");
            // Get the object that was hit.
            GameObject hitObject = rayHit.collider.gameObject;
            HighlightObject(hitObject);
        }
        else
        {
            ClearHighlighted();
        }
    }

    void Update()
    {
        HighlightObjectInCenterOfCam();
    }

 
 
}
