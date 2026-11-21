using UnityEngine;

// Base class for every interactive object in the room (items, drawers, fuse box, keypad...).
// Each puzzle object inherits from this and defines what happens in Interact().
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private string promptText = "Press E to interact";

    public virtual string PromptText => promptText;

    public abstract void Interact(PlayerInteractor player);
}
