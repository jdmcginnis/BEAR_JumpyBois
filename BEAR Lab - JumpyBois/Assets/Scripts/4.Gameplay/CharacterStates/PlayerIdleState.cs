using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerIdleState : PlayerBaseState
{

    private float timeElapsed; // used for transitioning idle animations

    private enum idleAnims
    {
        idle, // 0
        idle_faceForward, // 1
        idle_snackTime, // 2
        idle_sleepyTime // 3
    }

    private idleAnims currentIdleState; // for better performance

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Idle State! (Remove this message!)");

        timeElapsed = 0;
        player.playerAnim.CrossFade(player.Idle, 0, 0);
        currentIdleState = idleAnims.idle;
    }

    // Increments timer and plays idle animations
    public override void UpdateState(PlayerStateManager player)
    {
        timeElapsed += Time.fixedDeltaTime;

        if (currentIdleState == idleAnims.idle && timeElapsed >= 5)
        {
            player.playerAnim.CrossFade(player.Idle_FaceForward, 0 , 0);
            currentIdleState = idleAnims.idle_faceForward;
        } 
        
        else if (currentIdleState == idleAnims.idle_faceForward && timeElapsed >= 10)
        {
            player.playerAnim.CrossFade(player.Idle_ToSnackTime, 0, 0);
            currentIdleState = idleAnims.idle_snackTime;
        }

        else if (currentIdleState == idleAnims.idle_snackTime && timeElapsed >= 30)
        {
            player.playerAnim.CrossFade(player.Idle_FromSnackTime, 0, 0);
            currentIdleState = idleAnims.idle_sleepyTime;
        }

        // Continuously monitor if we have reached our points goal
        if (player.pointsBar.goalReached)
            player.SwitchState(player.JumpingState);

    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D trigger)
    {
        throw new System.NotImplementedException();
    }
}
