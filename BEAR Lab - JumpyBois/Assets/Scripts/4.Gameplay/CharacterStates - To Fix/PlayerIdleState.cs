using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerIdleState : PlayerBaseState
{

    private float timeElapsed;

    private enum idleAnims
    {
        idle, // 0
        idle_faceForward, // 1
        idle_snackTime, // 2
        idle_sleepyTime // 3
    }

    private int currentIdleState; // for better performance

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Idle State!");
        timeElapsed = 0;
        player.playerAnim.CrossFade(player.Idle, 0, 0);
        currentIdleState = (int)idleAnims.idle;

        // Open up option to skip this obstacle (spacebar)
        player.inputManager.canPlayerSkip = true;
    }


    public override void UpdateState(PlayerStateManager player)
    {

        // Start timer and play idle animations!
        timeElapsed += Time.fixedDeltaTime;

        if (currentIdleState == (int)idleAnims.idle && timeElapsed >= 5)
        {
            player.playerAnim.CrossFade(player.Idle_FaceForward, 0 , 0);
            currentIdleState = (int)idleAnims.idle_faceForward;
        } 
        
        else if (currentIdleState == (int)idleAnims.idle_faceForward && timeElapsed >= 10)
        {
            player.playerAnim.CrossFade(player.Idle_ToSnackTime, 0, 0);
            currentIdleState = (int)idleAnims.idle_snackTime;
        }

        else if (currentIdleState == (int)idleAnims.idle_snackTime && timeElapsed >= 30)
        {
            player.playerAnim.CrossFade(player.Idle_FromSnackTime, 0, 0);
            currentIdleState = (int)idleAnims.idle_sleepyTime;
        }


        if (player.pointsBar.goalReached)
        {
            player.SwitchState(player.JumpingState);
        }
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
