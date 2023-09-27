using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using System;
using data;
using System.Linq;

public class ipc_connect: MonoBehaviour {
    
    TcpListener listener;
    TcpClient client;
    NetworkStream stream;
    private Thread clientReceiveThread; 
    private CSV_Writer writer;	
    private int currMajority;
    private int bufferSize = 5;
    byte[] buffer;

    private Queue<int> bufferQueue = new Queue<int>();

    [SerializeField] private InputManager inputManager;

    // Start is called before the first frame update
    public void Start() { 
        for(int i = 0; i < bufferSize; i++) {
            addToBuffer(bufferQueue, 0);
        }

        connectToServer();
    }

    private void OnApplicationQuit() {
        Debug.Log("Application is quitting. Performing cleanup...");
    }

    public void connectToServer() {
        try {  	
			clientReceiveThread = new Thread(new ThreadStart(listenForData)); 			
			clientReceiveThread.IsBackground = true; 			
			clientReceiveThread.Start(); 
		} 		
		catch (Exception e) { 			
			Debug.Log("On client connect exception " + e);
		}
    }


    public void listenForData() {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 1924;

        listener = new TcpListener(ipAddress, port);
        writer = new CSV_Writer();

        try
        {
            listener.Start();
            Debug.Log("Waiting for connection...");

            // Create a buffer to store received data
            buffer = new byte[1024];

            while(true) {
                using(client = listener.AcceptTcpClient()) {
                    Debug.Log("Client connected.");

                    using(stream = client.GetStream()) {
                        int length;
                        while ((length = stream.Read(buffer, 0, buffer.Length)) != 0) {

                            UnityMainThreadDispatcher.ExecuteOnMainThread(() =>
                            {
                                var incommingData = new byte[length]; 							
							    Array.Copy(buffer, 0, incommingData, 0, length);  							
							
                                // Convert byte array to string message. 							
							    string clientMessage = Encoding.ASCII.GetString(incommingData);
                                
                                // Processes data in buffer and calculates majority
                                if(clientMessage != null)
                                {
                                    Debug.Log("client message received as: " + clientMessage);
                                    addToBuffer(bufferQueue, int.Parse(clientMessage));
                                    Queue<int> tempQueue = bufferQueue;
                                    currMajority = calculateMode(tempQueue);

                                    // Creates a new DataPoint instance with the data calculated above and inputs into Delsys game control
                                    DataPoint dataValue = new DataPoint(getCurrentTime(), calculateMode(tempQueue));
                                    Debug.Log("The value is: " + dataValue.majority);
                                    inputManager.OnDelsysInput(dataValue.majority);
                                    
                                    // Records data to CSV
                                    writer.WriteHeaders("Timestamp, Buffer Values, Majority Output, Taking Input");
                                    writer.WriteDataPoint(dataValue, getBufferContents(tempQueue), inputManager.enableInput);
                                    Debug.Log("time stamp: " + dataValue.timeStamp + ", majority: " + dataValue.majority + " ,is taking input: " + inputManager.enableInput);
                                } else
                                {
                                    Debug.Log("No data received");
                                } 
                            });
                        } 
                    }
                }
            }
        } catch (SocketException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 
		}   
    }

    private void addToBuffer(Queue<int> queue, int value) {
        if (queue.Count >= 5) {
            queue.Dequeue();
        }
        queue.Enqueue(value);

        getBufferContents(queue);
    }

    private String getBufferContents(Queue<int> q) {
        String consolePrintBuffer = "";
        foreach (var item in q){
            consolePrintBuffer += " " + item;
        }
        return consolePrintBuffer;
    }

    private String getCurrentTime() {
        DateTime currentTime = DateTime.Now;
        return currentTime.ToString("mm:ss:fff");
    }

    private int calculateMode(Queue<int> queue) {
        Dictionary<int, int> frequencyMap = new Dictionary<int, int>();

        Queue<int> tempQueue = new Queue<int>(queue);

        while (tempQueue.Count > 0)
        {
            int currentNumber = tempQueue.Dequeue();
            if (frequencyMap.ContainsKey(currentNumber))
                frequencyMap[currentNumber]++;
            else
                frequencyMap[currentNumber] = 1;
        }

        int mode = frequencyMap.OrderByDescending(kv => kv.Value).First().Key;
        return mode;
    }

}
