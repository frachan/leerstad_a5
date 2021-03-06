using System;

namespace Week03
{
	class MainClass
	{
		static void Oef4_664 ()
		{
			for (int hour = 0; hour<24; hour++) {
				string hourStr = "";
				if (10 > hour) {
					hourStr = "0";
				}
				hourStr += hour + "h";

				for (int min = 0; min<60; min++) {
					string minStr = hourStr;
					if (10 > min) {
						minStr += "0";
					}
					minStr += min + "m";

					for (int sec = 0; sec<60; sec++) {
						string secStr = minStr;
						if (10 > sec) {
							secStr += "0";
						}
						secStr += sec + "s";
						System.Console.WriteLine (secStr);
					}
				}
			}
		}


		static void Oef5_665 ()
		{
			int input;
			do {
				Console.Write("Geef het aantal seconden, om te zetten in dagen, uren, minuten & seconden: ");
				input = Convert.ToInt32(Console.ReadLine ());

				if ( 0 <= input ) {
					int days = input / (60 * 60 * 24);
					int rem = input % (60 * 60 * 24);
					int hours = rem / (60 * 60);
					rem %= (60 * 60);
					int min = rem / 60;
					int sec = rem % 60;

					string output;
					if ( 0 < days ) {
						output = "" + days + (( 1 == days ) ? " dag" : " dagen");
						Console.WriteLine (output);
					}
					if ( 0 < hours ) {
						output = "" + hours + (( 1 == hours ) ? " uur" : " uren");
						Console.WriteLine (output);
					}
					if ( 0 < min ) {
						output = "" + min + (( 1 == min ) ? " minuut" : " minuten");
						Console.WriteLine (output);
					}
					if ( 0 < sec ) {
						output = "" + sec + (( 1 == sec ) ? " second" : " seconden");
						Console.WriteLine (output);
					}
				} else {
					Console.WriteLine ("Enkel positieve waarden!");
				}
			} while ( 0 != input );
		}


		static void Oef6_666 ()
		{
			int total = 0;
			int input = 1;
			while ( 0 != input ) {
				input = Convert.ToInt32 (Console.ReadLine ()); 
				if ( 0 != input ) {
					total += input;
					Console.WriteLine ("+");
				}
			}
			Console.WriteLine ("=\n" + total);
		}


		static void Oef7_667 ()
		{
			int input;
			int total;
			total = input = Convert.ToInt32 (Console.ReadLine ());
			char oper;
			do {
				// Ask why I cannot use Console.Read(), it makes fail reading for next input.
				oper = Console.ReadLine ()[0];
				if ( '=' != oper ) {
					input = Convert.ToInt32 (Console.ReadLine ());
					switch ( oper ) {
					case '+':
						total += input;
						break;
					case '-':
						total -= input;
						break;
					default:
						Console.WriteLine ("Wrong operator");
						break;
					}
				}
			} while ( '=' != oper );
			Console.WriteLine (total);
		}


		static void Oef8_668 ()
		{
			Console.Write ("Will convert your number to octals: ");
			int input = Convert.ToInt32 (Console.ReadLine ());
			Console.WriteLine (Convert.ToString(input, 8) + 'o');
		}


		static void Oef9_669 ()
		{
			Console.Write ("Will convert your number to hexadecimal: ");
			int input = Convert.ToInt32 (Console.ReadLine ());
			Console.WriteLine (Convert.ToString(input, 16) + 'h');
		}


		static void Oef10_6610 ()
		{
			Console.Write ("Will convert your number to binary: ");
			int input = Convert.ToInt32 (Console.ReadLine ());
			Console.WriteLine (Convert.ToString(input, 2) + 'b');
		}


		static void Oef11_6611 ()
		{
			Console.WriteLine ("Enter a sentence to have all capitals converted to smallcaps:");
			string sentence = Console.ReadLine ();
			string output = "";
			for (int i=0; i<sentence.Length; i++) {
				char c;
				c = sentence[i];
				if ( 'A' <= c && 'Z' >= c ) {
					c = Convert.ToChar ('a' - 'A' + c);
				}
				output += c;
			}
			Console.WriteLine (output);
		}


		static void Oef12_6612 ()
		{
			Console.WriteLine ("Enter a sentence to have all space replaced with an underscore:");
			string sentence = Console.ReadLine ();
			string output = "";
			for (int i=0; i<sentence.Length; i++) {
				char c;
				c = sentence[i];
				if ( ' ' == c ) {
					c = '_';
				}
				output += c;
			}
			Console.WriteLine (output);
		}


		static void Oef13_6613 ()
		{
			// This implementation does not consider proper word-bounderies
			Console.WriteLine ("Enter a sentence, next a search term in the sentence:");
			string sentence = Console.ReadLine ();
			string pattern = Console.ReadLine ();
			int matchCnt = 0;
			if (0 != pattern.Length) {
				int i = 0;
				while (i <= (sentence.Length - pattern.Length)) {
					int j = 0;
					while ((j < pattern.Length) && (sentence[i+j] == pattern[j])) {
						j++;
					}
					if ( j == pattern.Length ) {
						matchCnt++;
						i += pattern.Length;
					} else {
						i++;
					}
				}
			}
			string message = "The ";
			message += (0 == pattern.Length) ? "empty search term " : "search term '" + pattern + "' ";
			switch (matchCnt) {
			case 0:
				message += "did not occur.";
				break;
			case 1:
				message += "occurred once.";
				break;
			default:
				message += "occurred " + matchCnt + " times.";
				break;
			}
			Console.WriteLine (message);
		}


		public static void Oef1_711()
		{
			Console.WriteLine ("Enter numbers. The code stops and outputs the largest number when a negative value is entered.");
			int input = Convert.ToInt32( Console.ReadLine ());
			int largestNumber = input;
			while ( 0 <= input ) {
				input = Convert.ToInt32( Console.ReadLine ());
				if ( input > largestNumber ) {
					largestNumber = input;
				}
			}
			Console.WriteLine ("The largest number is: " + largestNumber + ".");
		}


		public static void Oef2_712 ()
		{
			Console.WriteLine ("Enter 2 boundary numbers. The program outputs all numbers between (excluding) the 2 bouderies.");
			Console.Write ("First boundary number: ");
			int bound1 = Convert.ToInt32 (Console.ReadLine ());
			Console.Write ("Second boundary number: ");
			int bound2 = Convert.ToInt32 (Console.ReadLine ());

			int loop = bound1;
			int end = bound2;
			if (bound1 > bound2) {
				loop = bound2 ;
				end = bound1;
			}
			for (loop += 1; loop < end; loop++) {
				Console.WriteLine ("" + loop);
			}
		}


		public static void Oef3_713 ()
		{
			Console.Write ("Enter a number larger than zero: ");
			int input = Convert.ToInt32 (Console.ReadLine ());

			if (0 < input) {
				int value = input;
				while (1 != value) {
					if (0 == value % 2) {
						value /= 2;
					} else {
						value = value * 3 + 1;
					}
					Console.WriteLine ("" + value);
				}
			} else {
				Console.WriteLine ("Ouch! You cannot read ...");
			}
		}


		public static void Main (string[] args)
		{
			//Oef4_664();
			//Oef5_665();
			//Oef6_666();
			//Oef7_667();
			//Oef8_668();
			//Oef9_669();
			//Oef10_6610();
			//Oef11_6611();
			//Oef12_6612();
			//Oef13_6613();

			//Oef1_711();
			//Oef2_712();
			Oef3_713();
		}
	}
}

