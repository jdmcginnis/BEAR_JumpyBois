using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBaseState
{
    public enum charSelDir
    {
        nextStyle, // 0
        prevStyle, // 1
        nextChar, // 2
        prevChar // 3
    }

    public abstract void EnterState(CharacterSelectManager character);

    public abstract void ChangeCharacterStyle(CharacterSelectManager character, int direction);

    protected abstract void ChangeStyleInd(int direction);

    public abstract void LogCharacterData();
}
