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

    public Button enter_button;
    public Button main_menu;
    public TMP_InputField name_field;
    public TMP_Text score_board;
    public int player_score = 100;


    private string highscores_path;
    

    // Start is called before the first frame update
    void Start()
    {
        enter_button.onClick.AddListener(OnClick);
        main_menu.onClick.AddListener(MainMenu);
        highscores_path =  Application.dataPath + "/data/high_scores.txt";
        DisplayHighScore();
    }

    void DisplayHighScore()
    {
        string[] lines = File.ReadAllLines(highscores_path);
        var scores = lines[1].Split(",");
        var names = lines[2].Split(",");
        string score_board_text = ""; 

        for(int x = 0 ; x <= scores.Length-1; ++x)
        {
            score_board_text += names[x] + " " +  scores[x] + "\n";
        }

        score_board.text = score_board_text;

    }

    void OnClick()
    {
        //end if no text is entered
        if(name_field.text == null)
        {
            return;
        }

        //readfile
        string[] lines = File.ReadAllLines(highscores_path);
        string return_data; 

        int order_header = -1;
        int pointer = Convert.ToInt16(lines[0]);

        
        //seperates the pointer, scores, names and converts them
        
        var scores = lines[1].Split(",");
        int[] int_scores = new int[10];
        var names = lines[2].Split(",");

        //conver string list to int list
        for(int x = 0; x < 10; x++){int_scores[x] = Convert.ToInt32(scores[x]);}


        //find the first value its smaller then and stores the location as order_head
        for(int x = pointer; x >= 0; x--)
        {
            if(int_scores[x] < player_score)
            {
                order_header = x;
            }
            
            //if no high score ends function
            if(x == 0 && order_header == -1)
            {
                return;   
            }
        }


        //moves all the values down, and places highscore in appropiate location
        if(order_header != 9)
        {
            for(int x = pointer; x != order_header-1; x--)
            {

                if(x != 9)
                {
                    int_scores[x+1] = int_scores[x];
                    names[x+1] = names[x];
                }

            }
        }


            
        int_scores[order_header] = player_score;
        names[order_header] = name_field.text;
        
        

        //if the list isnt full it will find the correct position to place the high score
        if(pointer < 9)
        {
            pointer ++;
        }


                

        //converts int list to string list
        for(int x = 0; x < 10; x++){scores[x] = int_scores[x].ToString();}
        //converts all the data into one string
        return_data = pointer.ToString()+ "\n" + ListToString(scores) + "\n" + ListToString(names);
        //writes the data back to file
        File.WriteAllText(highscores_path, return_data);
        //Debug.Log(return_data);
        //set the text box to null
        name_field.text = "";
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
        string return_list = null;
        for(int x = 0; x < list.Length; x++)
        {
            if(x<9)
            {
                return_list += list[x]+",";
            }else
            {
                return_list += list[x];
            }
            
        }
        return return_list;
    }

    
}
