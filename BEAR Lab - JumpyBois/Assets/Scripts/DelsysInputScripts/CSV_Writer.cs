using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using data;

public class CSV_Writer
{
    private string fileDirectory;
    private string fileName;
    string filePath;

    public CSV_Writer()
    {
        fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        fileName = "bear_data.csv";
        filePath = Path.Combine(fileDirectory, fileName);
    }

    public void WriteHeaders(string headers)
    {
        using (StreamWriter s = new StreamWriter(filePath, true))
        {
            s.WriteLine(headers);
        }
    }
    public void WriteDataPoint(DataPoint dp, String bufferValues, bool isTakingInput) {

        using (StreamWriter s = new StreamWriter(filePath, true)) {
            s.WriteLine(dp.timeStamp + "," + bufferValues + "," + dp.majority + "," + isTakingInput);
        }
        
    }

    public void CloseFile()
    {
        //Look up how to close StreamWriter and when.
    }

}
