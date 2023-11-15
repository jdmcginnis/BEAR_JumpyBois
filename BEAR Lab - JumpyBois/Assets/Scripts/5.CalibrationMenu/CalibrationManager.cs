using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// TODO: Maybe have something where you have an amplitude bar serving as visual feedback
// so they go back to resting instead of flexing outwards

public class CalibrationManager : MonoBehaviour
{
    public static CalibrationManager Instance;
    public GameLookup.graspNamesEnum currentGrasp;

    [System.Serializable]
    public class UserCalibrationValues
    {
        public float graspHoldDuration;
        public int repeatQty;
    }

    [SerializeField] private UserCalibrationValues adjustableCalibrationValues = new UserCalibrationValues();
    
    [SerializeField] private Image image1;
    [SerializeField] private Image image2;
    [SerializeField] private Image image3;
    [SerializeField] private Image imageStart;
    [SerializeField] private Image imageRestText;
    [SerializeField] private Sprite spriteRest;
    [SerializeField] private GameObject graspImageObj;
    [SerializeField] private Sprite[] graspImagesArray;
    [SerializeField] private TextMeshProUGUI graspText;
    [SerializeField] private GameObject progressBarObj;

    private IDictionary<GameLookup.graspNamesEnum, int> remainingGraspsCount; // graspEnum : numRemaining
    private List<GameLookup.graspNamesEnum> remainingGraspsList;

    [SerializeField] private SignalRecording signalRecording;

    private void Awake()
    {
        remainingGraspsCount = new Dictionary<GameLookup.graspNamesEnum, int>();
        remainingGraspsList = new List<GameLookup.graspNamesEnum>();

        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(SetupGraspSelection());
    }

    private IEnumerator SetupGraspSelection()
    {
        foreach (GameLookup.graspNamesEnum grasp in PlayerData.PlayerDataRef.activeGrasps)
        {
            remainingGraspsCount.Add(grasp, adjustableCalibrationValues.repeatQty);
            remainingGraspsList.Add(grasp);
        }
        yield return null;
    }

    // Acts as entry point for the calibration process; activated by clicking button
    public void ExecuteCalibration(GameObject button)
    {
        progressBarObj.SetActive(true);
        button.SetActive(false);

        StartCoroutine(CycleThroughGrasps());  
    }


    // Loops through each grasp *repeatQty* number of times
    private IEnumerator CycleThroughGrasps()
    {
        signalRecording.enabled = true;

        while (remainingGraspsList.Count > 0)
        {
            currentGrasp = FetchRandomGrasp();
            yield return StartCoroutine(LoadGrasp(currentGrasp));
            remainingGraspsCount[currentGrasp] -= 1;

            if (remainingGraspsCount[currentGrasp] <= 0)
                remainingGraspsList.Remove(currentGrasp);
        }
    }


    // Returns a random grasp from the list of remaining active grasps
    private GameLookup.graspNamesEnum FetchRandomGrasp()
    {
        int randNum = Random.Range(0, remainingGraspsList.Count);
        Debug.Log(randNum);
        return remainingGraspsList[randNum];
    }


    // Goes through the calibration process for the provided grasp
    private IEnumerator LoadGrasp(GameLookup.graspNamesEnum grasp)
    {
        // TODO: Set up an array of images!

        graspImageObj.GetComponent<Image>().sprite = graspImagesArray[(int)grasp];
        graspText.SetText(grasp.ToString());
        graspImageObj.SetActive(true);
        progressBarObj.GetComponent<Slider>().value = 0;

        // TODO: Implement a Loop using array of images!
        StartCoroutine(FadeImage(true, image3));
        yield return new WaitForSecondsRealtime(1);
        StartCoroutine(FadeImage(false, image3));

        StartCoroutine(FadeImage(true, image2));
        yield return new WaitForSecondsRealtime(1);
        StartCoroutine(FadeImage(false, image2));

        StartCoroutine(FadeImage(true, image1));
        yield return new WaitForSecondsRealtime(1);
        StartCoroutine(FadeImage(false, image1));

        Debug.Log("Start! Grasp Type: " + grasp.ToString());
        StartCoroutine(FadeImage(true, imageStart));
        StartCoroutine(RenderProgressBar());
        yield return new WaitForSecondsRealtime(3);
        StartCoroutine(FadeImage(false, imageStart));


        StartCoroutine(FadeImage(true, imageRestText));
        graspImageObj.GetComponent<Image>().sprite = spriteRest;
        graspText.SetText("Rest");
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(FadeImage(false, imageRestText));

        graspImageObj.SetActive(false);
    }


    // Smoothing the graphical transitions; purely visual
    private IEnumerator FadeImage(bool fadingIn, Image image)
    {

        float duration = 0.5f;
        Color startColor = image.color;
        Color endColor = image.color;

        if (fadingIn)
        {
            startColor.a = 0;
            endColor.a = 1;
        } else
        {
            startColor.a = 1;
            endColor.a = 0;
        }
        

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            image.color = Color.Lerp(startColor, endColor, normalizedTime);
        }

        image.color = endColor;
        yield return null;
    }


    // Progress Bar reflects how long we want user to hold the current grasp for
    private IEnumerator RenderProgressBar()
    {
        float deltaTime = 0;
        Slider progressBar = progressBarObj.GetComponent<Slider>();

        while (deltaTime < adjustableCalibrationValues.graspHoldDuration)
        {
            deltaTime += Time.unscaledDeltaTime;
            progressBar.value = deltaTime;
            yield return new WaitForEndOfFrame();
        }

        progressBar.value = adjustableCalibrationValues.graspHoldDuration;
    }

}
