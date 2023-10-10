using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject numTestsField;
    [SerializeField] private GameObject numTestsPlaceholder;
    [SerializeField] private GameObject delsysToggleLabel;
    [SerializeField] private GameObject delsysToggleSwitch;

    private NumPerGraspManager numPerGraspManager;

    void Awake()
    {
        numPerGraspManager = this.GetComponent<NumPerGraspManager>();

        PlayerData.PlayerDataRef.numTests = int.Parse(numTestsPlaceholder.GetComponent<TextMeshProUGUI>().text);

        if (delsysToggleLabel.GetComponent<TextMeshProUGUI>().text == "OFF")
            PlayerData.PlayerDataRef.usingDelsys = false;
        else
            PlayerData.PlayerDataRef.usingDelsys = true;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync("0.MainMenu");
        SaveData();
        Destroy(GameObject.Find("PlayerData"));
        Destroy(PlayerData.PlayerDataRef);
    }

    public void ReturnToGraspSelection()
    {
        SceneManager.LoadSceneAsync("1.GraspSelectionMenu");
        Debug.Log("TODO: Implement This!");
    }

    // Changing the number of tests set all the active grasps to have the default number of trials per grasp
    public void ChangeNumTests()
    {
        var isNumeric = int.TryParse(numTestsField.GetComponent<TMP_InputField>().text, out int userInput);

        if (isNumeric && userInput > 0)
        {
            PlayerData.PlayerDataRef.numTests = userInput;
            numPerGraspManager.AssignDefaultTestsPerGrasp();
        } else
            numTestsField.GetComponent<TMP_InputField>().text = PlayerData.PlayerDataRef.numTests.ToString();
    }

    public void OnDelsysToggleClick()
    {
        if (PlayerData.PlayerDataRef.usingDelsys)
        {
            delsysToggleLabel.GetComponent<TextMeshProUGUI>().text = "OFF";
            PlayerData.PlayerDataRef.usingDelsys = false;
            delsysToggleSwitch.transform.localPosition += new Vector3(-35, 0, 0);
        } else
        {
            delsysToggleLabel.GetComponent<TextMeshProUGUI>().text = "ON";
            PlayerData.PlayerDataRef.usingDelsys = true;
            delsysToggleSwitch.transform.localPosition += new Vector3(35, 0, 0);
        }
    }

    // Saves to: C:\Users\jmcgi\AppData\LocalLow\DefaultCompany\BEAR Lab - JumpyBois
    private void SaveData()
    {
        string gameSettingsData = JsonUtility.ToJson(PlayerData.PlayerDataRef);
        string filePath = Application.persistentDataPath + "/GameSettings.json";
        System.IO.File.WriteAllText(filePath, gameSettingsData);
    }

}
