using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    private PlayerControls playerControls;
    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        playerControls.GameStart.GameStart.performed += StartGame;
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.GameStart.GameStart.performed -= StartGame;
        playerControls.Disable();
    }

    private void StartGame(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene("CenaArthuro");
    }
}
