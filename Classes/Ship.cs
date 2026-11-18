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
            //TODO this code is ugly and unwieldy; it works right now,
            //but we should be able to use game.currentPlayer to clean it up,
            //or pass the player directly

            //flip hit cell's isHit bool
            if (mainContext.game.currentplayer == mainContext.game.p1)
            {
                //only change isHit if the cell hasn't been hit before
                if (mainContext.game.p2.board.p1_board[position[0], position[1]].isHit == false)
                {
                    mainContext.game.p2.board.p1_board[position[0], position[1]].isHit = true;
                    //add one to the ship's number of hits taken
                    hits++;

                    //check if boat is sunk
                    IsSunk(mainContext.game.p2.board.p1_board);
                }
            }
            else
            {
                if (mainContext.game.p1.board.p1_board[position[0], position[1]].isHit == false)
                {
                    mainContext.game.p1.board.p1_board[position[0], position[1]].isHit = true;

                    hits++;

                    //check if boat is sunk
                    IsSunk(mainContext.game.p1.board.p1_board);
                }
            }
        }


        public bool IsSunk(Cell[,] playerBoard)
        {
            // An unplaced ship cannot be sunk
            //TODO this always returns false, we need to figure out why
            if (this.positions == null || this.positions.Count == 0)
            {
                return false;
            }

            // Check every position occupied by this ship
            foreach (int[] place in positions)
            {
                if (playerBoard[place[0], place[1]].isHit == false)
                {
                    return false;
                }
            }
            //this section will only run if the ship has sunk

            //display message "You sunk the [ship name]!"
            mainContext.DisplaySunkMessage(this.name);

            //remove ship from fleet list
            mainContext.game.currentplayer.board.fleet.Remove(this);
            return true;
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


        public bool PlaceShip(Cell[,] Playerboard, (int, int) focus, out List<int[]> positions)
        {
            //vars to pass result from first if-else to second
            bool shipFits = false;
            bool verticalAlign = true;

            if (mainContext.HorizontalOption.IsChecked == true)
            {
                verticalAlign = false;
            }

            //determine whether ship is in row or column
            if (verticalAlign == true)
            {
                //vertical: check if ship fits onto board top to bottom
                if (Playerboard.GetLength(0) >= this.size + focus.Item1)
                {
                    //ship fits on board
                    int row = focus.Item1;
                    int col = focus.Item2;
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
                else
                {
                    //ship pokes off of map
                }
            }
            else
            {
                //horizontal: check if ship fits onto board left to right
                if (Playerboard.GetLength(1) >= this.size + focus.Item2)
                {
                    //ship fits on board
                    int row = focus.Item1;
                    int col = focus.Item2;
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
                    //ship doesn't fit
                }
            }

            if (shipFits)
            {
                //populate positions lists
                //redeclare variables (redeclared instead of expanding scope to prevent
                //previous code from changing position
                int row = focus.Item1;
                int col = focus.Item2;
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
                foreach (int[] position in this.positions)
                {
                    Playerboard[position[0], position[1]].ship = this;
                }

                //remove ship option from selector
                mainContext.ShipSelector.Items.Remove(mainContext.ShipSelector.SelectedItem);

                //TODO call method to update color of affected cells
            }
            else
            {
                //ship can not be placed here; warn player the ship was not placed.
            }

            positions = this.positions;
            return shipFits;
        }

    }
}