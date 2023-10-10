using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager: MonoBehaviour
{

    [SerializeField] private int minGraspTypes; // Adjustable By Administrator
    [SerializeField] private int maxGraspTypes; // Adjustable By Administrator
    [SerializeField] private GameObject NavNextButton;
    [SerializeField] private GameObject NavBackButton;
    [SerializeField] private GameObject[] graspButtons; // Stored in same order of enum
    
    public int numSelGrasps { get; private set; }
    
    private void Awake()
    {
        NavNextButton.SetActive(false);
    }

    private void Start()
    {
        numSelGrasps = 0;

        // Preserves grasp choices if you click back button from ControlsAndSetup
        if (PlayerData.PlayerDataRef.graspsChosen)
        {
            Debug.Log("Test");
            foreach (GameLookup.graspNamesEnum grasp in PlayerData.PlayerDataRef.activeGrasps)
            {
                graspButtons[(int)grasp].GetComponent<Image>().color = new Color32(203, 221, 202, 255);
                numSelGrasps++;
            }
            PlayerData.PlayerDataRef.graspsChosen = false;
        } else
        {
            int totalGrasps = 10;
            for (int i = 0; i < totalGrasps; i++)
            {
                PlayerData.PlayerDataRef.inactiveGrasps.Add((GameLookup.graspNamesEnum)i);
            }
        }

        if (numSelGrasps >= minGraspTypes)
            EnableNextButton();
        else if (numSelGrasps < minGraspTypes - 1)
            DisableNextButton();

    }

    public void OnGraspButtonPress(GameObject button)
    {
        GameLookup.graspNamesEnum graspName = (GameLookup.graspNamesEnum)Enum.Parse(typeof(GameLookup.graspNamesEnum), button.name);

        if (!IsGraspActive(graspName))
        {
            SelectGrasp(button, graspName);
            if (numSelGrasps >= maxGraspTypes)
                DeactivateRemainingGrasps();
        } else
        {
            if (numSelGrasps == maxGraspTypes)
                ActivateRemainingGrasps();

            DeselectGrasp(button, graspName);
        }

        if (numSelGrasps == minGraspTypes)
            EnableNextButton();
        else if (numSelGrasps == minGraspTypes - 1)
            DisableNextButton();
    }

    private bool IsGraspActive(GameLookup.graspNamesEnum graspName)
    {
        if (PlayerData.PlayerDataRef.activeGrasps.Contains(graspName))
            return true;
        else
            return false;
    }

    private void SelectGrasp(GameObject button, GameLookup.graspNamesEnum graspName)
    {
        if (numSelGrasps < maxGraspTypes)
        {
            numSelGrasps++;
            PlayerData.PlayerDataRef.activeGrasps.Add(graspName);
            PlayerData.PlayerDataRef.inactiveGrasps.Remove(graspName);

            button.GetComponent<Image>().color = new Color32(203, 221, 202, 255);
        }
    }

    private void DeselectGrasp(GameObject button, GameLookup.graspNamesEnum graspName)
    {
        numSelGrasps--;
        PlayerData.PlayerDataRef.activeGrasps.Remove(graspName);
        PlayerData.PlayerDataRef.inactiveGrasps.Add(graspName);

        button.GetComponent<Image>().color = new Color32(253, 253, 253, 255);
    }

    private void DeactivateRemainingGrasps()
    {
        foreach (GameLookup.graspNamesEnum graspName in PlayerData.PlayerDataRef.inactiveGrasps)
        {
            graspButtons[(int)graspName].GetComponent<Image>().color = new Color32(253, 253, 253, 150);
            graspButtons[(int)graspName].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(253, 253, 253, 150);
            graspButtons[(int)graspName].GetComponent<Button>().enabled = false;
        }
    }

    private void ActivateRemainingGrasps()
    {
        foreach (GameLookup.graspNamesEnum graspName in PlayerData.PlayerDataRef.inactiveGrasps)
        {
            graspButtons[(int)graspName].GetComponent<Image>().color = new Color32(253, 253, 253, 255);
            graspButtons[(int)graspName].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(253, 253, 253, 255);
            graspButtons[(int)graspName].GetComponent<Button>().enabled = true;
        }
    }

    private void EnableNextButton()
    {
        NavNextButton.SetActive(true);
    }

    private void DisableNextButton()
    {
        NavNextButton.SetActive(false);
    }

    public void LoadNextScene()
    {
        PlayerData.PlayerDataRef.graspsChosen = true;
        SceneManager.LoadSceneAsync("2.ControlsAndSetup");
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("TODO: Revert any changes! Have PlayerData read from the saved file.");
        Debug.Log("TODO: Delete PlayerData object!");
        SceneManager.LoadSceneAsync("0.MainMenu");
    }
}
