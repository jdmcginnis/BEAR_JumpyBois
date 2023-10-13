using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entering Jumping State! (Remove this notice!)");

        // We have already received all the input we need for the current trial
        player.skillCheckManager.acceptingInput = false;

        // Update Image Sprite
        player.graspSelector.MarkGraspCompletedImage();

        // Add enough force to get character over the obstacle
        player.playerRB.AddForce(Vector2.up * 11, ForceMode2D.Impulse);

        // temporarily disable box collider so it doesn't interact with obstacle
        player.GetComponent<BoxCollider2D>().enabled = false;

        // Handle Jumping Animation
        player.playerAnim.CrossFade(player.Jump, 0.1f, 0);
    }


    public override void UpdateState(PlayerStateManager player)
    {
        // when player is at top of jump
        int apogeeHeight = 2;

        // enable box collider again before it hits the ground
        if ((int)player.playerLoc.position.y >= apogeeHeight)
        {
            player.GetComponent<BoxCollider2D>().enabled = true;
            player.playerAnim.speed = 0;
        } else
        {
            player.playerAnim.speed = 1;
        }

        // Keep player moving horizontally as it is jumping
        player.transform.Translate(10 * Time.fixedDeltaTime, 0, 0);
    }

    // When we hit the ground again, we will start walking again
    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.collider.gameObject.layer == player.groundLayer)
            player.SwitchState(player.WalkingState);
    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D trigger)
    {
        throw new System.NotImplementedException();
    }
}
