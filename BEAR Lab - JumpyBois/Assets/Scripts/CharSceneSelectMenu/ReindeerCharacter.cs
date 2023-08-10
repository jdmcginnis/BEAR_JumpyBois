using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReindeerCharacter : CharacterBaseState
{
    public enum reindeerTypes
    {
        reindeer_brown, // 0
        reindeer_grey, // 1
        reindeer_rudolph, // 2
        reindeer_white, // 3
        reindeer_whiteandbrown // 4
    }

    public int currentStyle = 0; // start off with a brown reindeer

    private Animator reindeerAnim;
    

    public override void EnterState(CharacterSelectManager character)
    {
            reindeerAnim = character.charAnimContainer.GetComponent<Animator>();
            reindeerAnim.Play(((reindeerTypes)currentStyle).ToString());
        
    }

    public override void ChangeCharacterStyle(CharacterSelectManager character, int direction)
    {
        ChangeStyleInd(direction);
        reindeerAnim.Play(((reindeerTypes)currentStyle).ToString());
    }


    // Prevents index from going out of bounds of the enum reindeerTypes
    protected override void ChangeStyleInd(int direction)
    {
        if (direction == (int)CharacterBaseState.charSelDir.nextStyle)
        {
            if (currentStyle == 4)
                currentStyle = 0;
            else 
                currentStyle += 1;
        } else if (direction == (int)CharacterBaseState.charSelDir.prevStyle) {
            if (currentStyle == 0)
                currentStyle = 4;
            else
                currentStyle -= 1;
        }
    }

    public override void LogCharacterData()
    {
        GlobalStorage.GameSettings.characterSelection = ((reindeerTypes)currentStyle).ToString();
    }
}
