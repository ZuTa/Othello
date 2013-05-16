using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Othello.Game
{
    public class PlayersInfo : INotifyPropertyChanged
    {
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

        public PlayersInfo()
        {
            this.nameOfPlayer1 = "";
            this.nameOfPlayer2 = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public static bool Verify(PlayersInfo playersInfo)
        {
            bool result = true;
            if (playersInfo.NameOfPlayer1.Trim() == string.Empty ||
                playersInfo.NameOfPlayer2.Trim() == string.Empty ||
                playersInfo.NameOfPlayer1.Trim().Length > 20 ||
                playersInfo.NameOfPlayer2.Trim().Length > 20)
                result = false;

            return result;
        }
    }
}
