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

        //the instance of the game
        public Game game;

        public MainWindow()
        {
            InitializeComponent();

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

            // Connect to GameBoard and Player later
            // row and col identify which cell was clicked
            // board identifies which grid was clicked
            
            //^^work in progress zone^^

            StatusText.Text =
                "Selected: " +
                ((char)('A' + col)) + (row + 1);
        }

        // Start game button
        private void StartButton_Click(
            object sender, RoutedEventArgs e)
        {
            // Connect to Game.startGame() when merged
            StatusText.Text = "Start Game selected.";
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
            game = new Game(this);
        }
    }
}
