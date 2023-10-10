using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    CharacterBaseState currentCharacter;
    public ReindeerCharacter Reindeer = new ReindeerCharacter();
    public SealCharacter Seal = new SealCharacter();

    public GameObject charAnimContainer;
    
    private int curCharInd;

    public enum charSelDir
    {
        nextStyle, // 0
        prevStyle, // 1
        nextChar, // 2
        prevChar // 3
    }

    private void Start()
    {
        // Setting default states
        curCharInd = (int)GameLookup.characterTypes.reindeer;

        if (curCharInd == (int)GameLookup.characterTypes.reindeer)
            currentCharacter = Reindeer;
        else if (curCharInd == (int)GameLookup.characterTypes.seal)
            currentCharacter = Seal;
        else
            Debug.Log("ERROR: You need to implement this character!");
        
        currentCharacter.EnterState(this);
    }


    public void OnUpArrowClick() // Next Style
    {
        currentCharacter.ChangeCharacterStyle(this, (int)charSelDir.nextStyle);
    }

    public void OnDownArrowClick() // Previous Style
    {
        currentCharacter.ChangeCharacterStyle(this, (int)charSelDir.prevStyle);
    }

    public void OnRightArrowClick() // Next Character
    {
        ChangeCharacterInd((int)charSelDir.nextChar);
        ChangeCurrentCharacter(curCharInd);
    }

    public void OnLeftArrowClick() // Previous Character
    {
        ChangeCharacterInd((int)charSelDir.prevChar);
        ChangeCurrentCharacter(curCharInd);
    }

    // Handles looping indices (max index -> min index & min index -> max index)
    private void ChangeCharacterInd(int direction)
    {
        if (direction == (int)charSelDir.nextChar)
        {
            if (curCharInd == 1)
                curCharInd = 0;
            else
                curCharInd += 1;
        } else if (direction == (int)charSelDir.prevChar)
        {
            if (curCharInd == 0)
                curCharInd = 1;
            else
                curCharInd -= 1;
        }
    }

    private void ChangeCurrentCharacter(int charType)
    {
        switch (charType)
        {
            case (int)GameLookup.characterTypes.reindeer:
                currentCharacter = Reindeer;
                break;
            case (int)GameLookup.characterTypes.seal:
                currentCharacter = Seal;
                break;
        }

        currentCharacter.EnterState(this);
    }

    public void LogCharacterData()
    {
        currentCharacter.LogCharacterData();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync("4.Game");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync("0.MainMenu");
        Destroy(GameObject.Find("PlayerData"));
        Destroy(PlayerData.PlayerDataRef);
    }
}
