using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathPanelScript : MonoBehaviour
{
    public Button restartButton;
    public Button quitButton;

    public GameObject deathPanelUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathPanelUI.SetActive(false);

        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu"); // Reload the current scene
    }
}
