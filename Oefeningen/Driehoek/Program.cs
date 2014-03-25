using System;

namespace Driehoek_francismeyvis
{
	class Punt2D {
		public readonly double X, Y;

		public Punt2D(double x = 0.0, double y = 0.0) {
			X = x; Y = y;
		}

		public static double BerekenAfstand(Punt2D a, Punt2D b) {
			double x = a.X - b.X;
			double y = a.Y - b.Y;
			return Math.Sqrt(x*x + y*y);
		}

		public static double BerekenOmtrek(Punt2D[] punten) {
			double omtrek = 0.0;
			int i;
			for (i = 1; i < punten.Length; i++) {
				omtrek += Punt2D.BerekenAfstand (punten[i-1], punten[i]);
			}
			omtrek += Punt2D.BerekenAfstand (punten[i-1], punten[0]);
			return omtrek;
		}
	}

	class Veelhoek {
		private readonly Punt2D[] _Punten;

		public Veelhoek(params Punt2D[] punten) {
			_Punten = punten;
		}

		public double BerekenOmtrek() {
			return Punt2D.BerekenOmtrek(_Punten);
		}
	}


	class Driehoek : Veelhoek {
		public Driehoek(Punt2D a, Punt2D b, Punt2D c) :
			base (new Punt2D[] {a, b, c}) {}
	}


	class MainClass
	{
		/// Helper voor efficienter testen en minder typen
		static class C {
			private static int cnt = 0;
			public static void WL<T> (T d) { Console.WriteLine("" + ++cnt + ": " + d);}
			public static void W<T> (T d) { Console.Write(d);}
		}

		public static void Main (string[] args)
		{
			Driehoek d1 = new Driehoek (new Punt2D (1, 2), new Punt2D (5, 5), new Punt2D (2, 6));
			C.WL (12.2854 == Math.Round (d1.BerekenOmtrek (), 4));

			Veelhoek v1 = new Veelhoek (new Punt2D(-2, 2), new Punt2D(2, 2), new Punt2D(2, -2), new Punt2D(-2, -2));
			C.WL (16.0 == Math.Round (v1.BerekenOmtrek (), 4));
		}
	}
}
