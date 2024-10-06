using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public EventSystem eventSystem; // Referenz auf das EventSystem

    private void Start()
    {
        // Setze den Fokus auf den ersten Button zu Beginn
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Setze den aktuell ausgewählten Button, wenn die Maus über den Button schwebt
        eventSystem.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hier geschieht nichts, um den Button weiterhin ausgewählt zu lassen
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Hier kannst du optional etwas tun, wenn der Button ausgewählt wird
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Hier geschieht nichts, um sicherzustellen, dass der Button ausgewählt bleibt
    }
}
