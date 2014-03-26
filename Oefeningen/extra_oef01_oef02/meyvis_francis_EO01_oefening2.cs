/* Copyright 2013 Francis Meyvis*/

using System;

namespace extra_oef01_oef02
{
	class MainClass
	{
		// Druk het volgende patroon af:
		// * ** *** **** *****
		public static void Main (string[] args)
		{
			const int MAX_PATTERN_LEN = 5;

			String s = " ";
			for (int i = 0; i < MAX_PATTERN_LEN; i++) {
				s = s.Insert(0, "*");
				Console.Write (s);
			}
		}
	}
}
