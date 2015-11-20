using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Threading;
using PLA.Model;

namespace PLA.ViewModel
{
	class LineVM
	{
		public static readonly string Prefix = "Line_";
		#region LinePattern

		public enum LinePatternEnum
		{
			Solid = 1,
			Dash = 2,
			Dot = 3,
			DashDot = 4,
			None = 5
		}

		public static void SetLinePattern(Line line, Brush color, double thickness, LinePatternEnum pattern)
		{
			line.Stroke = color;
			line.StrokeThickness = thickness;

			switch (pattern)
			{
				case LinePatternEnum.Dash:
					line.StrokeDashArray = new DoubleCollection(new double[2] { 4, 3 });
					break;
				case LinePatternEnum.Dot:
					line.StrokeDashArray = new DoubleCollection(new double[2] { 1, 2 });
					break;
				case LinePatternEnum.DashDot:
					line.StrokeDashArray = new DoubleCollection(new double[4] { 4, 2, 1, 2 });
					break;
				case LinePatternEnum.None:
					line.Stroke = Brushes.Transparent;
					break;
			}
		}

		#endregion


		private List<Line> _decisionHistory = null;
		private List<Line> _weightHistory = null;

		private VM _vm = null;
		private System.Threading.Timer Timer;

		public int TimeFrame { get; set; }

		public int Speed { get; set; }

		public LineVM(VM vm)
		{
			_vm = vm;
			_decisionHistory = new List<Line>();
			_weightHistory = new List<Line>();
			TimeFrame = 0;
			Speed = 10;
		}


		public void CleanHistory()
		{
			_decisionHistory.Clear();
			_weightHistory.Clear();
		}

		private void remove()
		{
			var lineSet = (
			    from p in _vm.ChartCanvas.Children.OfType<Line>()
			    where (p.Tag != null) && (p.Tag.ToString().Contains(LineVM.Prefix))
			    select p);

			List<UIElement> lineToRemove = new List<UIElement>(lineSet);
			foreach (Line e in lineToRemove)
				_vm.ChartCanvas.Children.Remove(e);
		}
		public void RemoveLines()
		{
			Application.Current.Dispatcher.BeginInvoke(
				System.Windows.Threading.DispatcherPriority.Normal,
				new Action(() => remove())
			);
		}


		public void StartTimer()
		{
			StopTimer();
			Timer = new Timer(x => Timer_Tick(), null, 0, 8000/Speed);
		}

		public void StopTimer()
		{
			if (Timer != null)
			{
				Timer.Dispose();
				Timer = null;
			}
			TimeFrame = 0;
		}

		private void Timer_Tick()
		{
			RemoveLines();
			DrawDecisionLineAndWeightLine(TimeFrame);
			TimeFrame++;
			if (TimeFrame >= _decisionHistory.Count)
			{
				StopTimer();
			}
		}

		
		public void CalcLinesInTimeFrames()
		{
			foreach (var v in _vm.Peceptron.WeightHistory)
			{
				Vector weight = v.Item1;
				double bias = v.Item2;
				Line decisionLine = CreateDecisionLine(weight, bias );
				Line weightLine = CreateWeightLine(weight, bias);

				_decisionHistory.Add(decisionLine);
				_weightHistory.Add(weightLine);
			}

			Console.Out.WriteLine("In total there are {0} time frames", _decisionHistory.Count);
		}

		private Line CreateDecisionLine(Vector weight, double bias)
		{
			double threashold = _vm.Peceptron.Threashold;
			double xmin = _vm.ChartStyleGridlines.Xmin;
			double xmax = _vm.ChartStyleGridlines.Xmax;
			double ymin = _vm.ChartStyleGridlines.Ymin;
			double ymax = _vm.ChartStyleGridlines.Ymax;

			double x1 = ((threashold - bias) - weight.Y * ymin) / weight.X;
			double x2 = ((threashold - bias) - weight.Y * ymax) / weight.X;
			double y1 = ((threashold - bias) - weight.X * xmin) / weight.Y;
			double y2 = ((threashold - bias) - weight.X * xmax) / weight.Y;

			Point start = new Point();
			Point end = new Point();

			if (Math.Abs(weight.X) < Constant.EPISOLON)
			{
				start.X = xmin;
				start.Y = y1;
				end.X = xmax;
				end.Y = y2;
			}
			else
			{
				start.X = x1;
				start.Y = ymin;
				end.X = x2;
				end.Y = ymax;
			}

			start = _vm.ChartStyleGridlines.MapToCanvasPosition(start);
			end = _vm.ChartStyleGridlines.MapToCanvasPosition(end);

			Line decisionLine = new Line()
			{
				X1 = start.X,
				Y1 = start.Y,
				X2 = end.X,
				Y2 = end.Y
			};

			decisionLine.Tag = Prefix + "DecisionLine";
			SetLinePattern(decisionLine, Brushes.Black, 1, LinePatternEnum.Solid);
			
			return decisionLine;
		}

		private Line CreateWeightLine(Vector weight, double bias)
		{
			double threashold = _vm.Peceptron.Threashold;

			//y=(w2/w1)x
			//-b/||w||
			Vector v = ((-(bias - threashold)) / (weight.LengthSquared)) * weight;

			Point start = new Point(0, 0);
			Point end = new Point(v.X, v.Y);

			start = _vm.ChartStyleGridlines.MapToCanvasPosition(start);
			end = _vm.ChartStyleGridlines.MapToCanvasPosition(end);

			Line weightLine = new Line(){
				X1 = start.X,
				Y1 = start.Y,
				X2 = end.X,
				Y2 = end.Y
			};
	
			weightLine.Tag = Prefix + "WeightLine";
			SetLinePattern(weightLine, Brushes.Red, 1, LinePatternEnum.Dash);
			
			return weightLine;
		}

		private void addDecisionLine(Line decisionLine, Line weightLine)
		{
			_vm.ChartCanvas.Children.Add(decisionLine);
			_vm.ChartCanvas.Children.Add(weightLine);
		}
		
		
		
		public void DrawDecisionLineAndWeightLine(int theTimeFrame)
		{
			Line decisionLine = _decisionHistory[theTimeFrame];
			Line weightLine = _weightHistory[theTimeFrame];

			Application.Current.Dispatcher.BeginInvoke(
				System.Windows.Threading.DispatcherPriority.Normal,
				new Action(()=>_vm.ChartCanvas.Children.Add(decisionLine))
			);
			Application.Current.Dispatcher.BeginInvoke(
				System.Windows.Threading.DispatcherPriority.Normal,
				new Action(() => _vm.ChartCanvas.Children.Add(weightLine))
			);
			//_vm.ChartCanvas.Children.Add(weightLine);
		}

		public void DrawDecisionLineAndWeightLine()
		{
			// draw decision boundary
			Line decisionLine = CreateDecisionLine(_vm.Peceptron.Weights, _vm.Peceptron.Bias);
			// draw weight line
			Line weightLine = CreateWeightLine(_vm.Peceptron.Weights, _vm.Peceptron.Bias);
	
			_vm.ChartCanvas.Children.Add(decisionLine);
			_vm.ChartCanvas.Children.Add(weightLine);
		}



	}
}
