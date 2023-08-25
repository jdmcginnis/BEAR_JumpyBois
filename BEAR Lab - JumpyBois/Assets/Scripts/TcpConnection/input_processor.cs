using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using data;


public class input_processor : MonoBehaviour
{
    private ipc_connect tcp;
    private csv_writer writer;	

    // Start is called before the first frame update
    void Start() {
        tcp = new ipc_connect();
        // writer = new csv_writer(); 
        tcp.connectToServer();
    }

    // Update is called once per frame
    // void Update() {
        // DataPoint dataValue = new DataPoint(DateTime.Now.ToString(), tcp.getCurrentMajority());
        // writer.writeCSV(dataValue);
        // UnityEngine.Debug.Log("time stamp: " + dataValue.timeStamp + ", majority: " + tcp.getCurrentMajority()); 	
    // }

}
