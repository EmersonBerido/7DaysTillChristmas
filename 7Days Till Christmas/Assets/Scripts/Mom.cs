using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class Mom : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite angrySprite;

    [Header("Dog Variables")]
    [SerializeField] private GameObject dog;
    [SerializeField] private Transform dogEndPosition;
    private Vector2 dogStartPosition;
    [SerializeField] private Sprite idleDogSprite;
    [SerializeField] private Sprite activeDogSprite;
    [SerializeField] private AudioClip dogBark;
    [SerializeField] private float dogSpeed = 1f;
    [SerializeField] private float barkDuration = 1f;
    [SerializeField] private float timeTillBark = 5f;
    private bool moveDog = false;
    private bool dogAtEnd = false;

    [Header("Timing Variables")]
    [SerializeField] private float angerIntervalMin = 2f;
    [SerializeField] private float angerIntervalMax = 10f;
    [SerializeField] private float angerDurationMin = 3f;
    [SerializeField] private float angerDurationMax = 6f;
    [SerializeField] private float angerWarning = 1f;
    [SerializeField] private float gracePeriod = 1f;
    private bool isWatching = false;
    private bool isCaught = false;

    void Start()
    {
        dogStartPosition = dog.transform.position;
        
        StartCoroutine(CheckKid());
        
    }

    void Update()
    {
        if (isWatching && !isCaught)
        {
            if (PlayerControls.Instance.IsUnwrapping())
                StartCoroutine(Caught());
        }

        // used in dog routine to move dog between desired location
        if (moveDog && !dogAtEnd)
        {
            // moves dog towards active position
            dog.transform.position = Vector2.MoveTowards(dog.transform.position, dogEndPosition.position, dogSpeed * Time.deltaTime);
            if (Vector2.Distance(dog.transform.position, dogEndPosition.position) < 0.1f)
                moveDog = false;
            
        } else if (moveDog && dogAtEnd)
        {
            // moves dog towards initial position
            dog.transform.position = Vector2.MoveTowards(dog.transform.position, dogStartPosition, dogSpeed * Time.deltaTime);
            if (Vector2.Distance(dog.transform.position, dogStartPosition) < 0.1f) 
            {
                dogAtEnd = false;
                moveDog = false;
            }
        }
                
        
    }

    IEnumerator Caught()
    {
        // GameOver
        isCaught = true;
        UpdateSprite(angrySprite, gameObject);
        Debug.Log("Mom angry. you lost");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Ending");
    }

    private IEnumerator CheckKid()
    {
        while (true)
        {
            // cooldown
            float idleDuration = Random.Range(angerIntervalMin, angerIntervalMax);
            yield return new WaitForSeconds(idleDuration);
        
            // start routine
            float selected = Random.Range(0, 1f);
            if (selected < 0.2f)
                yield return StartCoroutine(DogCheck());
            else 
                yield return StartCoroutine(RegularCheck());
        }


    }

    private IEnumerator RegularCheck()
    {
        /* WARNING PHASE */
        float warning = Random.Range(0, 1f);
        if (warning < 0.5f)
        {
            Debug.Log("This is a warning");
            yield return new WaitForSeconds(angerWarning);
            Debug.LogWarning("Add Exlamation Mark to Mom Sprite");
        }

        // decide how long to be angry
        yield return StartCoroutine(MomCheck());
    }

    private IEnumerator MomCheck()
    {
        // decide duration
        float duration = Random.Range(angerDurationMin, angerDurationMax);
        UpdateSprite(activeSprite, gameObject);
        Debug.Log("About to check!");

        yield return new WaitForSeconds(gracePeriod);
        isWatching = true;
        yield return new WaitForSeconds(duration);

        isWatching = false;
        UpdateSprite(idleSprite, gameObject);
    }

    private IEnumerator DogCheck()
    {
        // Bring dog to end position
        moveDog = true;
        dogAtEnd = false;
        yield return new WaitForSeconds(timeTillBark);

        // bark for duration
        UpdateSprite(activeDogSprite, dog);
        moveDog = false;
        // AudioSource.PlayClipAtPoint(dogBark, dog.transform.position);
        yield return new WaitForSeconds(barkDuration);
        UpdateSprite(idleDogSprite, dog);
        Debug.LogWarning("Dog bark sound here");

        // enable mom to check
        yield return StartCoroutine(MomCheck());

        // return to start position
        moveDog = true;
        dogAtEnd = true;

        yield return null;
           
    }
    private void UpdateSprite(Sprite newSprite, GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().sprite = newSprite;
    }


}
