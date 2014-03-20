using System;


namespace beurs
{

	class Koers : IComparable<Koers> {
		public readonly double OpenWaarde;
		public readonly double SluitWaarde;
		public readonly DateTime Dag;

		/// Maak een Koers instantie met de gegeven openings-, sluitingswaarde en dag
		public Koers(double openWaarde, double sluitWaarde, DateTime dag) {
			OpenWaarde = openWaarde;
			SluitWaarde = sluitWaarde;
			Dag = dag;
		}

		/// Vergelijk a.d.h.v. de absolute waarde (het gemiddelde van de openings- en sluitingswaarde)
		public int CompareTo(Koers that) {
			return this.Dag.CompareTo(that.Dag);
		}
	}


	// Om Koers instanties te vergelijken op basis van hun absolute gemiddelde waarde
	struct KoersWaarde : IComparable<KoersWaarde> {
		readonly Koers _Koers;

		/// Vergelijk a.d.h.v. gemiddelde van open & sluit waarde
		public int CompareTo(KoersWaarde that) {
			return ((double)this).CompareTo((double)that);
		}

		/// Maak een KoersVerschil instantie
		public KoersWaarde(Koers koers) {
			_Koers = koers;
		}

		/// Converteer een KoersWaarde instance naar een double
		public static implicit operator double(KoersWaarde verschil) {
			return (verschil._Koers.SluitWaarde + verschil._Koers.OpenWaarde) / 2;
		}
	}


	// Om Koers instanties te vergelijken op basis van hun procentuele groei van hun open en sluit waarde
	struct KoersProcVerschil : IComparable<KoersProcVerschil> {
		public readonly Koers Koers;

		/// Vergelijk a.d.h.v. procentuele groei
		public int CompareTo(KoersProcVerschil that) {
			return ((double)this).CompareTo((double)that);
		}

		/// Maak een KoersProcVerschil instantie
		public KoersProcVerschil(Koers koers) {
			Koers = koers;
		}

		/// Converteer het KoersProcVerschil instance naar een double
		public static implicit operator double(KoersProcVerschil KoersProcVerschil) {
			return (KoersProcVerschil.Koers.SluitWaarde - KoersProcVerschil.Koers.OpenWaarde) / 
				KoersProcVerschil.Koers.OpenWaarde;
		}
	}


	interface IKoers {
		void VoegToe(Koers koers); 
	}


	class KoersWijziging : IKoers, IComparable<KoersWijziging>, System.Collections.IEnumerable {
		private Koers[] _Koersen = new Koers[0];
		public readonly string Naam;

		/// Voeg een Koers instantie toe, enkel dat nog niet is gebeurt
		public void VoegToe(Koers koers) {
			if (!BevatKoersDag(koers.Dag)) {
				Array.Resize(ref _Koersen, _Koersen.Length + 1);
				_Koersen[_Koersen.Length - 1] = koers;
				Array.Sort (_Koersen);
			}
		}

		/// Om KoersWijziging instanties te vergelijken of sorteren op basis van de Naam
		public int CompareTo(KoersWijziging koersWijziging) {
			return Naam.CompareTo(koersWijziging.Naam);
		}

		/// Geeft een iterator over alle Koers instanties
		public System.Collections.IEnumerator GetEnumerator() {
			foreach (Koers koers in _Koersen) {
				yield return koers;
			}
		}

		/// Maak een instantie met de gegeven naam en startDag
		public KoersWijziging(string naam) {
			Naam = naam;
		}

		/// Haal de Koers instantie van een bepaalde dag (een ongeldige dag resulteert in een exceptie)
		public Koers this [DateTime dag] {
			get {
				int index = Array.BinarySearch(_Koersen, new Koers(0, 0, dag));
				if (0 <= index) {
					return _Koersen [index];
				} else {
					return null;
				}
			}
		}

		/// True indien de Koers instantie deel uitmaakt van KoersWijziging instantie
		public bool BevatKoersDag(DateTime dag) {
			return 0 <= Array.BinarySearch(_Koersen, new Koers(0, 0, dag));
		}

	} // class KoersWijziging


