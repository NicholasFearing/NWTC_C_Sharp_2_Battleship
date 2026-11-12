using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game_Battleship
{
    public class Game
    {
        // Stores Player 1.
        public Player p1;

        // Stores Player 2.
        public Player p2;

        // Indicates whether Player 1 is controlled by the computer.
        public bool p1IsCPU;

        // Indicates whether Player 2 is controlled by the computer.
        public bool p2IsCPU;

        // Counts the number of turns played.
        public int num_turns;

        // Stores the player whose turn it is.
        public Player currentplayer;

        // Indicates whether the game has ended.
        public bool gameIsOver;

        // Stores the winner, or null if nobody has won.
        public Player? winner;

        // Constructor: Initializes a new game.
        public Game(MainWindow main)
        {
            // Creates Player 1.
            p1 = new Player(main);

            // Creates Player 2.
            p2 = new Player(main);

            // Both players start as human players.
            p1IsCPU = false;
            p2IsCPU = false;

            // No turns have been played.
            num_turns = 0;

            // Player 1 starts first.
            currentplayer = p1;

            // The game has not ended.
            gameIsOver = false;

            // Nobody has won yet.
            winner = null;
        }

        //Starts or restarts the game.
        public void StartGame()
        {
            // Resets the turn counter.
            num_turns = 0;

            // Player 1 starts.
            currentplayer = p1;

            // Marks the game as active.
            gameIsOver = false;

            // Clears the previous winner.
            winner = null;

            // Resets both players' scores.
            p1.score = 0;
            p2.score = 0;

            // Resets both players' shot counters.
            p1.shotsFired = 0;
            p2.shotsFired = 0;
        }

        // Switches between Player 1 and Player 2.
        public void SwitchTurn()
        {
            // Do not switch turns after the game ends.
            if (gameIsOver)
            {
                return;
            }

            // Checks whether it is Player 1's turn.
            if (currentplayer == p1)
            {
                // Switches to Player 2.
                currentplayer = p2;
            }
            else
            {
                // Switches to Player 1.
                currentplayer = p1;
            }

            // Increases the turn counter.
            num_turns++;
        }

        // Records the winner and ends the game.
                                         //// This is part of week two of game project, 
                                         /// we will finish wiring it up in the game then
        //public void CheckWinner()
        //{
        //    // Checks whether Player 1 has lost.
        //    if (p1.HasLost())
        //    {
        //        // Player 2 wins.
        //        winner = p2;

        //        // Ends the game.
        //        gameIsOver = true;
        //    }
        //    // Checks whether Player 2 has lost.
        //    else if (p2.HasLost())
        //    {
        //        // Player 1 wins.
        //        winner = p1;

        //        // Ends the game.
        //        gameIsOver = true;
        //    }
        //}
    }
}