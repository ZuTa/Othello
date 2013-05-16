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
using System.Globalization;

namespace Othello.Game
{
	/// <summary>
	/// Interaction logic for GameWindow.xaml
	/// </summary>
	public partial class GameWindow : Window
	{
        private Game game;
        string name1 = "";
        string name2 = "";

		public GameWindow(string name1, string name2)
		{
			InitializeComponent();

            this.name1 = name1;
            this.name2 = name2;
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            game = new Game(name1, name2);
            game.OnGameFinished = FinishingGame;
            game.Start();
            this.DataContext = game;
        }

        private void FinishingGame(object sender)
        {
            Game game = (Game)sender;

            if (MessageBox.Show("Game is over! Do you want to start new game?", "Othello", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                game.Start();
                this.DataContext = game;
            }
        }
	}

    public class PositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageBalls ball = ImageBalls.None;
            if (value != null)
            {
                switch ((GameStep)value)
                {
                    case GameStep.Blue:
                        ball = ImageBalls.BlueBall;
                        break;
                    case GameStep.Red:
                        ball = ImageBalls.RedBall;
                        break;
                    default:
                        ball = ImageBalls.None;
                        break;
                }
            }
            if (ball == ImageBalls.None)
                return DependencyProperty.UnsetValue;
            else
                return (BitmapImage)App.Current.FindResource(ball.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
