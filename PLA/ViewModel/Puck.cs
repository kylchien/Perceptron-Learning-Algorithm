using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using PLA.Model;
using PLA.ViewModel;

namespace PLA.ViewModel
{
	class Puck
	{
        public static readonly string Prefix = "Puck_";

        public enum Label
        {
            BLUE = 1,
            RED  = 0
        }

        private static VM _vm = null;
        private static bool _initialized = false;
        private static Style BLUE_PUCK = null;
        private static Style RED_PUCK = null;
        private static int index = 1;

		private Ellipse _ellipse = null;
		public Ellipse Ellipse { get { return _ellipse; } }

		private TrainingSample _ts = null;
		public TrainingSample TrainingSample { get { return _ts; } }

        public static void Initialize(VM vm)
        {
            _vm = vm;
            if (!_initialized)
            {
                ResourceDictionary rd = new ResourceDictionary();
                rd.Source = new Uri("pack://application:,,,/view/Resource.xaml", UriKind.RelativeOrAbsolute);
                BLUE_PUCK = rd["BluePuck"] as Style;
                RED_PUCK = rd["RedPuck"] as Style;
                _initialized = true;
            }
        }


        public Puck(int label)
		{
            Point chartPos = _vm.PosInChart;
            Point canvasPos = _vm.ChartStyleGridlines.MapToCanvasPosition(chartPos);
            string id = Prefix + (index++).ToString();

            //model
            _ts = new TrainingSample()
            {
                ID = id ,
                Vector = new Vector(chartPos.X, chartPos.Y),
                Label = label
            };
            _vm.Peceptron.TrainingSet.Add(_ts);
            
            //view
            _ellipse = new Ellipse { 
                Height=15,
                Width=15, 
                Tag=id
            };

            _ellipse.Style = (label == Constant.POSITIVE) ? (BLUE_PUCK) : (RED_PUCK);

			double x = canvasPos.X - 0.5 * _ellipse.Width;
			double y = canvasPos.Y - 0.5 * _ellipse.Height;
			Canvas.SetLeft(_ellipse, x);
			Canvas.SetTop(_ellipse, y);

            _vm.ChartCanvas.Children.Add(_ellipse);
		}

        public static void RemovePucks()
        {
            var ellipseSet = (
				from c in _vm.ChartCanvas.Children.OfType<Ellipse>()
				where c.Tag.ToString().Contains(Puck.Prefix)
				select c);

			List<UIElement> ellipseToRemove = new List<UIElement>(ellipseSet);
			foreach (Ellipse e in ellipseToRemove)
				_vm.ChartCanvas.Children.Remove(e);
        }
	}
}
