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
    private Rigidbody2D rb;
    
    public float gravity_strength = 0;
    public bool is_game_running ;
    private float vertical_speed;
    private float moveVertical;
    private bool change_direction;
    private bool input_connected;
    
    //threading varibles
    Thread mThread;
    //network varibles
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25000;
    private string disconnectMessage = "!DISCONNECT!";
    private IPAddress localAdd;
    private TcpListener listener;
    private TcpClient client;
    private bool listening = false;



    private void Start()
    {
        //makes player experence no gravity
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity_strength; 
        
   
        
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
        
    }
        
    private void Update()
    {
        if(is_game_running && vertical_speed == 0)
        {
            vertical_speed = 1;
            Debug.Log("Game is running");

        }

        //if player blinks their velocity is inverted
        Debug.Log(change_direction);
        if(change_direction)
        {
            vertical_speed = vertical_speed * -1;
            //Debug.Log(vertical_speed);
            change_direction = false;
        
        }
        //Starts listen if no doing so already
        if(!listening && Time.time>1)
        {
            ThreadStart ts = new ThreadStart(GetInfo);
            mThread = new Thread(ts);
            mThread.Start();
            
        }

    }

    private void FixedUpdate()
    {
        //sets players velocity
        rb.transform.Translate(new Vector2(0,1) * Time.deltaTime * vertical_speed);
    }

    private void GetInfo()
    {
        //listens for client
        Debug.Log("[SERVER] running");
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listening = true;
        listener.Start();

        //client is connected
        client = listener.AcceptTcpClient();
        Debug.Log("[SERVER] connected to client");
        input_connected = true;
        
        

        //Senses if a blink occurs 
        int data = 0;
        int previous_data = 0;

        while (input_connected)
        {
            data = SendAndReceiveData();

            if(data == -1)
            {
                is_game_running = true;
            }
            else if (data > previous_data && data != 0)
            {
                    Debug.Log("[Blink Input] blinked");
                    change_direction = true;
            }
            else if(data != -1)
            {
                 previous_data = data;
            }
            

           
        }
    
        //closes the TCP connection
        listening = false;
        listener.Stop();
        client.Close();
        mThread.Abort();
    
    }

    private int SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        

        byte[] buffer = new byte[client.ReceiveBufferSize];
        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
       
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string
        
        //if(!string.IsNullOrEmpty(dataReceived)){Debug.Log(dataReceived);}

        if(dataReceived == disconnectMessage)
        {
            Debug.Log("[SERVER] closed"); 
            input_connected = false;
        
            return 0;
        }

        //
        try
        {
            int dataReceivedInt = int.Parse(dataReceived);
            if(dataReceivedInt == -1){return -1;}
            return dataReceivedInt;
        }
        catch(FormatException)
        {
            
            Debug.Log("[SERVER ERROR] Unexpected value recieved: " + dataReceived);
            return 0;
        }

        
        
    }

}