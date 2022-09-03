using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    [field: SerializeField] private AudioClip pickUpSound;
    [field: SerializeField] private AudioClip putDownSound;
    [field: SerializeField] private AudioClip cardSwipeSound;
    [field: SerializeField] private AudioClip buttonClickSound;

    public static UnityEvent OnPickUp = new UnityEvent();
    public static UnityEvent OnPutDown = new UnityEvent();
    public static UnityEvent OnCardSwipe = new UnityEvent();

    private AudioSource audioSource;

    private void Awake()
    {
        OnPickUp.RemoveAllListeners();
        OnPutDown.RemoveAllListeners();
        OnCardSwipe.RemoveAllListeners();

        OnPickUp.AddListener(PickUpPlay);
        OnPutDown.AddListener(PutDownPlay);
        OnCardSwipe.AddListener(CardSwipePlay);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ButtonClickPlay()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }

    private void PickUpPlay()
    {
        audioSource.PlayOneShot(pickUpSound);
    }

    private void PutDownPlay()
    {
        audioSource.PlayOneShot(putDownSound);
    }

    private void CardSwipePlay()
    {
        audioSource.PlayOneShot(cardSwipeSound);
    }
}
