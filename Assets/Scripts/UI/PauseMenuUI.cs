
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject controlsPage;
    public GameObject settingsPage;
    public GameObject menuPage;

    private void Start()
    {
        menuPage.SetActive(true);
        HideControlsPage();
        HideSettingsPage();
    }
    public void ShowControlsPage()
    {
        controlsPage.SetActive(true);
    }

    public void HideControlsPage()
    {
        controlsPage.SetActive(false);
    }

    public void ShowSettingsPage()
    {
        settingsPage.SetActive(true);
    }

    public void HideSettingsPage()
    {
        settingsPage.SetActive(false);
    }
    
}
