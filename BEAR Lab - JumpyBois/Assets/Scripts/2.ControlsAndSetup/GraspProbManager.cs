using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraspProbManager : MonoBehaviour
{
    [SerializeField] private GameObject[] graspButtons; // Stored in same order of ENum
    [SerializeField] private GameObject[] graspProbFields; // Stored in same order of ENum
    [SerializeField] private GameObject[] probInputFields; // Stored in same order of ENum
    [SerializeField] private GameObject NavNextButton;

    // Which probabilities you have edited; prevents duplicated entries
    private HashSet<int> customizedProbs = new HashSet<int>(); 

    void Start()
    {
        Debug.Log("TODO: DELETE GraspProbManager Script!");
        // Grasps are initially inactive
        foreach (int graspName in PlayerData.PlayerDataRef.activeGrasps)
            graspButtons[graspName].SetActive(true);

        AssignDefaultProbs();
        UpdateUI();
    }


    // Evenly split all probabilities while ensuring they add up to 100
    // Keeps track of probabilities in GlobalStorage.cs
    // Updates UI to reflect these probabilities
    private void AssignDefaultProbs()
    {
        int numGrasps = PlayerData.PlayerDataRef.activeGrasps.Count;
        int probability = 100 / numGrasps;
        int remainder = 100 % numGrasps;
        int tempCounter = 0;

        foreach (int grasp in PlayerData.PlayerDataRef.activeGrasps)
        {
            if (remainder != 0 && tempCounter == numGrasps -1)
                probability = 100 - (probability * tempCounter);

            PlayerData.PlayerDataRef.graspProbs[grasp] = probability;
            tempCounter++;
        }
    }

    public void AssignCustomProbs(int graspName)
    {
        var isNumeric = int.TryParse(probInputFields[graspName].GetComponent<TMP_InputField>().text, out int userInput);

        if (isNumeric && userInput >= 0)
        {
            PlayerData.PlayerDataRef.graspProbs[graspName] = userInput;
            customizedProbs.Add(graspName);
        }
        
        if (!AddsTo100())
        {
            RedistributeProbs();

            // After redistributing the probabilities, we must check the sum again
            if (AddsTo100())
                NavNextButton.SetActive(true);
            else
                NavNextButton.SetActive(false);
        } else
            NavNextButton.SetActive(true);


        UpdateUI();

        // Debug.Log("New Entered Probability: " + userInput);
        // Debug.Log("Number of Elements in Hash Set: " + customizedProbs.Count);
    }

    private void UpdateUI()
    {
        TMP_Text probField;

        foreach (int grasp in PlayerData.PlayerDataRef.activeGrasps)
        {
            if (!customizedProbs.Contains(grasp))
            {
                probField = graspProbFields[grasp].GetComponent<TextMeshProUGUI>();
                probField.text = PlayerData.PlayerDataRef.graspProbs[grasp].ToString();
            }
        }
    }

    // Evenly redistributes remaining default probabilities so they add up to 100
    private void RedistributeProbs()
    {
        int custProbTot = 0;
        int numDefaultGrasps = PlayerData.PlayerDataRef.activeGrasps.Count - customizedProbs.Count;

        if (numDefaultGrasps == 0)
            return;

        foreach (int graspName in PlayerData.PlayerDataRef.activeGrasps)
        {
            if (customizedProbs.Contains(graspName))
                custProbTot += PlayerData.PlayerDataRef.graspProbs[graspName];
        }

        int splitAmount = (100 - custProbTot) / numDefaultGrasps;
        int remainder = (100 - custProbTot) % numDefaultGrasps;

        foreach (int graspName in PlayerData.PlayerDataRef.activeGrasps)
        {
            if (!customizedProbs.Contains(graspName))
            {
                if (splitAmount < 0)
                {
                    PlayerData.PlayerDataRef.graspProbs[graspName] = 0;
                    continue;
                }

                if (numDefaultGrasps == 1)
                    PlayerData.PlayerDataRef.graspProbs[graspName] = splitAmount + remainder;
                else
                {
                    PlayerData.PlayerDataRef.graspProbs[graspName] = splitAmount;
                    numDefaultGrasps--;
                }
            }
        }

    }

    // Verifies all the grasp probabilities add up to 100
    private bool AddsTo100()
    {
        int totalSum = 0;
        foreach (int grasp in PlayerData.PlayerDataRef.activeGrasps)
            totalSum += PlayerData.PlayerDataRef.graspProbs[grasp];

        if (totalSum == 100)
            return true;
        else
            return false;
    }

    // If user enters invalid input, revert the entry
    public void VerifyNumericInput(int graspName)
    {
        var isNumeric = int.TryParse(probInputFields[graspName].GetComponent<TMP_InputField>().text, out int userInput);

        if (!isNumeric || userInput < 0)
            probInputFields[graspName].GetComponent<TMP_InputField>().text = PlayerData.PlayerDataRef.graspProbs[graspName].ToString();
    }
}
