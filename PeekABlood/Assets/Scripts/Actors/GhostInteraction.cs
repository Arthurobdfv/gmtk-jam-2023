using UnityEngine;

public class GhostInteraction : PlayerInteraction, IHidingObject
{
    public bool IsHiding => _hidingObj != null;

    private Vector2 _previousPos;
    private IHideableObject _hidingObj;
    private SpriteRenderer _renderer;

    protected override void Awake()
    {
        base.Awake();
        _renderer = GetComponentInChildren<SpriteRenderer>(true);
    }
    protected override void Interact(IInteractible obj)
    {
        if(obj is IHideableObject)
        {
            TryHideInObject((IHideableObject)obj);
            return;
        }
        //else
        //    LeaveHiding();
    }

    private void TryHideInObject(IHideableObject objectToHide)
    {
        if (!IsHiding)
        {
            if (!objectToHide.TryHide(this))
                return;
            _hidingObj = objectToHide;
            HideInObject();
        }
        else
        {
            if (!objectToHide.Equals(_hidingObj))
                return;
            _hidingObj.Leave();
            LeaveHiding();
        }
    }

    private void HideInObject()
    {
        _playerMovement.enabled = false;
        _previousPos = transform.position;
        _renderer.enabled = false;
    }

    public bool LeaveHiding()
    {
        if (!IsHiding)
            return false;
        _playerMovement.enabled = true;
        _renderer.enabled = true;
        transform.position = _previousPos;
        _hidingObj = null;
        return true;
    }
}
