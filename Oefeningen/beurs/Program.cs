using System;

namespace beurs
{

	class Koers : IComparable<Koers> {
		public readonly float OpenWaarde;
		public readonly float SluitWaarde;
		public readonly string Dag;

		public Koers(float openWaarde, float sluitWaarde, string dag) {
			OpenWaarde = openWaarde;
			SluitWaarde = sluitWaarde;
			Dag = dag;
		}

		public int CompareTo(Koers koers) {
			return Dag.CompareTo (koers.Dag);
		}
	}


	interface IKoers {
		void VoegToe(Koers koers); 
	}


	class Koersen : IKoers, IComparable<Koersen>, System.Collections.IEnumerable {
		private Koers[] _Koersen = new Koers[0];

		public readonly string Naam;

		public Koersen(string naam) {
			Naam = naam;
		}

		public int CompareTo(Koersen koersen) {
			return Naam.CompareTo(koersen.Naam);
		}

		public System.Collections.IEnumerator GetEnumerator() {
			foreach (Koers koers in _Koersen) {
				yield return koers;
			}
		}

		public void VoegToe(Koers koers) {
			Array.Resize(ref _Koersen, _Koersen.Length + 1);
			_Koersen[_Koersen.Length - 1] = koers;
			Array.Sort (_Koersen);
		}
	}


	class Aandeel : Koersen {
		public Aandeel(string naam) : base(naam) {
		}
	}


	class Obligatie : Koersen {
		public Obligatie(string naam) : base(naam) {
		}
	}


	class Beurs : System.Collections.IEnumerable {

		private Koersen[] _Koersen = new Koersen[0];

		public void VoegToe(Koersen koersen) {
			Array.Resize(ref _Koersen, _Koersen.Length + 1);
			_Koersen[_Koersen.Length - 1] = koersen;
			Array.Sort (_Koersen);
		}

		public System.Collections.IEnumerator GetEnumerator() {
			foreach (Koersen koersen in _Koersen) {
				yield return koersen;
			}
		}

		public Koersen SterksteDagStijger(string dag = null) {
			if (null == dag) {
				DateTime now = DateTime.Now;
				dag = now.ToString("YYYYMMDD");
			}
			Koersen sterksteKoersen = null;
			foreach (Koersen koersen in _Koersen) {
				foreach
			}
		}

	}

	

	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			Koers koers1 = new Koers (10.0f, 20.0f, "20140317");
			Console.WriteLine (0 == koers1.CompareTo (koers1));
			Koers koers2 = new Koers (10.0f, 20.0f, "20140318");
			Console.WriteLine (-1 == koers1.CompareTo (koers2));
			Console.WriteLine (1 == koers2.CompareTo (koers1));

			Koersen koersen1 = new Koersen ("AAA");
			Console.WriteLine (0 == koersen1.CompareTo(koersen1));
			Koersen koersen2 = new Koersen ("BBB");
			Console.WriteLine (-1 == koersen1.CompareTo(koersen2));
			Console.WriteLine (1 == koersen2.CompareTo(koersen1));

			koersen1.VoegToe (koers2);
			koersen1.VoegToe (koers1);
			foreach(Koers koers in koersen1) {
				Console.WriteLine(koers.Dag);
			}

			Beurs beurs1 = new Beurs ();
			beurs1.VoegToe (koersen2);
			beurs1.VoegToe (koersen1);

			foreach (Koersen k in beurs1) {
				Console.WriteLine(k.Naam);
			}
		}
	}
}
