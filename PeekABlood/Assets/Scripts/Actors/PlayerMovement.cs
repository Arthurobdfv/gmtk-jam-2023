using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerControls _inputControls;
    [SerializeField] private float speed = 5;

    private InputAction _moveAction;

    private Vector3? moveTowards;

    Rigidbody2D rb;
    Animator anim;
    

    Vector2 movement;

    // Start is called before the first frame update
    void Awake()
    {
        _inputControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        //_inputControls.Player.Move += Move;
        _moveAction = _inputControls.Player.Move;
        _inputControls.Enable();
    }

    private void OnDisable()
    {
        rb.velocity = Vector2.zero;
        //_inputControls.Player.Move.performed -= Move;
        _inputControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();
        rb.velocity = movement * speed;
        HandleSpriteDirection();
        movement = Vector2.zero;
    }

    private void HandleSpriteDirection()
    {
        //anim.SetFloat("Horizontal", movement.x);
        //anim.SetFloat("Vertical", movement.y);
        //anim.SetFloat("Speed", speed);

        if (movement.x < 0)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        else if (movement.x > 0)
        {
            transform.localScale = new Vector2(1f, 1f);
        }
    }

    public void Move(InputAction.CallbackContext value)
    {
        moveTowards = null;
        movement = value.ReadValue<Vector2>();
    }

    public void MoveTo(Vector3 target)
    {
        // Pathfinding?
    }
}
