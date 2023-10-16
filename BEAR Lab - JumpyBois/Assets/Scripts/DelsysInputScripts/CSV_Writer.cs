using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using data;
using Unity.VisualScripting;
using System.Linq;

public class CSV_Writer
{
    private string fileDirectory;
    private string fileName;
    string filePath;

    public CSV_Writer()
    {
        string rootDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "bearlab");
        findOrCreate(rootDirectoryPath);

        string currentDate = DateTime.Now.ToString("MM-dd-yyyy");
        string todaysDirPath = Path.Combine(rootDirectoryPath, currentDate);
        findOrCreate(todaysDirPath);
        int fileCount = getLatestFileCount(todaysDirPath, "*.csv") + 1;

        // fileName = "bear_data.csv";
        fileName = fileCount + ".csv";
        filePath = Path.Combine(todaysDirPath, fileName);
    }

    public void WriteHeaders(string headers)
    {
        using (StreamWriter s = new StreamWriter(filePath, true))
        {
            s.WriteLine(headers);
        }
    }
    public void WriteDataPoint(DataPoint dp, String bufferValues, int expectedGrasp, bool isTakingInput) {

        using (StreamWriter s = new StreamWriter(filePath, true)) {
            s.WriteLine(dp.timeStamp + "," + bufferValues + "," + dp.majority + "," + expectedGrasp + ","+ isTakingInput);
        }
        
    }

    public void CloseFile()
    {
        //Look up how to close StreamWriter and when.
    }

    private void findOrCreate(string path)
    {
        if(!File.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Created directory to " + path);
        } else
        {
            Debug.Log(path + " already exists!");
        }
    }

    private int getLatestFileCount(string directoryPath, string filePattern)
    {
        string[] files = Directory.GetFiles(directoryPath, filePattern);
        int largestNumber = 0;

        if (files.Length > 0)
        {
            //ISSUE IN GETTING LARGEST FILE NUMBER HERE!!
            largestNumber = files
                .Select(file => Path.GetFileNameWithoutExtension(file))
                .Where(fileName => int.TryParse(fileName, out _))
                .Select(fileName => int.Parse(fileName))
                .Max();

            Debug.Log("The largest number is: " + largestNumber);
        }
        else
        {
            Debug.Log("No matching files found in the directory.");
        }
        return largestNumber;
    }

}
