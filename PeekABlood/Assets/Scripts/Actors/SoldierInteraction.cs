using UnityEngine;

public class SoldierInteraction : PlayerInteraction
{
    protected override void Interact(IInteractible obj)
    {
        if (obj is IHideableObject)
            Kick((IHideableObject)obj);
    }

    private void Kick(IHideableObject obj)
    {
        if (obj.Leave())
            Debug.Log("There was a ghost!!!");
    }
}
