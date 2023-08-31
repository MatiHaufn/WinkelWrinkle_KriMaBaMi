using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditModeExecution : MonoBehaviour
{
    List<GameObject> twoDGameObjects = new List<GameObject>();
    [SerializeField] GameObject PlattmacherPrefab; 
    [SerializeField] GameObject ExitPrefab; 
    [SerializeField] GameObject PlatformPrefab; 
    
    List<GameObject> allBoxes = new List<GameObject>();
    void Start()
    {
        twoDGameObjects.AddRange(GameObject.FindGameObjectsWithTag("TwoDEbene"));
        allBoxes.AddRange(GameObject.FindGameObjectsWithTag("Box"));
    }
    private void Update()
    {
        foreach (GameObject tagObject in twoDGameObjects)
        {
            if (tagObject.GetComponent<BoxCollider>() == null)
            {
                tagObject.name = "2DEbene";
                tagObject.AddComponent<BoxCollider>();
                tagObject.GetComponent<BoxCollider>().isTrigger = true;
                tagObject.GetComponent<BoxCollider>().size = new Vector3(5, 5, 0.01f);

                tagObject.AddComponent<TwoDLayerScript>();

                GameObject newPlattmacher = Instantiate(PlattmacherPrefab);
                GameObject newExit = Instantiate(ExitPrefab);
                GameObject newPlatform = Instantiate(PlatformPrefab);

                newPlattmacher.transform.position = tagObject.transform.position;
                newExit.transform.position = tagObject.transform.position;
                newPlatform.transform.position = tagObject.transform.position;

                newPlattmacher.transform.SetParent(tagObject.transform);
                newExit.transform.SetParent(tagObject.transform);
                newPlatform.transform.SetParent(tagObject.transform);

            }
        }
        /*
        foreach (GameObject box in allBoxes)
        {
            if (box.GetComponent<Box>() == null)
            {
                box.AddComponent<Box>();
            }

            if (box.GetComponent<BoxCollider>() == null)
            {
                box.AddComponent<BoxCollider>();
                box.GetComponent<BoxCollider>().isTrigger = true;
                box.GetComponent<BoxCollider>().size = new Vector3(1.5f, 1, 1.5f);
            }
        }*/
    }
}
