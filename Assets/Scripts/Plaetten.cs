using UnityEngine;
using UnityEngine.VFX;

public class Plaetten : MonoBehaviour
{
    [SerializeField] GameObject PlayerThreeD;
    [SerializeField] GameObject PlayerTwoD;

    GameObject ausgeloesterPlaetter;
    GameObject erzwungenPlaetter;

    int standsInKombi = 0;

    private void Update()
    {
        Plattmacher(); 
    }
    void Plattmacher()
    {
        if (GameManager.instance.playerFlach == false)
        {
            PlayerThreeD.SetActive(true);
            PlayerTwoD.SetActive(false);
        }
        else if (GameManager.instance.playerFlach == true && ausgeloesterPlaetter != null)
        {
            PlayerThreeD.SetActive(false);
            PlayerTwoD.SetActive(true);
            transform.position = new Vector3(transform.position.x, transform.position.y, ausgeloesterPlaetter.gameObject.transform.parent.position.z);
        }
        else if (GameManager.instance.playerFlach == true && erzwungenPlaetter != null)
        {
            PlayerThreeD.SetActive(false);
            PlayerTwoD.SetActive(true);
            transform.position = new Vector3(transform.position.x, transform.position.y, erzwungenPlaetter.gameObject.GetComponent<EreignisTrigger>().objectOne.transform.position.z);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Plattmacher" && other.gameObject.GetComponent<ObjectRotationByClick>().rotatingProcess == true)
        {
            ausgeloesterPlaetter = other.gameObject;
            if(GameManager.instance.playerFlach == false)
            {
                GameObject newVfxPoof = Instantiate( GameManager.instance.vfxPoof, GameManager.instance.Player.transform.position + Vector3.up + Vector3.back, transform.rotation);
            }
            GameManager.instance.playerFlach = true;
        }
        
        if (other.gameObject.tag == "ErzwungenPlatt")
        {
            Debug.Log("TRUEEE");
            erzwungenPlaetter = other.gameObject;
            if (GameManager.instance.playerFlach == false)
            {
                GameObject newVfxPoof = Instantiate(GameManager.instance.vfxPoof, GameManager.instance.Player.transform.position + Vector3.up + Vector3.back, transform.rotation);
            }
            GameManager.instance.playerFlach = true;
        }

        if (other.gameObject.tag == "Exit2D" && Input.GetKey(KeyCode.E))
        {
            if (GameManager.instance.playerFlach == true)
            {
                GameObject newVfxPoof = Instantiate(GameManager.instance.vfxPoof, GameManager.instance.Player.transform.position + Vector3.up + Vector3.back, transform.rotation);
            }
            GameManager.instance.playerFlach = false;
            erzwungenPlaetter = null;
            ausgeloesterPlaetter = null;
        }
    }
}