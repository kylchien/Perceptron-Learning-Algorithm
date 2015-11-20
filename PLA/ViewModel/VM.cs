using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;			//Point
using System.Windows.Shapes;
using System.ComponentModel;	//INotifyProperty
using System.Windows.Controls;  //Canvas
using System.Windows.Media;
using PLA.Model;

namespace PLA.ViewModel
{
	class VM : INotifyPropertyChanged
	{
		#region PropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

		#region Coordinate Properties

		// coordinate in chart system
		private Point _posInChart;
		public Point PosInChart
		{
			get { return _posInChart; }
		}

		// coordinate in canvas system
		private Point _posInCanvas;
		public Point PosInCanvas
		{
			get { return _posInCanvas; }
			set
			{
				_posInCanvas = value;
				_posInChart = ChartStyleGridlines.MapToChartPosition(_posInCanvas);
				_posInChart = ChartStyleGridlines.CapPoint(_posInChart);
				NotifyPropertyChanged("PositionInfo"); //must be the same as the name of Property
			}
		}

		// helper method 
		private string FormateDouble(double value)
		{
			return value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
		}

		public string PositionInfo
		{
			get
			{
				return "(" + FormateDouble(PosInChart.X) + ", " + FormateDouble(PosInChart.Y) + ")";
			}
		}
		#endregion

		#region Various Properties
		private Perceptron _perceptron = null;
		public Perceptron Peceptron { get { return _perceptron; } }

		private Canvas _chartCanvas = null;
		public Canvas ChartCanvas { get { return _chartCanvas; } }

		private ChartStyleGridlines _chartStyle = null;
		public ChartStyleGridlines ChartStyleGridlines { get { return _chartStyle; } }

		private List<Puck> _pucks = null;
		private Canvas _textCanvas = null;

		private LineVM _lineVM = null;
		#endregion


		public VM(Canvas textCanvas, Canvas chartCanvas)
		{
			_perceptron = new Perceptron();

			_textCanvas = textCanvas;
			_chartCanvas = chartCanvas;

			initChartStyleGridLines();

			Puck.Initialize(this);
			_pucks = new List<Puck>();

			_lineVM = new LineVM(this);

		}

		private void initChartStyleGridLines()
		{
			_chartStyle = new ChartStyleGridlines()
			{
				ChartCanvas = _chartCanvas,
				TextCanvas = _textCanvas,
				Title = "Perceptron Learning Algorithm",
				Xmin = -10,
				Xmax = 10,
				Ymin = -10,
				Ymax = 10,
				XTick = 1,
				YTick = 1,
				GridlinePattern = ChartStyleGridlines.GridlinePatternEnum.Dot,
				GridlineColor = System.Windows.Media.Brushes.Black
			};
		}

		public void AddTrainingSample(int label)
		{
			_pucks.Add(new Puck(label));
		}

		public void ClearTrainingSet()
		{
			_perceptron.TrainingSet.Clear();
			_lineVM.RemoveLines();
			_lineVM.CleanHistory();
			Puck.RemovePucks();
		}

		public void StartLearning()
		{
			_perceptron.Learning();
			_lineVM.CalcLinesInTimeFrames();
			_lineVM.StartTimer();
		}


		public String printTrainingSet()
		{
			string msg = "";
			foreach (TrainingSample ts in _perceptron.TrainingSet)
			{
				msg += "(" + ts.Vector.X + ", " + ts.Vector.Y + ") => " + ts.Label + "\n";
			}
			return msg;
		}




	}//class ViewModel : INotifyPropertyChanged
}

