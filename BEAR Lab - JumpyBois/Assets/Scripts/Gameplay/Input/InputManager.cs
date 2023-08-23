using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Player Input is sampled at same rate as FixedUpdate()
public class InputManager : MonoBehaviour
{
    [SerializeField] private int samplingRate; // samples per second

    public int sampleTest = 0;

    public bool recievingInput = false;

    public bool enableInput; // enables player input
    

    [SerializeField] private SkillCheckBar skillCheckBar;
    [SerializeField] private PointsBar pointsBar;

    private void Awake()
    {
        // Ensures game and input runs according to our samplingRate
        Time.fixedDeltaTime = (float)1 / (float)samplingRate;

        enableInput = false;
    }


    public void OnGraspInput(InputAction.CallbackContext context)
    {
        if (enableInput == true)
        {
            if (context.performed)
            {
                Debug.Log("Starting Action...");
                recievingInput = true;
                StartCoroutine(pointsBar.RenderCorrectInput());
                StartCoroutine(LogDataCoroutine());
            } else if (context.canceled)
            {
                Debug.Log("Ending Action...");
                recievingInput = false;
            }
        }

    }
    
    public void OnSkip(InputAction.CallbackContext value)
    {
        Debug.Log("Skip This Skillcheck!");
    }

    IEnumerator LogDataCoroutine()
    {
        while (recievingInput && enableInput)
        {
            LogData();
            yield return new WaitForFixedUpdate();
        }
    }
    

    private void LogData()
    {
        // Log data here
    }

}
