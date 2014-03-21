/* Copyright 2014 Francis Meyvis*/

/* 
 * Implementatie van oefening 8
 * 
 * Naast Koers zijn er nog de wrappers KoersWaarde & KoersVerschil die
 * het mogelijk maken om het verschil en groei van een koers te bereken.
 * KoersWaarde wordt gebruikt in Beurs<>.Overzicht() om "3" opeenvolgende koersverandering vast te stellen
 * KoersVerschil wordt gebruikt voor het vinden van sterkste dalers & stijgers.
 * 
 * De implementatie maakt gebruik van enkele nieuwe faciliteiten
 * - cast operator (in KoersWaarde & KoersVerschil), generics, Tuple<>, delegate, yield, nullable (double?)
 * 
 * Deze implementatie heeft een DateTime in de Koers instantie.
 * Dat is implementatie gewijs eenvoudiger (sorteren & zoeken naar een bepaalde Koers in KoersWijziging)
 * Maar kan lijden tot een KoersWijziging met "gaten"
 * Vandaar ook het gebruik van de double? 
 * Wanneer een bepaalde KoersWijziging geen Koers heeft i.v.m. een andere KoersWijziging.
 * 
 * Met de nieuwe kennis vergaart (na) deze oefening zou ik gebruik maken van 
 * Array<Tuple<KoersWijziging, double>> om de sterkste daler/stijger te zoeken.
 * Lijkt me properder en simpler dan de Array<double?> techniek ... sigh
 */

using System;
using System.Collections;
using System.Collections.Generic;


namespace beurs_francismeyvis
{
	/// Module met algemene helpers
	namespace Util {

		class Extreem<T> where T: struct /* nodig omdat T as nullable is gebruikt en de compiler daarop anders een penaltie geeft*/ {
			/// function pointer
			public delegate bool Vergelijk(T a, T b);

			/// Geef zowel de index als extreme waarde terug in de gegeven array met nullables, gegeven de vergelijkings methode
			public static Tuple<int, T?> Zoek(T?[] array, Vergelijk vergelijk) {
				T? extreem = null;
				int index = -1;
				for (int i = 0; i < array.Length; ++i) {
					if (array[i].HasValue) {
						if (!extreem.HasValue || vergelijk(array[i].Value, extreem.Value)) {
							extreem = array[i];
							index = i;
						}
					}
				}
				return new Tuple<int, T?>(index, extreem);
			}
		} // class Extreem
	} // ns Util


    /// Instantie die de koers evolution op een bepaalde dag bijhoudt
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

		/// Vergelijk a.d.h.v. de dag, laat toe Koers te sorteren op basis van tijd
		public int CompareTo(Koers that) {
			return this.Dag.CompareTo(that.Dag);
		}

