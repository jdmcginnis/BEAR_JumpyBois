using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages progression of the PointsBar
// Attached to SkillCheckContainer/PointsBar
public class PointsBar : MonoBehaviour
{
    // minimum percentage of correct grasp predictions user must get to pass obstacle
    [SerializeField] private float passPercentage; // Value of 1-100
    public bool goalReached { get; private set; } // if player reached the passPercentage
    private float pointsGoal;
    private float stepSize; // how much to increment bar every FixedUpdate frame

    [SerializeField] private InputManager inputManager;
    [SerializeField] private GraspSelector graspSelector;
    [SerializeField] private SkillCheckManager skillCheckManager;
    [SerializeField] private RectTransform goalIndicator;

    private Slider slider; // For visual feedback of points gained for the current trial

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = this.GetComponent<RectTransform>().sizeDelta.x;

        passPercentage = passPercentage / 100;
        pointsGoal = passPercentage * (slider.maxValue);
        goalIndicator.anchoredPosition = new Vector2(pointsGoal, 0);
    }


    private void OnEnable()
    {
        slider.value = 0;
        goalReached = false;
    }


    private void Start()
    {
        stepSize = (Time.fixedDeltaTime / skillCheckManager.userInputTime) * slider.maxValue;
    }

    private void FixedUpdate()
    {
        bool correctPrediction = inputManager.graspPrediction == graspSelector.currentGraspForTrial;

        if (skillCheckManager.acceptingInput & correctPrediction)
        {
            slider.value += stepSize;

            if (slider.value >= pointsGoal)
                goalReached = true;
        }
    }

}
