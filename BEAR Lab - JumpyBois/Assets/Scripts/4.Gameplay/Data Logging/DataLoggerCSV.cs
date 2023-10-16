using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataLoggerCSV : MonoBehaviour
{
    private string fileDirectory;
    private string fileName;
    private string filePath;

    StreamWriter csvWriter;
    private string csvHeader;

    // Listening to these references for data logging purposes
    [SerializeField] private InputManager inputManager;
    [SerializeField] private SkillCheckManager skillCheckManager;
    [SerializeField] private GraspSelector graspSelector;

    private void Start()
    {
        fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        fileName = "bear_data.csv";
        filePath = Path.Combine(fileDirectory, fileName);

        csvHeader = "Timestamp, Buffer Values, Predicted Value, True Value, Accepting Input?";

        using (csvWriter = new StreamWriter(filePath, true))
            csvWriter.WriteLine(csvHeader);
    }

    private void FixedUpdate()
    {
        WriteDataPoint();
    }

    private void WriteDataPoint()
    {
        using (csvWriter = new StreamWriter(filePath, true))
        {
            string bufferTemp = string.Join(" ", inputManager.bufferNums);
            csvWriter.WriteLine(
                Time.fixedUnscaledTime + 
                "," +  bufferTemp + 
                "," + (int)inputManager.graspPrediction + 
                "," + (int)graspSelector.currentGraspForTrial + 
                "," + skillCheckManager.acceptingInput);
        }
    }
}