	class Aandeel : KoersWijziging {
		public Aandeel(string naam) : base(naam) {}
	} // class Aandeel


	class Obligatie : KoersWijziging {
		public Obligatie(string naam) : base(naam) {}
	} // class Obligatie


	class Beurs<T> : System.Collections.IEnumerable 
	where T : KoersWijziging {

		private KoersWijziging[] _KoersWijziging = new KoersWijziging[0];

		public void VoegToe(KoersWijziging koersenWijziging) {
			if (0 > Array.BinarySearch (_KoersWijziging, koersenWijziging)) {
				// Enkel toevoegen indien het aandeel nog niet aanwezig is
				Array.Resize (ref _KoersWijziging, _KoersWijziging.Length + 1);
				_KoersWijziging [_KoersWijziging.Length - 1] = koersenWijziging;
				Array.Sort (_KoersWijziging);
			}
		}

		public System.Collections.IEnumerator GetEnumerator() {
			foreach (KoersWijziging kw in _KoersWijziging) {
				yield return kw;
			}
		}

		private KoersProcVerschil[] GetVerschillen(DateTime dag, ref int validIndex) {
			KoersProcVerschil[] verschillen = new KoersProcVerschil[_KoersWijziging.Length];
			int teller = 0;
			validIndex = verschillen.Length;
			foreach (KoersWijziging kw in _KoersWijziging) {
				if (kw.BevatKoersDag(dag)) {
					if (verschillen.Length == validIndex) {
						validIndex = teller;
					}
					verschillen[teller] = new KoersProcVerschil(kw [dag]);
				}
				teller++;
			}
			return verschillen;
		}

		public KoersWijziging GetSterksteDagStijger(DateTime dag) {
			KoersWijziging koersWijziging = null;
			int index = 0;
			KoersProcVerschil[] verschillen = GetVerschillen(dag, ref index);
			for (int i = index + 1; i < verschillen.Length; ++i) {
				if (null != verschillen[i] && 1 == verschillen[i].CompareTo(verschillen[index])) {
					index = i;
					koersWijziging = _KoersWijziging [index];
				}
			}
			return koersWijziging;
		}

		public KoersWijziging GetSterksteWeekStijger(DateTime dag) {
			KoersWijziging koersWijziging = null;
			return koersWijziging;
		}
	}
	 // class Beurs

	

