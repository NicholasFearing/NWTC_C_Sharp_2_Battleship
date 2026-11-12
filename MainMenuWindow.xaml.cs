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
            this.Close();
        }

        private void MenuAboutButton_Click(object sender, RoutedEventArgs e)
        {
            //show information regarding developers, version
            AboutWindow about = new AboutWindow();

            about.Owner = this;
            about.ShowDialog();
        }

        private void MenuStartButton_Click(object sender, RoutedEventArgs e)
        {
            //start a new game of battleship by loading MainWindow.xaml
            MainWindow main = new MainWindow();
            main.Show();
        }
    }
}
