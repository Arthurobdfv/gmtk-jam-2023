using UnityEngine;

public interface IInteractible
{
    void OnCursorOver();
    void OnInteract();
    Vector3 ObjectPosition { get; }
}