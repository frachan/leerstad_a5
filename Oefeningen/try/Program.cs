using System;

namespace try_francismeyvis
{
	public static class XX {
		public static string x;
		static XX() {
			x = "Hello world";
		}
		public static bool IsCapitalized(this string s) {
			return (null != s) && char.IsUpper(s [0]);
		}
	}
	static class MainClass
	{
		private const string 
		_GrootteLabelTot50    = "groen",             // Voor oppervlakte <50
		_GrootteLabelTussen   = "oranje",            // Voor oppervlakte >=50 <=100
		_GrootteLabelBoven100 = "rood";              // Voor oppervlakte >100
		private static readonly string[] _GrootteLabels = {
			"groen", "oranje", "rood"
		};


		/// Helper voor efficienter testen en minder typen
		static class C {
			private static int cnt = 0;
			public static void WL<T> (T d) { Console.WriteLine("" + ++cnt + ": " + d);}
			public static void W<T> (T d) { Console.Write(d);}
		}


		class A {
			protected virtual void m1() { Console.WriteLine("Ay");}
			public void m() { m1();}
		}

		interface IB { }

		sealed class B : A, IB {
			protected sealed override void m1() { Console.WriteLine("B");}
		}

		class D  {
			
		}

		public static void Main(string[] args)
		{
			foreach(var i in new int[3] {1, 2, 3}) {
				Console.WriteLine(i);
			}

			C.WL ("Hello".IsCapitalized ());
			C.WL ("world".IsCapitalized ());
			C.WL (XX.x);

			A a = new B ();
			IB ib = (IB)a;
			a.m ();
		}

	}
}
