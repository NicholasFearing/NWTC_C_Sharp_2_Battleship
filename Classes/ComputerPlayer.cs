using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Battleship
{
    /// <summary>
    /// Represents CPU players; inherits from player class
    /// </summary>

    //AI needs to be fully functional by week 6 of class
    //  & needs to be able to make random shots by week 4

    public class ComputerPlayer : Player
    {
        //fields
        public string difficulty;

        private Random rng = new Random();

        //methods
        public ComputerPlayer(MainWindow main, string difficulty)
            : base(main, "Computer")
        {
            //constructor
            this.difficulty = difficulty;
        }

        /*TODO public (int row, int col) ChooseAttack()
        { }*/

        /* public void ChoosePosition()
         {
             if (board.fleet.Count > 0)
             {
                 foreach (Ship ship in this.board.fleet.ToList())
                     board.RandomEnemyShip(ship);
             }
         } */

        public void ChoosePosition()
        {
            int shipsToPlace = 4;

            while (shipsToPlace >= 0)
            {
                (int row, int col) startPosition = (rng.Next(0, 10), rng.Next(0, 10));

                if (mainContext.game.p2.board.fleet[shipsToPlace].PlaceShip(
                    mainContext.game.p2.board.p1_board, startPosition, out List<int[]> positions))
                {
                    shipsToPlace--;
                }
            }
        }

    }

}
