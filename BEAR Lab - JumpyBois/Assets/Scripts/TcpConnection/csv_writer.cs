using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using data;

public class csv_writer
{
    private string fileDirectory;
    private string filename;
    string filePath;

    public csv_writer()
    {
        fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        filename = "bear_data.csv";
        filePath = Path.Combine(fileDirectory, filename);
    }

    public void writeHeaders(string headers)
    {
        using (StreamWriter s = new StreamWriter(filePath, true))
        {
            s.WriteLine(headers);
        }
    }
    public void writeDataPoint(DataPoint dp, String bufferValues, bool isTakingInput) {

        using (StreamWriter s = new StreamWriter(filePath, true)) {
            s.WriteLine(dp.timeStamp + "," + bufferValues + "," + dp.majority + "," + isTakingInput);
        }
        //Look up how to close StreamWriter and when.
    }

}
