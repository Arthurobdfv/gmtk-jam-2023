using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouseTarget : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _hoverPointer;
    [SerializeField] private IInteractible? _interactibleItem;

    public static IInteractible? InteractibleItemHover;
    public delegate void OnClickEventHandler(object sender, OnMouseClickEventArgs e);
    public static event OnClickEventHandler OnClick;

    private PlayerControls _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerControls();
    }
    private void OnEnable()
    {
        _playerInput.Player.Mira.performed += OnMouseChange;
        _playerInput.Player.Attack.performed += OnMouseClick;
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Player.Mira.performed -= OnMouseChange;
        _playerInput.Player.Attack.performed -= OnMouseClick;
        _playerInput.Disable();
    }

    private void Start()
    {
        _hoverPointer = GetComponentInChildren<SpriteRenderer>(true);
    }
    // Update is called once per frame
    void Update()
    {
        HandlePointer();
    }

    private void HandlePointer()
    {
        if (_interactibleItem != null)
        {
            _hoverPointer.enabled = true;
            transform.position = _interactibleItem.ObjectPosition;
        }
        else
            _hoverPointer.enabled = false;
    }

    public void OnMouseChange(InputAction.CallbackContext value)
    {
        var inputPos = value.ReadValue<Vector2>();
        Vector2 pos = value.control.ToString().Contains("Mouse") ? Camera.main.ScreenToWorldPoint(inputPos) : inputPos;
        IInteractible interactible = CheckInteractible(pos);
        _interactibleItem = interactible;
        InteractibleItemHover = interactible;
    }

    private IInteractible? CheckInteractible(Vector2 pos)
    {
        var hit = Physics2D.Raycast(pos, Vector2.zero);
        IInteractible interactible = null;
        if (hit.collider != null)
        {
            var hitGameObject = hit.collider?.gameObject;
            interactible = hitGameObject?.GetComponent<IInteractible>();
            interactible?.OnCursorOver();
        };
        return interactible;
    }

    public void OnMouseClick(InputAction.CallbackContext ctx)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var interactible = CheckInteractible(pos);
        OnClick?.Invoke(this, new OnMouseClickEventArgs(interactible, pos));
    }

}

public class OnMouseClickEventArgs {
    public IInteractible? Interactible { get; }
    public Vector2 ClickPosition { get; }
    public OnMouseClickEventArgs(IInteractible? _interactible, Vector2 _clickPos)
    {
        ClickPosition = _clickPos;
        Interactible = _interactible;
    }
}