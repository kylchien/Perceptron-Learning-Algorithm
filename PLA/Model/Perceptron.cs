using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;

namespace PLA.Model
{


	class Perceptron
	{
		

		public double Threashold { get; set; }

		public double LearningRate { get; set; }

		public double Bias { get; set; }

		private Vector _weights = new Vector(0, 0);
		public Vector Weights { get { return _weights; } set { _weights = value; } }

		public List<Tuple<Vector, double>> WeightHistory { get; set; }

		public List<TrainingSample> TrainingSet { get; set; }

		public Perceptron()
		{
			Threashold = 0.5;
			LearningRate = 0.1;
			Bias = 0.5;

			Weights = new Vector(0.5, 0.5);
			WeightHistory = new List<Tuple<Vector, double>>();
			WeightHistory.Add(new Tuple<Vector, double>(Weights, Bias));

			TrainingSet = new List<TrainingSample>();
		}

		public int Output(Vector weights, double bias, Vector v)
		{
			double dotProduct = weights * v + bias; 
			if (dotProduct > Threashold) { return Constant.POSITIVE; }
			return Constant.NEGATIVE;
		}

		public void Learning()
		{
			int count = 0;
			while (true)
			{
				int errorCount = 0;
				
				foreach (var ts in TrainingSet)
				{
					int sign = ts.Label - Output(Weights, Bias, ts.Vector);
					if (sign != 0)
					{
						errorCount++;
						Weights += LearningRate * sign * ts.Vector;
						Bias += LearningRate * sign;
						WeightHistory.Add(new Tuple<Vector, double>(Weights, Bias));

					}
				}
				
				if (errorCount == 0) break;

				//naive exception handling
				if (++count > 10000)
				{
					count = 0;
					Console.Out.WriteLine("...Infinite Loop?");
					Console.ReadKey();
				}
			}//end while

		}//end learning


	}
}