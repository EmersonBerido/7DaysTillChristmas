using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PresentUI))]
public class PlayerControls : MonoBehaviour
{
    [Header("Present Variables")]
    [SerializeField] private GameObject present;
    [SerializeField] private Vector2 presentActivePosition;
    private Vector2 presentInactivePosition;
    [SerializeField] private float MaxPresentHealth = 100f;
    private float currentPresentHealth;
    private PresentUI presentUI;

    [Header("Player Variables")]
    [SerializeField] private float unwrapSpeed;
    [SerializeField] private GameObject playerHands; // used when active
    [SerializeField] private InputActionReference unwrapAction;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite idleSprite;
    private Sprite currSprite;
    
    private bool isUnwrapping = false;


    void OnEnable() =>
        unwrapAction.action.Enable();

    void OnDisable() =>
        unwrapAction.action.Disable();

    void Start()
    {
        presentInactivePosition = present.transform.position;
        currentPresentHealth = MaxPresentHealth;
        presentUI = GetComponent<PresentUI>();
        currSprite = idleSprite;
    }

    void Update()
    {
        if (unwrapAction.action.IsPressed())
            UnwrapPresent();
        else 
            StopUnwrapping();
    }

    private void UnwrapPresent()
    {
        if ((Vector2)present.transform.position != presentActivePosition)
            present.transform.position = presentActivePosition;

        if (!isUnwrapping)
        {
            isUnwrapping = true;
            playerHands.SetActive(true);
            UpdateSprite(activeSprite);
        }
        
        // Decrease present progress;
        currentPresentHealth -= Time.deltaTime * unwrapSpeed; // Example damage per second
        presentUI.UpdateBar(currentPresentHealth / MaxPresentHealth * 100f);

        if (currentPresentHealth <= 0)
            ResetPresent();
    }

    private void StopUnwrapping()
    {
        if (!isUnwrapping) return;
        isUnwrapping = false;
        playerHands.SetActive(false);
        present.transform.position = presentInactivePosition;
        UpdateSprite(idleSprite);
    }

    private void ResetPresent()
    {
        present.transform.position = presentInactivePosition;
        currentPresentHealth = MaxPresentHealth;
        isUnwrapping = false;
        presentUI.ResetBar();

        // Select new present image
        Debug.LogWarning("Need new present image");
    }

    private void UpdateSprite(Sprite newSprite)
    {
        currSprite = newSprite;
        GetComponent<SpriteRenderer>().sprite = currSprite;
    }
        
}