		public override string ToString() {
			return Dag.ToString ("yyyyMMdd:") + OpenWaarde + "," + SluitWaarde;
		}
	} // class Koers


	/// Om Koers instanties te vergelijken op basis van hun gemiddelde waarde
	class KoersWaarde : IComparable<KoersWaarde> {
		readonly Koers _Koers;

		/// Vergelijk a.d.h.v. het gemiddelde van de open & sluit waarde
		public int CompareTo(KoersWaarde that) {
			return ((double)this).CompareTo((double)that);
		}

		/// Maak een KoersVerschil instantie
		public KoersWaarde(Koers koers) {
			_Koers = koers;
		}

		/// Converteer een KoersWaarde instance naar zijn gemiddelde in een double
		public static implicit operator double(KoersWaarde verschil) {
			return (verschil._Koers.SluitWaarde + verschil._Koers.OpenWaarde) / 2;
		}
	} // class KoersWaarde


	/// Om Koers instanties te vergelijken op basis van hun groei van hun open en sluit waarde
	class KoersVerschil : IComparable<KoersVerschil> {
		public readonly Koers Koers;

		/// Vergelijk a.d.h.v. de groei van de open & sluit waarde (kan negatief zijn)
		public int CompareTo(KoersVerschil that) {
			return ((double)this).CompareTo((double)that);
		}

		/// Maak een KoersVerschil instantie
		public KoersVerschil(Koers koers) {
			Koers = koers;
		}

		/// Converteer het KoersVerschil instance naar groei waarde in een double
		public static implicit operator double(KoersVerschil KoersProcVerschil) {
			return (KoersProcVerschil.Koers.SluitWaarde - KoersProcVerschil.Koers.OpenWaarde) / 2 +
				Math.Min(KoersProcVerschil.Koers.SluitWaarde, KoersProcVerschil.Koers.OpenWaarde);
		}
	} // class KoersVerschil


    /// De interface uit de opgave
	interface IKoers {
		void VoegToe(Koers koers); 
	}


    /// Instantie die dagwaarden bijhoudt van een bepaald aandeel of obligatie
	class KoersWijziging : IKoers, IComparable<KoersWijziging>, IEnumerable<Koers> {
		private const int SIGNAAL_TELLER = 3;
		private const string VERKOOP_STR = " V!";
		private const string AANKOOP_STR = " A!";

		private Koers[] _Koersen = new Koers[0];
		public readonly string Naam;

		/// Voeg een _nieuwe_ Koers instantie toe (negeer als het reeds bestaat)
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

		/// Geeft een iterator om over alle Koers instanties
		public IEnumerator<Koers> GetEnumerator() {
            return ((IEnumerable<Koers>)this._Koersen).GetEnumerator();
		}
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }		

		/// Geeft een iterator over alle Koers instanties voor de gegeven duur op het gegeven moment
		public IEnumerator<Koers> GetEnumerator(DateTime startDag, int aantalDagen) {
			int index = Array.BinarySearch(_Koersen, new Koers(0, 0, startDag));
			if ((0 <= index) && (index + aantalDagen <= _Koersen.Length)) {
                // Geeft enkel de iterator indien de gegeven dag en de length bestaat!
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
				return (0 <= index) ? _Koersen [index] : null;
			}
		}

		/// True indien de Koers instantie deel uitmaakt van KoersWijziging instantie
		public bool BevatKoersDag(DateTime dag) {
			return 0 <= Array.BinarySearch(_Koersen, new Koers(0, 0, dag));
		}

		/// Geeft een overzicht van alles in de KoersWijziging, inclusief aankoop/verkoop signalen
		public void Overzicht() {
			int verkoopTeller = 0, aankoopTeller = 0;
			KoersWaarde w = null;

			Console.WriteLine(GetType() + ": " + Naam);
			foreach (Koers k in this) {
				if (null == w) {
					w = new KoersWaarde (k);
				}
				Console.Write("    " + k);
				if (1 == w.CompareTo(new KoersWaarde(k))) { 
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
				Console.WriteLine ();
			}
		}
	} // class KoersWijziging


	class Aandeel : KoersWijziging {
		public Aandeel(string naam) : base(naam) {}
	} // class Aandeel


	class Obligatie : KoersWijziging {
		public Obligatie(string naam) : base(naam) {}
	} // class Obligatie


	class Beurs<T> : IEnumerable<KoersWijziging>
	                 where T : KoersWijziging 
	{
        private const int WEEK_TELLER = 5;

        /// Aandelen & obligaties in dit beurs object
		private KoersWijziging[] _KoersWijziging = new KoersWijziging[0];

        /// Voeg een _nieuw_ aandeel of obligatie toe aan dit beurs object
		public void VoegToe(KoersWijziging koersenWijziging) {
			if (0 > Array.BinarySearch (_KoersWijziging, koersenWijziging)) {
				// Enkel toevoegen indien het aandeel nog niet aanwezig is
				Array.Resize (ref _KoersWijziging, _KoersWijziging.Length + 1);
				_KoersWijziging [_KoersWijziging.Length - 1] = koersenWijziging;
				Array.Sort (_KoersWijziging);
			}
		}

        /// Implementatie van IEnumerable voor support van foreach(Beurs)
		public IEnumerator<KoersWijziging> GetEnumerator() {
            return ((IEnumerable<KoersWijziging>)this._KoersWijziging).GetEnumerator();
		}
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        /// Geef de cumulatie van de verschillen van KoersWijziging instanties over een bepaalde periode
        private double?[] GetVerschillen(DateTime dag, int aantalDagen = 1) {
            double?[] verschillen = new double?[_KoersWijziging.Length];
            int index = 0;
            foreach (KoersWijziging kw in _KoersWijziging) {
                double? verschil = null;
                IEnumerator<Koers> enumerator = kw.GetEnumerator(dag, aantalDagen);
                while (enumerator.MoveNext()) {
                    if (!verschil.HasValue) {
                        verschil = 0.0;
                    }
                    verschil += new KoersVerschil(enumerator.Current);
                    //Console.WriteLine ("index=" + index + ": " + enumerator.Current + ": " + "verschil=" + (double)new KoersVerschil (enumerator.Current) + " verschilTotal=" + verschil);
                }
                verschillen[index++] = verschil;
            }
            return verschillen;
        }

        /// Zoek de koerswijziging die het sterkst stijgt op de gegeven dag
        public KoersWijziging GetSterksteDagStijger(DateTime dag) {
            double?[] verschillen = GetVerschillen(dag);
			Tuple<int, double?> result = Util.Extreem<double>.Zoek(
				verschillen, delegate(double a, double b) { return a > b; });
            return (result.Item2.HasValue) ? _KoersWijziging[result.Item1] : null;
		}

        /// Zoek de koerswijziging die het sterkst daalt op de gegeven dag
        public KoersWijziging GetSterksteDagDaler(DateTime dag) {
            double?[] verschillen = GetVerschillen(dag);
			Tuple<int, double?> result = Util.Extreem<double>.Zoek(
				verschillen, delegate(double a, double b) { return a < b; });
            return (result.Item2.HasValue) ? _KoersWijziging[result.Item1] : null;
        }

        /// Zoek de koerswijziging die het sterkst stijgt over 5 dagen vanaf de gegeven dag
		public KoersWijziging GetSterksteWeekStijger(DateTime dag) {
            double?[] verschillen = GetVerschillen(dag, WEEK_TELLER);
			Tuple<int, double?> result = Util.Extreem<double>.Zoek(
				verschillen, delegate(double a, double b) { return a > b; });
            return (result.Item2.HasValue) ? _KoersWijziging[result.Item1] : null;
		}

        /// Zoek de koerswijziging die het sterkst daalt over 5 dagen vanaf de gegeven dag
        public KoersWijziging GetSterksteWeekDaler(DateTime dag) {
            double?[] verschillen = GetVerschillen(dag, WEEK_TELLER);
			Tuple<int, double?> result = Util.Extreem<double>.Zoek(
				verschillen, delegate(double a, double b) { return a < b; });
            return (result.Item2.HasValue) ? _KoersWijziging[result.Item1] : null;
        }

		/// Geeft een overzicht van alles in Beurs, inclusief aankoop/verkoop signalen
		public void Overzicht() {
			foreach (KoersWijziging kw in _KoersWijziging) {
				kw.Overzicht ();
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
			Console.WriteLine(15.0 == kpv1);
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
			Console.WriteLine(null == beurs1.GetSterksteDagStijger (d1));
			Console.WriteLine(null == beurs1.GetSterksteDagDaler (d1));
			Console.WriteLine(null == beurs1.GetSterksteWeekStijger (d1));
			Console.WriteLine(null == beurs1.GetSterksteWeekDaler (d1));

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

			beurs1.Overzicht ();

            // Slecht 1 geldig aandeel in beurs1: dat is zowel het sterkste als het zwakste
			Console.WriteLine(aandeel1 == beurs1.GetSterksteWeekStijger (d1));
            Console.WriteLine(aandeel1 == beurs1.GetSterksteWeekDaler(d1));

			aandeel2.VoegToe (new Koers (40, 30, d1));
            Console.WriteLine(aandeel1 == beurs1.GetSterksteWeekStijger(d1));
            Console.WriteLine(aandeel1 == beurs1.GetSterksteWeekDaler(d1));

			aandeel2.VoegToe (new Koers (-10, 60, d5));
            Console.WriteLine(aandeel1 == beurs1.GetSterksteWeekStijger(d1));
            Console.WriteLine(aandeel2 == beurs1.GetSterksteWeekDaler(d1));

            Console.WriteLine(aandeel1 == beurs1.GetSterksteDagStijger(d4));
            Console.WriteLine(aandeel2 == beurs1.GetSterksteDagDaler(d4));

			Obligatie obligatie1 = new Obligatie ("C");
			obligatie1.VoegToe (new Koers (10, 100, d1));
			beurs1.VoegToe (obligatie1);
			Console.WriteLine(obligatie1 == beurs1.GetSterksteDagStijger(d1));

			List<Aandeel> list = new List<Aandeel> ();
			list.Add (aandeel1);
			list.Add (aandeel2);
			list.Add (aandeel2);

			Action<bool> cwlb = Console.WriteLine;
			Action cwl = Console.WriteLine;
			cwlb (3 == list.Count);

			foreach (Aandeel a in list) {
				a.Overzicht ();
				cwl ();
			}


			/************ Tests op Util.Extreem ****************/

			int?[] ints = new int?[10];
			Tuple<int, int?> resultaat;

			resultaat = Util.Extreem<int>.Zoek (ints, delegate(int a, int b) {
				return a > b;
			});
			cwlb (-1 == resultaat.Item1);
			cwlb (!resultaat.Item2.HasValue);

			ints [2] = 5;
			resultaat = Util.Extreem<int>.Zoek (ints, delegate(int a, int b) {
				return a > b;
			});
			cwlb (2 == resultaat.Item1);
			cwlb (resultaat.Item2.HasValue);

			ints [7] = 10;
			resultaat = Util.Extreem<int>.Zoek (ints, delegate(int a, int b) {
				return a > b;
			});
			cwlb (7 == resultaat.Item1);
			cwlb (10 == resultaat.Item2.Value);
		}
	}
}
