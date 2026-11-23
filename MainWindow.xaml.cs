using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game_Battleship
{
    public partial class MainWindow : Window
    {
        // Tracks which players boards are being shown
        private int displayedPlayer = 1;
        private int nextDisplayedPlayer = 1;

        // Stores the selected attack position
        private int selectedShotRow = -1;
        private int selectedShotCol = -1;

        // Tracks whether the player has fired this turn
        private bool shotFiredThisTurn = false;

        // Track completed ship placements
        private bool player1PlacementReady = false;
        private bool player2PlacementReady = false;

        private string player1Name;
        private string player2Name;

        //the instance of the game
        public Game game;

        public (int, int) focus;

        public MainWindow(string name1, string name2)
        {
            InitializeComponent();

            player1Name = name1;
            player2Name = name2;

            CreateLabels();

            // Create all four baords
            CreateBoard(Player1TopGrid);
            CreateBoard(Player1BottomGrid);
            CreateBoard(Player2TopGrid);
            CreateBoard(Player2BottomGrid);

            ShowPlayerBoards(1);
        }

        // Generate 100 buttons for each board
        private void CreateBoard(UniformGrid board)
        {
            board.Children.Clear();

            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    Button button = new Button();

                    button.Background = Brushes.DodgerBlue;
                    button.Margin = new Thickness(1);

                    // Save the row and column
                    button.Tag = (row, col);

                    button.Click += Cell_Click;

                    board.Children.Add(button);
                }
            }
        }

        // Create A-J and 1-10 for a board
        private void CreateBoardLabels(
            UniformGrid columnLabels,
            UniformGrid rowLabels)
        {
            columnLabels.Children.Clear();
            rowLabels.Children.Clear();

            for (int i = 0; i < 10; i++)
            {
                string letter =
                   ((char)('A' + i)).ToString();

                string number = (i + 1).ToString();

                columnLabels.Children.Add(
                    MakeLabel(letter));

                rowLabels.Children.Add(
                    MakeLabel(number));
            }
        }

        // Creates the labels for all four boards
        private void CreateLabels()
        {
            CreateBoardLabels(
                Player1TopColumnLabels,
                Player1TopRowLabels);

            CreateBoardLabels(
                Player1BottomColumnLabels,
                Player1BottomRowLabels);

            CreateBoardLabels(
                Player2TopColumnLabels,
                Player2TopRowLabels);

            CreateBoardLabels(
                Player2BottomColumnLabels,
                Player2BottomRowLabels);
        }

        private TextBlock MakeLabel(string text)
        {
            return new TextBlock
            {
                Text = text,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold
            };
        }

        // Get one of the four grids
        public UniformGrid GetBoardGrid(
            int playerNumber, bool topBoard)
        {
            if (playerNumber == 1)
            {
                return topBoard
                    ? Player1TopGrid
                    : Player1BottomGrid;
            }

            if (playerNumber == 2)
            {
                return topBoard
                    ? Player2TopGrid
                    : Player2BottomGrid;
            }

            throw new System.ArgumentOutOfRangeException(
                nameof(playerNumber));
        }

        // Change the color of a single square
        public void SetCellColor(
            int playerNumber,
            bool topBoard,
            int row,
            int col,
            Brush color)
        {
            if (row < 0 || row >= 10 ||
                col < 0 || col >= 10)
            {
                return;
            }

            UniformGrid board =
                GetBoardGrid(playerNumber, topBoard);

            int index = row * 10 + col;

            Button button =
                (Button)board.Children[index];

            button.Background = color;
        }

        // Display whether an attack was a hit or miss
        private void DisplayAttackResult(
            int playerNumber,
            int row,
            int col,
            bool isHit)
        {
            // Check if the attack was a hit
            if (isHit)
            {
                SetCellColor(
                    playerNumber, true, row, col, Brushes.Red);

                StatusText.Text = "HIT!";
            }
            else
            {
                SetCellColor(
                    playerNumber, true, row, col, Brushes.Gray);

                StatusText.Text = "MISS!";
            }
        }

        // Announce when a ship has been sunk
        public void DisplaySunkMessage(string shipName)
        {
            StatusText.Text = "You sunk the " + shipName + "!";
        }

        // Announce the winner and end the game
        public void DisplayWinner()
        {
            if (game.gameIsOver == true)
            {
                string winningPlayer = "Player 2";

                if (game.winner == game.p1)
                {
                    winningPlayer = "Player 1";
                }

                StatusText.Text = winningPlayer + " Wins!";

                MessageBox.Show(
                    winningPlayer + " Wins!",
                    "Game Over");

                // Stop further attacks
                Player1TopGrid.IsEnabled = false;
                Player2TopGrid.IsEnabled = false;
            }
        }

        // Display only one players boards
        public void ShowPlayerBoards(int playerNumber)
        {
            if (playerNumber != 1 && playerNumber != 2)
                return;

            displayedPlayer = playerNumber;

            Player1BoardsPanel.Visibility =
                playerNumber == 1
                ? Visibility.Visible
                : Visibility.Collapsed;

            Player2BoardsPanel.Visibility =
                playerNumber == 2
                ? Visibility.Visible
                : Visibility.Collapsed;

            StatusText.Text =
                "Player " + playerNumber + "'s boards.";
        }

        // All the buttons use this one method
        private void Cell_Click(
            object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            var (row, col) =
                ((int, int))button.Tag;

            UniformGrid board =
                (UniformGrid)button.Parent;

            this.focus = (row, col);

            // Do not allow a previously fired square to be selected
            if (button.Background == Brushes.Red ||
                button.Background == Brushes.Gray)
            {
                StatusText.Text = "You already fired at this square.";
                return;
            }


            // Select an attack position on the top board
            if (board == Player1TopGrid ||
                board == Player2TopGrid)
            {
                // Prevent more than one shot per turn
                if (shotFiredThisTurn)
                {
                    StatusText.Text = "You have already fired this turn.";
                    return;
                }

                // Do not allow shots after the game ends
                if (game.gameIsOver)
                {
                    return;
                }


                // Restore the previous selected square to blue
                if (selectedShotRow != -1 && selectedShotCol != -1)
                {
                    SetCellColor(
                        displayedPlayer,
                        true,
                        selectedShotRow,
                        selectedShotCol,
                        Brushes.DodgerBlue);
                }

                // Save the newly selected square
                selectedShotRow = row;
                selectedShotCol = col;

                // Highlight the selected square yellow
                SetCellColor(
                    displayedPlayer,
                    true,
                    row,
                    col,
                    Brushes.Gold);

                // Enable Confirm Shot
                ConfirmShotButton.IsEnabled = true;


                StatusText.Text =
                    "Selected " +
                    ((char)('A' + col)) + (row + 1) +
                    ". Click Confirm Shot to fire.";
            }

            else
            {
                // Bottom board is for ship placement
                Ship currentShip = new Ship(this);

                //find ship in dropdown
                if (displayedPlayer == 1)
                {
                    foreach (Ship ship in this.game.p1.board.fleet)
                    {
                        if (this.ShipSelector.Text == ship.name)
                        {
                            currentShip = ship;
                        }
                    }
                }
                else
                {
                    //ship was not on list
                }

                if (currentShip.PlaceShip(game.currentplayer.board.p1_board, focus, out List<int[]> positions))
                {
                    foreach (int[] cell in positions)
                    {
                        SetCellColor(displayedPlayer, false, cell[0], cell[1], Brushes.Snow);
                    }

                    // Count successfully placed ships
                    if (displayedPlayer == 1)
                    {
                        player1ShipsPlaced++;

                        if (player1ShipsPlaced >= 5)
                        {
                            player1PlacementReady = true;
                        }
                    }
                    else
                    {
                        player2ShipsPlaced++;

                        if (player2ShipsPlaced >= 5)
                        {
                            player2PlacementReady = true;
                        }
                    }

                    // Update the Start Battle button
                    UpdateStartButton();
                }

                StatusText.Text =
                    "Selected: " +
                    ((char)('A' + col)) + (row + 1);
            }

            //switch turn after each shot;
            //this is commented out as we are not switching turns this week
            //game.SwitchTurn();
        }


        private void ConfirmShotButton_Click(
            object sender, RoutedEventArgs e)
        {
            // Make sure a square was selected
            if (selectedShotRow == -1 || selectedShotCol == -1)
            {
                return;
            }

            // Save the selected position
            int row = selectedShotRow;
            int col = selectedShotCol;

            // Clear the selection
            selectedShotRow = -1;
            selectedShotCol = -1;

            ConfirmShotButton.IsEnabled = false;

            // Use the team's existing attack method
            game.currentplayer.Attack(
                row, col, game.currentplayer.board.p2_board);

            // Display the result on the attack grid
            DisplayAttackResult(
                displayedPlayer,
                row,
                col,
                game.currentplayer.board.p2_board[row, col].isHit);

            // Lock additional shots for this turn
            shotFiredThisTurn = true;
            ConfirmShotButton.IsEnabled = false;
        }


        // Check whether both players have placed their ships
        private void UpdateStartButton()
        {
            // TEMPORARY: Only require Player 1's ships
            // TODO: Require both players before final submission

            StartButton.IsEnabled =
                player1PlacementReady;

            // && player2PlacementReady
        }

        // Count successful ship placements
        private int player1ShipsPlaced = 0;
        private int player2ShipsPlaced = 0;
        

        // Start game button
        private void StartButton_Click(
            object sender, RoutedEventArgs e)
        {
            // Start the game
            game.StartGame();

            game.p1.board.p2_board = game.p2.board.p1_board;

            // Show the attack boards
            Player1AttackPanel.Visibility = Visibility.Visible;
            Player2AttackPanel.Visibility = Visibility.Visible;

            // Allow players to select targets
            Player1TopGrid.IsEnabled = true;
            Player2TopGrid.IsEnabled = true;

            // Hide ship placement controls
            ShipPlacementControls.Visibility = Visibility.Collapsed;

            // Show the Confirm Shot button
            ConfirmShotButton.Visibility = Visibility.Visible;
            ConfirmShotButton.IsEnabled = false;

            // Hide Start Battle
            StartButton.Visibility = Visibility.Collapsed;

            StatusText.Text = "Select a square on the Target Grid.";

            // Show End Turn during gameplay
            EndTurnButton.Visibility = Visibility.Visible;
        }


        // Show the turn switching screen
        public void ShowTurnOverlay(
            string message, int nextPlayer)
        {
            if (nextPlayer != 1 && nextPlayer != 2)
                return;

            nextDisplayedPlayer = nextPlayer;

            TurnMessage.Text = message;
            TopControls.IsEnabled = false;

            TurnOverlay.Visibility = Visibility.Visible;
        }

        // End turn button
        private void EndTurnButton_Click(
            object sender, RoutedEventArgs e)
        {
            // UI demo only, not Game.switchTurn()
            int nextPlayer =
                displayedPlayer == 1 ? 2 : 1;

            ShowTurnOverlay(
                "Pass to Player " + nextPlayer,
                nextPlayer);
        }

        // Show the next players boards
        public void RevealBoards()
        {
            ShowPlayerBoards(nextDisplayedPlayer);

            // Allow the next player to fire
            shotFiredThisTurn = false;

            // Clear the previous player's shot selection
            selectedShotRow = -1;
            selectedShotCol = -1;
            ConfirmShotButton.IsEnabled = false;

            TurnOverlay.Visibility = Visibility.Collapsed;
            TopControls.IsEnabled = true;
        }

        // Ready button
        private void ReadyButton_Click(
            object sender, RoutedEventArgs e)
        {
            // For now this switches the visible UI
            // Team game logic will manage actual turns

            RevealBoards();
        }

        // Return to the main menu
        private void ReturnToMenu_Click(
            object sender, RoutedEventArgs e)
        {
            MainMenuWindow menu = new MainMenuWindow();

            Application.Current.MainWindow = menu;
            menu.Show();

            Close();
        }

        // Quit game
        private void QuitGame_Click(
            object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // About popup
        private void AboutGame_Click(
            object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();

            about.Owner = this;
            about.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            game = new Game(this, player1Name, player2Name);
        }
    }
}

