using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Sprite[] mainMenuSprites;
    

    void Awake()
    {
        int randNum = Random.Range(0, mainMenuSprites.Length);
        this.GetComponent<Image>().sprite = mainMenuSprites[randNum];
    }


    // Calibrate grasp signals first if not already done, then play game
    public void PlayGameButton()
    {
        Debug.Log("Play Game");
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
        Debug.Log("Administrator Menu");
    }

    // Calibrate grasp signals
    public void SetupButton()
    {
        Debug.Log("Setup");
    }
}
