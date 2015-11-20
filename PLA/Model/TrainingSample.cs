using System;
using System.Collections.Generic;
using System.Windows;

namespace PLA.Model
{
	class TrainingSample
	{
		private Vector _vec;
		public Vector Vector { get { return _vec; } set { _vec = value; } }
		
		public int Label { get; set; }

		public String ID { get; set; }
	}
}
