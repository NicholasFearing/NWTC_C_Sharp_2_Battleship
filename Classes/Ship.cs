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
        public string Name;

        public int Size;

        public int Hits;

        public List<int[]> Positions;

        //methods

        /// <summary>
        /// Records damage to ship
        /// </summary>
        public void TakeHit(int[] position)
        {
            //flip hit cell's isHit bool
            if (Game.currentplayer == Game.p1)
            {
                GameBoard.p2_board[position[0], position[1]].isHit = true;

                //check if boat is sunk
                if (this.IsSunk(GameBoard.p2_board));
            }
            else
            {
                GameBoard.p1_board[position[0], position[1]].isHit = true;

                //check if boat is sunk
                if (this.IsSunk(GameBoard.p1_board));
            }

            //add one to the ship's number of hits taken
            Hits++;
        }

        public bool IsSunk(Cell[,] PlayerBoard)
        {
            bool isSunk = true;

            int row = 0;
            int col = 0;
            foreach (int[] place in Positions)
            {
                if (PlayerBoard[place[0],place[1]].isHit == false)
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
            int RemainingHealth = 0;

            foreach (int[] place in Positions)
            {
                if (PlayerBoard[place[0],place[1]].isHit == false)
                {
                    RemainingHealth++;
                }
            }

            return RemainingHealth;
        }

        public void PlaceShip(Cell[] Playerboard)
        {
            //determine which player is placing the ship
            if (Game.currentplayer == Game.p1)
            {
                //Player one is the player
                //pseudocode until I have uniformgrid example
                //UniformGridNameHere.SelectedButton.
            }
            else
            }
            }
        }
    }
}
