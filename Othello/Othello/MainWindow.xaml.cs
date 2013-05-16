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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Othello.Game;

namespace Othello
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	public partial class MainWindow : Window
	{
        private SettingWindow sw = null;
        private GameWindow gw = null;

        private string nameOfPlayer1;
        private string nameOfPlayer2;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void btnSettings_Click(object sender, RoutedEventArgs e)
		{
            if (sw == null)
            {
                sw = new SettingWindow();
                sw.Show();
            }
            else
            {
                sw.WindowState = WindowState.Normal;
                sw.Focus();
            }
            sw.Closed += new EventHandler(sw_Closed);
		}

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            switch (Game.Game.GetGameStyle())
            {
                case GameStyle.Single:
                    PlayerNameWindow p = new PlayerNameWindow();
                    p.ShowDialog();
                    nameOfPlayer1 = p.PlayersInfo.NameOfPlayer1;
                    nameOfPlayer2 = p.PlayersInfo.NameOfPlayer2;
                    break;
                case GameStyle.Multi:
                    PlayersNameWindow pnw = new PlayersNameWindow();
                    pnw.ShowDialog();
                    nameOfPlayer1 = pnw.PlayersInfo.NameOfPlayer1;
                    nameOfPlayer2 = pnw.PlayersInfo.NameOfPlayer2;
                    break;
                default:
                    break;
            }            

            if (gw == null)
            {
                gw = new GameWindow(nameOfPlayer1, nameOfPlayer2);
                gw.Show();
            }
            else
            {
                gw.WindowState = WindowState.Normal;
                gw.Focus();
            }

            gw.Closed += new EventHandler(gw_Closed);
        }

        private void gw_Closed(object sender, EventArgs e)
        {
            this.Show();
            gw = null;
        }

        private void sw_Closed(object sender, EventArgs e)
        {
            sw = null;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {

        }

	}
}
