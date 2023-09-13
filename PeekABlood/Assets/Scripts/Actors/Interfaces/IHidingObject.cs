using UnityEngine;

public interface IHidingObject 
{
    public bool IsHiding { get; }
    public bool LeaveHiding();
}
