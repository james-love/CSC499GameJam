using UnityEngine;

[RequireComponent(typeof(DisplayInteractPopup))]
public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();
}
