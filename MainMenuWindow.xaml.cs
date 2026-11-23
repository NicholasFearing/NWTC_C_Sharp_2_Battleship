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
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        private void MenuQuitButton_Click(object sender, RoutedEventArgs e)
        {
            //closes the program
            Application.Current.Shutdown();
        }

        private void MenuAboutButton_Click(object sender, RoutedEventArgs e)
        {
            //show information regarding developers, version
            AboutWindow about = new AboutWindow();

            about.Owner = this;
            about.ShowDialog();
        }


        private void MenuStartButton_Click(
            object sender, RoutedEventArgs e)
        {
            // Open player setup
            PlayerSetupWindow setup = new PlayerSetupWindow();
            setup.Owner = this;

            if (setup.ShowDialog() == true)
            {
                // Create game window using entered names
                MainWindow main = new MainWindow(
                    setup.Player1Name,
                    setup.Player2Name);

                Application.Current.MainWindow = main;
                main.Show();

                Close();
            }
        }

    }
}
