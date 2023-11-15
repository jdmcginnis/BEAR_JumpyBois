using System.Collections;
using System.Collections.Generic;
using System.IO;
using csmatio.io;
using csmatio.types;
using System;

// MATLAB File Handler
// Utility class to write and read from MATLAB files
public static class MatLabFileHandler
{
    public static void recSessionMlWriter(RecSession recSession)
    {
        string rootDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "bearlab");

        // creates a corresponding MATLAB 1x1 structure
        MLStructure structure = new MLStructure("recSession", new int[] { 1, 1 });

        // create a MATLAB variables and add it to the structure
        MLDouble sessionSF = new MLDouble("", new double[] { recSession.sF }, 1);
        MLDouble sessioncT = new MLDouble("", new double[] { recSession.cT }, 1);
        MLDouble sessionrT = new MLDouble("", new double[] { recSession.rT }, 1);
        MLDouble sessionnM = new MLDouble("", new double[] { recSession.nM }, 1);
        MLDouble sessionnR = new MLDouble("", new double[] { recSession.nR }, 1);
        MLDouble sessionsT = new MLDouble("", new double[] { recSession.sT }, 1);
        MLDouble sessionnCh = new MLDouble("", new double[] { recSession.nCh }, 1);

        structure["sF", 0] = sessionSF;
        structure["cT", 0] = sessioncT;
        structure["rT", 0] = sessionrT;
        structure["nM", 0] = sessionnM;
        structure["nR", 0] = sessionnR;
        structure["sT", 0] = sessionsT;
        structure["nCh", 0] = sessionnCh;

        List<MLArray> mlList = new List<MLArray>();
        mlList.Add(structure);
        string recSessionFilePath = Path.Combine(rootDirectoryPath, "data.mat"); 
        MatFileWriter mfw = new MatFileWriter(recSessionFilePath, mlList, false);
    }

}
