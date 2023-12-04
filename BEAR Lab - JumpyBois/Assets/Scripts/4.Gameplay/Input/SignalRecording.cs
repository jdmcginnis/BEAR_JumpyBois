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

    [SerializeField] private DAQInputHandler daqInputHandler;
    [SerializeField] private CalibrationManager calibrationManager;

    void Start()
    {
        Debug.Log("Starting Signal Recording");
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
        recSessionData = new double[recSession.nM, tdataXDimension, recSession.nCh];
        
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
        recSessionData[graspNum, matrixRowCnt, 0] = ch0Data;
        recSessionData[graspNum, matrixRowCnt, 1] = ch1Data;
        recSessionData[graspNum, matrixRowCnt, 2] = ch2Data;
        recSessionData[graspNum, matrixRowCnt, 3] = ch3Data;
        recSessionData[graspNum, matrixRowCnt, 4] = ch4Data;
        recSessionData[graspNum, matrixRowCnt, 5] = ch5Data;
        recSessionData[graspNum, matrixRowCnt, 6] = ch6Data;

        matrixRowCnt += 1;

        // Save struct to some readable file format
        Debug.Log("Saving recSession");
        recSession.tdata = recSessionData;

        //Save data in recSession to a MATLAB file
        MatLabFileHandler.recSessionMlWriter(recSession);

    }

}
