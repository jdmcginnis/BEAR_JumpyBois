using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{

    private bool apogee; // when player is at top of jump

    public override void EnterState(PlayerStateManager player)
    {

        Debug.Log("Entering Jumping State!");

        player.playerRB.AddForce(Vector2.up * 11, ForceMode2D.Impulse);

        // temporarily disable box collider
        player.GetComponent<BoxCollider2D>().enabled = false;

        // Update Image Sprite
        player.graspSelector.MarkGraspCompleted();

        player.inputManager.enableInput = false;

        player.playerAnim.Play(player.Jump);
        apogee = false;

        player.inputManager.canPlayerSkip = false;
        player.inputManager.playerDidSkip = false;

    }


    public override void UpdateState(PlayerStateManager player)
    {
        player.transform.Translate(10 * Time.fixedDeltaTime, 0, 0);

        apogee = false;

        // enable box collider again before it hits the ground
        if ((int)player.playerLoc.position.y >= 2 )
            apogee = true;

        if (player.playerLoc.position.y >= 2)
        {
            // Debug.Log(player.playerLoc.position.y);
            player.GetComponent<BoxCollider2D>().enabled = true;
        }



        if (apogee)
            player.playerAnim.speed = 0;
        else
        {
            player.playerAnim.speed = 1;
            apogee = false;
        }
            
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.collider.gameObject.layer == player.groundLayer)
        {
            player.SwitchState(player.WalkingState);
        }
            

    }
}
