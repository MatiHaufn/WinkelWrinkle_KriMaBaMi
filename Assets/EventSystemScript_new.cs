using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemScript_new : MonoBehaviour
{
    void SetSelectedButton(GameObject Button)
    {
        EventSystem.current.SetSelectedGameObject(Button);
    }
}
