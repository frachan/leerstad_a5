/* Copyright 2014 Francis Meyvis*/

/* 
 * Implementatie van oefening 6
 * 
 * Bib stuff
 * Deze oefening heb ik pas later gemaakt; ze bevat technieken die op dat moment niet gezien zijn.
 * Er is niet veel commentaar. Alles spreekt eigenlijk voor zich na 't lezen van de opgave.
 * De class Boek bevat niets omtrend uitlenen; vind dat verkeer in de opgave
 * Voor een uitgeleend boek is er een aparte class GeleendBoek
 */

using System;

namespace Bib_francismeyvis
{
	/// Generic module tool voor operaties op gesorteerde collecties van de bib's leden en boeken
	/// T moet de IComparable<T> interface implementeren voor correcte werking.
	class Tool<T> {
		/// Geeft de index indien item voorkomt in array
		public static int Zoek(T[] array, T item) {
			return Array.BinarySearch(array, item);
		}

		/// Geeft true als het _nieuwe_ item in de array is toegevoegd
		public static bool VoegToe(ref T[] array, T item) {
			if (0 > Zoek (array, item)) {
				Array.Resize (ref array, array.Length + 1);
				array [array.Length - 1] = item;
				Array.Sort (array);
				return true;
			} else {
				return false;
			}
		}

		/// Geeft true als het item uit de array is verwijderd
		public static bool Verwijder(ref T[] array, T item) {
			int index = Zoek (array, item);
			if (0 <= index) {
				Array.Copy (array, index + 1, array, index, array.Length - index - 1);
				Array.Resize (ref array, array.Length - 1);
				return true;
			} else {
				return false;
			}
		}

		/// Display de items in array
		public static void Toon(T[] array) {
			foreach (T i in array) {
				Console.WriteLine ("  " + i);
			}
		}
	} // Module Tool


	/// Instance die unieke nummers genereert (voor boeken & leden)
	class Teller {
		private int _Waarde;
		public int Waarde { get { return _Waarde++;} }

		public Teller(int defaultStart = 1) {
			_Waarde = defaultStart;
		}
	} // class Teller


	class Adres {
		public readonly string Straat;
		public readonly string Nummer;
		public readonly string Postcode;
		public readonly string Gemeente;

		public Adres(string straat, string nr, string postcode, string gemeente) {
			Straat = straat;
			Nummer = nr;
			Postcode = postcode;
			Gemeente = gemeente;
		}

		public override string ToString() {
			return Straat + " " + Nummer + ", " + Postcode + " " + Gemeente;
		}
	} // class adres


	class Persoon {
		public readonly string Naam;
		public readonly string Voornaam;
		public readonly Adres Woonplaats;

		public Persoon(string naam, string voornaam, Adres woonplaats) {
			Naam = naam;
			Voornaam = voornaam;
			Woonplaats = woonplaats;
		}

		public override string ToString() {
			return Naam + ", " + Voornaam + " (" + Woonplaats + ")";
		}
	} // class Persoon


	class Auteur : Persoon {
		public Auteur(string Naam, string voornaam, Adres adres) : base(Naam, voornaam, adres) {
		}

		public override string ToString() {
			return Naam + ", " + Voornaam;
		}
	} // class Auteur


	class Lid : Persoon, IComparable<Lid> {
		private static Teller _Teller = new Teller();

		public readonly int Nummer;

		public int CompareTo(Lid that) {
			return this.Nummer - that.Nummer;
		}

		public Lid(string Naam, string voornaam, Adres adres) : base(Naam, voornaam, adres) {
			Nummer = _Teller.Waarde;
		}

		public override string ToString() {
			return Nummer + ": " + base.ToString();
		}
	} // class Lid


	class Boek : IComparable<Boek> {
		private static Teller _Teller = new Teller();

		public readonly int Nummer;
		public readonly string Titel;
		public readonly Auteur Schrijver;

		public int CompareTo(Boek that) {
			return this.Nummer - that.Nummer;
		}

		public Boek(string titel, Auteur schrijver) {
			Nummer = _Teller.Waarde;
			Titel = titel;
			Schrijver = schrijver;
		}

		public override string ToString() {
			return Nummer + ": " + Titel + " (" + Schrijver + ")";
		}
	} // class Boek


	class GeleendBoek : IComparable<GeleendBoek> {
		public readonly Boek BoekW;
		public readonly Lid LidW;
		public readonly DateTime OntleenDag;
		public readonly int DagenTermijn;

		public GeleendBoek(Boek boek, Lid lid, DateTime ontleenDag, int dagenTermijn) {
			BoekW = boek;
			LidW = lid;
			OntleenDag = ontleenDag;
			DagenTermijn = dagenTermijn;
		}

		public int CompareTo(GeleendBoek that) {
			if (this.BoekW.Nummer < that.BoekW.Nummer) {
				return -1;
			} else if (this.BoekW.Nummer < that.BoekW.Nummer) {
				return 1;
			} else {
				return this.LidW.Nummer - that.LidW.Nummer;
			}
		}

		public int GetDagenOvertijd(DateTime nu) {
			return Math.Max(0, (nu - OntleenDag).Days - DagenTermijn);
			//return (nu - OntleenDag).Days - DagenTermijn;
		}
	} // class GeleendBoek


	class Bib {
		private const int AANTAL_LEEN_DAGEN = 7 * 3;
		private const float BOETE_PER_DAG = 1.0f;

