using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{

    [SerializeField] public GameObject endingMenuUI;
    [SerializeField] public GameObject blurCamera;

    public IEnumerator DisplayEndGameMenu()
    {
        Debug.Log("Inside IEnumerator");
        yield return new WaitForSeconds(2);
        blurCamera.SetActive(true);
        endingMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    // Called from EndGameMenu/MainMenuButton
    public void MainMenu()
    {
        Destroy(GameObject.Find("PlayerData"));
        Destroy(PlayerData.PlayerDataRef);
        SceneManager.LoadSceneAsync("0.MainMenu");

        Time.timeScale = 1f;
    }

    // Called from EndGameMenu/QuitButton
    public void QuitGameButton()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void PlayAgain()
    {
        Debug.Log("TODO: Fix Camera Being Destroyed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
