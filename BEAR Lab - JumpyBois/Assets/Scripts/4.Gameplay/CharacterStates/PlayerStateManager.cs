using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEditor.Animations;
using System;

public class PlayerStateManager : MonoBehaviour
{
    // Adjustable from Inspector
    [field: SerializeField] public float playerSpeed { get; private set; }

    // For Player States
    PlayerBaseState currentState;
    public PlayerWalkingState WalkingState = new PlayerWalkingState();
    public PlayerJumpingState JumpingState = new PlayerJumpingState();
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerEndingState EndingState = new PlayerEndingState();

    // For Skill Checks
    [SerializeField] public SkillCheckManager skillCheckManager;
    [SerializeField] public PointsBar pointsBar;
    [SerializeField] public GraspSelector graspSelector;

    // For Menus
    [SerializeField] public EndGameMenu endGameMenu;

    // For Animations
    [SerializeField] private SpriteLibraryAsset[] spriteLibraries;
    [HideInInspector] public Animator playerAnim;
    [SerializeField] private AnimatorController[] animControllers;

    // Hashing Animations for Performance
    [HideInInspector] public int Idle = Animator.StringToHash("Base Layer.idle");
    [HideInInspector] public int Walk = Animator.StringToHash("Base Layer.walk");
    [HideInInspector] public int Jump = Animator.StringToHash("Base Layer.jump");
    [HideInInspector] public int Idle_FaceForward = Animator.StringToHash("Base Layer.idle_faceForward");
    [HideInInspector] public int Idle_ToSnackTime = Animator.StringToHash("Base Layer.idle_toSnackTime");
    [HideInInspector] public int Idle_SnackTime = Animator.StringToHash("Base Layer.idle_snackTime");
    [HideInInspector] public int Idle_FromSnackTime = Animator.StringToHash("Base Layer.idle_fromSnackTime");
    [HideInInspector] public int Idle_ToSleepyTime = Animator.StringToHash("Base Layer.idle_toSleepyTime");
    [HideInInspector] public int Idle_SleepyTime = Animator.StringToHash("Base Layer.idle_sleepyTime");
    [HideInInspector] public int Idle_FromSleepyTime = Animator.StringToHash("Base Layer.idle_fromSleepyTime");
    [HideInInspector] public int gameComplete = Animator.StringToHash("Base Layer.gameComplete");

    // Miscellaneous necessary components/references
    [SerializeField] public CameraMotor cameraMotor;
    public BoxCollider2D playerBoxCollider { get; private set; }
    public Rigidbody2D playerRB { get; private set; }
    public Transform playerLoc { get; private set; }
    public int obstacleLayer { get; private set; }
    public int groundLayer { get; private set; }    


    private void Awake()
    {
        Physics2D.queriesStartInColliders = false;
        playerAnim = this.GetComponent<Animator>();
        playerBoxCollider = this.GetComponent<BoxCollider2D>();
        playerRB = this.GetComponent<Rigidbody2D>();
        playerLoc = this.GetComponent<Transform>();

        obstacleLayer = LayerMask.NameToLayer("Obstacle"); // not a trigger
        groundLayer = LayerMask.NameToLayer("Ground"); // not a trigger
    }


    private void Start()
    {
        SetAnimatorController();
        SetSpriteLibrary();

        // Set Initial State
        currentState = WalkingState;
        currentState.EnterState(this);
    }


    // Main driving function of player
    private void FixedUpdate()
    {
        currentState.UpdateState(this);
    }


    // Handles collisions (colliders w/o 'Is Trigger' selected)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(this, collision);
    }


    // Handles triggers ((colliders with 'Is Trigger' selected))
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        currentState.OnTriggerEnter2D(this, trigger);
    }


    // Handles transitioning between states
    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }


    // Assigns the correct animator controller for chosen character
    private void SetAnimatorController()
    {
        if (PlayerData.PlayerDataRef.characterType == GameLookup.characterTypes.reindeer)
        {
            playerAnim.runtimeAnimatorController = animControllers[0];
            this.GetComponent<SpriteRenderer>().flipX = true;
        } else if (PlayerData.PlayerDataRef.characterType == GameLookup.characterTypes.seal)
            playerAnim.runtimeAnimatorController = animControllers[1];
        else
            Debug.Log("ERROR: No Animation Controller Exists for " + PlayerData.PlayerDataRef.characterType);
    }


    // Assigns the correct sprite library for the chosen character
    // (Not the most glamorous of approaches, but performance better than other approaches)
    private void SetSpriteLibrary()
    {
        int characterInd = -1; // Defaults to an invalid value
        GameLookup.characterTypes characterType = PlayerData.PlayerDataRef.characterType;

        if (characterType == GameLookup.characterTypes.reindeer)
        {
            GameLookup.reindeerTypes reindeerType = PlayerData.PlayerDataRef.reindeerSelection;
            switch(reindeerType)
            {
                case GameLookup.reindeerTypes.brown:
                    characterInd = 0;
                    break;
                case GameLookup.reindeerTypes.grey:
                    characterInd = 1;
                    break;
                case GameLookup.reindeerTypes.ruldolph:
                    characterInd = 2;
                    break;
                case GameLookup.reindeerTypes.white:
                    characterInd = 3;
                    break;
                case GameLookup.reindeerTypes.whiteandbrown:
                    characterInd = 4;
                    break;
            }

        } else if (characterType == GameLookup.characterTypes.seal)
        {
            GameLookup.sealTypes sealType = PlayerData.PlayerDataRef.sealSelection;
            switch (sealType)
            {
                case GameLookup.sealTypes.black:
                    characterInd = 5;
                    break;
                case GameLookup.sealTypes.cream:
                    characterInd = 6;
                    break;
                case GameLookup.sealTypes.grey:
                    characterInd = 7;
                    break;
                case GameLookup.sealTypes.white:
                    characterInd = 8;
                    break;
            }

        } else
        {
            Debug.Log("ERROR: No Sprite Library Exists for this character!");
        }

        this.GetComponent<SpriteLibrary>().spriteLibraryAsset = spriteLibraries[characterInd];
    }

}
