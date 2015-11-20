using System;
using System.Windows.Controls;
using System.Windows;

namespace PLA.ViewModel
{
	public class ChartStyle
	{
		private double xmin = 0;
		private double xmax = 1;
		private double ymin = 0;
		private double ymax = 1;

		private Canvas chartCanvas;

		public ChartStyle()
		{
		}

		public Canvas ChartCanvas
		{
			get { return chartCanvas; }
			set { chartCanvas = value; }
		}

		public double Xmin
		{
			get { return xmin; }
			set { xmin = value; }
		}

		public double Xmax
		{
			get { return xmax; }
			set { xmax = value; }
		}

		public double Ymin
		{
			get { return ymin; }
			set { ymin = value; }
		}

		public double Ymax
		{
			get { return ymax; }
			set { ymax = value; }
		}

		public void ResizeCanvas(double width, double height)
		{
			ChartCanvas.Width = width;
			ChartCanvas.Height = height;
		}

		public Point MapToCanvasPosition(Point pt)
		{
			if (ChartCanvas.Width.ToString() == "NaN")
				ChartCanvas.Width = 500;
			if (ChartCanvas.Height.ToString() == "NaN")
				ChartCanvas.Height = 500;
			Point result = new Point();
			result.X = (pt.X - Xmin) / (Xmax - Xmin) * ChartCanvas.Width;
			result.Y = ChartCanvas.Height - (pt.Y - Ymin) / (Ymax - Ymin) * ChartCanvas.Height;
			return result;
		}

		public Point MapToChartPosition(Point pt)
		{
		    double x = Xmin + (pt.X/ChartCanvas.Width) * (Xmax - Xmin);
		    double y = Ymax - (pt.Y / ChartCanvas.Height) * (Ymax - Ymin);
		    return new Point(x, y);
		}
	}
}
