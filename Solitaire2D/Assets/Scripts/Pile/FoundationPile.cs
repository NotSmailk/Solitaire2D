using System.Collections.Generic;
using UnityEngine;

public class FoundationPile : MonoBehaviour
{
    [field: SerializeField] public CardSuit foundationSuit;    

    [field: HideInInspector] public CardValue nextCardValue = CardValue.Ace;
    [field: HideInInspector] public List<GameObject> cards = new List<GameObject>();

    private int currentCardIndex = -1;

    public void PlaceCard(Card card)
    {
        CardInfo cardInfo = card.GetCardInfo();

        if (cardInfo.Suit == foundationSuit && cardInfo.Value == nextCardValue)
        {
            card.transform.parent = transform;
            card.transform.position = transform.position;

            cards.Add(card.gameObject);
            currentCardIndex++;
            nextCardValue++;

            if (currentCardIndex > 0)
            {
                cards[currentCardIndex - 1].SetActive(false);
            }

            if (cardInfo.Value == CardValue.King)
            {
                SoliataireManager.GlobalSoliataireManager.collectedPiles++;

                SoliataireManager.GlobalSoliataireManager.Win();
            }
        }
    }

    public void RemoveCard(Card card)
    {
        cards.Remove(card.gameObject);
        currentCardIndex--;
        nextCardValue--;

        if (currentCardIndex >= 0)
        {
            cards[currentCardIndex].SetActive(true);
        }

        if (card.GetCardInfo().Value == CardValue.King)
        {
            SoliataireManager.GlobalSoliataireManager.collectedPiles--;
        }
    }

    public void ShowPreviosCard(bool show)
    {
        if (currentCardIndex <= 0)
            return;

        cards[currentCardIndex - 1].SetActive(show);
    }
}
