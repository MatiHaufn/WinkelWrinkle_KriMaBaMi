using UnityEngine;

public class PlaettenBox : MonoBehaviour
{
    public bool boxFlach = false;

    public GameObject box3D;
    public GameObject box3DStartPosition;
    public GameObject box2D;
    public GameObject box2DStartPosition;

    [SerializeField] GameObject Plaetter;

    private void Update()
    {
        Plattmacher();
    }

    public void Plattmacher()
    {
        if(boxFlach == false)
        {
            box3D.SetActive(true);
            box2D.SetActive(false); 
        }
        else
        {
            box3D.SetActive(false); 
            box2D.SetActive(true);
           // box2D.transform.position = new Vector3(box2DStartPosition.transform.position.x, box2DStartPosition.transform.position.y, Plaetter.gameObject.transform.parent.position.z);
        }
    }
}