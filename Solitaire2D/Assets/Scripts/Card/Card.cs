using UnityEngine;

public class Card : MonoBehaviour
{
    [field: HideInInspector] public Transform parrentTransform;
    [field: HideInInspector] public DealRow curDealRow;
    [field: HideInInspector] public FoundationPile curFoundationPile;
    [field: HideInInspector] public AdditionalCardsPile curAdditionalCardsPile;
    [field: HideInInspector] public bool hasCard = false;
    [field: HideInInspector] public Vector3 linkedPosition;

    private CardInfo cardInfo;
    private Sprite cardFace;
    private Sprite cardBack;

    private bool isFaceUp = false;
    private bool isDragging = false;
    private bool isCollected = false;

    private Collider2D cardCollider;
    private DealRow prevDealRow;

    private float grabOffsetX = 0f;
    private float grabOffsetY = 0f;

    private void Start()
    {
        cardBack = SkinsManager.GlobalSkinsManager.currentCardBack;

        parrentTransform = transform.parent;
        cardCollider = GetComponent<BoxCollider2D>();

        linkedPosition = transform.position;
    }

    private void OnMouseOver()
    {
        if (SettingsMenu.MenuOpened)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && isFaceUp)
        {
            isDragging = true;

            GetOffest();

            CheckAdditionalCards();

            CheckFoundationCards();

            SFXManager.OnPickUp.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && isDragging)
        {
            transform.position = GetNewCardPosition(grabOffsetX, grabOffsetY);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && isDragging && isFaceUp)
        {
            isDragging = false;

            CheckCardNewLink();

            CheckAdditionalCards();

            CheckFoundationCards();

            DealRowNewCard();

            transform.position = linkedPosition;

            SetChildsPosition();
        }
    }

    private void CheckAdditionalCards()
    {
        if (curAdditionalCardsPile != null)
        {
            curAdditionalCardsPile.ShowPreviosCard(isDragging);
        }
    }

    private void CheckFoundationCards()
    {
        if (curFoundationPile != null)
        {
            curFoundationPile.ShowPreviosCard(isDragging);
        }
    }

    public void ChangeCardSprite()
    {
        GetComponent<SpriteRenderer>().sprite = isFaceUp ? cardFace : cardBack;
    }

    public void SetCardFace(Sprite face)
    {
        cardFace = face;

        ChangeCardSprite();
    }

    public void SetCardBack(Sprite back)
    {
        cardBack = back;

        ChangeCardSprite();
    }

    public void SetCardInfo(CardInfo info)
    {
        cardInfo = info;
    }

    public void FaceUp(bool facedUp)
    {
        isFaceUp = facedUp;

        ChangeCardSprite();
    }

    public bool GetDragging()
    {
        return isDragging;
    }

    public CardInfo GetCardInfo()
    {
        return cardInfo;
    }

    private void GetOffest()
    {
        grabOffsetX = transform.position.x - GetMousePosition().x;
        grabOffsetY = transform.position.y - GetMousePosition().y;
    }

    private Vector3 GetNewCardPosition(float offsetX, float offsetY)
    {
        Vector3 newPosition = new Vector3(GetMousePosition().x + offsetX, GetMousePosition().y + offsetY, -9f);

        return newPosition;
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = SkinsManager.GlobalSkinsManager.mainCamera.ScreenToWorldPoint(Input.mousePosition);

        return mousePosition;
    }

    private void CheckCardNewLink()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(GetMousePosition(), transform.lossyScale / 2, 0f);

        if (hitColliders == null)
            return;

        CheckNewCard(hitColliders);

        CheckNewDealRow(hitColliders);

        CheckNewFoundationPile(hitColliders);
    }

    private void CheckNewCard(Collider2D[] hitColliders)
    {
        foreach (Collider2D hit in hitColliders)
        {
            Card card = hit.gameObject.GetComponent<Card>();

            if (card == null)
                continue;

            if (hit != cardCollider && card.isFaceUp && card.transform.childCount == 0 && card.curFoundationPile == null)
            {
                isCollected = false;

                CheckCardValue(card);
            }
        }
    }

    private void CheckNewDealRow(Collider2D[] hitColliders)
    {
        foreach (Collider2D hit in hitColliders)
        {
            DealRow dealRow = hit.gameObject.GetComponent<DealRow>();

            if (dealRow == null)
                continue;

            if (cardInfo.Value == CardValue.King && dealRow.cards.Count == 0)
            {
                dealRow.PlaceCard(this);

                linkedPosition = transform.position;
                isCollected = false;

                SFXManager.OnPutDown.Invoke();

                CheckAdditionalCardsPile();

                CheckFoundationPile();

                CheckDealRow();
            }
        }
    }

    private void CheckNewFoundationPile(Collider2D[] hitColliders)
    {
        foreach (Collider2D hit in hitColliders)
        {
            FoundationPile foundationPile = hit.gameObject.GetComponent<FoundationPile>();

            if (foundationPile == null)
                continue;

            if (cardInfo.Suit == foundationPile.foundationSuit && cardInfo.Value == foundationPile.nextCardValue && transform.childCount == 0)
            {
                foundationPile.PlaceCard(this);

                linkedPosition = transform.position;
                linkedPosition.z--;
                curFoundationPile = foundationPile;
                isCollected = true;

                SFXManager.OnPutDown.Invoke();

                CheckAdditionalCardsPile();

                CheckDealRow();
            }
        }
    }

    private void DealRowNewCard()
    {
        if (prevDealRow == null)
            return;

        if (prevDealRow == curDealRow)
        {
            curDealRow.cards.Remove(gameObject);
            curDealRow.UpdateCards();
            curDealRow = null;
        }
    }

    private void CheckDealRow()
    {
        if (curDealRow != null)
        {
            prevDealRow = curDealRow;
        }
    }

    private void CheckAdditionalCardsPile()
    {
        if (curAdditionalCardsPile != null)
        {
            AdditionalCardsPile.GlobalAdditionalCardPile.RemoveCard(gameObject);
            curAdditionalCardsPile = null;
        }
    }

    private void CheckFoundationPile()
    {
        if (curFoundationPile != null && !isCollected)
        {
            curFoundationPile.RemoveCard(this);
            curFoundationPile = null;
        }
    }

    private void CheckCardValue(Card card)
    {
        if (cardInfo.DealValue.Equals(card.GetCardInfo().Value) && card.curAdditionalCardsPile == null)
        {
            CheckCardSuit(card);
        }
    }

    private void CheckCardSuit(Card card)
    {
        if (cardInfo.PossibleDealSuits.Contains(card.GetCardInfo().Suit))
        {
            parrentTransform = card.transform;
            transform.parent = parrentTransform;

            linkedPosition = new Vector3(card.transform.position.x, card.transform.position.y - 0.3f, card.transform.position.z - 0.05f);

            SFXManager.OnPutDown.Invoke();

            CheckAdditionalCardsPile();

            CheckFoundationPile();

            CheckDealRow();
        }
    }

    private void SetChildsPosition()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Card>().linkedPosition = child.position;
        }
    }
}
