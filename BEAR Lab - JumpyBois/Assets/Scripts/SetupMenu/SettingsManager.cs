using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    [SerializeField] private GameObject numTestsField;
    [SerializeField] private GameObject numTestsPlaceholder;
    [SerializeField] GameObject delsysToggle;
    [SerializeField] GameObject delsysToggleLabel;
    [SerializeField] GameObject delsysToggleSwitch;

    void Start()
    {
        GlobalStorage.GameSettings.numTests = int.Parse(numTestsPlaceholder.GetComponent<TextMeshProUGUI>().text);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync("3.CharacterAndSceneSelection");
    }

    public void ChangeNumTests()
    {
        var isNumeric = int.TryParse(numTestsField.GetComponent<TMP_InputField>().text, out int userInput);

        if (isNumeric && userInput > 0)
            GlobalStorage.GameSettings.numTests = userInput;
        else
            numTestsField.GetComponent<TMP_InputField>().text = GlobalStorage.GameSettings.numTests.ToString();

    }

    public void OnDelsysToggleClick()
    {
        if (GlobalStorage.GameSettings.usingDelsys)
        {
            delsysToggleLabel.GetComponent<TextMeshProUGUI>().text = "OFF";
            GlobalStorage.GameSettings.usingDelsys = false;
            delsysToggleSwitch.transform.localPosition += new Vector3(-35, 0, 0);
        } else
        {
            delsysToggleLabel.GetComponent<TextMeshProUGUI>().text = "ON";
            GlobalStorage.GameSettings.usingDelsys = true;
            delsysToggleSwitch.transform.localPosition += new Vector3(35, 0, 0);
        }
    }

}
