using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsBar : MonoBehaviour
{
    // Percentage of total possible grasps user must get right to move on to next obstacle
    [SerializeField] private float passPercentage;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private SkillCheckBar skillCheckBar;


    [SerializeField] private RectTransform goalIndicator; 
    [SerializeField] private GameObject currPointsObj;

    private RectTransform currPointsBar; // grows in size with amount of correct input
    private float pointsBarLen;

    private float pointStepSize;

    public bool goalReached;
    private float pointGoal;


    private void Awake()
    {
        passPercentage = passPercentage / 100;

        // Set goal indicator at passPercentage of bar length
        pointGoal = passPercentage * this.GetComponent<RectTransform>().sizeDelta.x;
        goalIndicator.anchoredPosition = new Vector2(pointGoal, 0);

        currPointsBar = currPointsObj.GetComponent<RectTransform>();
        pointsBarLen = this.GetComponent<RectTransform>().sizeDelta.x;
    }

    private void OnEnable()
    {
        currPointsBar.sizeDelta = new Vector2(0, 0);

        goalReached = false;
    }

    // Initialization order: Prevents pointStepSize from using old fixedDeltaTime/samplingRate
    private void Start()
    {
        pointStepSize = (Time.fixedDeltaTime / skillCheckBar.userInputTime) * pointsBarLen;
        Debug.Log("pointStepSize: " + pointStepSize);
        Debug.Log("deltaTime: " + Time.fixedDeltaTime);
    }

    public IEnumerator RenderCorrectInput()
    {
        while (inputManager.recievingInput && inputManager.enableInput)
        {

            // Grow size of bar
            if (currPointsBar.sizeDelta.x < pointsBarLen)
                currPointsBar.sizeDelta += new Vector2(pointStepSize, 0);

            if (currPointsBar.sizeDelta.x > pointGoal)
                goalReached = true;

            yield return new WaitForFixedUpdate();

        }
    }
}
