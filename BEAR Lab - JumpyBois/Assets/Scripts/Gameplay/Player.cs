using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float playerSpeed;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject skillCheckObj;
    [SerializeField] private SkillCheckBar skillCheckBar;

    private RaycastHit2D collisionCheck;
    private BoxCollider2D playerBoxCollider;

    private float skillcheckTotTime;

    // NO LONGER USED?

    private void Start()
    {
        playerBoxCollider = this.GetComponent<BoxCollider2D>();
        Physics2D.queriesStartInColliders = false;

        skillcheckTotTime = skillCheckBar.userPrepTime + skillCheckBar.userInputTime;
    }

    private void FixedUpdate()
    {
        CheckForObstacle();

        // WalkForwards();
    }

    private void WalkForwards()
    {
        this.transform.Translate(playerSpeed * Time.fixedDeltaTime, 0, 0);
    }

    private void CheckForObstacle()
    {
        collisionCheck = Physics2D.BoxCast(transform.position, playerBoxCollider.size, 0, 
            new Vector2(1, 0), Mathf.Abs(playerSpeed * skillcheckTotTime),
            LayerMask.GetMask("Obstacle", "Player"));


        if (collisionCheck.collider != null)
        {
            skillCheckObj.SetActive(true);
        } 
        else
        {
            skillCheckObj.SetActive(false);
        }
    }
}
