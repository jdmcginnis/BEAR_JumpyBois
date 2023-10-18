using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraspSelector : MonoBehaviour
{
    [SerializeField] private PointsBar pointsBar;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Image graspImg;

    // Holds array of images/sprites to display for each grasp
    // Order In Array Matters! See GameLookup -> graspNamesEnum
    [SerializeField] private Sprite[] delsysGraspCheck;
    [SerializeField] private Sprite[] delsysGraspPass;
    [SerializeField] private Sprite[] keyboardGraspCheck;
    [SerializeField] private Sprite[] keyboardGraspPass;

    // Data containers allowing us to show each grasp a specified number of times
    private IDictionary<GameLookup.graspNamesEnum, int> remainingGraspsCount; // graspEnum : numRemaining
    private List<GameLookup.graspNamesEnum> remainingGraspsList;

    public GameLookup.graspNamesEnum currentGraspForTrial { get; private set; }


    private void Awake()
    {
        remainingGraspsList = new List<GameLookup.graspNamesEnum>();
        remainingGraspsCount = PlayerData.PlayerDataRef.numTestsPerGrasp;
        StartCoroutine(SetupGraspsForGame());
        currentGraspForTrial = GameLookup.graspNamesEnum.Rest; // Initially a resting state
    }


    // Populates remainingGraspsList
    private IEnumerator SetupGraspsForGame()
    {
        foreach (GameLookup.graspNamesEnum grasp in PlayerData.PlayerDataRef.activeGrasps)
        {
            remainingGraspsList.Add(grasp);
        }

        yield return null;
    }


    // Chooses a random grasp, decrements the count, and loads the image
    // Called from PlayerWalkingState
    public void LoadNextGrasp()
    {
        currentGraspForTrial = ChooseRandomGrasp();
        remainingGraspsCount[currentGraspForTrial] -= 1;
        LoadNextGraspImage();
    }


    // Returns a randomly selected grasp from the list of remaining ones
    private GameLookup.graspNamesEnum ChooseRandomGrasp()
    {
        int randNum = Random.Range(0, remainingGraspsList.Count);
        return remainingGraspsList[randNum];
    }


    // Handles loading the correct image for the type of grasp and input medium
    private void LoadNextGraspImage()
    {
        if (PlayerData.PlayerDataRef.usingDelsys == true)
        {
            graspImg.sprite = delsysGraspCheck[(int)currentGraspForTrial];
        } else
        {
            graspImg.sprite = keyboardGraspCheck[(int)currentGraspForTrial];
        }

    }


    // Handles loading the correct completed image for the type of grasp and input medium
    public void MarkGraspCompletedImage()
    {
        if (pointsBar.goalReached)
        {
            if (PlayerData.PlayerDataRef.usingDelsys == true)
            {
                graspImg.sprite = delsysGraspPass[(int)currentGraspForTrial];
            } else
            {
                graspImg.sprite = keyboardGraspPass[(int)currentGraspForTrial];
            }
        }

        // Once we complete the trial, reset this variable back to rest
        currentGraspForTrial = GameLookup.graspNamesEnum.Rest;
    }


}
