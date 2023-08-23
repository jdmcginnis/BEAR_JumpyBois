using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Ensure SkillCheckContainer is initially disabled or the stepSize will be wrong
    // (the wrong fixeddeltatime value will be used)
public class SkillCheckBar : MonoBehaviour
{

    // SkillCheck total time = userPrepTime + userInputTime
    [SerializeField] public float userPrepTime;
    [SerializeField] public float userInputTime;
    private float skillBarSize;
    public float stepSize; 
    private float barPrepRegion; // length of prep portion of skillBar

    private Color32 invalidColor = new Color32(241, 166, 162, 255);
    private Color32 validColor = new Color32(84, 130, 53, 255);

    [SerializeField] private GameObject currentIndObj;
    [SerializeField] private RectTransform inputRegionBar;

    private RectTransform indicatorPos;
    private Image indicatorImg;

    [SerializeField] private InputManager inputManager; // for rendering correct/incorrect input

    private bool inputEnabled;

    private void Awake()
    {

        indicatorPos = currentIndObj.GetComponent<RectTransform>();
        indicatorImg = currentIndObj.GetComponent<Image>();

        skillBarSize = this.GetComponent<RectTransform>().sizeDelta.x;
        

        barPrepRegion = (skillBarSize / (userPrepTime + userInputTime)) * userPrepTime;

        inputRegionBar.sizeDelta = new Vector2(skillBarSize - barPrepRegion, 0);
    }

    private void OnEnable()
    {
        indicatorPos.anchoredPosition = new Vector3(1.5f, 0, 1);
        inputEnabled = false;

    }

    private void Start()
    {
        stepSize = (Time.fixedDeltaTime / (userPrepTime + userInputTime)) * skillBarSize;
    }

    private void FixedUpdate()
    {
        UpdateSkillCheckBar();
    }

    private void UpdateSkillCheckBar()
    {
        

        if (indicatorPos.anchoredPosition.x < skillBarSize)
        {
            indicatorPos.localPosition += new Vector3(stepSize, 0, 0);
        }

        if (!inputEnabled && indicatorPos.anchoredPosition.x < barPrepRegion)
        {
            indicatorImg.color = invalidColor;
            
        } else if (!inputEnabled)
        {
            indicatorImg.color = validColor;
            inputManager.enableInput = true;
            inputEnabled = true;
        }

        
    }



}
