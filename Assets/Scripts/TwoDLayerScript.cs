using System.Collections.Generic;
using UnityEngine;

public class TwoDLayerScript : MonoBehaviour
{
    List<GameObject> AllChildren = new List<GameObject>();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            AllChildren.Add(child.gameObject);
        }

        for (int i = 0; i < AllChildren.Count; i++)
        {
            AllChildren[i].transform.position = new Vector3(AllChildren[i].transform.position.x, AllChildren[i].transform.position.y, transform.position.z);
        }
    }
}
