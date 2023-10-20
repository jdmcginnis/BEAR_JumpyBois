using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using System;
using System.Linq;

// Establishes a TCP connection w/Matlab and receives a stream of integers representing predictions
// Temporary file until we can port everything over into Unity
public class MatlabConnection : MonoBehaviour
{
    // Sends predictions here
    [SerializeField] private InputManager inputManager;

    // Establishing/Maintaining connect w/MatLab
    private TcpListener listener;
    private TcpClient client;
    private NetworkStream stream;
    private Thread clientReceiveThread;
    private byte[] incomingDataBuffer;
    private bool connectionEstablished = false;

    private void Awake()
    {
        // if we are using keyboard input, disable this script
        if (PlayerData.PlayerDataRef.usingDelsys == false)
            this.gameObject.SetActive(false);
    }

    private void Start()
    {
        // Game initially paused; waiting for connection to resume
        Time.timeScale = 0.01f;
        ConnectToServer();
        StartCoroutine(HandleGamePaused());

    }

    private void ConnectToServer()
    {
        // Pause game until a connection is established
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

        try
        {
            listener.Start();
            Debug.Log("Waiting for connection...");

            incomingDataBuffer = new byte[1024];

            while (true)
            {
                

                using (client = listener.AcceptTcpClient())
                {
                    Debug.Log("Client connected.");
                    connectionEstablished = true;

                    using (stream = client.GetStream())
                    {

                        // Before int is sent from Matlab, it is split into an array of bytes
                        // We reconstruct these bytes to form a string
                        // We then convert this string back into an int
                        int length;
                        while ((length = stream.Read(incomingDataBuffer, 0, incomingDataBuffer.Length)) != 0)
                        {
                            UnityMainThreadDispatcher.ExecuteOnMainThread(() =>
                            {
                                var incommingData = new byte[length];
                                Array.Copy(incomingDataBuffer, 0, incommingData, 0, length);

                                // Convert byte array to string message. 							
                                string clientMessage = Encoding.ASCII.GetString(incommingData);

                                // Processes data in buffer and calculates majority
                                if (clientMessage != null)
                                {
                                    Debug.Log("client message received as: " + clientMessage);
                                    inputManager.currentGraspInput = (GameLookup.graspNamesEnum)int.Parse(clientMessage);
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

    private IEnumerator HandleGamePaused()
    {
        yield return new WaitUntil(() => connectionEstablished);
        Time.timeScale = 1;
    }

    private void OnDisable()
    {
        Debug.Log("Closing Port...");
        Debug.Log("TODO: Close Port out properly!");
        if (listener != null)
            listener.Stop();
    }
}
