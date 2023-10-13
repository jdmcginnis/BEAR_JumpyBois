using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages progression of the ProgressBar
// Attached to SkillCheckContainer/ProgressBar
public class ProgressBar : MonoBehaviour
{
    [SerializeField] private SkillCheckManager skillCheckManager;

    // For Small Sliding Bar
    [SerializeField] private GameObject indicatorObj;
    private RectTransform indicatorPos;
    private Image indicatorImg;
    private bool indicatorAtEndOfBar;
    private float stepSize; // how much to increment sliding bar every frame
    private Color32 invalidColor = new Color32(241, 166, 162, 255);
    private Color32 validColor = new Color32(84, 130, 53, 255);

    // For Progress Bar
    [SerializeField] private RectTransform inputRegionBar;
    private float barTotalLength; // length of progress bar
    private float barPrepLength; // length of preparation region of progress bar
    

    // Since this object will be enabled/disabled repeatedly, so have all initialization in
    // Awake() instead of Start() due to initialization order
    private void Awake()
    {
        indicatorPos = indicatorObj.GetComponent<RectTransform>();
        indicatorImg = indicatorObj.GetComponent<Image>();

        barTotalLength = this.GetComponent<RectTransform>().sizeDelta.x;
        barPrepLength = (barTotalLength / skillCheckManager.skillCheckTotTime) * skillCheckManager.userPrepTime;
        stepSize = (Time.fixedDeltaTime / (skillCheckManager.skillCheckTotTime)) * barTotalLength;

        // Visual communication to user where input is accepted
        inputRegionBar.sizeDelta = new Vector2(barTotalLength - barPrepLength, 0);
    }


    // Reset Position of Small Sliding Bar
    private void OnEnable()
    {
        indicatorPos.anchoredPosition = new Vector3(1.5f, 0, 1);
        indicatorAtEndOfBar = false;
        StartCoroutine(UpdateProgressBar());
    }

    // Handles updating the appearance of the progress bar
    private IEnumerator UpdateProgressBar()
    {
        while (!indicatorAtEndOfBar)
        {
            if (indicatorPos.anchoredPosition.x < barTotalLength)
                indicatorPos.localPosition += new Vector3(stepSize, 0, 0);
            else
                indicatorAtEndOfBar = true;

            if (!skillCheckManager.acceptingInput && indicatorPos.anchoredPosition.x < barPrepLength)
                indicatorImg.color = invalidColor;
            else if (!skillCheckManager.acceptingInput)
            {
                indicatorImg.color = validColor;
                skillCheckManager.acceptingInput = true;
            }

            yield return new WaitForFixedUpdate();
        }

    }
}