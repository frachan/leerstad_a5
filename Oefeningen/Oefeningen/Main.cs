using System;

namespace francismeyvis
{
	class MainClass
	{
		private static void Oef1_391() 
		{
			Console.Write ("Geef een getal waarvan het kwadraat wordt berekend: ");
			Double kwadraat;
			kwadraat = Math.Pow(Convert.ToDouble(Console.ReadLine ()), 2);
			Console.WriteLine ("Het kwadraat is " + kwadraat + ".");
		}

		private static void Oef2_392 ()
		{
			Console.Write ("Geef je naam: ");
			string name;
			name = Console.ReadLine ();
			Console.Write (name + ", geeft je lengte (in floating point formaat): ");
			double rawLength;
			rawLength = Convert.ToDouble (Console.ReadLine ());
			int mLength, cLength;
			mLength = (int)rawLength;
			cLength = Math.Abs ((int)((rawLength - (double)mLength) * 100F));
			// ok, I know the -0.50 does not really work out as it should ...
			if (2 <= mLength) {
				Console.Write (name + ", je bent groot! " + mLength + " meters");
			} else if (0 > mLength) {
				Console.Write (name + ", te gek, je bent een buitenaards! " + mLength + " meters");
			} else if (0 == mLength) {
				Console.Write (name + ", je bent klein. Minder dan 1 meter");
			} else {
				Console.Write (name + ", je bent " + mLength + " meter");
			}
			if (0 == cLength) {
				Console.WriteLine (".");
			} else {
				Console.WriteLine (" en " + cLength + " centimeters.");
			}
		}

		private static void Oef3_393 ()
		{
			Console.Write ("Geef 2 getallen in voor wat rekenkundige spielerei.\nJe eerste getal: ");
			int g1;
			g1 = Convert.ToInt32 (Console.ReadLine ());
			Console.Write ("Geef je tweede getal: ");
			int g2;
			g2 = Convert.ToInt32 (Console.ReadLine ());
			int som, product, rest;
			som = g1 + g2;
			product = g1 * g2;
			Console.Write ("Som: " + som + ", product: " + product);
			if (0 != g2) {
				rest = g1 % g2;
				Console.WriteLine (", rest: " + rest);
			} else {
				Console.WriteLine (", de restberekening is onmogelijk.");
			}
		}

		private static void Oef1_561 ()
		{
			Console.Write ("Geef een getal voor onderzoek op nul: ");
			Double v;
			v = Convert.ToDouble (Console.ReadLine ());
			if (0.0 == v) {
				Console.WriteLine ("Je getal is nul.");
			} else {
				Console.WriteLine ("Je getal, " + v + ", is niet nul.");
			}
		}

		private static void Oef2_562 ()
		{
			Console.Write ("Geef 2 getallen voor een deling. De teller: ");
			Double t;
			t = Convert.ToDouble (Console.ReadLine ());
			Console.Write ("De noemer: ");
			Double n;
			n = Convert.ToDouble (Console.ReadLine ());
			if (0.0 == n) {
				Console.WriteLine ("Deling door nul niet mogelijk.");
			} else {
				Double d = t / n;
				Console.WriteLine ("Het resultaat van de deling is: " + d + ".");
			}
		}

		private static void Oef3_563 ()
		{
			Console.Write ("Geef 2 getallen voor ter vergelijking. het eerste getal: ");
			Double g1;
			g1 = Convert.ToDouble (Console.ReadLine ());
			Console.Write ("Het tweede getal: ");
			Double g2;
			g2 = Convert.ToDouble (Console.ReadLine ());
			string vergelijking;
			if (g1 < g2) {
				vergelijking = "kleiner dan";
			} else if (g1 == g2) {
				vergelijking = "gelijk aan";
			} else {
				vergelijking = "groter dan";
			}
			Console.WriteLine ("Het eerste getal " + g1 + " is " + vergelijking + " het tweede getal " + g2 + ".");
		}

		private static void Oef4_564 ()
		{
			// some poor substitues
			const char EURO = 'm';
			const char E_EGUE = 'e';
			const char E_TREMA = 't';

			Console.Write ("Geeft een character dat moet ge-escaped worden in HTML: ");
			char c;
			c = Convert.ToChar(Console.Read());
			Console.WriteLine ();

			string escaped_value;
			// Found something on http://www.theukwebdesigncompany.com/articles/entity-escape-characters.php
			switch (c) {
			case '<':
				escaped_value = "&lt;";
				break;
			case '>':
				escaped_value = "&gt;";
				break;
			case '&':
				escaped_value = "&amp;";
				break;
			case EURO:
				escaped_value = "&euro;";
				break;
			case E_EGUE:
				escaped_value = "&eacute;";
				break;
			case E_TREMA:
				escaped_value = "&euml;";
				break;
			default:
				escaped_value = "";
				Console.WriteLine ("Dumbass!");
				break;
			}

			if ("" != escaped_value) {
				Console.WriteLine ("De escape replacement is: '" + escaped_value + "'.");
			}
		}

