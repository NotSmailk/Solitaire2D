using System;
using UnityEngine;

public class SkinData
{
    public Sprite cardBack;
    public Color cardTableColor = new Color(0f, 98f, 0f);
}

public class SkinsManager : MonoBehaviour
{
    [Header("Settings")]
    public Color cardTableColor = new Color(0f, 98f, 0f);
    public Sprite currentCardBack;
    public Sprite[] cardBacks;
    public Sprite[] cardFaces;

    [Header("Main Objects")]
    public Camera mainCamera;

    public static SkinsManager GlobalSkinsManager;

    private void Awake()
    {
        GlobalSkinsManager = this;

        LoadSkinsData();
    }

    private void Start()
    {
        ChangeBackColor();
    }

    public void ChangeCardsBack()
    {
        SaveSkinsData();

        foreach (Card card in FindObjectsOfType<Card>())
        {
            card.SetCardBack(currentCardBack);
        }

        FindObjectOfType<AdditionalCardsPile>().SetCardBack(currentCardBack);
    }

    public void ChangeBackColor()
    {
        SaveSkinsData();

        mainCamera.backgroundColor = cardTableColor;
    }

    public void SaveSkinsData()
    {
        SkinData skinData = new SkinData();

        skinData.cardTableColor = cardTableColor;
        skinData.cardBack = currentCardBack;

        SaveManager.SaveSkins(skinData);
    }

    public void LoadSkinsData()
    {
        SaveManager.LoadSkins(out SkinData skinData);

        if (skinData == null)
            return;

        if (skinData.cardBack != null)
            currentCardBack = skinData.cardBack;

        if (skinData.cardTableColor != null)
            cardTableColor = skinData.cardTableColor;
    }
}
