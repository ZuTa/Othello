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

namespace Othello
{
	/// <summary>
	/// Interaction logic for SettingWindow.xaml
	/// </summary>
	public partial class SettingWindow : Window
	{
		public SettingWindow()
		{
			InitializeComponent();
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default.Level = cbLevel.SelectedIndex;
            Properties.Settings.Default.Game = cbGame.SelectedIndex;

			Properties.Settings.Default.Save();
            this.Close();
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbLevel.SelectedIndex = Properties.Settings.Default.Level;
            cbGame.SelectedIndex = Properties.Settings.Default.Game;

            btnSave.Focus();
        }
	}
}
