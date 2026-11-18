using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game_Battleship
{
    public class GameBoard
    {
        // Stores the ships placed in the game.
        public List<Ship> fleet;

        // Stores the number of rows.
        public int rows = 10;

        // Stores the number of columns.
        public int columns = 10;

        // Stores Player 1's board.
        public Cell[,] p1_board;

        // Stores Player 2's board.
        public Cell[,] p2_board;

        // Constructor: Creates both game boards.
        public GameBoard(MainWindow main)
        {
            // Creates a 10 x 10 array for Player 1.
            p1_board = new Cell[rows, columns];

            // Creates a 10 x 10 array for Player 2.
            p2_board = new Cell[rows, columns];

            // Creates list to contain ships
            //      this needs to be filled in
            fleet = new List<Ship>()
            {
                new Ship(main, "Carrier", 5),
                new Ship(main, "Battleship", 4),
                new Ship(main, "Destroyer", 3),
                new Ship(main, "Submarine", 3),
                new Ship(main, "Patrol Boat", 2)
            };

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
            //this deletes the fleet which was just instantiated.
            //new game creates new gameboard anyway, so it will have a cleared fleet.
            //fleet.Clear();
        }

        // Checks whether all ships in the fleet are sunk.
        public bool allshipssunk(Cell[,] playerBoard)
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
                if (ship.IsSunk(playerBoard) == false)
                {
                    return false;
                }
            }

            // All ships in the fleet have sunk.
            return true;
        }

        // allows us to generate random positions
        private Random random = new Random();


        public void RandomEnemyShip(Ship ship)
        {
            bool shipPlaced = false;

            while (!shipPlaced)
            {
                // Choose a random starting position.
                int row = random.Next(0, rows);
                int column = random.Next(0, columns);

                // Choose a random direction.
                bool horizontal = random.Next(0, 2) == 0;

                // Check whether the ship fits.
                if (horizontal && column + ship.size > columns)
                {
                    continue;
                }

                if (!horizontal && row + ship.size > rows)
                {
                    continue;
                }

                // Check whether another ship occupies these cells.
                bool canPlace = true;

                for (int i = 0; i < ship.size; i++)
                {
                    int newRow = row;
                    int newColumn = column;

                    if (horizontal)
                    {
                        newColumn += i;
                    }
                    else
                    {
                        newRow += i;
                    }

                    if (p1_board[newRow, newColumn].ship != null)
                    {
                        canPlace = false;
                        break;
                    }
                }

                // Try another position if cells are occupied.
                if (!canPlace)
                {
                    continue;
                }

                // Place the ship.
                for (int i = 0; i < ship.size; i++)
                {
                    int newRow = row;
                    int newColumn = column;

                    if (horizontal)
                    {
                        newColumn += i;
                    }
                    else
                    {
                        newRow += i;
                    }

                    p1_board[newRow, newColumn].ship = ship;

                    ship.positions.Add(new int[] { newRow, newColumn });
                }

                fleet.Add(ship);
                shipPlaced = true;
            }
        }
    }
}