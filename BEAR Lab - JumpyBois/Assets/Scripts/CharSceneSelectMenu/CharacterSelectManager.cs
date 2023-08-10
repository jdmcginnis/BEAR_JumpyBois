using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    CharacterBaseState currentCharacter;
    public ReindeerCharacter Reindeer = new ReindeerCharacter();
    public SealCharacter Seal = new SealCharacter();

    public GameObject charAnimContainer;

    //public enum characterType
    //{
    //    reindeer, // 0
    //    seal // 1
    //}

    // Default states
    private int curCharInd;

    private void Start()
    {
        // Setting default states
        curCharInd = (int)GlobalStorage.characterTypes.reindeer;

        if (curCharInd == (int)GlobalStorage.characterTypes.reindeer)
            currentCharacter = Reindeer;
        else if (curCharInd == (int)GlobalStorage.characterTypes.seal)
            currentCharacter = Seal;
        else
            Debug.Log("ERROR: You need to implement this character!");
        
        currentCharacter.EnterState(this);
    }


    public void OnUpArrowClick() // Next Style
    {
        currentCharacter.ChangeCharacterStyle(this, (int)CharacterBaseState.charSelDir.nextStyle);
    }

    public void OnDownArrowClick() // Previous Style
    {
        currentCharacter.ChangeCharacterStyle(this, (int)CharacterBaseState.charSelDir.prevStyle);
    }

    public void OnRightArrowClick() // Next Character
    {
        ChangeCharacterInd((int)CharacterBaseState.charSelDir.nextChar);
        ChangeCurrentCharacter(curCharInd);
    }

    public void OnLeftArrowClick() // Previous Character
    {
        ChangeCharacterInd((int)CharacterBaseState.charSelDir.prevChar);
        ChangeCurrentCharacter(curCharInd);
    }

    private void ChangeCharacterInd(int direction)
    {
        if (direction == (int)CharacterBaseState.charSelDir.nextChar)
        {
            if (curCharInd == 1)
                curCharInd = 0;
            else
                curCharInd += 1;
        } else if (direction == (int)CharacterBaseState.charSelDir.prevChar)
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
            case (int)GlobalStorage.characterTypes.reindeer:
                currentCharacter = Reindeer;
                break;
            case (int)GlobalStorage.characterTypes.seal:
                currentCharacter = Seal;
                break;
        }

        currentCharacter.EnterState(this);
    }

    public void LogCharacterData()
    {
        currentCharacter.LogCharacterData();
    }
}
