using System;

namespace Oef_20140204
{
	namespace oef_22_5_1 
	{
		class Werknemer {
			private readonly double _Maandloon;

			public Werknemer(double maandloon) {
				_Maandloon = maandloon;
			}

			public virtual double BerekenJaarloon() {
				return 12 * _Maandloon;
			}

			public override string ToString() {
				return Convert.ToString (BerekenJaarloon());
			}
		}


		class Manager : Werknemer {
			private readonly double _Premie;

			public Manager(double maandloon, double premie) : base(maandloon) {
				_Premie = premie;
			}

			public override double BerekenJaarloon() {
				return _Premie + base.BerekenJaarloon();
			}
		}
	} // oef_22_5_1

	namespace oef_22_5_2
	{
		class Addition {
			public double Operand1;
			public double Operand2;

			public Addition(double operand1, double operand2) {
				Operand1 = operand1;
				Operand2 = operand2;
			}

			public double GetSum() {
				return Operand1 + Operand2;
			}

			public override string ToString () {
				return "" + Operand1 + " + " + Operand2 + " = " + GetSum ();
			}
		}
	} // oef_22_5_2

	namespace oef_22_5_3
	{
		class Airport {
			private readonly string Name;
			private readonly string Place;

			public Airport(string name, string place) {
				Name = name;
				Place = place;
			}

			public override string ToString ()
			{
				return Name + ": " + Place;
			}
		}


		class Flight {
			private readonly Airport Src;
			private readonly Airport Dst;

			public Flight(Airport src, Airport dst) {
				Src = src;
				Dst = dst;
			}

			public override string ToString ()
			{
				return "" + Src + " --> " + Dst;
			}
		}


		class Holiday {
			private Flight[] flights = new Flight[0];
			public int Count { 
				get { return flights.Length; }
			}

			public void Add(Flight f) {
				Array.Resize(ref flights, flights.Length + 1);
				flights[flights.Length - 1] = f;
			}

			public Flight Item(int i) {
				return flights[i];
			}
		}
	} // oef_22_5_3


	class MainClass
	{
		public static void Main (string[] args)
		{
			oef_22_5_1.Werknemer w1 = new oef_22_5_1.Werknemer (2000.0);
			Console.WriteLine (w1);

			oef_22_5_1.Werknemer w2 = new oef_22_5_1.Manager (1500.0, 10000.0);
			Console.WriteLine (w2);

			Console.WriteLine ();

			oef_22_5_2.Addition addition1 = new oef_22_5_2.Addition(3, 4);
			Console.WriteLine(addition1.GetSum());
			Console.WriteLine(addition1);

			addition1.Operand1 = 5;
			addition1.Operand2 = 6;
			Console.WriteLine(addition1.GetSum());
			Console.WriteLine(addition1);

			Console.WriteLine ();

			oef_22_5_3.Airport airport1 = new oef_22_5_3.Airport("ZAV", "Brussels");
			oef_22_5_3.Airport airport2 = new oef_22_5_3.Airport("NYK", "New York Kennedy");
			//
			oef_22_5_3.Flight flight1 = new oef_22_5_3.Flight(airport1, airport2);
			oef_22_5_3.Flight flight2 = new oef_22_5_3.Flight(airport2, airport1);
			//
			oef_22_5_3.Holiday holiday1 = new oef_22_5_3.Holiday();
			holiday1.Add(flight1);
			holiday1.Add(flight2);
			//
			for (int index = 0; index < holiday1.Count; index++)
			{
				Console.WriteLine(holiday1.Item(index).ToString());
			}
		}
	}
}
