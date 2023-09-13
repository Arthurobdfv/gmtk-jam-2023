using UnityEngine;

public class InteractibleObject : MonoBehaviour, IInteractible
{
    public Vector3 ObjectPosition => transform.position;

    public void OnCursorOver()
    {
        Debug.Log("Mouse over me!!!");
    }

    public void OnInteract()
    {
        Debug.Log("Tried to interact with me!!!");
    }
}
