using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
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
        public string name;

        public int score;

        public int shotsFired;

        public GameBoard board;

        public MainWindow mainContext;

        ///constructor
        public Player(MainWindow main)
        {
            name = "";

            score = 0;
            shotsFired = 0;
            board = new GameBoard(main);
            mainContext = main;
        }

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
                board[row, col].ship.TakeHit(position, this);
            }
            else
            {
                //cell does not contain ship
            }

            mainContext.game.CheckWinner();

            if (mainContext.game.gameIsOver)
            {
                mainContext.DisplayWinner();
                return;
            }

            // Only switch turns if the game continues
            //TODO commented out for this week so only p1 goes this week
            //mainContext.game.SwitchTurn();
        }

        /// <summary>
        /// Adds to the score field; left blank until we decide the details of the score system 
        /// </summary>
       /* public void AddScore(int)
        { } */

        /// runs when a player runs out of ships
        /// not fully written yet
        public bool hasLost()
        {
            bool hasLost = true;
            ///display win/lose message, 
            ///record player on leaderboard, 
            ///reset to main screen
            return hasLost;
        }
    }
}
