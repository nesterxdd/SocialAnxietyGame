
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    
    public GameObject creditsScreen;
    public Transform[] creditsPositions;

    private void Start()
    {
        creditsScreen.SetActive(false);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisplayCredits()
    {
        creditsScreen.SetActive(true);
        creditsScreen.transform.position = creditsPositions[0].position;
        LeanTween.moveY(creditsScreen, creditsPositions[1].position.y, 0.15f);
    }

    public void HideCredits()
    {
        creditsScreen.SetActive(false);
    }
}
