using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Battleship
{
    /// <summary>
    /// Represents single cell of gameboard grid.
    /// </summary>
    public class Cell
    {
        // Stores the ship occupying this cell.
        // Null means the cell is empty.
        public Ship? ship;

        // Records whether this cell has been hit by an attack.
        public bool isHit;

        // Constructor: Creates an empty cell.
        public Cell()
        {
            // No ship is placed in this cell initially.
            ship = null;

            // The cell has not been hit.
            isHit = false;
        }

        // Checks whether this cell contains a ship.
        public bool ContainsShip()
        {
            // Returns true if a ship exists in this cell.
            return ship != null;
        }
    }
}