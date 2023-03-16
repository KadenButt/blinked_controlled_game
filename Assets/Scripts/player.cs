using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
public class player : MonoBehaviour
{
    Thread mThread;
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25000;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;

    bool running;

    private void Update()
    {
        
    }

    private void Start()
    {
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
        Debug.Log("[SERVER] running");
    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();

        client = listener.AcceptTcpClient();
        Debug.Log("[SERVER] connected to client");

        running = true;
        while (running)
        {
            SendAndReceiveData();
           
        }
        listener.Stop();
        //mThread.Abort();
    
    }

    void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        string disconnectMessage = "!DISCONNECT!";

        byte[] buffer = new byte[client.ReceiveBufferSize];
        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
       
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string
        
        if(dataReceived != null){Debug.Log(dataReceived);}

        if(dataReceived == disconnectMessage){Debug.Log("[SERVER] closed"); running=false;}
        
    }

}