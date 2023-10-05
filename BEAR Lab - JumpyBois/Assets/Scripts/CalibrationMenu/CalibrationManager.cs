using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalibrationManager : MonoBehaviour
{
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

    private IDictionary<GlobalStorage.graspNamesEnum, int> remainingGraspsCount; // graspEnum : numRemaining
    private List<GlobalStorage.graspNamesEnum> remainingGraspsList;

    private void Awake()
    {
        remainingGraspsCount = new Dictionary<GlobalStorage.graspNamesEnum, int>();
        remainingGraspsList = new List<GlobalStorage.graspNamesEnum>();
    }

    private void Start()
    {
        StartCoroutine(SetupGraspSelection());
    }

    // Acts as entry point for the calibration process; activated by clicking button
    public void ExecuteCalibration(GameObject button)
    {
        Debug.Log("Beginning Calibration!");
        progressBarObj.SetActive(true);
        button.SetActive(false);

        StartCoroutine(CycleThroughGrasps());  
    }

    // Returns a random grasp from the list of remaining active grasps
    private GlobalStorage.graspNamesEnum FetchRandomGrasp()
    {
        int randNum = Random.Range(0, remainingGraspsList.Count);
        Debug.Log(randNum);
        return remainingGraspsList[randNum];
    }

    private IEnumerator SetupGraspSelection()
    {
        foreach (GlobalStorage.graspNamesEnum grasp in GlobalStorage.GameSettings.activeGrasps) {
            remainingGraspsCount.Add(grasp, adjustableCalibrationValues.repeatQty);
            remainingGraspsList.Add(grasp);
        }
        yield return null;
    }

    // Loops through each grasp *repeatQty* number of times
    private IEnumerator CycleThroughGrasps()
    {
        while (remainingGraspsList.Count > 0)
        {
            GlobalStorage.graspNamesEnum graspName = FetchRandomGrasp();
            yield return StartCoroutine(LoadGrasp(graspName));
            remainingGraspsCount[graspName] -= 1;

            if (remainingGraspsCount[graspName] <= 0)
                remainingGraspsList.Remove(graspName);
        }
    }
    
    // Goes through the calibration process for the provided grasp
    private IEnumerator LoadGrasp(GlobalStorage.graspNamesEnum grasp)
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



    // Randomly cycle through the active grasps contained in GlobalStorage
    // Change image and text of grasp
    // Prompt user to hold for graspHoldDuration seconds
    // Do each grasp repeatQty times each

    // Store this information in a (temporary?) data file to be later analyzed by
    // the feature extraction script (To Do)

    // Have slider progress bar increment over graspHoldDuration seconds

    // Activate/Deactivate numbers to count down
    // Activate 'Start' image when you want user to start making the grasp
    // Activate 'Stop' image when you want user to stop making the grasp
    // Maybe have something where you have an amplitude bar serving as visual feedback
    // so they go back to resting instead of flexing outwards

}
