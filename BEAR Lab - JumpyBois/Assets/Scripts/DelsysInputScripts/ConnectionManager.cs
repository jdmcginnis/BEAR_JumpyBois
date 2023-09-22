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

public class ConnectionManager : MonoBehaviour
{
    private TcpListener listener;
    private TcpClient client;
    private NetworkStream stream;
    private Thread clientReceiveThread;
    private CSV_Writer writer;
    private int currMajority;
    [SerializeField] private int bufferSize;
    private byte[] buffer;

    private Queue<int> bufferQueue;

    [SerializeField] private InputManager inputManager;

    private void Awake()
    {
        // if we are using keyboard input, disable this script
        if (GlobalStorage.GameSettings.usingDelsys == false)
            this.enabled = false;
    }

    private void Start()
    {
        bufferQueue = new Queue<int>(bufferSize);
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        try
        {
            // New thread will continuously listen for input
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        } catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }

    private void ListenForData()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 1925;

        listener = new TcpListener(ipAddress, port);
        writer = new CSV_Writer();

        // if port is occupied, evict the squatters


        try
        {
            listener.Start();
            Debug.Log("Waiting for connection...");

            buffer = new byte[1024]; // Create a buffer to store received data

            while (true)
            {
                using (client = listener.AcceptTcpClient())
                {
                    Debug.Log("Client connected.");
                    using (stream = client.GetStream())
                    {
                        int length;
                        while ((length = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            UnityMainThreadDispatcher.ExecuteOnMainThread(() =>
                            {
                                var incommingData = new byte[length];
                                Array.Copy(buffer, 0, incommingData, 0, length);

                                // Convert byte array to string message. 							
                                string clientMessage = Encoding.ASCII.GetString(incommingData);

                                // Processes data in buffer and calculates majority
                                if (clientMessage != null)
                                {
                                    Debug.Log("client message received as: " + clientMessage);
                                    AddToBuffer(bufferQueue, int.Parse(clientMessage));
                                    Queue<int> tempQueue = bufferQueue;
                                    currMajority = CalculateMode(tempQueue);

                                    // Creates a new DataPoint instance with the data calculated above and inputs into Delsys game control
                                    DataPoint dataValue = new DataPoint(GetCurrentTime(), CalculateMode(tempQueue));
                                    Debug.Log("The value is: " + dataValue.majority);
                                    inputManager.OnDelsysInput(dataValue.majority);

                                    // Records data to CSV
                                    writer.WriteHeaders("Timestamp, Buffer Values, Majority Output, Taking Input");
                                    writer.WriteDataPoint(dataValue, GetBufferContents(tempQueue), inputManager.enableInput);
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
        } catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }

    private void AddToBuffer(Queue<int> queue, int value)
    {
        if (queue.Count >= 5)
        {
            queue.Dequeue();
        }
        queue.Enqueue(value);

        GetBufferContents(queue);
    }

    private String GetBufferContents(Queue<int> q)
    {
        String consolePrintBuffer = "";
        foreach (var item in q)
        {
            consolePrintBuffer += " " + item;
        }
        return consolePrintBuffer;
    }

    private int CalculateMode(Queue<int> queue)
    {
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

    private string GetCurrentTime()
    {
        DateTime currentTime = DateTime.Now;
        return currentTime.ToString("mm:ss:fff");
    }

    private void OnDisable()
    {
        Debug.Log("Closing Port...");
        listener.Stop();
    }
}
