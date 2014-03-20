using System;
using System.Collections;
using System.Collections.Generic;

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

		public override string ToString() {
			return Dag.ToString ("yyyyMMdd:") + OpenWaarde + "," + SluitWaarde;
		}
	}


	// Om Koers instanties te vergelijken op basis van hun absolute gemiddelde waarde
	class KoersWaarde : IComparable<KoersWaarde> {
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


	// Om Koers instanties te vergelijken op basis van hun groei van hun open en sluit waarde
	class KoersVerschil : IComparable<KoersVerschil> {
		public readonly Koers Koers;

		/// Vergelijk a.d.h.v. procentuele groei
		public int CompareTo(KoersVerschil that) {
			return ((double)this).CompareTo((double)that);
		}

		/// Maak een KoersProcVerschil instantie
		public KoersVerschil(Koers koers) {
			Koers = koers;
		}

		/// Converteer het KoersProcVerschil instance naar een double
		public static implicit operator double(KoersVerschil KoersProcVerschil) {
			return (KoersProcVerschil.Koers.SluitWaarde - KoersProcVerschil.Koers.OpenWaarde) / 2 +
				Math.Min(KoersProcVerschil.Koers.SluitWaarde, KoersProcVerschil.Koers.OpenWaarde);
		}
	}


	interface IKoers {
		void VoegToe(Koers koers); 
	}


	class KoersWijziging : IKoers, IComparable<KoersWijziging>, IEnumerable<Koers> {
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
		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}		
		public IEnumerator<Koers> GetEnumerator() {
			return _Koersen.GetEnumerator ();
		}

		/// Geeft een iterator over alle Koers instanties
		public System.Collections.IEnumerator GetEnumerator(DateTime startDag, int aantalDagen) {
			int index = Array.BinarySearch(_Koersen, new Koers(0, 0, startDag));
			if ((0 <= index) && (index + aantalDagen <= _Koersen.Length)) {
				for (int i = index; i < index + aantalDagen; ++i) {
					yield return _Koersen[i];
				}
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


	class Beurs<T> : IEnumerable 
	                 where T : KoersWijziging 
	{
		const int SIGNAAL_TELLER = 3;
		const int WEEK_TELLER    = 5;
		const string VERKOOP_STR = " V!";
		const string AANKOOP_STR = " A!";

		private KoersWijziging[] _KoersWijziging = new KoersWijziging[0];

		public void VoegToe(KoersWijziging koersenWijziging) {
			if (0 > Array.BinarySearch (_KoersWijziging, koersenWijziging)) {
				// Enkel toevoegen indien het aandeel nog niet aanwezig is
				Array.Resize (ref _KoersWijziging, _KoersWijziging.Length + 1);
				_KoersWijziging [_KoersWijziging.Length - 1] = koersenWijziging;
				Array.Sort (_KoersWijziging);
			}
		}

		public IEnumerator GetEnumerator() {
			return _KoersWijziging.GetEnumerator ();
		}

		private KoersVerschil[] GetVerschillen(DateTime dag, ref int validIndex) {
			KoersVerschil[] verschillen = new KoersVerschil[_KoersWijziging.Length];
			int teller = 0;
			validIndex = verschillen.Length;
			foreach (KoersWijziging kw in _KoersWijziging) {
				if (kw.BevatKoersDag(dag)) {
					if (verschillen.Length == validIndex) {
						validIndex = teller;
					}
					verschillen[teller] = new KoersVerschil(kw [dag]);
				}
				teller++;
			}
			return verschillen;
		}

		public KoersWijziging GetSterksteDagStijger(DateTime dag) {
			KoersWijziging koersWijziging = null;
			int index = 0;
			KoersVerschil[] verschillen = GetVerschillen(dag, ref index);
			Console.WriteLine (verschillen);
			for (int i = index + 1; i < verschillen.Length; ++i) {
				if ((null != verschillen[i]) && (1 == verschillen[i].CompareTo(verschillen[index]))) {
					index = i;
					koersWijziging = _KoersWijziging [index];
				}
			}
			return koersWijziging;
		}

		// Zoek de koerswijziging die het sterkst steeg over 5 dagen vanaf de gegeven dag
		public KoersWijziging GetSterksteWeekStijger(DateTime dag) {
			KoersWijziging koersWijziging = null;

			double?[] verschillen = new double?[_KoersWijziging.Length];
		    int index = 0;
			foreach (KoersWijziging kw in _KoersWijziging) {
				bool heeftVerschil = false;
				double verschil = 0.0;
				var enumerator = kw.GetEnumerator (dag, WEEK_TELLER);
				while (enumerator.MoveNext()) {
					heeftVerschil = true;
					verschil += new KoersVerschil ((Koers)enumerator.Current);
					Console.WriteLine ("index=" + index + ": " + enumerator.Current + ": " + 
					                   "verschil=" + (double)new KoersVerschil ((Koers)enumerator.Current) + 
					                   " verschilTotal=" + verschil);
				}
				if (heeftVerschil) {
					verschillen [index] = verschil;
				}
				++index;
			}
			index = 0;
			foreach(double? d in verschillen) {
				if (d.HasValue) {
					Console.WriteLine ("index=" + index + ": " + d);
				}
				++index;
			}
			return koersWijziging;
		}

		// Overzicht van alle aandelen, inclusief aankoop/verkoop signalen
		public void Display() {
			foreach (KoersWijziging kw in _KoersWijziging) {
				int verkoopTeller;
				int aankoopTeller;
				verkoopTeller = aankoopTeller = 0;
				Console.Write (kw.GetType() + ": " + kw.Naam + "\n");
				KoersWaarde w = null;
				foreach (Koers k in kw) {
					Console.Write ("    " + k);
					if (null == w) {
						w = new KoersWaarde (k);
					}
					if (1 == w.CompareTo (new KoersWaarde (k))) { 
						// koers daling
						if (SIGNAAL_TELLER == aankoopTeller + 1) {
							Console.Write (AANKOOP_STR);
						} else {
							verkoopTeller = 0;
							++aankoopTeller;
						}
					} else if (-1 == w.CompareTo (new KoersWaarde (k))) { 
						// koers stijging
						if (SIGNAAL_TELLER == verkoopTeller + 1) {
							Console.Write (VERKOOP_STR);
						} else {
							aankoopTeller = 0;
							++verkoopTeller;
						}
					} else {
						// status quo (reset tellers)
						verkoopTeller = aankoopTeller = 0;
					}
					w = new KoersWaarde (k);
					Console.Write ("\n");
				}
				Console.WriteLine ();
			}
		}
	} // class Beurs

	

	class MainClass
	{
		public static void Main (string[] args)
		{
			// Enkele constanten, algemeen gebruikte objecten
			DateTime d1  = new DateTime (2014, 1, 1);
			DateTime d2  = new DateTime (2014, 1, 2);
			DateTime d3  = new DateTime (2014, 1, 3);
			DateTime d4  = new DateTime (2014, 1, 4);
			DateTime d5  = new DateTime (2014, 1, 5);
			DateTime d6  = new DateTime (2014, 1, 6);
			DateTime d7  = new DateTime (2014, 1, 7);
			DateTime d8  = new DateTime (2014, 1, 8);
			DateTime d9  = new DateTime (2014, 1, 9);
			DateTime d10 = new DateTime (2014, 1, 10);
			DateTime d11 = new DateTime (2014, 1, 11);
			DateTime d12 = new DateTime (2014, 1, 12);
			DateTime d13 = new DateTime (2014, 1, 13);
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
			KoersVerschil kpv1 = new KoersVerschil (koers1);
			// implicit conversion to double
			Console.WriteLine(1.0 == kpv1);
			KoersVerschil kpv2 = new KoersVerschil (koers1);
			Console.WriteLine (0 == kpv1.CompareTo(kpv2));
			kpv2 = new KoersVerschil (new Koers(10, 30, d1));
			Console.WriteLine (-1 == kpv1.CompareTo(kpv2));
			kpv2 = new KoersVerschil (new Koers(10, 15, d1));
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

			aandeel1.VoegToe (new Koers (20, 30, d2)); // 1ste stijging
			aandeel1.VoegToe (new Koers (30, 40, d3)); // 2de stijging
			aandeel1.VoegToe (new Koers (40, 50, d4)); // 3de strijging: verkoop
			aandeel1.VoegToe (new Koers (50, 40, d5)); // status quo (reset tellers)
			aandeel1.VoegToe (new Koers (40, 51, d6)); // 1ste stijging
			aandeel1.VoegToe (new Koers (51, 55, d7)); // 2de stijging
			aandeel1.VoegToe (new Koers (55, 30, d8)); // 1ste daling
			aandeel1.VoegToe (new Koers (30, 20, d9)); // 2de daling
			aandeel1.VoegToe (new Koers (20, 10, d10)); // 3de daling: aankoop
			aandeel1.VoegToe (new Koers (10, 0, d11)); // 4de daling: aankoop
			aandeel1.VoegToe (new Koers (0, -1, d12)); // 5de daling: aankoop
			aandeel1.VoegToe (new Koers (-1, 0, d13)); // status quo

			aandeel2.VoegToe (new Koers ( 0, -10, d4));
			aandeel2.VoegToe (new Koers (20,  0, d3));
			aandeel2.VoegToe (new Koers (30, 20, d2));

			beurs1.Display ();

			Console.WriteLine ("--------------");
			beurs1.GetSterksteWeekStijger (d1);

			aandeel2.VoegToe (new Koers (40, 30, d1));
			Console.WriteLine ("--------------");
			beurs1.GetSterksteWeekStijger (d1);

			aandeel2.VoegToe (new Koers (-10, 60, d5));
			Console.WriteLine ("--------------");
			beurs1.GetSterksteWeekStijger (d1);


		}
	}
}
