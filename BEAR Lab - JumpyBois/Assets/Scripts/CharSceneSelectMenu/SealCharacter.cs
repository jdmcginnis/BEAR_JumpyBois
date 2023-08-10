using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealCharacter : CharacterBaseState
{
    public enum sealTypes
    {
        seal_black, // 0
        seal_cream, // 1
        seal_grey, // 2
        seal_white, // 3
    }

    public int currentStyle = 0; // starts off with a black seal

    private Animator sealAnim;

    public override void EnterState(CharacterSelectManager character)
    {
            sealAnim = character.charAnimContainer.GetComponent<Animator>();
            sealAnim.Play(((sealTypes)currentStyle).ToString());
        
    }

    public override void ChangeCharacterStyle(CharacterSelectManager character, int direction)
    {
        ChangeStyleInd(direction);
        sealAnim.Play(((sealTypes)currentStyle).ToString());
    }

    // Prevents index from going out of bounds of the enum sealTypes
    protected override void ChangeStyleInd(int direction)
    {
        if (direction == (int)CharacterBaseState.charSelDir.nextStyle)
        {
            if (currentStyle == 3)
                currentStyle = 0;
            else
                currentStyle += 1;
        } else if (direction == (int)CharacterBaseState.charSelDir.prevStyle)
        {
            if (currentStyle == 0)
                currentStyle = 3;
            else
                currentStyle -= 1;
        }
    }

    public override void LogCharacterData()
    {
        GlobalStorage.GameSettings.characterSelection = ((sealTypes)currentStyle).ToString();
    }
}
