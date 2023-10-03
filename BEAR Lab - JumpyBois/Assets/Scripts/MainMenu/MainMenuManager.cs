using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Sprite[] mainMenuSprites;

    // Start is called before the first frame update
    void Awake()
    {
        int randNum = Random.Range(0, mainMenuSprites.Length);
        this.GetComponent<Image>().sprite = mainMenuSprites[randNum];
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayGameButton()
    {
        Debug.Log("Play Game");
    }
    
    public void QuitGameButton()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void AdministratorMenuButton()
    {
        Debug.Log("Administrator Menu");
    }

    public void SetupButton()
    {
        Debug.Log("Setup");
    }
}
