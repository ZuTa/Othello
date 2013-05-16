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

namespace Othello.Game
{
    /// <summary>
    /// Interaction logic for CellControl.xaml
    /// </summary>
    public partial class CellControl : UserControl
    {
        private ImageBalls ball;
        public ImageBalls Ball
        {
            get { return ball; }
            set
            {
                ball = value;
                img.Source = (BitmapImage)App.Current.FindResource(Ball.ToString());
            }
        }

        private int id;
        public int ID
        {
            get { return id; }
        }

        private Game game;

        public CellControl()
        {
        }

        public CellControl(int id, Game game, ImageBalls ball)
        {
            InitializeComponent();
            this.id = id;
            this.game = game;
            Ball = ball;
        }

        private void img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!game.IsFinished)
                game.DoStepStuff(this);
        }

    }
}
