using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Game_Battleship
{
    /// <summary>
    /// Interaction logic for PlayerSetupWindow.xaml
    /// </summary>
    public partial class PlayerSetupWindow : Window
    {
        // Names entered by the players
        public string Player1Name { get; private set; }
        public string Player2Name { get; private set; }

        public PlayerSetupWindow()
        {
            InitializeComponent();
        }

        // Validate names and continue
        private void ContinueButton_Click(
            object sender, RoutedEventArgs e)
        {
            string name1 = Player1NameTextBox.Text.Trim();
            string name2 = Player2NameTextBox.Text.Trim();

            // Make sure both names are entered
            if (name1 == "" || name2 == "")
            {
                MessageBox.Show(
                    "Please enter both player names.",
                    "Player Setup");

                return;
            }

            // Save the names
            Player1Name = name1;
            Player2Name = name2;

            // Return to the window that opened setup
            DialogResult = true;
        }

        // Return without starting
        private void BackButton_Click(
            object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

