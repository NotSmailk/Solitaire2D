using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AdditionalCardsPile : MonoBehaviour
{
    [field: HideInInspector] public List<GameObject> cards = new List<GameObject>();

    [field: SerializeField] public Transform additionalCardsTransform;

    [field: SerializeField] private Sprite cardBack;
    [field: SerializeField] private Sprite emptySlot;

    private GameObject currentActiveCard = null;
    private int currentActiveCardIndex = -1;
    private SpriteRenderer spriteRenderer;

    public static AdditionalCardsPile GlobalAdditionalCardPile;

    private void Awake()
    {
        GlobalAdditionalCardPile = this;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            NextCard();

            SFXManager.OnCardSwipe.Invoke();
        }
    }

    public void HideCards()
    {
        foreach (GameObject card in cards)
        {
            card.SetActive(false);

            card.GetComponent<Card>().FaceUp(true);
        }
    }

    public void NextCard()
    {
        currentActiveCardIndex = currentActiveCardIndex < cards.Count - 1 ? ++currentActiveCardIndex : -1;

        currentActiveCard?.SetActive(false);

        currentActiveCard = currentActiveCardIndex != -1 ? cards[currentActiveCardIndex] : null;

        currentActiveCard?.SetActive(true);

        spriteRenderer.sprite = cards[cards.Count - 1].activeInHierarchy ? emptySlot : cardBack;
    }

    public void ShowPreviosCard(bool show)
    {
        if (currentActiveCardIndex <= 0)
            return;

        cards[currentActiveCardIndex - 1].SetActive(show);
    }

    public void SetCardBack(Sprite back)
    {
        cardBack = back;

        spriteRenderer.sprite = cardBack;
    }

    public void RemoveCard(GameObject card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);

            currentActiveCardIndex--;

            currentActiveCard = currentActiveCardIndex != -1 ? cards[currentActiveCardIndex] : null;
        }
    }
}
