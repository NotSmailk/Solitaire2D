using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [field: SerializeField] public GameObject restartButton;
    [field: SerializeField] public GameObject settingsButton;
    [field: SerializeField] public GameObject settingsMenu;

    private SkinsManager skinsManager;

    public static bool MenuOpened = false;
    public static SettingsMenu GlobalSettingsMenu;

    private void Awake()
    {
        GlobalSettingsMenu = this;
    }

    private void Start()
    {
        MenuOpened = false;

        skinsManager = SkinsManager.GlobalSkinsManager;
    }

    public void ShowMenu()
    {
        settingsMenu.SetActive(true);

        restartButton.SetActive(false);
        settingsButton.SetActive(false);

        MenuOpened = true;
    }

    public void CloseMenu()
    {
        settingsMenu.SetActive(false);

        restartButton.SetActive(true);
        settingsButton.SetActive(true);

        MenuOpened = false;
    }

    public void ChangeCardBack(Sprite cardBack)
    {
        skinsManager.currentCardBack = cardBack;

        skinsManager.ChangeCardsBack();
    }

    public void ChangeBackColor(GameObject buttonColor)
    {
        skinsManager.cardTableColor = buttonColor.GetComponent<Image>().color;

        skinsManager.ChangeBackColor();
    }
}
