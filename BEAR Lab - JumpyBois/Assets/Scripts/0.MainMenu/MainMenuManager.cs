using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // Main Menu Images
    [SerializeField] private Sprite[] mainMenuSprites;
    
    void Awake()
    {
        int randNum = Random.Range(0, mainMenuSprites.Length);
        this.GetComponent<Image>().sprite = mainMenuSprites[randNum];

        Debug.Log("Create a new PlayerData object, reading in from saved data file");
    }

    // Calibrate grasp signals first if not already done, then play game
    public void PlayGameButton()
    {
        Debug.Log("TODO: Check if already calibrated. If so, play game. If not, go to CalibrationMenu/Setup");
        Debug.Log("TODO: For now, we'll test this by going directly to the game...");
        SceneManager.LoadSceneAsync("4.Game");
    }
    
    // Exit Application
    public void QuitGameButton()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    // Disabled by default unless user has administrator access
    // Select grasps, total number of tests, num of tests per grasp, and input method
    public void AdministratorMenuButton()
    {
        Debug.Log("TODO: Destroy PlayerData Object");
        Debug.Log("TODO: Go through Admin Menu for setup; save setup at the end");
        Debug.Log("TODO: Load saved data upon main menu open");
        Debug.Log("TODO: Make inactive, when inactive, set transparency");
        SceneManager.LoadSceneAsync("1.GraspSelectionMenu");
    }

    // Calibrate grasp signals
    public void SetupButton()
    {
        SceneManager.LoadSceneAsync("5.CalibrationMenu");
    }
}
