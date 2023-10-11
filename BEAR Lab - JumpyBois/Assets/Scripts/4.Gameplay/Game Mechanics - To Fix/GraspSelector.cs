using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using Random = System.Random;

public class GraspSelector : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Image graspImg;

    // Order In Array Matters!
    // IndexFlexion = 0
    // Key = 1
    // Pinch = 2
    // Point = 3
    // Power = 4
    // Tripod = 5
    // WristExtension = 6
    // WristFlexion = 7;
    // WristRotation = 8;
    // WristRotationAndPower = 9;
    [SerializeField] private Sprite[] delsysGraspCheck;
    [SerializeField] private Sprite[] delsysGraspPass;
    [SerializeField] private Sprite[] keyboardGraspCheck;
    [SerializeField] private Sprite[] keyboardGraspPass;

    // private Random rnd = new Random();
    public int[] accumulatedGraspProbs; // for fetching grasps with probability P

    public GameLookup.graspNamesEnum randGrasp;


    private void Awake()
    {
        accumulatedGraspProbs = new int[10]; // 10 grasps; order matters
    }


    private void Start()
    {
        int tempAccum = 0;

        foreach (GameLookup.graspNamesEnum graspName in PlayerData.PlayerDataRef.activeGrasps)
        {
            tempAccum += PlayerData.PlayerDataRef.graspProbs[(int)graspName];
            accumulatedGraspProbs[(int)graspName] = tempAccum;
        }


    }

    public IEnumerator LoadNextGrasp()
    {
        yield return new WaitForEndOfFrame(); // corrects initialization order (so this doesn't execute before Start() function
        randGrasp = GetRandomGrasp();
        LoadNextGraspImage();

        if (PlayerData.PlayerDataRef.usingDelsys == false)
            inputManager.ChangeKeyBinding((int)randGrasp);

        yield return null;
    }

    public void MarkGraspCompleted()
    {
        if (!inputManager.playerDidSkip)
        {
            if (PlayerData.PlayerDataRef.usingDelsys == true)
            {
                graspImg.sprite = delsysGraspPass[(int)randGrasp];
            } else
            {
                graspImg.sprite = keyboardGraspPass[(int)randGrasp];
            }
        }
    }

    private void LoadNextGraspImage()
    {
        if (PlayerData.PlayerDataRef.usingDelsys == true)
        {
            graspImg.sprite = delsysGraspCheck[(int)randGrasp];
        } else
        {
            graspImg.sprite = keyboardGraspCheck[(int)randGrasp];
        }

    }

    private GameLookup.graspNamesEnum GetRandomGrasp()
    {
        // int randNum = rnd.Next(1, 101); // upper bound is exclusive (didn't seem to be too random)
        int randNum = Random.Range(1, 100); // both bounds are inclusive

        // Debug.Log("RANDOM NUMBER: " + randNum);

        int currentLowest = 100;
        GameLookup.graspNamesEnum graspName = GameLookup.graspNamesEnum.IndexFlexion; // setting a default value

        for (int i = 0; i < 10; i++)
        {
            if (accumulatedGraspProbs[i] != 0 && randNum <= accumulatedGraspProbs[i])
            {
                if (accumulatedGraspProbs[i] <= currentLowest)
                {
                    currentLowest = accumulatedGraspProbs[i];
                    graspName = (GameLookup.graspNamesEnum)i;
                }
            }
        }

        return graspName;
    }


}
