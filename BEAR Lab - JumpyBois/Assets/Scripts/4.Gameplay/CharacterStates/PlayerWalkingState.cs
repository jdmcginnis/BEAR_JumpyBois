using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    // True when we detect an obstacle which triggers a skill check
    private bool obstacleDetected;


    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered PlayerWalkingState! (Remove This Message)");

        player.playerAnim.Play(player.Walk);
        player.skillCheckManager.HideSkillCheck();
        obstacleDetected = false;

        // Whenever we enter the walking state, we want to load up the next random grasp in the background
        player.graspSelector.LoadNextGrasp();
    }


    // Frame-by-frame updates
    public override void UpdateState(PlayerStateManager player)
    {
        player.transform.Translate(player.playerSpeed * Time.fixedDeltaTime, 0, 0);

        if (!obstacleDetected)
        {
            RaycastHit2D collisionCheck = CheckForUpcomingCollision(player);

            if (collisionCheck.collider != null)
            {
                player.skillCheckManager.ShowSkillCheck();
                obstacleDetected = true;
            }
        }
    }


    // When the player physically collides with the obstacle
    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        // when there is an obstacle collision, enter either the jumping or idle state
        if (collision.collider.gameObject.layer == player.obstacleLayer)
        {
            // Checking if the player has enough points to proceed
            if (player.pointsBar.goalReached)
            {
                player.SwitchState(player.JumpingState);
            } else
                player.SwitchState(player.IdleState);
        }

    }


    // The EndGame trigger is the only trigger that exists
    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D trigger)
    {
        player.SwitchState(player.EndingState);
    }


    private RaycastHit2D CheckForUpcomingCollision(PlayerStateManager player)
    {
        RaycastHit2D collisionCheck = Physics2D.BoxCast(player.transform.position, player.playerBoxCollider.size, 0,
                new Vector2(1, 0), Mathf.Abs(player.playerSpeed * player.skillCheckManager.skillCheckTotTime) + player.playerBoxCollider.size.x / 2,
                LayerMask.GetMask("Obstacle", "Player"));

        return collisionCheck;
    }

}
