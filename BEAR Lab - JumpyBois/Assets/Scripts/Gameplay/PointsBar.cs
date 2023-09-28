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
    // [SerializeField] private GameObject currPointsObj;

    //private RectTransform currPointsBar; // grows in size with amount of correct input
    private float pointGoal;

    private float pointStepSize;

    public bool goalReached;

    private Slider slider;

    public int numCalls = 0;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        passPercentage = passPercentage / 100;
        slider.maxValue = this.GetComponent<RectTransform>().sizeDelta.x;
        pointGoal = passPercentage * (slider.maxValue);
        goalIndicator.anchoredPosition = new Vector2(pointGoal, 0);

        //currPointsBar = currPointsObj.GetComponent<RectTransform>();
        
    }

    private void OnEnable()
    {
        slider.value = 0;
        //currPointsBar.sizeDelta = new Vector2(0, 0);

        goalReached = false;
    }

    // Initialization order: Prevents pointStepSize from using old fixedDeltaTime/samplingRate
    private void Start()
    {
        pointStepSize = (Time.fixedDeltaTime / skillCheckBar.userInputTime) * slider.maxValue;

        Debug.Log("Time: " + Time.fixedDeltaTime);
        Debug.Log("User Input Time: " + skillCheckBar.userInputTime);
        Debug.Log("Max Value: " + slider.maxValue);
        Debug.Log("Point Step Size: " + pointStepSize);

    }

    public IEnumerator RenderCorrectInput()
    {
        if (inputManager.recievingEarlyInput)
        {
            yield return new WaitUntil(inputManager.HandleKeyboardEarlyInput);
        }


        while (inputManager.recievingInput && inputManager.enableInput)
        {

            numCalls++;
            // Grow size of bar
            slider.value += pointStepSize;

            if (slider.value >= pointGoal)
            {
                goalReached = true;
            }

            yield return new WaitForFixedUpdate();
        }

    }




    //public IEnumerator RenderCorrectInput()
    //{
    //    while (inputManager.recievingInput && inputManager.enableInput)
    //    {

    //        // Grow size of bar
    //        if (currPointsBar.sizeDelta.x < pointsBarLen)
    //            currPointsBar.sizeDelta += new Vector2(pointStepSize, 0);

    //        if (currPointsBar.sizeDelta.x > pointGoal)
    //            goalReached = true;

    //        yield return new WaitForFixedUpdate();

    //    }
    //}
}
