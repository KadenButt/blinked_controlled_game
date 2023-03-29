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
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;

    bool running;



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
        //moveVertical = Input.GetAxisRaw("Vertical");


        if(change_direction)
        {
            vertical_speed = vertical_speed * -1;
            Debug.Log(vertical_speed);
            change_direction = false;
        }




    }

    private void FixedUpdate()
    {
        //Debug.Log(change_direction);

        //Debug.Log(vertical_speed);
        rb2D.transform.Translate(new Vector2(0,1) * Time.deltaTime * vertical_speed);
    }

    private void GetInfo()
    {
        
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();
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
            if(data > previous_data)
            {
                Debug.Log("[Blink Input] blinked");
                change_direction = true;
            }



            previous_data = data;
           
        }
        listener.Stop();
        //mThread.Abort();
    
    }

    private int SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        string disconnectMessage = "!DISCONNECT!";

        byte[] buffer = new byte[client.ReceiveBufferSize];
        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
       
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string
        
        
        //if(!string.IsNullOrEmpty(dataReceived)){Debug.Log(dataReceived);}

        if(dataReceived == disconnectMessage){Debug.Log("[SERVER] closed"); running=false;}

        //
        try
        {
            return int.Parse(dataReceived);
        }
        catch(InvalidCastException)
        {
            Debug.Log("[SERVER ERROR] Unexpected int value recieved");
            return 0;
        }

        
        
    }

}