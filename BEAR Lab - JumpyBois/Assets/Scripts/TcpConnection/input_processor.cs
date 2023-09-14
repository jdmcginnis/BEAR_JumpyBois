using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using data;


public class input_processor : MonoBehaviour
{
    [SerializeField] private ipc_connect tcp;

    // Start is called before the first frame update
    void Start() {
        tcp.Start();
        tcp.connectToServer();
    }

    // Update is called once per frame
    // void Update() {
        // DataPoint dataValue = new DataPoint(DateTime.Now.ToString(), tcp.getCurrentMajority());
        // writer.writeCSV(dataValue);
        // UnityEngine.Debug.Log("time stamp: " + dataValue.timeStamp + ", majority: " + tcp.getCurrentMajority()); 	
    // }

}
