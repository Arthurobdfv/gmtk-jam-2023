using UnityEngine;

public class HideableObject : MonoBehaviour, IHideableObject
{
    public IHidingObject HidingEntity => _hidingObj;

    public Vector3 ObjectPosition => transform.position;

    private IHidingObject _hidingObj;

    public void OnCursorOver()
    {
        Debug.Log("Mouse over me!!!");
    }

    public void OnInteract()
    {
        Debug.Log("Tried to interact with me!!!");
    }

    public bool TryHide(IHidingObject objectHiding)
    {
        if (HidingEntity != null)
            return false;
        else _hidingObj = objectHiding;
        return true;
    }

    public bool Leave()
    {
        if (HidingEntity == null)
            return false;
        else
        {
            _hidingObj.LeaveHiding();
            _hidingObj = null;
        }
        return true;
    }
}
