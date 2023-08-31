using UnityEngine;

public class RotationForever : MonoBehaviour
{
    [SerializeField] float rotationSpeed; 

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
