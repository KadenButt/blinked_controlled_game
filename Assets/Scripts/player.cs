using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;



public class player : MonoBehaviour
{
    //game realted varibles
    private Rigidbody2D rb;
    
    public float gravityStrength = 0;
    public static int score;
    public bool isGameRunning;
    public static bool inputConnection; 
    private int startTime;
    private float verticalSpeed;
    private bool changeDirection;
    
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
        rb.gravityScale = gravityStrength; 

        //places player in starting position
        transform.position = new Vector2(-10,0);
        
   
        
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
        
    }
        
    private void Update()
    {
        if(isGameRunning && verticalSpeed == 0)
        {
            verticalSpeed = 1;
            Debug.Log("[GAME] Running");

        }

        //if player blinks their velocity is inverted
        if(changeDirection)
        {
            verticalSpeed = verticalSpeed * -1;
            changeDirection = false;
        
        }
        
        // Starts listen if no doing so already
        if(!listening && isGameRunning)
        {
            ThreadStart ts = new ThreadStart(GetInfo);
            mThread = new Thread(ts);
            mThread.Start();
            
        }

        if(!isGameRunning)
        {
            startTime = (int)Math.Round(Time.time);
        }else
        {
            score = (int)Math.Round(Time.time) - startTime;
        }
        

    }

    private void FixedUpdate()
    {
        //sets players velocity
        rb.transform.Translate(new Vector2(0,1) * Time.deltaTime * verticalSpeed);
        //causes player to change direction when out side boundaries 
        if(transform.position[1] > 4.5 && verticalSpeed == 1 || transform.position[1] <-4.5 && verticalSpeed == -1){changeDirection = true;}

      
        
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
        inputConnection = true;
        
        

        //Senses if a blink occurs 
        int data = 0;
        int previousData = 0;

        while (inputConnection)
        {
            data = SendAndReceiveData();

            if(data == -1)
            {
                isGameRunning = true;
            }
            else if (data > previousData && data != 0)
            {
                    Debug.Log("[Blink Input] blinked");
                    changeDirection = true;
            }
            else if(data != -1)
            {
                 previousData = data;
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
            inputConnection = false;

            return 0;
        }

        //
        try
        {
            int dataReceivedInt = int.Parse(dataReceived);
            return dataReceivedInt;
        }
        catch(FormatException)
        {
            Debug.Log("[SERVER ERROR] Unexpected value recieved: " + dataReceived);
            return 0;
        }

        
        
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "enemy")
        {
            inputConnection = false;
            SceneManager.LoadScene("GameOver");
        }
    }

}