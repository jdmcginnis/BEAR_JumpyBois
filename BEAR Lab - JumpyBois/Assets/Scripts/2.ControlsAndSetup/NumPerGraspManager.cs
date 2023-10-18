using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumPerGraspManager : MonoBehaviour
{
    [SerializeField] private GameObject[] graspButtons; // Stored in same order of enum
    [SerializeField] private GameObject[] numTestPlaceholders; // Stored in same order of enum
    [SerializeField] private GameObject[] numTestInputFields; // Stored in same order of enum

    [SerializeField] private GameObject numTestsField;

    private bool initialRun = true; // Prevents duplicate enums from being added to numTestsPerGrasp dictionary

    // Which fields you have edited; prevents duplicated entries
    private HashSet<GameLookup.graspNamesEnum> customNumTests = new HashSet<GameLookup.graspNamesEnum>();

    private void Start()
    {
        // All Grasp Buttons are initially inactive; activate the selected ones
        foreach (int graspName in PlayerData.PlayerDataRef.activeGrasps)
            graspButtons[graspName].SetActive(true);

        AssignDefaultTestsPerGrasp();
    }


    // Occurs upon 'On Value Changed (String)'
    public void AssignCustomNumTest(int graspName)
    {
        var isNumeric = int.TryParse(numTestInputFields[graspName].GetComponent<TMP_InputField>().text, out int userInput);

        if (isNumeric && userInput >= 0)
        {
            PlayerData.PlayerDataRef.numTestsPerGrasp[(GameLookup.graspNamesEnum)graspName] = userInput;
            customNumTests.Add((GameLookup.graspNamesEnum)graspName);
        }
    }

    // If user enters invalid input, revert the entry
    // Occurs upon 'On End Edit (String)'
    public void VerifyNumericInput(int graspName)
    {
        var isNumeric = int.TryParse(numTestInputFields[graspName].GetComponent<TMP_InputField>().text, out int userInput);

        if (!isNumeric || userInput < 0)
            numTestInputFields[graspName].GetComponent<TMP_InputField>().text = PlayerData.PlayerDataRef.numTestsPerGrasp[(GameLookup.graspNamesEnum)graspName].ToString();
    }

    // Occurs upon 'On End Edit (String)'
    // If the sum of the custom grasp trials exceed what is contained in the
    // ..'Number of Tests to Administer' field, then replace it with the sum of custom grasp trials
    public void HandleNewInput()
    {
        // only if all the default probs have been edited
        if (customNumTests.Count == PlayerData.PlayerDataRef.activeGrasps.Count)
            SetNumTestsToSumOfTests();
        else
            RedistributeNumTests();
    }

    public void AssignDefaultTestsPerGrasp()
    {
        int numGrasps = PlayerData.PlayerDataRef.activeGrasps.Count;
        int numTestPerGrasp = PlayerData.PlayerDataRef.numTests / numGrasps;
        int remainder = PlayerData.PlayerDataRef.numTests % numGrasps;

        Debug.Log("We are here");

        AddRedistributionToDictionary(numGrasps, numTestPerGrasp, remainder, true);
        UpdateUI();
    }


    // Refreshes the UI to reflect the numTestsPerGrasp contained in PlayerData.cs
    private void UpdateUI()
    {
        TMP_Text numTestField; // Placeholder component

        foreach (GameLookup.graspNamesEnum grasp in PlayerData.PlayerDataRef.activeGrasps)
        {
            if (!customNumTests.Contains(grasp))
            {
                // delete any previously existing entry and put the default number in the placeholder
                numTestInputFields[(int)grasp].GetComponent<TMP_InputField>().text = "";

                numTestField = numTestPlaceholders[(int)grasp].GetComponent<TextMeshProUGUI>();
                numTestField.text = PlayerData.PlayerDataRef.numTestsPerGrasp[grasp].ToString();
            }
        }
    }

    // Distributes total remaining tests/trials among the ones with the default number of trials value
    private void RedistributeNumTests()
    {
        int numDefaultsGraspsRem = PlayerData.PlayerDataRef.activeGrasps.Count - customNumTests.Count;
        int nonDefaultSum = 0;

        foreach (GameLookup.graspNamesEnum grasp in customNumTests)
        {
            nonDefaultSum += PlayerData.PlayerDataRef.numTestsPerGrasp[grasp];
        }

        int numSplitAmongDefault = PlayerData.PlayerDataRef.numTests - nonDefaultSum;

        if (numSplitAmongDefault < 0)
        {
            SetNumTestsToSumOfTests();
            numSplitAmongDefault = PlayerData.PlayerDataRef.numTests - nonDefaultSum;
        }

        int numTestPerGrasp = numSplitAmongDefault / numDefaultsGraspsRem;
        int remainder = numSplitAmongDefault % numDefaultsGraspsRem;

        AddRedistributionToDictionary(numDefaultsGraspsRem, numTestPerGrasp, remainder, false);

        UpdateUI();
    }

    private void SetNumTestsToSumOfTests()
    {
        int sumTrials = 0;
        foreach (GameLookup.graspNamesEnum grasp in PlayerData.PlayerDataRef.activeGrasps)
        {
            sumTrials += PlayerData.PlayerDataRef.numTestsPerGrasp[grasp];
        }

        PlayerData.PlayerDataRef.numTests = sumTrials;
        numTestsField.GetComponent<TMP_InputField>().text = sumTrials.ToString();

    }


    // Called whenever an automatic redistribution of grasp trials occurs
    // numGrasps: how many unique active grasps are being written to the dictionary
    // numTestPerGrasp: the minimum amount of tests/trials per grasp
    // remainder: if there are leftover trials after evenly splitting trials among active grasps, handle these
    // assigningDefault: Signals if we need to add a new entry to dictionary or overwrite the current value
    private void AddRedistributionToDictionary(int numGrasps, int numTestPerGrasp, int remainder, bool assigningDefault)
    {

        int tempCounter = 0;
        foreach (GameLookup.graspNamesEnum grasp in PlayerData.PlayerDataRef.activeGrasps)
        {
            // We don't want to overwrite the value for grasps with a custom number of trials entry
            if (customNumTests.Contains(grasp))
                continue;

            // We only want one entry in dictionary; only gets added on the initial run
            if ((remainder != 0) && (tempCounter >= numGrasps - remainder))
            {
                if (initialRun)
                    PlayerData.PlayerDataRef.numTestsPerGrasp.Add(grasp, numTestPerGrasp + 1);
                else
                    PlayerData.PlayerDataRef.numTestsPerGrasp[grasp] = numTestPerGrasp + 1;
            } else
            {
                if (initialRun)
                    PlayerData.PlayerDataRef.numTestsPerGrasp.Add(grasp, numTestPerGrasp);
                else
                    PlayerData.PlayerDataRef.numTestsPerGrasp[grasp] = numTestPerGrasp;
            }


            if (customNumTests.Contains(grasp) && assigningDefault)
                customNumTests.Remove(grasp);

            tempCounter++;
        }
        initialRun = false;

    }

    public void markGraspsDefault()
    {
        foreach (GameLookup.graspNamesEnum grasp in PlayerData.PlayerDataRef.activeGrasps)
            customNumTests.Remove(grasp);
    }
}
