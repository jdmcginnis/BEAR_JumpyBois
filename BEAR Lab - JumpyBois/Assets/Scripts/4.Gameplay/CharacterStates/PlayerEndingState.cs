using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndingState : PlayerBaseState
{
    private Rigidbody2D playerRB;

    public override void EnterState(PlayerStateManager player)
    {
        playerRB = player.GetComponent<Rigidbody2D>();
        player.playerAnim.CrossFade(player.gameComplete, 0, 0);
        player.cameraMotor.enableCameraMove = false;
        player.StartCoroutine(player.endGameMenu.DisplayEndGameMenu());
    }


    public override void UpdateState(PlayerStateManager player)
    {
        playerRB.velocity = new Vector3(15, 8, 0);
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