	class MainClass
	{
		public static void Main (string[] args)
		{
			// Enkele constanten, algemeen gebruikte objecten
			DateTime d1 = new DateTime (2014, 1, 1);
			DateTime d2 = new DateTime (2014, 1, 2);
			DateTime d3 = new DateTime (2014, 1, 3);
			DateTime d4 = new DateTime (2014, 1, 4);
//			DateTime d5 = new DateTime (2014, 1, 5);
//			DateTime d6 = new DateTime (2014, 1, 6);
//			DateTime d7 = new DateTime (2014, 1, 7);
			int teller;


			/************ Tests op Koers ****************/

			// Koers.CompareTo tests
			Koers koers1 = new Koers (10, 20, d2);
			Koers koers2 = new Koers (20, 30, d2);
			Console.WriteLine (0 == koers1.CompareTo(koers2)); // matching dates
			koers2 = new Koers (10, 20, d3);
			Console.WriteLine (-1 == koers1.CompareTo(koers2)); // matching dates
			koers1 = new Koers (10, 20, d4);
			Console.WriteLine (1 == koers1.CompareTo(koers2)); // matching dates


			/************ Tests op KoersWaarde ****************/

			koers1 = new Koers (10, 20, d1);
			KoersWaarde kw1 = new KoersWaarde (koers1);
			// implicit conversion to double
			Console.WriteLine(15.0 == kw1);
			KoersWaarde kw2 = new KoersWaarde (koers1);
			// KoersWaarde.CompareTo
			Console.WriteLine(0 == kw1.CompareTo(kw2));
			kw2 = new KoersWaarde (new Koers(10, 30, d1));
			Console.WriteLine(-1 == kw1.CompareTo(kw2));
			kw2 = new KoersWaarde (new Koers(10, 10, d1));
			Console.WriteLine(1 == kw1.CompareTo(kw2));


			/************ Tests op KoersProcVerschil ****************/

			koers1 = new Koers (10, 20, d1);
			KoersProcVerschil kpv1 = new KoersProcVerschil (koers1);
			// implicit conversion to double
			Console.WriteLine(1.0 == kpv1);
			KoersProcVerschil kpv2 = new KoersProcVerschil (koers1);
			Console.WriteLine (0 == kpv1.CompareTo(kpv2));
			kpv2 = new KoersProcVerschil (new Koers(10, 30, d1));
			Console.WriteLine (-1 == kpv1.CompareTo(kpv2));
			kpv2 = new KoersProcVerschil (new Koers(10, 15, d1));
			Console.WriteLine (1 == kpv1.CompareTo(kpv2));


			/************ Tests op Aandeel/KoersWijziging/Obligatie ****************/

			Aandeel aandeel1 = new Aandeel ("A");
			Console.WriteLine ("A" == aandeel1.Naam);
			Aandeel aandeel2 = new Aandeel ("A");
			Console.WriteLine (0 == aandeel1.CompareTo(aandeel2));
			aandeel2 = new Aandeel ("B");
			Console.WriteLine (-1 == aandeel1.CompareTo(aandeel2));
			aandeel1 = new Aandeel ("C");
			Console.WriteLine (1 == aandeel1.CompareTo(aandeel2));

			// Moet leeg zijn
			teller = 0;
			foreach (Koers k in aandeel1) { ++teller;}
			Console.WriteLine (0 == teller);

			Console.WriteLine (false == aandeel1.BevatKoersDag (d1));

			Console.WriteLine (null == aandeel1[d1]);

			aandeel1.VoegToe (new Koers(10, 20, d1));

			// Moet 1 koers bevatten
			teller = 0;
			foreach (Koers k in aandeel1) { ++teller;}
			Console.WriteLine (1 == teller);

			Console.WriteLine (true == aandeel1.BevatKoersDag (d1));

			Console.WriteLine (null != aandeel1[d1]);


			/************ Tests op Beurs ****************/

			Beurs<Aandeel> beurs1 = new Beurs<Aandeel> ();
			// Heeft lege stijgers & dalers
			//Console.WriteLine(null == beurs1.GetSterksteDagStijger (d1));
			//			Console.WriteLine(null == beurs1.GetSterksteDagDaler (d1));
			//			Console.WriteLine(null == beurs1.GetSterksteWeekStijger (d1));
			//			Console.WriteLine(null == beurs1.GetSterksteWeekDaler (d1));

			// Moet leeg zijn
			teller = 0;
			foreach (Aandeel a in beurs1) { ++teller;}
			Console.WriteLine(0 == teller);

			aandeel1 = new Aandeel ("A");
			beurs1.VoegToe (aandeel1);

			// Moet 1 aandeel bevatten
			teller = 0;
			foreach (Aandeel a in beurs1) { ++teller;}
			Console.WriteLine(1 == teller);

			// Toevoegen met dezelfde naam wordt genegeerd
			beurs1.VoegToe (new Aandeel("A"));
			teller = 0;
			foreach (Aandeel a in beurs1) { ++teller;}
			Console.WriteLine(1 == teller);

			aandeel2 = new Aandeel ("B");
			beurs1.VoegToe (aandeel2);
			teller = 0;
			foreach (Aandeel a in beurs1) { ++teller;}
			Console.WriteLine(2 == teller);

			Console.WriteLine(null == beurs1.GetSterksteDagStijger (d1));

			aandeel1.VoegToe (new Koers (10, 20, d1));
			Console.WriteLine(null != beurs1.GetSterksteDagStijger (d1));
		}
	}
}
