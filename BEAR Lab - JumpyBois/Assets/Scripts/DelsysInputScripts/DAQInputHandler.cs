using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Janelia;

// Reading in raw voltages from the National Instruments Data Acquisition System
public class DAQInputHandler : MonoBehaviour
{
    private NiDaqMx.InputParams inputParams;
    public double[] readData;
    public bool showEachRead;

    private void Start()
    {
        // Input channels you are reading from here
        // Each EMG input is one channel on the DAQ
        inputParams = new NiDaqMx.InputParams
        {
            // Note: If we are collecting data faster than we are reading it in our script
                // (sampling rate of data) > (Update or FixedUpdate Time)
                // Then it will be stored in a buffer
            SamplesPerSec = 1 / Time.fixedDeltaTime, 
            SampleBufferSize = 1000, // Must be large enough for all samples from all channels
            VoltageMax = 10,
            VoltageMin = -10,
            // EMG sensors 1-7
            ChannelNames = new string[] { "ai0", "ai1", "ai2", "ai3", "ai4", "ai5", "ai6" }
        };


        if (!NiDaqMx.CreateInputs(inputParams))
        {
            Debug.Log("Creating inputs failed");
            Debug.Log(NiDaqMx.GetLatestError());
            return;
        }

        readData = new double[inputParams.SampleBufferSize];
    }

    private void FixedUpdate()
    {
        // Reading
        int numReadPerChannel = 0;
        if (!NiDaqMx.ReadFromInputs(inputParams, ref readData, ref numReadPerChannel))
        {
            Debug.Log("Frame " + Time.frameCount + ": read from input failed");
            Debug.Log(NiDaqMx.GetLatestError());
        } else
        {
            if (numReadPerChannel > 0)
            {
                float[] rot = { 0, 0 };
                for (int iChannel = 0; iChannel < inputParams.ChannelNames.Length; iChannel++)
                {
                    double sum = 0;
                    // Debug.Log("Channel " + iChannel + " has " + numReadPerChannel + " ")
                    for (int i = 0; i < numReadPerChannel; i++)
                    {
                        int j = NiDaqMx.IndexInReadBuffer(iChannel, numReadPerChannel, i);
                        sum += readData[j];
                    }
                    float mean = (float)(sum / numReadPerChannel);

                    if (showEachRead)
                    {
                        Debug.Log("Frame " + Time.frameCount + ": buffer size " + readData.Length +
                            ", read " + numReadPerChannel + " values with mean " + mean);
                    }

                }
            } else
            {
                Debug.Log("Frame " + Time.frameCount + ": unexpectedly, read " + numReadPerChannel + " values");
            }
        }
    }

    private void OnDestroy()
    {
        NiDaqMx.OnDestroy();
    }

}
