using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Battleship
{
    public class GameBoard
    {
        // Stores the ships placed in the game.
        public List<Ship> fleet = new List<Ship>();

        // Stores the number of rows.
        public int rows = 10;

        // Stores the number of columns.
        public int columns = 10;

        // Stores Player 1's board.
        public Cell[,] p1_board;

        // Stores Player 2's board.
        public Cell[,] p2_board;

        // Constructor: Creates both game boards.
        public GameBoard()
        {
            // Creates a 10 x 10 array for Player 1.
            p1_board = new Cell[rows, columns];

            // Creates a 10 x 10 array for Player 2.
            p2_board = new Cell[rows, columns];

            // Calls the method that initializes both boards.
            initializeboard();
        }

        //Initializes all cells on both boards.
        public void initializeboard()
        {
            // Goes through every row.
            for (int row = 0; row < rows; row++)
            {
                // Goes through every column.
                for (int column = 0; column < columns; column++)
                {
                    // Creates an empty cell for Player 1.
                    p1_board[row, column] = new Cell();

                    // Creates an empty cell for Player 2.
                    p2_board[row, column] = new Cell();
                }
            }

            // Clears the ship list when starting a new game.
            fleet.Clear();
        }

        // Checks whether all ships in the fleet are sunk.
        public bool allshipssunk()
        {
            // If there are no ships, the fleet is not considered sunk.
            if (fleet.Count == 0)
            {
                return false;
            }

            // Goes through every ship in the fleet.
            foreach (Ship ship in fleet)
            {
                // If at least one ship is not sunk, return false.
                if (!ship.IsSunk())
                {
                    return false;
                }
            }

            // All ships in the fleet have sunk.
            return true;
        }
    }
}