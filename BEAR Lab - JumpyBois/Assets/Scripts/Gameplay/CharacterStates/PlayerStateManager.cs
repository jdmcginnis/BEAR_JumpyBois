using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;



public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;
    public PlayerWalkingState WalkingState = new PlayerWalkingState();
    public PlayerJumpingState JumpingState = new PlayerJumpingState();
    public PlayerIdleState IdleState = new PlayerIdleState();

    public float playerSpeed;

    public GameObject skillCheckObj;
    [SerializeField] private SkillCheckBar skillCheckBar;
    public float skillcheckTotTime { get; private set; }

    public BoxCollider2D playerBoxCollider { get; private set; }
    public Rigidbody2D playerRB { get; private set; }
    public Transform playerLoc { get; private set; }
    public float obstacleLayer { get; private set; }
    public float groundLayer { get; private set; }

    [SerializeField] public PointsBar pointsBar;

    // For Animations
    [SerializeField] private SpriteLibraryAsset[] spriteLibraries;
    [HideInInspector] public Animator playerAnim;

    public InputManager inputManager;

    // Hashing for Performance
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

    // References indices of each sprite library inside spriteLibraries
    private enum SpriteLibraries
    {
        reindeer_brown, // 0
        reindeer_grey // 1
    }

    private void Start()
    {
        // Animator & Animation Reskinning
        playerAnim = this.GetComponent<Animator>();
        int charIndTemp = (int)System.Enum.Parse(typeof(SpriteLibraries), GlobalStorage.GameSettings.characterSelection);
        this.GetComponent<SpriteLibrary>().spriteLibraryAsset = spriteLibraries[charIndTemp];

        // Starting state
        currentState = WalkingState;
        currentState.EnterState(this);

        playerBoxCollider = this.GetComponent<BoxCollider2D>();
        Physics2D.queriesStartInColliders = false;

        skillcheckTotTime = skillCheckBar.userPrepTime + skillCheckBar.userInputTime;

        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        groundLayer = LayerMask.NameToLayer("Ground");

        playerRB = this.GetComponent<Rigidbody2D>();
        playerLoc = this.GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        currentState.UpdateState(this);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(this, collision);
    }

    // 
    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

}
