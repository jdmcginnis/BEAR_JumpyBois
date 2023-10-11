using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndingState : PlayerBaseState
{
    private Rigidbody2D playerRB;

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered Ending State!");
        player.playerAnim.CrossFade(player.gameComplete, 0, 0);
        playerRB = player.GetComponent<Rigidbody2D>();
        player.cameraMotor.enableCameraMove = false;
        player.StartCoroutine(player.DisplayEndGameMenu());
    }


    public override void UpdateState(PlayerStateManager player)
    {
        playerRB.velocity = new Vector3(15, 8, 0);
        
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D trigger)
    {
        throw new System.NotImplementedException();
    }

}
