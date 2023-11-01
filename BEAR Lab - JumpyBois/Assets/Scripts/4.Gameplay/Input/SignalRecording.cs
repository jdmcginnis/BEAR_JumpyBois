using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class SignalRecording : MonoBehaviour
{
    RecSession recSession;
    double[,,] recSessionData;
    List<List<double>> allData;

    void Start()
    {
        recSession = new RecSession();
        recSession.sF = CallibrationSettings.sF;
        recSession.cT = CallibrationSettings.cT;
        recSession.rT = CallibrationSettings.rT;
        recSession.nM = CallibrationSettings.nM;
        recSession.nR = CallibrationSettings.nR;
        recSession.nCh = CallibrationSettings.nCh;
        recSession.sT = (recSession.cT + recSession.rT) * recSession.nR;

        int tdataXDimension = recSession.sF * recSession.sT;
        recSessionData = new double[tdataXDimension, recSession.nCh, recSession.nM];

        allData = new List<List<double>>();
    }

    void FixedUpdate()
    {
        // TODO: Take data (presumably from DAQInputHandler) and load into tdata
        int ex = 0;

        // Copies contents of 2D array allData into a 3D array recSessionData at the third dimension slice indicated by ex
        int numRows = recSessionData.GetLength(0);  // Number of rows
        int numCols = recSessionData.GetLength(1);  // Number of columns

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
               // recSessionData[i, j, ex] = allData[ex * numRows * numCols + i * numCols + j];
            }
        }

        // TODO: Save struct to some readable file format
    }


}
