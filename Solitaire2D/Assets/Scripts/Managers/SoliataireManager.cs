using System.Collections.Generic;
using UnityEngine;

public class SoliataireManager : MonoBehaviour
{
    [field: SerializeField] private GameObject cardPrefab;
    [field: SerializeField] private GameObject victoryMenu;

    [field: SerializeField] private List<DealRow> dealRows = new List<DealRow>();

    private List<CardInfo> deck = new List<CardInfo>();

    private SkinsManager skinsManager;
    private AdditionalCardsPile additionalCardsPile;

    [field: HideInInspector] public int collectedPiles = 0;

    public static SoliataireManager GlobalSoliataireManager;

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CardTable"); 
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }   

    public void Win()
    {
        if (collectedPiles != 4)
            return;

        victoryMenu.SetActive(true);

        SettingsMenu.GlobalSettingsMenu.restartButton.SetActive(false);
        SettingsMenu.GlobalSettingsMenu.settingsButton.SetActive(false);
    }

    private void Awake()
    {
        GlobalSoliataireManager = this;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        skinsManager = SkinsManager.GlobalSkinsManager;
        additionalCardsPile = AdditionalCardsPile.GlobalAdditionalCardPile;

        PlayCards();

        foreach (DealRow row in dealRows)
        {
            PlaceCardsInDeal(row.startCardsCount, row.GetComponent<Transform>(), row.cards);
            row.UpdateCards();
        }

        PlaceCardInAdditionalCardsPile(additionalCardsPile.additionalCardsTransform, additionalCardsPile.cards);
        additionalCardsPile.HideCards();

        skinsManager.ChangeCardsBack();
    }

    private void PlaceCardInAdditionalCardsPile(Transform originTransfrom, List<GameObject> cards)
    {
        for (int i = 0; i < deck.Count - 1; i++)
        {
            GameObject temp = Instantiate(cardPrefab, originTransfrom.position, Quaternion.identity, originTransfrom);

            temp.name = $"{deck[0].Value} of {deck[0].Suit}";
            temp.GetComponent<Card>().SetCardFace(deck[0].CardFace);
            temp.GetComponent<Card>().SetCardInfo(deck[0]);
            temp.GetComponent<Card>().curAdditionalCardsPile = additionalCardsPile;

            cards.Add(temp);
            deck.RemoveAt(0);
        }
    }

    private void PlaceCardsInDeal(int cardsCount, Transform originTransfrom, List<GameObject> cards)
    {
        float offsetY = 0f;
        float offsetZ = 0.05f;

        for (int i = 0; i < cardsCount; i++)
        {
            Vector3 cardPosition = GetPositionWithOffset(originTransfrom.position, 0f, offsetY, offsetZ);

            GameObject temp = Instantiate(cardPrefab, cardPosition, Quaternion.identity, originTransfrom);

            temp.name = $"{deck[0].Value} of {deck[0].Suit}";
            temp.GetComponent<Card>().SetCardFace(deck[0].CardFace);
            temp.GetComponent<Card>().SetCardInfo(deck[0]);
            temp.GetComponent<Card>().curDealRow = originTransfrom.GetComponent<DealRow>();

            deck.RemoveAt(0);
            cards.Add(temp);

            offsetY += 0.3f;
            offsetZ += 0.05f;
        }
    }

    private void PlayCards()
    {
        deck = GenerateDeck();

        Shuffle(deck);
    }

    private static List<CardInfo> GenerateDeck()
    {
        List<CardInfo> newDeck = new List<CardInfo>();

        int cardIndex = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                newDeck.Add(new CardInfo((CardSuit)i, (CardValue)j, SkinsManager.GlobalSkinsManager.cardFaces[cardIndex]));

                cardIndex++;
            }
        }

        return newDeck;
    }

    private void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        while (n > 1)
        {            
            int k = random.Next(n);
            n--;

            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    private Vector3 GetPositionWithOffset(Vector3 position, float offsetX, float offsetY, float offsetZ)
    {
        return new Vector3(position.x - offsetX, position.y - offsetY, position.z - offsetZ);
    }
}
