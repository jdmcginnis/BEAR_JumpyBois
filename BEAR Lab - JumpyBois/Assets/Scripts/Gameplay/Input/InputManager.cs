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

    public bool recievingInput = false;

    public bool enableInput; // enables player input

    // Order matters; consult GlobalStorage graspNamesEnum variable
    public string[] graspToKeybind; // Assigns each grasp to a keyboard key input

    [SerializeField] private SkillCheckBar skillCheckBar;
    [SerializeField] private PointsBar pointsBar;

    [SerializeField] private InputActionReference gameKeyboardInput;

    private void Awake()
    {
        // Ensures game and input runs according to our samplingRate
        Time.fixedDeltaTime = (float)1 / (float)samplingRate;

        enableInput = false;

    }

    // Handles Keyboard Input Only
    public void OnGraspInput(InputAction.CallbackContext context)
    {
        if (enableInput == true)
        {
            if (context.performed)
            {
                Debug.Log("Starting Action...");
                recievingInput = true;
                StartCoroutine(pointsBar.RenderCorrectInput());
                // StartCoroutine(LogDataCoroutine());
            } else if (context.canceled)
            {
                Debug.Log("Ending Action...");
                recievingInput = false;
            }
        }

    }

    // Handles Delsys Input Only
    public void OnDelsysInput(int input)
    {
        // StartCoroutine(LogDataCoroutine());
        if (enableInput == true)
        {
            if (input == 1) // TODO: Change '1' to randGraspNum
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
        Debug.Log("Skip This Skillcheck!");
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

    public void ChangeKeyBinding(int graspNum)
    {
        Debug.Log(graspToKeybind[graspNum]);

        gameKeyboardInput.action.Disable();
        gameKeyboardInput.action.ApplyBindingOverride(0, graspToKeybind[graspNum]);
        gameKeyboardInput.action.Enable();

    }

}
