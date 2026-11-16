using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class SavePlayerInfo

{
    //Save player's name and score
    public void SavePlayer(string playerName, int score)

    {
        //Writes the player's information into a text file
        File.WriteAllText("PlayerInfo.txt", playerName + "," + score);

    }


    public void LoadPlayer()

    {
        //Loads players saved info
        if (File.Exists("PlayerInfo.txt"))

        {
            //Read the information from the file
            string playerInfo = File.ReadAllText("PlayerInfo.txt");


        }

    }

}