		private static void Oef5_565 ()
		{
			//Vraag de gebruiker hoeveel hamburgers, komkommers, kilo's ijs, chocoladerepen, koekjes, 
			//stukken taart (en zo verder) hij/zij vandaag gegeten heeft en 
			//bereken daaruit het aantal caloriën (verzin maar wat voor de caloriën, het gaat om het oefenen ) 
			//Daarna kun je aangeven of de gebruiker teveel, te weinig of ongeveer genoeg heeft gegeten.
			const double hamburgersCalorien = 10;
			const double komkommerCalorien = 15;
			const double ijsCalorien = 20;
			const double chocoladeCalorien = 20;
			const double koekjeCalorien = 25;
			const double taartCalorien = 30;
			const double maxCalorien = 500; // depends on age, length, activity I guess
			const double calorieMarge = 10;

			Console.WriteLine ("Nadat je ingeeft wat je vandaag at, krijg je dieet suggesties.");

			Console.Write ("Hoeveel kilo hamburger: ");
			double hamburgerGewicht;
			hamburgerGewicht = Convert.ToDouble (Console.ReadLine ());
			Console.Write ("Hoeveel kilo komkommer: ");
			double komkommerGewicht;
			komkommerGewicht = Convert.ToDouble (Console.ReadLine ());
			Console.Write ("Hoeveel kilo ijs: ");
			double ijsGewicht;
			ijsGewicht = Convert.ToDouble (Console.ReadLine ());
			Console.Write ("Hoeveel kilo chocolade: ");
			double chocoladeGewicht;
			chocoladeGewicht = Convert.ToDouble (Console.ReadLine ());
			Console.Write ("Hoeveel kilo koekje: ");
			double koekjeGewicht;
			koekjeGewicht = Convert.ToDouble (Console.ReadLine ());
			Console.Write ("Hoeveel kilo taart: ");
			double taartGewicht;
			taartGewicht = Convert.ToDouble (Console.ReadLine ());

			if (0 <= hamburgerGewicht && 0 <= komkommerGewicht &&
				0 <= ijsGewicht && 0 <= chocoladeGewicht &&
				0 <= koekjeGewicht && 0 <= taartGewicht) {
				double calorien;
				calorien = hamburgersCalorien * hamburgerGewicht + 
					komkommerCalorien * komkommerGewicht +
					ijsCalorien * ijsGewicht +
					chocoladeCalorien * chocoladeGewicht +
					koekjeCalorien * koekjeGewicht + 
					taartCalorien * taartGewicht;
				Console.WriteLine ("Je at " + calorien + " calorien. De grens is " + maxCalorien + " met een marge van " + calorieMarge + ".");
				if (calorien < maxCalorien - calorieMarge) {
					Console.WriteLine ("Terug naar tafel; schransen maar ventje!");
				} else if (calorien >= maxCalorien + calorieMarge) {
					Console.WriteLine ("Naar de gym jij papzakje!");
				} else {
					Console.WriteLine ("Een uitgekient dieet!");
				}
			} else {
				Console.WriteLine ("Paljaske! Speel de plezante op een ander.");
			}
		}

		private static void Oef1_661 ()
		{
			Console.WriteLine ("Een serie van 20 tot 10: ");
			for (int i=20; i>=10; i--) {
				Console.WriteLine (i + ".");
			}
		}

		private static void Oef2_662 ()
		{
			const double basis = 2.0;
			const double max = 512.0;
			Console.WriteLine ("De machten van " + basis + " tot " + max + ": ");
			double exponent = 1.0;
			double result;
			result = Math.Pow (basis, exponent);
			while (result <= max) {
				Console.WriteLine ("base: " + basis + ", exponent: " + exponent + ", result: " + result + ".");
				result = Math.Pow (basis, ++exponent);
			}
		}

		private static void Oef3_663 ()
		{
			Console.Write ("Berekent de faculteit. Geef het getal: ");

			int getal = Convert.ToInt32 (Console.ReadLine ());
			if (0 > getal) {
				Console.WriteLine ("Paljaske! Speel de plezante op een ander.");
			} else {
				long faculteit = 1;
				for (int i=2; i<=getal; i++) {
					faculteit *= i;
				}
				Console.WriteLine ("De faculteit van " + getal + "! is " + faculteit + ".");
			}
		}

		public static void Main (string[] args)
		{
			//Oef1_391();
			//Oef2_392();
			//Oef3_393();

			//Oef1_561();
			//Oef2_562();
			//Oef3_563();
			//Oef4_564();
			//Oef5_565();

			Oef1_661();
			Oef2_662();
			Oef3_663();
		}
	}
}
