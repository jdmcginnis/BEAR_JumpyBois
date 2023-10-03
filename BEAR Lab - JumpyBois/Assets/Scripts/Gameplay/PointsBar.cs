using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PointsBar : MonoBehaviour
{
    // Percentage of total possible grasps user must get right to move on to next obstacle
    [SerializeField] private float passPercentage;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private SkillCheckBar skillCheckBar;
    [SerializeField] private RectTransform goalIndicator; 

    private float pointGoal;

    private float pointStepSize;

    public bool goalReached;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        passPercentage = passPercentage / 100;
        slider.maxValue = this.GetComponent<RectTransform>().sizeDelta.x;
        pointGoal = passPercentage * (slider.maxValue);
        goalIndicator.anchoredPosition = new Vector2(pointGoal, 0);        
    }

    private void OnEnable()
    {
        slider.value = 0;
        goalReached = false;
    }

    // Initialization order: Prevents pointStepSize from using old fixedDeltaTime/samplingRate
    private void Start()
    {
        pointStepSize = (Time.fixedDeltaTime / skillCheckBar.userInputTime) * slider.maxValue;

    }

    public IEnumerator RenderCorrectInput()
    {
        if (inputManager.recievingEarlyInput)
        {
            yield return new WaitUntil(inputManager.HandleKeyboardEarlyInput);
        }

        while (inputManager.recievingInput && inputManager.enableInput)
        {
            // Grow size of bar
            slider.value += pointStepSize;

            if (slider.value >= pointGoal)
            {
                goalReached = true;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
