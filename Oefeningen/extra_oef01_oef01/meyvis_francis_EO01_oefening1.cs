/* Copyright 2013 Francis Meyvis*/

using System;

namespace extra_oef01_oef01
{
	class MainClass
	{
		// Druk af: 
		// 1, 4, 9, 16, 25, 36, 49
	    // Dit zijn de opeenvolgende kwadraten kleiner dan n=50. 
		// De moeilijkheid bestaat er hier vooral in ervoor te zorgen 
		// dat er geen komma wordt afgedrukt na het laatste getal.
		public static void Main (string[] args)
		{
			const double MAX_SQUARE = 50.0;  ///< largest permissable square

			double i = 1.0;
			double square = Math.Pow (i, 2.0);
			if (square < MAX_SQUARE) {
				// No comma to print for first square
				System.Console.Write (square);
				i += 1.0;
				square = Math.Pow (i, 2.0);
				while (square < MAX_SQUARE) {
					// Once inside the while loop, print the coma before the square
					System.Console.Write (", " + square);
					i += 1.0;
					square = Math.Pow (i, 2.0);
				} 
			}
			System.Console.WriteLine();
		}
	}
}
