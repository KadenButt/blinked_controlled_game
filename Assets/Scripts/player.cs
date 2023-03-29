using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
public class player : MonoBehaviour
{
    //game realted varibles
    private Rigidbody2D rb2D;
    
    private float vertical_speed;
    private float moveVertical;
    
    private bool change_direction;
    private bool input_connected;
    
    public float gravity_strength = 0;
    
    
    //threading varibles
    Thread mThread;
    
    //network varibles
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25000;
    string disconnectMessage = "!DISCONNECT!";
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;

    bool running = false;
    bool listening = false;



    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        rb2D.gravityScale = gravity_strength; 
        //moveVertical = 0;
        
   
        
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
        Debug.Log("[SERVER] running");
    }
        
    private void Update()
    {


        if(change_direction)
        {
            vertical_speed = vertical_speed * -1;
            Debug.Log(vertical_speed);
            change_direction = false;
        }

        Debug.Log(listening);
        if(!listening && Time.time>1)
        {
            ThreadStart ts = new ThreadStart(GetInfo);
            mThread = new Thread(ts);
            mThread.Start();
            Debug.Log("[SERVER] running");
    
        }




    }

    private void FixedUpdate()
    {

        rb2D.transform.Translate(new Vector2(0,1) * Time.deltaTime * vertical_speed);
    }

    private void GetInfo()
    {
        
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();
        listening = true;
        client = listener.AcceptTcpClient();

        
        Debug.Log("[SERVER] connected to client");
        
        input_connected = true;
        
        
        //allows for the player to start moving only when the player connects
        vertical_speed = 1;

        int data = 0;
        int previous_data = 0;

        while (input_connected)
        {
            data = SendAndReceiveData();
            if(data > previous_data && data != 0)
            {
                Debug.Log("[Blink Input] blinked");
                change_direction = true;
            }



            previous_data = data;
           
        }
    
        listener.Stop();
        client.Close();
        listening = false;
        mThread.Abort();
    
    }

    private int SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        

        byte[] buffer = new byte[client.ReceiveBufferSize];
        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
       
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string
        
        if(!string.IsNullOrEmpty(dataReceived)){Debug.Log(dataReceived);}

        if(dataReceived == disconnectMessage)
        {
            Debug.Log("[SERVER] closed"); 
            input_connected = false;
            return 0;
        }

        //
        try
        {
            return int.Parse(dataReceived);
        }
        catch(FormatException)
        {
            Debug.Log("[SERVER ERROR] Unexpected int value recieved");
            return 0;
        }

        
        
    }

}