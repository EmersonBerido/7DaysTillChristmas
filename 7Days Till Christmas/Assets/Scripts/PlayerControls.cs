using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PresentUI))]
[RequireComponent(typeof(AudioSource))]
public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance { get; private set; }

    [Header("Present Variables")]
    [SerializeField] private GameObject present;
    [SerializeField] private Vector2 presentActivePosition;
    private Vector2 presentInactivePosition;
    [SerializeField] private float MaxPresentHealth = 100f;
    private float currentPresentHealth;
    private PresentUI presentUI;
    [SerializeField] private List<Sprite> presents;

    [Header("Player Variables")]
    [SerializeField] private float unwrapSpeed;
    [SerializeField] private GameObject playerHands; // used when active
    [SerializeField] private InputActionReference unwrapAction;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite idleSprite;
    private Sprite currSprite;
    private int presentsUnwrapped = 0;
    
    private bool isUnwrapping = false;
    private AudioSource audioSource;


    void OnEnable() =>
        unwrapAction.action.Enable();

    void OnDisable() =>
        unwrapAction.action.Disable();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        presentInactivePosition = present.transform.position;
        currentPresentHealth = MaxPresentHealth;
        presentUI = GetComponent<PresentUI>();
        currSprite = idleSprite;
        audioSource = GetComponent<AudioSource>();
        audioSource.mute = true;
        playerHands.SetActive(false);

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
            audioSource.mute = false;
            UpdateSprite(activeSprite);
        }
        
        // Decrease present progress;
        currentPresentHealth -= Time.deltaTime * unwrapSpeed; // Example damage per second
        presentUI.UpdateBar(currentPresentHealth / MaxPresentHealth * 100f);

        if (currentPresentHealth <= 0)
        {
            presentsUnwrapped += 1;
            PlayerPrefs.SetInt("presents", presentsUnwrapped);
            ResetPresent();
        }
    }

    private void StopUnwrapping()
    {
        if (!isUnwrapping) return;
        isUnwrapping = false;
        playerHands.SetActive(false);
        audioSource.mute = true;
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
        present.GetComponent<SpriteRenderer>().sprite = ChoosePresent();
    }

    private void UpdateSprite(Sprite newSprite)
    {
        currSprite = newSprite;
        GetComponent<SpriteRenderer>().sprite = currSprite;
    }

    public bool IsUnwrapping() => isUnwrapping;

    private Sprite ChoosePresent()
    {
        int idx = Random.Range(0,presents.Count);
        return presents[idx];
    }
        
}
