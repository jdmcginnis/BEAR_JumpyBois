using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public bool gameIsPaused;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject blurCamera;


    public void Resume()
    {
        
        pauseMenuUI.SetActive(false);
        blurCamera.SetActive(false);
        RevertPause();
    }


    public void Pause(InputAction.CallbackContext context)
    {
        if (!gameIsPaused)
        {
            Debug.Log("The context is: " + context.phase);
            if (context.canceled)
            {
                gameIsPaused = true;
                Debug.Log("Pause Game!");
                pauseMenuUI.SetActive(true);
                blurCamera.SetActive(true);
                Time.timeScale = 0f;

            }
        }
    }

    public void MainMenu()
    {
        Debug.Log("Main Menu!");
        RevertPause();
        SceneManager.LoadSceneAsync("0.MainMenu");
    }

    // Exit Application
    public void QuitGameButton()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private void RevertPause()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
    }


}
