using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public abstract class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _interactionRange;
    protected PlayerMovement _playerMovement;
    private PlayerMouseTarget _mouseTarget;
    private float _sqrInteractionRange => _interactionRange * _interactionRange;

    // Start is called before the first frame update
    protected void OnEnable()
    {
        PlayerMouseTarget.OnClick += OnTryInteract;
    }

    protected void OnDisable()
    {
        PlayerMouseTarget.OnClick -= OnTryInteract;
    }
    protected virtual void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    protected virtual void OnTryInteract(object sender, OnMouseClickEventArgs e)
    {
        if (PlayerMouseTarget.InteractibleItemHover != null)
        {
            if (_sqrInteractionRange > (e.Interactible.ObjectPosition - transform.position).sqrMagnitude)
                Interact(e.Interactible);
            else // Podemos adicionar aqui uma corotine de Andar + Interagir quando chegar ao obj
                _playerMovement.MoveTo(e.ClickPosition);
        }
        else _playerMovement.MoveTo(e.ClickPosition);
    }

    protected abstract void Interact(IInteractible obj);
}
