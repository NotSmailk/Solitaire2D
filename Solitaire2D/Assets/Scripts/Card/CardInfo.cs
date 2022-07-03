using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardInfo
{
    public CardSuit Suit;
    public List<CardSuit> PossibleDealSuits;
    public CardValue Value;
    public CardValue DealValue;
    public Sprite CardFace;

    public CardInfo(CardSuit cardSuit, CardValue cardValue, Sprite cardFace)
    {
        Suit = cardSuit;
        Value = cardValue;
        DealValue = Value+1;
        CardFace = cardFace;
        GetPossibleDealSuits();
    }

    public void GetPossibleDealSuits()
    { 
        if (Suit == CardSuit.Club)
        {
            PossibleDealSuits = new List<CardSuit> { CardSuit.Diamond, CardSuit.Heart };
        }

        if (Suit == CardSuit.Spade)
        {
            PossibleDealSuits = new List<CardSuit> { CardSuit.Diamond, CardSuit.Heart };
        }

        if (Suit == CardSuit.Heart)
        {
            PossibleDealSuits = new List<CardSuit> { CardSuit.Club, CardSuit.Spade };
        }

        if (Suit == CardSuit.Diamond)
        {
            PossibleDealSuits = new List<CardSuit> { CardSuit.Club, CardSuit.Spade };
        }
    }
}

public enum CardSuit
{ 
    Club,
    Diamond,
    Heart,
    Spade
}

public enum CardValue
{ 
    Ace,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}