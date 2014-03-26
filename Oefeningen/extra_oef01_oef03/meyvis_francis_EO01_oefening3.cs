/* Copyright 2013 Francis Meyvis*/

using System;

namespace extra_oef01_oef03
{
	class MainClass
	{
		// Druk het volgende patroon af:
		// ***   ***   ***
		// ***   ***   ***
		//    ***   ***   ***
		//    ***   ***   ***
		// ***   ***   ***
		// ***   ***   ***
		//    ***   ***   ***
		//    ***   ***   ***
		// ***   ***   ***
		// ***   ***   ***
		//    ***   ***   ***
		//    ***   ***   ***		
		public static void Main (string[] args)
		{
			const int LOOP_LINE_CNT = 6;   // number of times to repeat the lines
			const int LINE_REPEAT_CNT = 2; // number of times to repeat the same line
			const int PATTERN_REPEAT_CNT = 3; // number of times to repeat pattern to build up line
			const int PATTERN_LEN = 3;     // length of a specific pattern in a line (e.g. "***   ")
			const char PATTERN_CHAR_1 = '*'; // characters to use in the pattern
			const char PATTERN_CHAR_2 = ' ' ;

			for (int loop_cnt = 0; loop_cnt < LOOP_LINE_CNT; loop_cnt++) {
				char char_a, char_b;
				// alternate the pattern characters based on the even or odd loop count
				if (0 == (loop_cnt & 1)) {
					char_a = PATTERN_CHAR_1;
					char_b = PATTERN_CHAR_2;
				} else {
					char_a = PATTERN_CHAR_2;
					char_b = PATTERN_CHAR_1;
				}
				// build-up a single line pattern with the pattern characters
				string line_pattern = "";
				for (int i = 0; i < PATTERN_LEN; i++) {
					line_pattern += char_a;
				}
				for (int i = 0; i< PATTERN_LEN; i++) {
					line_pattern += char_b;
				}
				// build-up the line using the line pattern
				string line = "";
				for (int j = 0; j < PATTERN_REPEAT_CNT; j++) {
					line += line_pattern;
				}
				// show the same line
				for (int line_cnt = 0; line_cnt < LINE_REPEAT_CNT; line_cnt++) {
					Console.WriteLine(line);
				}
			}
		}
	}
}
