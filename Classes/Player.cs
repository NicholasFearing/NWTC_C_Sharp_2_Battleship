using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Game_Battleship
{
    /// <summary>
    /// Represents the player; their information and actions
    /// </summary>
    public class Player
    {
        //fields
        public string Name;

        public int Score;

        public int ShotsFired;

        //methods

        /// <summary>
        /// Method for executing an attack
        /// </summary>
        public void Attack(int row, int col, Cell[,] board)
        {
            int[] position = new int[2];
            position[0] = row;
            position[1] = col;

            //if the chosen cell has a ship on it, it is hit
            if (board[row, col].ContainsShip())
            {
                board[row, col].Ship.TakeHit(position);
            }
            else { 
                //mark cell as having been hit but empty
                //display 'Miss!' message
            }

            Game.switchTurn();
        }

        /// <summary>
        /// Adds to the score field; left blank until we decide the details of the score system 
        /// </summary>
       /* public void AddScore(int)
        { } */

        /// Runs when a player runs out of ships
        ///public bool HasLost()
        ///{
            ///bool hasLost = false;
            ///display win/lose message, 
            ///record player on leaderboard, 
            ///reset to main screen
            ///return hasLost;
        ///}
    }
}
