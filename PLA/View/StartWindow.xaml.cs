using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using PLA.Model;
using PLA.ViewModel;

namespace PLA
{

	/// <summary>
	/// Interaction logic for StartWindow.xaml
	/// </summary>
	public partial class StartWindow : Window
	{
		private VM vm = null;
		        
		public StartWindow()
		{
			InitializeComponent();

			vm = new VM(textCanvas, chartCanvas);
			vm.ChartStyleGridlines.AddChartStyle(tbTitle, tbXLabel, tbYLabel);
			DataContext = vm;
          
		}

		private void chartCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			vm.PosInCanvas = e.GetPosition(chartCanvas);
 		
			//update floatingTip position
			if (!floatingTip.IsOpen) { floatingTip.IsOpen = true; }
			floatingTip.HorizontalOffset = vm.PosInCanvas.X + 20;
			floatingTip.VerticalOffset = vm.PosInCanvas.Y;

			//update puckCursor position
			Point cursorPos = vm.ChartStyleGridlines.MapToCanvasPosition(vm.PosInChart);
			cursorPos.X -= puckCursor.Width / 2;
			cursorPos.Y -= puckCursor.Height / 2;
			Canvas.SetLeft(puckCursor, cursorPos.X);
			Canvas.SetTop(puckCursor, cursorPos.Y);
			puckCursor.Visibility = Visibility.Visible;
		}


		private void chartCanvas_MouseLeave(object sender, MouseEventArgs e)
		{
			floatingTip.IsOpen = false;
			puckCursor.Visibility = Visibility.Collapsed;
		}

		private void chartCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			vm.AddTrainingSample(Constant.POSITIVE);
		}

		private void chartCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			vm.AddTrainingSample(Constant.NEGATIVE);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show(vm.printTrainingSet());
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			vm.StartLearning();

		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			vm.ClearTrainingSet();
		}

		
	}
}
