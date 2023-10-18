using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReindeerCharacter : CharacterBaseState
{
    public int currentStyle = 0; // start off with a brown reindeer

    private Animator reindeerAnim;

    private static int brown = Animator.StringToHash("Base Layer.reindeer_brown");
    private static int grey = Animator.StringToHash("Base Layer.reindeer_grey");
    private static int rudolph = Animator.StringToHash("reindeer_rudolph");
    private static int white = Animator.StringToHash("Base Layer.reindeer_white");
    private static int whiteandbrown = Animator.StringToHash("Base Layer.reindeer_whiteandbrown");

    private int[] animLookup = { brown, grey, rudolph, white, whiteandbrown };


    public override void EnterState(CharacterSelectManager character)
    {
            reindeerAnim = character.charAnimContainer.GetComponent<Animator>();
            reindeerAnim.Play(animLookup[currentStyle]);
        
    }

    public override void ChangeCharacterStyle(CharacterSelectManager character, int direction)
    {
        ChangeStyleInd(direction);
        reindeerAnim.Play(animLookup[currentStyle]);
    }


    // Prevents index from going out of bounds of the enum reindeerTypes
    protected override void ChangeStyleInd(int direction)
    {
        if (direction == (int)CharacterSelectManager.charSelDir.nextStyle)
        {
            if (currentStyle == 4)
                currentStyle = 0;
            else 
                currentStyle += 1;
        } else if (direction == (int)CharacterSelectManager.charSelDir.prevStyle) {
            if (currentStyle == 0)
                currentStyle = 4;
            else
                currentStyle -= 1;
        }
    }

    public override void LogCharacterData()
    {
        PlayerData.PlayerDataRef.characterType = GameLookup.characterTypes.reindeer;
        PlayerData.PlayerDataRef.reindeerSelection = (GameLookup.reindeerTypes)currentStyle;
    }
}
