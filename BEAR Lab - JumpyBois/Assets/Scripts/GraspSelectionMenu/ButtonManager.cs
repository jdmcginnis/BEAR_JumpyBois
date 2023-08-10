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
    [SerializeField] private GameObject[] graspButtons; // Stored in same order of ENum
    

    public int numSelGrasps { get; private set; }

    private void Start()
    {
        numSelGrasps = 0;
    }

    public void OnGraspButtonPress(GameObject button)
    {
        GlobalStorage.graspNamesEnum graspName = (GlobalStorage.graspNamesEnum)System.Enum.Parse(typeof(GlobalStorage.graspNamesEnum), button.name);

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

    private bool IsGraspActive(GlobalStorage.graspNamesEnum graspName)
    {
        if (GlobalStorage.GameSettings.activeGrasps.Contains(graspName))
            return true;
        else
            return false;
    }

    private void SelectGrasp(GameObject button, GlobalStorage.graspNamesEnum graspName)
    {
        if (numSelGrasps < maxGraspTypes)
        {
            numSelGrasps++;
            GlobalStorage.GameSettings.activeGrasps.Add(graspName);
            GlobalStorage.GameSettings.inactiveGrasps.Remove(graspName);
            // Debug.Log(button.name + " is now active!");

            button.GetComponent<Image>().color = new Color32(203, 221, 202, 255);
        }
    }

    private void DeselectGrasp(GameObject button, GlobalStorage.graspNamesEnum graspName)
    {
        numSelGrasps--;
        GlobalStorage.GameSettings.activeGrasps.Remove(graspName);
        GlobalStorage.GameSettings.inactiveGrasps.Add(graspName);
        // Debug.Log(button.name + " is now inactive!");

        button.GetComponent<Image>().color = new Color32(253, 253, 253, 255);
    }

    private void DeactivateRemainingGrasps()
    {
        Debug.Log("Deactivating remaining grasps!");

        foreach (GlobalStorage.graspNamesEnum graspName in GlobalStorage.GameSettings.inactiveGrasps)
        {
            graspButtons[(int)graspName].GetComponent<Image>().color = new Color32(253, 253, 253, 150);
            graspButtons[(int)graspName].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(253, 253, 253, 150);
            graspButtons[(int)graspName].GetComponent<Button>().enabled = false;
        }

    }

    private void ActivateRemainingGrasps()
    {

        Debug.Log("Activating remaining grasps!");

        foreach (GlobalStorage.graspNamesEnum graspName in GlobalStorage.GameSettings.inactiveGrasps)
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
        SceneManager.LoadSceneAsync("2.ControlsAndSetup");
    }

}
