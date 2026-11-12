using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Game_Battleship
{
    /// <summary>
    /// Represents an individual ship
    /// </summary>
    public class Ship
    {
        //fields
        public string name;

        public int size;

        public int hits;

        public List<int[]> positions;

        private MainWindow mainContext;

        ///constructor
        public Ship(MainWindow main)
        {
            name = "";
            size = 0;
            hits = 0;
            positions = new List<int[]>();
            mainContext = main;
        }

        //methods

        /// <summary>
        /// Records damage to ship
        /// </summary>
        /// 
        public void TakeHit(int[] position, Player player)
        {
            //flip hit cell's isHit bool
            if (mainContext.game.currentplayer == mainContext.game.p1)
            {
                mainContext.game.p2.board.p2_board[position[0], position[1]].isHit = true;

                //check if boat is sunk
                if (IsSunk(mainContext.game.p2.board.p2_board))
                {
                    //display message "You sunk my [ship name]!"
                    //remove ship from fleet list
                }
                ;
            }
            else
            {
                mainContext.game.p1.board.p1_board[position[0], position[1]].isHit = true;

                //check if boat is sunk
                if (this.IsSunk(mainContext.game.p1.board.p1_board));
            }

            //add one to the ship's number of hits taken
            hits++;
        }

        public bool IsSunk(Cell[,] playerBoard)
        {
            bool isSunk = true;

            int row = 0;
            int col = 0;
            foreach (int[] place in positions)
            {
                if (playerBoard[place[0], place[1]].isHit == false)
                {
                    isSunk = false;
                }
            }

            return isSunk;
        }

        /// <summary>
        /// Returns ships remaining unhit spots as int
        /// </summary>
        /// <returns></returns>
        public int GetRemainingHealth(Cell[,] PlayerBoard)
        {
            int remainingHealth = 0;

            foreach (int[] place in positions)
            {
                if (PlayerBoard[place[0], place[1]].isHit == false)
                {
                    remainingHealth++;
                }
            }

            return remainingHealth;
        }
        
        /*
        public void PlaceShip(Cell[] Playerboard)
        {
            //vars to pass result from first if-else to second
            bool shipFits = false;
            bool verticalAlign = false;

            //determine whether ship is in row or column
            if (mainContext.HorizontalOption.IsChecked == true)
            {
                //check if ship fits onto board left to right
            }
            else
            {
                //check if ship fits onto board top to bottom
            }

            //determine which player is placing the ship
            if (mainContext.game.currentplayer == mainContext.game.p1)
            {
                //Player one is the player
                //pseudocode until I have uniformgrid example
                //UniformGridNameHere.SelectedButton.
            }
            else
            {
            }
        }
        */
    }
}