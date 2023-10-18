using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool gameIsPaused;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject blurCamera;

    // Called from PauseMenu/ResumeButton
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        blurCamera.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(ResetPausedStatus());
    }

    // Called when player presses 'Pause' key
    public void Pause()
    {
        if (!gameIsPaused)
        {
            gameIsPaused = true;
            pauseMenuUI.SetActive(true);
            blurCamera.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // Called from PauseMenu/MainMenuButton
    public void MainMenu()
    {
        Resume();
        Destroy(GameObject.Find("PlayerData"));
        Destroy(PlayerData.PlayerDataRef);
        SceneManager.LoadSceneAsync("0.MainMenu");
    }

    // Called from PauseMenu/QuitButton
    public void QuitGameButton()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    // Waits until the next frame to accept further input from the pause key
    // Fixes bug where the inputs pile up with pause key is pressed multiple times
    private IEnumerator ResetPausedStatus()
    {
        yield return new WaitForFixedUpdate();
        gameIsPaused = false;
    }

}