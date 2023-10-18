using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBaseState
{
    public abstract void EnterState(CharacterSelectManager character);

    public abstract void ChangeCharacterStyle(CharacterSelectManager character, int direction);

    protected abstract void ChangeStyleInd(int direction);

    public abstract void LogCharacterData();

}
