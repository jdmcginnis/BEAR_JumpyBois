using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages the ProgressBar and PointsBar
// NOTE: Make Sure SkillCheckContainer is Initially Disabled/InActive!
public class SkillCheckManager : MonoBehaviour
{
    // For Reference by ProgressBar and PointsBar
    [field: SerializeField] public float userPrepTime { get; private set; }
    [field: SerializeField] public float userInputTime { get; private set; }
    public float skillCheckTotTime { get; private set; }

    // Manages the ProgressBar and PointsBar objects
    [SerializeField] private GameObject skillCheckContainer;

    // Value changed by ProgressBar and PlayerJumpingState
    // We are acceptingInput if 
    [HideInInspector] public bool acceptingInput;

    

    private void OnEnable()
    {
        acceptingInput = false;
    }

    private void Start()
    {
        skillCheckTotTime = userPrepTime + userInputTime;
    }


    // Called from PlayerStateManager
    public void ShowSkillCheck()
    {
        skillCheckContainer.SetActive(true);
    }


    // Called from PlayerStateManager
    public void HideSkillCheck()
    {
        skillCheckContainer.SetActive(false);
    }
}
