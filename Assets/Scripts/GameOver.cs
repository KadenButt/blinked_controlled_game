using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{

    public Button enterButton;
    public Button mainMenu;
    public TMP_InputField nameField;
    public TMP_Text scoreBoard;
    public player script;


    private string highScorePath;
    private int playerScore;
    
    

    // Start is called before the first frame update
    void Start()
    {
        playerScore = player.score;
        enterButton.onClick.AddListener(OnClick);
        mainMenu.onClick.AddListener(MainMenu);
        highScorePath =  Application.dataPath + "/data/high_scores.txt";
        DisplayHighScore();
    }

    void DisplayHighScore()
    {
        string[] lines = File.ReadAllLines(highScorePath);
        var scores = lines[1].Split(",");
        var names = lines[2].Split(",");
        string scoreBoardText = ""; 

        for(int x = 0 ; x <= scores.Length-1; ++x)
        {
            scoreBoardText += names[x] + " " +  scores[x] + "\n";
        }

        scoreBoard.text = scoreBoardText;

    }

    void OnClick()
    {
        //end if no text is entered
        if(nameField.text == null)
        {
            return;
        }

        //readfile
        string[] lines = File.ReadAllLines(highScorePath);
        string returnData; 

        int tail = -1;
        int pointer = Convert.ToInt16(lines[0]);

        
        //seperates the pointer, scores, names and converts them
        
        var scores = lines[1].Split(",");
        int[] instScore = new int[10];
        var names = lines[2].Split(",");

        //conver string list to int list
        for(int x = 0; x < 10; x++){instScore[x] = Convert.ToInt32(scores[x]);}


        //find the first value its smaller then and stores the location as order_head
        for(int x = pointer; x >= 0; x--)
        {
            if(instScore[x] < playerScore)
            {
                tail = x;
            }
            
            //if no high score ends function
            if(x == 0 && tail == -1)
            {
                return;   
            }
        }


        //moves all the values down, and places highscore in appropiate location
        if(tail != 9)
        {
            for(int x = pointer; x != tail-1; x--)
            {

                if(x != 9)
                {
                    instScore[x+1] = instScore[x];
                    names[x+1] = names[x];
                }

            }
        }


            
        instScore[tail] = playerScore;
        names[tail] = nameField.text;
        
        

        //if the list isnt full it will find the correct position to place the high score
        if(pointer < 9)
        {
            pointer ++;
        }


                

        //converts int list to string list
        for(int x = 0; x < 10; x++){scores[x] = instScore[x].ToString();}
        //converts all the data into one string
        returnData = pointer.ToString()+ "\n" + ListToString(scores) + "\n" + ListToString(names);
        //writes the data back to file
        File.WriteAllText(highScorePath, returnData);
        //Debug.Log(returnData);
        //set the text box to null
        nameField.text = "";
        //update score board
        DisplayHighScore();


    }

    void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
//function that turns a list to a string
    string ListToString(string[] list)
    {
        string returnList = null;
        for(int x = 0; x < list.Length; x++)
        {
            if(x<9)
            {
                returnList += list[x]+",";
            }else
            {
                returnList += list[x];
            }
            
        }
        return returnList;
    }

    
}
