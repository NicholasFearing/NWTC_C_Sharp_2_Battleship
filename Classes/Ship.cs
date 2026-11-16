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

        ///constructors
        ///TODO first constructor can be removed if nothing calls it
        public Ship(MainWindow main)
        {
            name = "";
            size = 0;
            hits = 0;
            positions = new List<int[]>();
            mainContext = main;
        }

        public Ship(MainWindow main, string shipName, int shipSize)
        {
            name = shipName;
            size = shipSize;
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
                if (this.IsSunk(mainContext.game.p1.board.p1_board)) ;
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


        public void PlaceShip(Cell[,] Playerboard)
        {
            //vars to pass result from first if-else to second
            bool shipFits = false;
            bool verticalAlign = true;

            if (mainContext.HorizontalOption.IsChecked == true)
            {
                verticalAlign = false;
            }
            /* do i need this chunk, when the playerboard is passed as argument?
             * consider removal.
            //determine which player is placing the ship
            if (mainContext.game.currentplayer == mainContext.game.p1)
            {
                //Player one is the player
                //pseudocode until I have uniformgrid example
                //UniformGridNameHere.SelectedButton.
            }
            else
            {
            } */

            //determine whether ship is in row or column
            if (verticalAlign == false)
            {
                //horizontal: check if ship fits onto board left to right
                if (Playerboard.GetLength(0) >= this.size + mainContext.focus.Item1 - 1)
                {
                    //ship fits on board
                    int row = mainContext.focus.Item1;
                    int col = mainContext.focus.Item2;
                    bool shipAlready = false;

                    //check that nothing is in the spots it would take up
                    for (int i = 0; i < this.size; i++)
                    {
                        if (Playerboard[row, col].ContainsShip())
                        {
                            shipAlready = true;
                        }
                        col++;
                    }

                    if (!shipAlready)
                    { shipFits = true; }
                }
                else
                {
                    //ship pokes off of map
                }
            }
            else
            {
                //vertical: check if ship fits onto board top to bottom
                if (Playerboard.GetLength(1) >= this.size + mainContext.focus.Item2 - 1)
                {
                    //ship fits on board
                    int row = mainContext.focus.Item1;
                    int col = mainContext.focus.Item2;
                    bool shipAlready = false;

                    //check that nothing is in the spots it would take up
                    for (int i = 0; i < this.size; i++)
                    {
                        if (Playerboard[row, col].ContainsShip())
                        {
                            shipAlready = true;
                        }
                        row++;
                    }

                    if (!shipAlready)
                    { shipFits = true; }
                }
            }

            if (shipFits)
            {
                //populate positions lists
                //redeclare variables (redeclared instead of expanding scope to prevent
                //previous code from changing position
                int row = mainContext.focus.Item1;
                int col = mainContext.focus.Item2;
                if (verticalAlign)
                {
                    //ship is in one column
                    for (int i = 0; i < this.size; i++)
                    {
                        this.positions.Add(new int[] { row, col });
                        row++;
                    }
                }
                else
                {
                    //ship is in one row
                    for (int i = 0; i < this.size; i++)
                    {
                        this.positions.Add(new int[] { row, col });
                        col++;
                    }
                }
                //update game board array
                foreach (int[] position in positions)
                {
                    Playerboard[position[0], position[1]].ship = this;
                }

                //TODO gray out/disable option to place this ship in the ui, as it's already placed
                //call method to update color of affected cells
            }
            else
            {
                //ship can not be placed here; warn player the ship was not placed.
            }

        }

    }
}