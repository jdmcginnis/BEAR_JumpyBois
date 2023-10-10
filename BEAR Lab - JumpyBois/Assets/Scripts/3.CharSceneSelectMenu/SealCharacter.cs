using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealCharacter : CharacterBaseState
{
    private int currentStyle = 0; // starts off with a black seal

    private Animator sealAnim;

    private static int black = Animator.StringToHash("Base Layer.seal_black");
    private static int cream = Animator.StringToHash("Base Layer.seal_cream");
    private static int grey = Animator.StringToHash("Base Layer.seal_grey");
    private static int white = Animator.StringToHash("Base Layer.seal_white");

    private int[] animLookup = { black, cream, grey, white };

    public override void EnterState(CharacterSelectManager character)
    {
        sealAnim = character.charAnimContainer.GetComponent<Animator>();
        sealAnim.Play(animLookup[currentStyle]);
    }

    public override void ChangeCharacterStyle(CharacterSelectManager character, int direction)
    {
        ChangeStyleInd(direction);
        sealAnim.Play(animLookup[currentStyle]);
    }

    // Prevents index from going out of bounds of the enum sealTypes
    protected override void ChangeStyleInd(int direction)
    {
        if (direction == (int)CharacterSelectManager.charSelDir.nextStyle)
        {
            if (currentStyle == 3)
                currentStyle = 0;
            else
                currentStyle += 1;
        } else if (direction == (int)CharacterSelectManager.charSelDir.prevStyle)
        {
            if (currentStyle == 0)
                currentStyle = 3;
            else
                currentStyle -= 1;
        }
    }

    public override void LogCharacterData()
    {
        PlayerData.PlayerDataRef.characterType = GameLookup.characterTypes.seal;
        PlayerData.PlayerDataRef.sealSelection = (GameLookup.sealTypes)currentStyle;
    }
}
