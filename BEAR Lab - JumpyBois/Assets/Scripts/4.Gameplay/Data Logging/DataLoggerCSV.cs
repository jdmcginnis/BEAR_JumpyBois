using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

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
        string rootDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "bearlab");
        FindOrCreate(rootDirectoryPath);

        string currentDate = DateTime.Now.ToString("MM-dd-yyyy");
        string todaysDirPath = Path.Combine(rootDirectoryPath, currentDate);
        FindOrCreate(todaysDirPath);
        int fileCount = GetLatestFileCount(todaysDirPath, "*.csv") + 1;

        fileName = fileCount + ".csv";
        filePath = Path.Combine(todaysDirPath, fileName);

        csvHeader = "Timestamp, Buffer Values, Predicted Value, True Value, Accepting Input?";

        using (csvWriter = new StreamWriter(filePath, true))
            csvWriter.WriteLine(csvHeader);
    }

    // Checks if directory exists; if it doesn't, creates it
    private void FindOrCreate(string path)
    {
        if (!File.Exists(path))
            Directory.CreateDirectory(path);
    }


    private int GetLatestFileCount(string directoryPath, string filePattern)
    {
        string[] files = Directory.GetFiles(directoryPath, filePattern);
        int largestNumber = 0;

        if (files.Length > 0)
        {
            largestNumber = files
                .Select(File => Path.GetFileNameWithoutExtension(File))
                .Where(fileName => int.TryParse(fileName, out _))
                .Select(fileName => int.Parse(fileName))
                .Max();
        }
        return largestNumber;
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
