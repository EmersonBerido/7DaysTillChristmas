using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] InputActionReference startAction;

    void OnEnable()
    {
        startAction.action.Enable();
        startAction.action.performed += StartGame;
        startAction.action.canceled += StartGame;
    }

    void OnDisable()
    {
        startAction.action.Disable();
        startAction.action.performed -= StartGame;
        startAction.action.canceled -= StartGame;
    }

  void StartGame(InputAction.CallbackContext ctx)
        => SceneManager.LoadScene(1);

    

}
