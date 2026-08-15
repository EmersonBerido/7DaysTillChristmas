using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Ending : MonoBehaviour
{
    [SerializeField] InputActionReference pressAction;
    [SerializeField] UIDocument uiDocument;

    void OnEnable()
    {
        pressAction.action.Enable();
        pressAction.action.performed += ReturnToMainMenu;
        pressAction.action.canceled += ReturnToMainMenu;
    }

    void OnDisable()
    {
        pressAction.action.Disable();
        pressAction.action.performed -= ReturnToMainMenu;
        pressAction.action.canceled -= ReturnToMainMenu;
    }

    void Start()
    {
        int presentsUnwrapped = PlayerPrefs.GetInt("presents", 0);


        var text = uiDocument.rootVisualElement.Q<VisualElement>("Panel")
            .Q<Label>("Text");
        text.text = $"OPENED {presentsUnwrapped} PRESENTS BEFORE MOM CAUGHT ME @ v @";
    }

    private void ReturnToMainMenu(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(0);
    }
}
