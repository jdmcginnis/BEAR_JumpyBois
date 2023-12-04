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
        double[][] data3x3 = ConvertToJaggedArray(recSession.tdata);
        double[,,] array3D = recSession.tdata;

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
        MLDouble matlabArray = new MLDouble("", data3x3);
        //MWNumericArray matlabArray = new MWNumericArray(your3DArray);


        structure["sF", 0] = sessionSF;
        structure["cT", 0] = sessioncT;
        structure["rT", 0] = sessionrT;
        structure["nM", 0] = sessionnM;
        structure["nR", 0] = sessionnR;
        structure["sT", 0] = sessionsT;
        structure["nCh", 0] = sessionnCh;
        structure["tdata", 0] = matlabArray;

        List<MLArray> mlList = new List<MLArray>();
        mlList.Add(structure);
        string recSessionFilePath = Path.Combine(rootDirectoryPath, "data.mat"); 
        MatFileWriter mfw = new MatFileWriter(recSessionFilePath, mlList, false);
    }

    public static double[][] ConvertToJaggedArray(double[,,] threeDArray)
    {
        int depth = threeDArray.GetLength(0);
        int rows = threeDArray.GetLength(1);
        int cols = threeDArray.GetLength(2);

        double[][] jaggedArray = new double[depth][];

        for (int d = 0; d < depth; d++)
        {
            jaggedArray[d] = new double[rows * cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[d][i * cols + j] = threeDArray[d, i, j];
                }
            }
        }

        return jaggedArray;
    }

}
