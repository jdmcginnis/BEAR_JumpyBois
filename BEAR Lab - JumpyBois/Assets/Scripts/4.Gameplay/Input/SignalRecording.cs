using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class SignalRecording : MonoBehaviour
{
    RecSession recSession;
    double[,,] recSessionData;
    int matrixRowCnt = 0;
    string saveFilePath = "data.mat";

    [SerializeField] private DAQInputHandler daqInputHandler;
    [SerializeField] private CalibrationManager calibrationManager;

    void Start()
    {
        matrixRowCnt = 0;

        recSession = new RecSession();
        recSession.sF = CallibrationSettings.sF;
        recSession.cT = CallibrationSettings.cT;
        recSession.rT = CallibrationSettings.rT;
        recSession.nM = CallibrationSettings.nM;
        recSession.nR = CallibrationSettings.nR;
        recSession.nCh = CallibrationSettings.nCh;
        recSession.sT = (recSession.cT + recSession.rT) * recSession.nR;
        recSession.date = DateTime.Now.ToString("MM-dd-yyyy");
        recSession.mov = PlayerData.PlayerDataRef.activeGrasps;

        int tdataXDimension = recSession.sF * recSession.sT;
        recSessionData = new double[tdataXDimension, recSession.nCh, recSession.nM];

    }

    void FixedUpdate()
    {
        // TODO: Takes data (presumably from DAQInputHandler) and load into matrix recSessionData
        int graspNum = (int)calibrationManager.currentGrasp;

        // Gets data from each channel
        double ch0Data = daqInputHandler.currentFrameSignals[0];
        double ch1Data = daqInputHandler.currentFrameSignals[1];
        double ch2Data = daqInputHandler.currentFrameSignals[2];
        double ch3Data = daqInputHandler.currentFrameSignals[3];
        double ch4Data = daqInputHandler.currentFrameSignals[4];
        double ch5Data = daqInputHandler.currentFrameSignals[5];
        double ch6Data = daqInputHandler.currentFrameSignals[6];

        // Adds data from each channel into matrix
        recSessionData[matrixRowCnt, 0, graspNum] = ch0Data;
        recSessionData[matrixRowCnt, 1, graspNum] = ch1Data;
        recSessionData[matrixRowCnt, 2, graspNum] = ch2Data;
        recSessionData[matrixRowCnt, 3, graspNum] = ch3Data;
        recSessionData[matrixRowCnt, 4, graspNum] = ch4Data;
        recSessionData[matrixRowCnt, 5, graspNum] = ch5Data;
        recSessionData[matrixRowCnt, 6, graspNum] = ch6Data;

        matrixRowCnt += 1;

        // Save struct to some readable file format
        Debug.Log("Saving recSession");
        recSession.tdata = recSessionData;

        //Save data in recSession to a MATLAB file
        MatLabFileHandler.recSessionMlWriter(recSession);

    }

}
