using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Othello.Game
{
    #region Enums

    public enum GameStyle
    {
        Single,
        Multi
    }

    public enum GameLevel
	{
		Standart = 8,
		High = 16,
		Master = 32
	}

    public enum GameStep
    {
        Red,
        Blue,
        None
    }

    public enum ImageBalls
    {
        None,
        BlueBall,
        RedBall
    }

    #endregion

    #region Delegates

    public delegate void OnGameFinished(object sender);

    #endregion

    public class Game : INotifyPropertyChanged
    {

        #region Fields and Properties

        private readonly int countStep = 2;

        private GameLevel level = GameLevel.Standart;
        private GameStyle style = GameStyle.Single;

        private GameStep step;
        public GameStep Step
        {
            get { return step; }
            set
            {
                step = value;
                NotifyPropertyChanged("Step");
            }
        }

        private bool isFinished;
        public bool IsFinished
        {
            get { return isFinished; }
            set
            {
                isFinished = value;
                NotifyPropertyChanged("IsFinished");
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged("Message");
            }
        }

        private OnGameFinished ongamefinished;
        public OnGameFinished OnGameFinished
        {
            get { return ongamefinished; }
            set { ongamefinished = value; }
        }

        private string nameOfPlayer1;
        public string NameOfPlayer1
        {
            get { return nameOfPlayer1; }
            set
            {
                nameOfPlayer1 = value;
                NotifyPropertyChanged("NameOfPlayer1");
            }
        }
        private string nameOfPlayer2;
        public string NameOfPlayer2
        {
            get { return nameOfPlayer2; }
            set
            {
                nameOfPlayer2 = value;
                NotifyPropertyChanged("NameOfPlayer2");
            }
        }

        private int countRedBalls;
        public int CountRedBalls
        {
            get { return countRedBalls; }
            set
            {
                countRedBalls = value;
                NotifyPropertyChanged("CountRedBalls");
            }
        }

        private int countBlueBalls;
        public int CountBlueBalls
        {
            get { return countBlueBalls; }
            set
            {
                countBlueBalls = value;
                NotifyPropertyChanged("CountBlueBalls");
            }
        }

        private int cellCount;
        public int CellCount
        {
            get
            {
                return cellCount;
            }
            set
            {
                cellCount = value;
                NotifyPropertyChanged("CellCount");
            }
        }

		public int CellColumns
		{
			get
			{
				return (int)level;
			}
		}

        private ObservableCollection<CellControl> cellCollection;
        public ObservableCollection<CellControl> CellCollection
		{
			get
			{
				return this.cellCollection;
			}
		}

        #endregion

        public Game(string name1, string name2)
		{
            this.level = Game.GetLevel();
            this.cellCollection = new ObservableCollection<CellControl>();
            this.nameOfPlayer1 = name1;
            this.nameOfPlayer2 = name2;
        }

        public void Start()
        {
            ClearData();
            //this.level = Game.GetLevel();
            this.level = GameLevel.Standart;
            this.CellCount = (int)level * (int)level;            
            this.style = Game.GetGameStyle();            

            for (int i = 0; i < CellCount; i++)
            {
                CellControl cc = new CellControl(i, this, ImageBalls.None);
                cellCollection.Add(cc);
            }

            SetDefault();
        }

        private void ClearData()
        {
            CellCollection.Clear();
            Message = "";
        }

        private void SetDefault()
        {
            this.IsFinished = false;
            Step = GameStep.Red;
            
            int x = ((int)level / 2) - 1;
            cellCollection[x * (int)level + x + 1].Ball = cellCollection[(x + 1) * (int)level + x].Ball = ImageBalls.RedBall;
            cellCollection[x * (int)level + x].Ball = cellCollection[(x + 1) * (int)level + x + 1].Ball = ImageBalls.BlueBall;

            CountRedBalls = 2;
            CountBlueBalls = 2;
        }

        private void Stop()
        {
            Step = GameStep.None;
            IsFinished = true;

            if (OnGameFinished != null)
                OnGameFinished(this);
        }
/*
        private void cellControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CellControl c = (CellControl)sender;
            DoStepStuff(c);
        }
 */

        public void DoStepStuff(CellControl c)
        {
            if (c.Ball == ImageBalls.None && !IsFinished)
            {
                bool result = false;
                switch (step)
                {
                    case GameStep.Red:
                        result = DoStepStuff(c, ImageBalls.RedBall);
                        break;
                    case GameStep.Blue:
                        result = DoStepStuff(c, ImageBalls.BlueBall);
                        break;
                    default:
                        break;
                }
                if (result)
                {
                    Step = (GameStep)(((int)step + 1) % countStep);
                    VerifyStep();
                }
            }
        }

        private void VerifyStep()
        {
            Message = string.Empty;

            if (!CanDoNextStep(step))
            {
                GameStep tmp = (GameStep)(((int)step + 1) % countStep);
                if (!CanDoNextStep(tmp))
                {
                    string winner = nameOfPlayer1;
                    if (CountBlueBalls > CountRedBalls)
                        winner = nameOfPlayer2;
                    Message = string.Format("Game is over! {0} wins", winner);
                    Stop();
                }
                else
                {
                    Message = string.Format("{0} loses turn.", step.ToString());
                    Step = tmp;
                }
            }

            if (step == GameStep.Blue && style == GameStyle.Single)
                DoComputerStep();
        }

        private void DoComputerStep()
        {
            // only computer
            // so, its only blue balls
            CellControl cellMax  = null;
            int maxCells = 0;
            foreach (CellControl cell in cellCollection)
            {
                if (cell.Ball == ImageBalls.None)
                {
                    int tmp = GetValidCells(cell, ImageBalls.BlueBall).Count;
                    if (tmp > 0 && (cell.ID == 0 || cell.ID == 7 || cell.ID == 56 || cell.ID == 63))
                    {
                        cellMax = cell;
                        break;
                    }
                    if (tmp > maxCells)
                    {
                        maxCells = tmp;
                        cellMax = cell;
                    }
                }
            }
            if (cellMax != null)
                DoStepStuff(cellMax);
        }

        private bool CanDoNextStep(GameStep step)
        {
            ImageBalls ball = ImageBalls.None;
            switch (step)
            {
                case GameStep.Red:
                    ball = ImageBalls.RedBall;
                    break;
                case GameStep.Blue:
                    ball = ImageBalls.BlueBall;
                    break;
                default:
                    break;
            }
            bool result= false;

            foreach (CellControl cell in cellCollection)
            {
                if (cell.Ball == ImageBalls.None && GetValidCells(cell, ball).Count > 0)
                {
                    result = true;
                    break;
                }
            }
            
            return result;
        }

        private int[] dx = { -1, -1, -1, 0, 1, 1, 1, 0 };
        private int[] dy = { -1, 0, 1, 1, 1, 0, -1, -1 };
        private List<CellControl> GetValidCells(CellControl cell, ImageBalls ball)
        {
            List<CellControl> result = new List<CellControl>();
            int x = cell.ID / (int)level;
            int y = cell.ID % (int)level;

            for (int i = 0; i < 8; i++)
                result.AddRange(DoStepStuff(x, y, dx[i], dy[i], ball));

            return result;
        }

        private bool DoStepStuff(CellControl cell, ImageBalls ball)
        {
            bool didStuff = false;
            List<CellControl> result = GetValidCells(cell, ball);

            if (result.Count > 0)
            {
                didStuff = true;
                cell.Ball = ball;
                switch (ball)
                {
                    case ImageBalls.BlueBall:
                        CountRedBalls -= result.Count;
                        CountBlueBalls += result.Count + 1;
                        break;
                    case ImageBalls.RedBall:
                        CountRedBalls += result.Count + 1;
                        CountBlueBalls -= result.Count;
                        break;
                    default:
                        break;
                }
                foreach (CellControl c in result)
                {
                    c.Ball = ball;
                }
            }
            return didStuff;
        }

        private List<CellControl> DoStepStuff(int x, int y, int dx, int dy, ImageBalls ball)
        {
            List<CellControl> result = new List<CellControl>();
            int oldX = x;
            int oldY = y;
            x += dx;
            y += dy;
            while (x >= 0 && y >= 0 && y < (int)level && x < (int)level && 
                    cellCollection[x * (int)level + y].Ball != ball && 
                    cellCollection[x * (int)level + y].Ball != ImageBalls.None)
            {
                x += dx;
                y += dy;
            }
            if (x >= 0 && y >= 0 && y < (int)level && x < (int)level && cellCollection[x * (int)level + y].Ball == ball)
            {
                x -= dx;
                y -= dy;
                while (x != oldX || y != oldY)
                {
                    if (!result.Contains(cellCollection[x * (int)level + y]))
                        result.Add(cellCollection[x * (int)level + y]);
                    x -= dx;
                    y -= dy;
                }
            }
            return result;
        }

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

        #region Static methods

        internal static GameLevel GetLevel()
        {
            GameLevel result = GameLevel.Standart;
            switch (Properties.Settings.Default.Level)
            {
                case 0:
                    result = GameLevel.Standart;
                    break;
                case 1:
                    result = GameLevel.High;
                    break;
                case 2:
                    result = GameLevel.Master;
                    break;
                default:
                    result = GameLevel.Standart;
                    break;
            }

            return result;
        }

        internal static GameStyle GetGameStyle()
        {
            GameStyle result = GameStyle.Single;
            switch (Properties.Settings.Default.Game)
            {
                case 0:
                    result = GameStyle.Single;
                    break;
                case 1:
                    result = GameStyle.Multi;
                    break;
                default:
                    result = GameStyle.Single;
                    break;
            }
            return result;
        }

        #endregion

    }
}