		private Boek[] _Boeken = new Boek[0];
		private Lid[] _Leden = new Lid[0];
		private GeleendBoek[] _GeleendeBoeken = new GeleendBoek [0];

		public readonly string Naam;

		// For debugging only
		private DateTime _Dag = DateTime.Now;
		public DateTime Dag {
			get { return _Dag;}
			set { _Dag = value;}
		}

		public Bib(string naam) {
			Naam = naam;
		}

		/* Methods ivm boeken*/

		public bool VoegToe (Boek boek) {
			return Tool<Boek>.VoegToe (ref _Boeken, boek);
		}

		public void ToonBoeken (){
			Console.WriteLine("De Boeken:");
			Tool<Boek>.Toon (_Boeken);
		}

		/* Methods ivm leden*/

		public bool VoegToe (Lid lid) {
			return Tool<Lid>.VoegToe (ref _Leden, lid);
		}

		public void ToonLeden () {
			Console.WriteLine("De leden:");
			Tool<Lid>.Toon (_Leden);
		}

		/* Methods ivm geleende werken*/

		public GeleendBoek Checkout (Boek boek, Lid lid) {
			GeleendBoek geleend = null;
			if (0 <= Tool<Boek>.Zoek (_Boeken, boek) &&
				0 <= Tool<Lid>.Zoek (_Leden, lid)) {
				geleend = new GeleendBoek (boek, lid, Dag, AANTAL_LEEN_DAGEN);
				if (!Tool<GeleendBoek>.VoegToe (ref _GeleendeBoeken, geleend)) {
					geleend = null;
				}
			}
			return geleend;
		}

		public bool Checkin(GeleendBoek boek) {
			if (Tool<GeleendBoek>.Verwijder (ref _GeleendeBoeken, boek)) {
				int dagenOvertijd = boek.GetDagenOvertijd (DateTime.Now);
				if (0 < dagenOvertijd) {
					Console.WriteLine ("Boete: " + (dagenOvertijd * BOETE_PER_DAG) + "EUR");
				} else {
					Console.WriteLine ("OK (geen boete)");
				}
				return true;
			} else {
				return false;
			}
		}

		public void ToonGeleendeBoeken () {
			Console.WriteLine("De geleende werken:");
			foreach (GeleendBoek g in _GeleendeBoeken) {
				Console.WriteLine (g.BoekW);
				Console.WriteLine ("  " + g.LidW);
			}
		}

	} // class Bib


	class MainClass
	{
		/// Helper voor efficienter testen en minder typen
		class C {
			private static int cnt = 0; 
			public static void WL<T> (T d) { Console.WriteLine("" + ++cnt + ": " + d);}
			public static void W<T> (T d) { Console.Write(d);}
		}

		public static void Main (string[] args)
		{
			Bib bib1 = new Bib ("A");
			C.WL ("A" == bib1.Naam);

			Adres a1 = new Adres ("Schrijversplein", "42", "2530", "Boekenstad");
			C.WL ("Schrijversplein" == a1.Straat);
			C.WL ("42" == a1.Nummer);
			C.WL ("2530" == a1.Postcode);
			C.WL ("Boekenstad" == a1.Gemeente);
			C.WL (a1);

			Adres a2 = new Adres ("Boetelaan", "13-7", "CA-1023", "Lenerdorp");
			C.WL ("Boetelaan" == a2.Straat);
			C.WL ("13-7" == a2.Nummer);
			C.WL ("CA-1023" == a2.Postcode);
			C.WL ("Lenerdorp" == a2.Gemeente);
			C.WL (a2);

			Auteur s1 = new Auteur("Deschrijver", "Dude", a1);
			C.WL ("Deschrijver" == s1.Naam);
			C.WL ("Dude" == s1.Voornaam);
			C.WL (a1 == s1.Woonplaats);
			C.WL (s1);

			Boek b1 = new Boek ("Het onleesbare geschrift", s1);
			C.WL ("Het onleesbare geschrift" == b1.Titel);
			C.WL (s1 == b1.Schrijver);
			C.WL (1 == b1.Nummer);
			C.WL (b1);

			Lid l1 = new Lid ("Delate", "Marcelleke", a2);
			C.WL ("Delate" == l1.Naam);
			C.WL ("Marcelleke" == l1.Voornaam);
			C.WL (a2 == l1.Woonplaats);
			C.WL (l1);

			C.WL (true == bib1.VoegToe (b1));
			C.WL (true == bib1.VoegToe (l1));

			bib1.ToonLeden ();
			bib1.ToonBoeken ();

			GeleendBoek geleend;
			geleend = bib1.Checkout (b1, l1);
			C.WL (null != geleend);
			C.WL ("dagen: " + geleend.GetDagenOvertijd(DateTime.Now));

			bib1.ToonGeleendeBoeken ();

			C.WL (true == bib1.Checkin (geleend));

			bib1.ToonGeleendeBoeken ();

			C.WL (false == bib1.Checkin (geleend));

			bib1.Dag = DateTime.Now.AddDays(-23);
			geleend = bib1.Checkout (b1, l1);
			C.WL (null != geleend);
			C.WL ("dagen: " + geleend.GetDagenOvertijd(DateTime.Now));

			C.WL (true == bib1.Checkin (geleend));
		}
	}
}
