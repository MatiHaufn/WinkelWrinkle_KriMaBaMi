using UnityEngine;

public class EreignisTrigger : MonoBehaviour
{
    public GameObject objectOne;
    [SerializeField] float moveSpeed;

    [SerializeField] bool boxEinrasten = false; 
    [SerializeField] bool importantAngle = false; 
    [SerializeField] bool activateObject = false; 
    [SerializeField] bool enableObject = false; 

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == objectOne)
        {
            //Debug.Log("Distanz: " + Vector3.Distance(other.gameObject.transform.position, this.gameObject.transform.position));
            //Genutzt für: Eine Box soll an die richtige Stelle geschoben, darf sich danach nicht mehr bewegen oder verschoben werden
            if(boxEinrasten)
            {
                if (importantAngle == true)
                { 
                    if((Vector3.Distance(other.gameObject.transform.position, this.gameObject.transform.position)) <= 0.5f && transform.rotation == objectOne.transform.rotation)
                    {
                        other.gameObject.transform.position = this.gameObject.transform.position;
                        other.gameObject.GetComponent<Box>().enabled = false;
                        boxEinrasten = false; 
                    }
                }
                else
                {
                    if ((Vector3.Distance(other.gameObject.transform.position, this.gameObject.transform.position)) <= 0.5f)
                    {
                        other.gameObject.transform.position = this.gameObject.transform.position;
                        other.gameObject.GetComponent<Box>().enabled = false;
                        boxEinrasten = false;
                    }
                }
            }
        }
        
        //Genutzt für: Rotationsplattform wird aktiv, wenn der Player in der Nähe steht
        if((other.gameObject.tag == "Player" || other.gameObject.tag == "Box") && activateObject == true)
        {
            objectOne.GetComponent<Rotation>().enabled = true; 
        }

        //Genutzt für verschwindene / zu aktivierende GameObjects
        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Box") && enableObject == true)
        {
            objectOne.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && activateObject == true)
        {
            objectOne.GetComponent<Rotation>().enabled = false;
        }

        if (other.gameObject.tag == "Player" && enableObject == true)
        {
            objectOne.SetActive(false);
        }
    }
}
