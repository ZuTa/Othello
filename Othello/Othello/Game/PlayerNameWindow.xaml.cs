using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Othello.Game
{
    /// <summary>
    /// Interaction logic for PlayerNameWindow.xaml
    /// </summary>
    public partial class PlayerNameWindow : Window
    {
        private PlayersInfo playersInfo;
        public PlayersInfo PlayersInfo
        {
            get { return playersInfo; }
        }

        public PlayerNameWindow()
        {
            InitializeComponent();
            playersInfo = new PlayersInfo();
            this.DataContext = playersInfo;
            PlayersInfo.NameOfPlayer2 = "Computer";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PlayersInfo.Verify(playersInfo))
                this.Close();
            else
                MessageBox.Show("Incorrect data!");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!PlayersInfo.Verify(playersInfo))
                e.Cancel = true;
        }
    }
}
