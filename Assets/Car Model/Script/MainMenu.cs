using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup OptionPanel;

    private bool isOptionsMenuOpen = false;

    private void Start()
    {
        OptionPanel.alpha = 0;
        OptionPanel.blocksRaycasts = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOptionsMenuOpen)
                CloseOptionsMenu();
            else
                OpenOptionsMenu();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Tutorial()
    {
        OpenOptionsMenu();
    }

    public void Back()
    {
        CloseOptionsMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OpenOptionsMenu()
    {
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
        isOptionsMenuOpen = true;
        Time.timeScale = 0f; // Pauses the game when the options menu is open
    }

    private void CloseOptionsMenu()
    {
        OptionPanel.alpha = 0;
        OptionPanel.blocksRaycasts = false;
        isOptionsMenuOpen = false;
        Time.timeScale = 1f; // Resumes the game when the options menu is closed
    }
}
