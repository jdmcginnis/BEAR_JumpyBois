using System.Collections;
using System.Collections.Generic;
using data;
using UnityEngine;
using UnityEngine.InputSystem;

// Player Input is sampled at same rate as FixedUpdate()
public class InputManager : MonoBehaviour
{
    [SerializeField] private int samplingRate; // samples per second

    public int sampleTest = 0;

    public bool recievingInput = false; // if user is actively sending input (Ex: Holding key down)
    public bool recievingEarlyInput = false; 
    public bool canPlayerSkip = false;
    public bool playerDidSkip = false;

    public bool enableInput; // enables player input

    // Order matters; consult GlobalStorage graspNamesEnum variable
    public string[] graspToKeybind; // Assigns each grasp to a keyboard key input

    [SerializeField] private SkillCheckBar skillCheckBar;
    [SerializeField] private PointsBar pointsBar;
    [SerializeField] private GraspSelector graspSelector;
    [SerializeField] private PlayerStateManager playerStateManager;

    [SerializeField] private InputActionReference gameKeyboardInput;


    private void Awake()
    {
        // Ensures game and input runs according to our samplingRate
        Time.fixedDeltaTime = (float)1 / (float)samplingRate;

        if (PlayerData.PlayerDataRef.usingDelsys)
            DisableGraspKeyboardInput();

        enableInput = false;
        recievingEarlyInput = false;

        
    }

    public void OnHIDInput(InputAction.CallbackContext context)
    {

    }

    // Handles Keyboard Input Only
    public void OnGraspInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // for fixing a bug where game won't register input if key is pressed earlier than expected
            if (enableInput == false)
                recievingEarlyInput = true;

            recievingInput = true;
            StartCoroutine(pointsBar.RenderCorrectInput());
        } else if (context.canceled)
        {
            recievingInput = false;
        }

    }

    // Handles Delsys Input Only
    public void OnDelsysInput(int input)
    {
        // StartCoroutine(LogDataCoroutine());
        if (enableInput == true)
        {
            if (input == (int)graspSelector.randGrasp)
            {
                Debug.Log("Starting Action...");
                recievingInput = true;
                StartCoroutine(pointsBar.RenderCorrectInput());
                // StartCoroutine(LogDataCoroutine());
            } else
            {
                recievingInput = false;
            }
        }
    }

    // Handles Both Deslsys & Keyboard Input Profiles
    public void OnSkip(InputAction.CallbackContext value)
    {
        if (canPlayerSkip)
        {
            playerDidSkip = true;
            playerStateManager.SwitchState(playerStateManager.JumpingState);
        }
    }

    IEnumerator LogDataCoroutine(DataPoint dataPoint)
    {
        while (recievingInput && enableInput)
        {
            LogData(ref dataPoint);
            yield return new WaitForFixedUpdate();
        }
    }
    
    private void LogData(ref DataPoint dataPoint)
    {
        
        // Log data here
        Debug.Log("Data Logged!");
    }

    private void DisableGraspKeyboardInput()
    {
        gameKeyboardInput.action.Disable();

        // For Debugging!

    }

    public void ChangeKeyBinding(int graspNum)
    {
        Debug.Log(graspToKeybind[graspNum]);

        gameKeyboardInput.action.Disable();
        gameKeyboardInput.action.ApplyBindingOverride(0, graspToKeybind[graspNum]);
        gameKeyboardInput.action.Enable();

    }

    public bool HandleKeyboardEarlyInput()
    {
        if (recievingEarlyInput && !enableInput)
        {
            // disable grasp key binding to prevent duplicate coroutine calls
            gameKeyboardInput.action.Disable();
            return false;
            
        } else if (enableInput)
        {
            gameKeyboardInput.action.Enable();
            recievingEarlyInput = false;
            return true;
        }

        return false;
    }

}
