public interface IHideableObject : IInteractible
{
    IHidingObject? HidingEntity { get; }
    bool IsOccupied => HidingEntity != null;
    public bool TryHide(IHidingObject objectHiding);
    public bool Leave();
}
