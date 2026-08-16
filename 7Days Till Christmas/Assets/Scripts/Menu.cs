using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] InputActionReference startAction;
    [SerializeField] AudioSource startSFX;

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
    {
        StartCoroutine(LoadGame());
    }
    IEnumerator LoadGame()
    {
        startSFX.Play();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    

}
