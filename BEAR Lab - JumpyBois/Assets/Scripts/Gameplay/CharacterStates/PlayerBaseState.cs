using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{

    // Manages entry into this state from external trigger
    public abstract void EnterState(PlayerStateManager player);

    // Manages any frame-by-frame updates that occur in the state
    public abstract void UpdateState(PlayerStateManager player);

    // Manages what happens to this state when a collision occurs
    public abstract void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision);

}
