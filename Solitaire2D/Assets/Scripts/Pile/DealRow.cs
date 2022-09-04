using System.Collections.Generic;
using UnityEngine;

public class DealRow : MonoBehaviour
{
    [field: HideInInspector] public List<GameObject> cards = new List<GameObject>();

    [field: SerializeField] public int startCardsCount = 0;

    public void UpdateCards()
    {
        if (cards.Count == 0)
            return;

        cards[cards.Count - 1].GetComponent<Card>().FaceUp(true);
    }

    public void PlaceCard(Card card)
    {
        card.transform.parent = transform;
        card.transform.position = transform.position;
    }
}
