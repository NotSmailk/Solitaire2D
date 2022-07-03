using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    [Header("Settings")]
    [field: SerializeField] public Color cardTableColor = new Color(0f, 98f, 0f);
    [field: SerializeField] public Sprite currentCardBack;
    [field: SerializeField] public Sprite[] cardBacks;
    [field: SerializeField] public Sprite[] cardFaces;
    [Header("Main Objects")]
    [field: SerializeField] public Camera mainCamera;

    public static SkinsManager GlobalSkinsManager;

    private void Awake()
    {
        GlobalSkinsManager = this;
    }

    private void Start()
    {
        ChangeBackColor();
    }

    public void ChangeCardsBack()
    {
        foreach (Card card in FindObjectsOfType<Card>())
        {
            card.SetCardBack(currentCardBack);
        }

        FindObjectOfType<AdditionalCardsPile>().SetCardBack(currentCardBack);
    }

    public void ChangeBackColor()
    {
        mainCamera.backgroundColor = cardTableColor;
    }
}
