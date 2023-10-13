using System.Collections;
using System.Collections.Generic;
using data;
using UnityEngine;
using UnityEngine.InputSystem;

// Handles all of the user input with an adjustable samplingRate
// Informs relevant scripts of user input as needed
public class InputManager : MonoBehaviour
{
    [SerializeField] private int samplingRate; // samples per second

    [SerializeField] private PauseMenu pauseMenu;
    private PlayerInput playerInput;

    public bool canPlayerSkip = false;

    // Defaults back to rest if not receiving any input
    public GameLookup.graspNamesEnum graspPrediction { get; private set; } = GameLookup.graspNamesEnum.Rest;

    // Handling Input Events
    private void OnEnable()
    {
        if (playerInput == null)
        {
            Debug.Log("TODO: Check if we're using delsys or keyboard input, and enable the corresponding controls");
            Debug.Log("TODO: Do this at the very end!");
            playerInput = new PlayerInput();
            playerInput.KeyboardInput_Universal.PauseGame.performed += i => pauseMenu.Pause();


            // Keyboard & Delsys Specific Bindings
            // if (PlayerData.PlayerDataRef.usingDelsys)
            // Debug.Log("TODO: Add Delsys Specific Bindings!");

            // Consult GameLookup -> graspNamesEnum to see what grasps these are binding to
            playerInput.KeyboardInput_Grasps.GraspInput_Key0.performed += i => graspPrediction = (GameLookup.graspNamesEnum)0;
            playerInput.KeyboardInput_Grasps.GraspInput_Key0.canceled += i => graspPrediction = GameLookup.graspNamesEnum.Rest;

            playerInput.KeyboardInput_Grasps.GraspInput_Key1.performed += i => graspPrediction = (GameLookup.graspNamesEnum)1;
            playerInput.KeyboardInput_Grasps.GraspInput_Key1.canceled += i => graspPrediction = GameLookup.graspNamesEnum.Rest;

            playerInput.KeyboardInput_Grasps.GraspInput_Key2.performed += i => graspPrediction = (GameLookup.graspNamesEnum)2;
            playerInput.KeyboardInput_Grasps.GraspInput_Key2.canceled += i => graspPrediction = GameLookup.graspNamesEnum.Rest;

            playerInput.KeyboardInput_Grasps.GraspInput_Key3.performed += i => graspPrediction = (GameLookup.graspNamesEnum)3;
            playerInput.KeyboardInput_Grasps.GraspInput_Key3.canceled += i => graspPrediction = GameLookup.graspNamesEnum.Rest;

            playerInput.KeyboardInput_Grasps.GraspInput_Key4.performed += i => graspPrediction = (GameLookup.graspNamesEnum)4;
            playerInput.KeyboardInput_Grasps.GraspInput_Key4.canceled += i => graspPrediction = GameLookup.graspNamesEnum.Rest;

            playerInput.KeyboardInput_Grasps.GraspInput_Key5.performed += i => graspPrediction = (GameLookup.graspNamesEnum)5;
            playerInput.KeyboardInput_Grasps.GraspInput_Key5.canceled += i => graspPrediction = GameLookup.graspNamesEnum.Rest;

            playerInput.KeyboardInput_Grasps.GraspInput_Key6.performed += i => graspPrediction = (GameLookup.graspNamesEnum)6;
            playerInput.KeyboardInput_Grasps.GraspInput_Key6.canceled += i => graspPrediction = GameLookup.graspNamesEnum.Rest;

            playerInput.KeyboardInput_Grasps.GraspInput_Key7.performed += i => graspPrediction = (GameLookup.graspNamesEnum)7;
            playerInput.KeyboardInput_Grasps.GraspInput_Key7.canceled += i => graspPrediction = GameLookup.graspNamesEnum.Rest;

            playerInput.KeyboardInput_Grasps.GraspInput_Key8.performed += i => graspPrediction = (GameLookup.graspNamesEnum)8;
            playerInput.KeyboardInput_Grasps.GraspInput_Key8.canceled += i => graspPrediction = GameLookup.graspNamesEnum.Rest;

            playerInput.KeyboardInput_Grasps.GraspInput_Key9.performed += i => graspPrediction = (GameLookup.graspNamesEnum)9;
            playerInput.KeyboardInput_Grasps.GraspInput_Key9.canceled += i => graspPrediction = GameLookup.graspNamesEnum.Rest;
        }

        playerInput.Enable();
    }

    private void FixedUpdate()
    {
        // Debug.Log("Key being pressed: " + keyBeingPressed.ToString());
    }

    // Handling Input Events
    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Awake()
    {
        // Sets our input rate to our sampling rate
        Time.fixedDeltaTime = 1f / (float)samplingRate;
    }

    private void Start()
    {
        Debug.Log("TODO: Put Getters and Setters On Public Variables!");
    }


























    //// Order matters; consult GlobalStorage graspNamesEnum variable
    //public string[] graspToKeybind; // Assigns each grasp to a keyboard key input

    //[SerializeField] private PointsBar pointsBar;
    //[SerializeField] private GraspSelector graspSelector;
    //[SerializeField] private PlayerStateManager playerStateManager;

    //[SerializeField] private InputActionReference gameKeyboardInput;


    // private void Awake()
    //{  

    //    if (PlayerData.PlayerDataRef.usingDelsys)
    //        DisableGraspKeyboardInput();

    //    enableInput = false;
    //    recievingEarlyInput = false;


    //}

    //// Handles Keyboard Input Only
    //public void OnGraspInput(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //    {
    //        // for fixing a bug where game won't register input if key is pressed earlier than expected
    //        if (enableInput == false)
    //            recievingEarlyInput = true;

    //        recievingInput = true;
    //        StartCoroutine(pointsBar.RenderCorrectInput());
    //    } else if (context.canceled)
    //    {
    //        recievingInput = false;
    //    }

    //}

    //// Handles Delsys Input Only
    //public void OnDelsysInput(int input)
    //{
    //    // StartCoroutine(LogDataCoroutine());
    //    if (enableInput == true)
    //    {
    //        if (input == (int)graspSelector.randGrasp)
    //        {
    //            Debug.Log("Starting Action...");
    //            recievingInput = true;
    //            StartCoroutine(pointsBar.RenderCorrectInput());
    //            // StartCoroutine(LogDataCoroutine());
    //        } else
    //        {
    //            recievingInput = false;
    //        }
    //    }
    //}

    //// Handles Both Deslsys & Keyboard Input Profiles
    //public void OnSkip(InputAction.CallbackContext value)
    //{
    //    if (canPlayerSkip)
    //    {
    //        playerDidSkip = true;
    //        playerStateManager.SwitchState(playerStateManager.JumpingState);
    //    }
    //}

    //IEnumerator LogDataCoroutine(DataPoint dataPoint)
    //{
    //    while (recievingInput && enableInput)
    //    {
    //        LogData(ref dataPoint);
    //        yield return new WaitForFixedUpdate();
    //    }
    //}

    //private void LogData(ref DataPoint dataPoint)
    //{

    //    // Log data here
    //    Debug.Log("Data Logged!");
    //}

    //private void DisableGraspKeyboardInput()
    //{
    //    gameKeyboardInput.action.Disable();

    //    // For Debugging!

    //}


}
