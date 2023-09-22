using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public RaycastHit2D collisionCheck;
    private bool collisionDetected; // For performance
    

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered PlayerWalkingState!");
        player.playerAnim.Play(player.Walk);
        player.skillCheckObj.SetActive(false);
        collisionDetected = false;

        // Loading the next random grasp up in the background whenever we enter the walking state
        player.StartCoroutine(player.graspSelector.LoadNextGrasp());
    }


    public override void UpdateState(PlayerStateManager player)
    {
        player.transform.Translate(player.playerSpeed * Time.fixedDeltaTime, 0, 0);

        if (!collisionDetected)
        {
            collisionCheck = Physics2D.BoxCast(player.transform.position, player.playerBoxCollider.size, 0,
                new Vector2(1, 0), Mathf.Abs(player.playerSpeed * player.skillcheckTotTime),
                LayerMask.GetMask("Obstacle", "Player"));

            if (collisionCheck.collider != null)
            {
                player.skillCheckObj.SetActive(true);
                collisionDetected = true;
            }
        }
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        // when there is a collision, enter either the jumping or idle state
        if (collision.collider.gameObject.layer == player.obstacleLayer)
        {

            // If points bar is filled up enough
            if (player.pointsBar.goalReached)
            {
                player.SwitchState(player.JumpingState);
                // TODO: Mark skill check as completed (visual)
            } else
                player.SwitchState(player.IdleState);
        }
        
    }
}
