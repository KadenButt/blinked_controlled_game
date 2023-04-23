using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerUI : MonoBehaviour
{
    public TMP_Text inputStatusDisplay;
    public TMP_Text score;
    public player player;


    // Start is called before the first frse update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.inputConnection){inputStatusDisplay.text = "";}

        if(player.isGameRunning)
        {
            score.text = "Score: " + player.score;
        }
        
    }
}
