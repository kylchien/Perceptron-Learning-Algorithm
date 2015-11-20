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

namespace PLA.view
{
	/// <summary>
	/// Interaction logic for Puck.xaml
	/// </summary>
	public partial class Puck : UserControl
	{
		public enum PuckColor { Blue, Red };

		public Puck()
		{
			InitializeComponent();
			this.Ellipse.Fill = getBrush(PuckColor.Red);
		}

		public Puck(PuckColor pc)
		{
			InitializeComponent();
			this.Ellipse.Fill = getBrush(pc);
		}

		private RadialGradientBrush getBrush(PuckColor pc)
		{
			var brush = new RadialGradientBrush { GradientOrigin = new Point(0.65, 0.25) };

			switch (pc)
			{
				case PuckColor.Blue:
					brush.GradientStops.Add(new GradientStop
					{
						Color = ((Color)ColorConverter.ConvertFromString("White")),
						Offset = 0.0
					});

					brush.GradientStops.Add(new GradientStop
					{
						Color = ((Color)ColorConverter.ConvertFromString("#FF4777CE")),
						Offset = 0.5
					});

					brush.GradientStops.Add(new GradientStop
					{
						Color = ((Color)ColorConverter.ConvertFromString("Blue")),
						Offset = 1.0
					});
					break;
				case PuckColor.Red:
					brush.GradientStops.Add(new GradientStop
					{
						Color = ((Color)ColorConverter.ConvertFromString("White")),
						Offset = 0.0
					});

					brush.GradientStops.Add(new GradientStop
					{
						Color = ((Color)ColorConverter.ConvertFromString("#FFD85A5A")),
						Offset = 0.5
					});

					brush.GradientStops.Add(new GradientStop
					{
						Color = ((Color)ColorConverter.ConvertFromString("Red")),
						Offset = 1.0
					});
					break;
			}//switch

			return brush;
		}

		// <summary>
		// Xaml exposed TextExposedInXaml property.
		// </summary>
		public static readonly DependencyProperty PuckColorProperty = DependencyProperty.Register("Skin", typeof(PuckColor), typeof(Puck), new PropertyMetadata(PuckColor.Red));
		
		public PuckColor Skin
		{
			get
			{
				return (PuckColor)GetValue(PuckColorProperty);
			}
			set
			{
				SetValue(PuckColorProperty, value);
				this.Ellipse.Fill = getBrush(value);
			}
		}
		/*
		private static Action EmptyDelegate = delegate() { };


		public static void Refresh(this UIElement uiElement)
		{
			uiElement.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, EmptyDelegate);
		}*/
		
	}
}
