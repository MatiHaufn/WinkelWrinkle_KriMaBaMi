using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemScript_new : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject firstSelectedObject;
    public List<GameObject> buttons;

    private bool mouseControlsActivated = false; // Status der Maussteuerung

    private void Start()
    {
        eventSystem.SetSelectedGameObject(firstSelectedObject);
    }

    private void Update()
    {
        HandleMouseInput();
        HandleKeyboardInput();
        HandlePointerSelection();
    }

    private void HandleMouseInput()
    {
        // Überprüfen, ob die Maus bewegt oder geklickt wird
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || Input.mousePosition != Vector3.zero)
        {
            // Setze den Fokus auf null, wenn die Maus benutzt wird
            eventSystem.SetSelectedGameObject(null);
            mouseControlsActivated = true; // Maussteuerung aktiviert
        }
        else if (mouseControlsActivated)
        {
            // Wenn die Maus nicht mehr benutzt wird, ignoriere die Eingabe
            return;
        }
    }

    private void HandleKeyboardInput()
    {
        if (Input.anyKeyDown)
        {
            // Wenn eine Taste gedrückt wird und die Maussteuerung aktiv ist
            if (!mouseControlsActivated)
            {
                SetFirstVisibleButtonAsSelected(); // Setze den ersten sichtbaren Button
            }
        }
    }

    private void HandlePointerSelection()
    {
        if (eventSystem.currentSelectedGameObject == null && mouseControlsActivated)
        {
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                // Setze den Fokus auf das UI-Element unter dem Mauszeiger
                eventSystem.SetSelectedGameObject(results[0].gameObject);
            }
        }
    }

    private void SetFirstVisibleButtonAsSelected()
    {
        foreach (var button in buttons)
        {
            if (button.activeInHierarchy)
            {
                eventSystem.SetSelectedGameObject(button);
                mouseControlsActivated = false; // Deaktiviert Maussteuerung, um Tastatursteuerung zu aktivieren
                break;
            }
        }
    }
}
