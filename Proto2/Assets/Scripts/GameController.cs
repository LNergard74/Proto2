using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject credits;

    public bool isPaused = false;
    private bool creditsToggle = false;

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Win()
    {
        SceneManager.LoadScene("WinScreen");
    }

    public void CreditsToggle()
    {
        creditsToggle = !creditsToggle;
        credits.SetActive(creditsToggle);
    }
    public void Resume()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;

        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.Locked;

    }

    public void Pause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            Cursor.visible = true;

            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            Cursor.visible = true;

            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
