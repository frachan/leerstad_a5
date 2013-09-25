using System;

namespace Week04
{
	class MainClass
	{

		static void Oef2_862()
		{
			Console.WriteLine("Geef een getallen die worden gesommeerd (0 om te stoppen)");
			int value;
			int sum = 0;
			do {
				Console.Write("Getal: ");
				value = Convert.ToInt32(Console.ReadLine ());
				if ( (10<value && 20>value) || (100<value) || (-1>=value) ) {
					sum += value;
				}
			} while(value != 0);
			Console.WriteLine ("De som is " + sum);
		}


		static void Oef1_10131 ()
		{
			int numofValues = 5;    
			// or ask the user, but then also validate the input
			//numofValues = Convert.ToInt32 (Console.ReadLine ());
			int[] values = new int[numofValues];
			for (int i=0; i<numofValues; i++) {
				Console.Write ("Getal " + (i+1) + ": ");
				values[i] = Convert.ToInt32(Console.ReadLine());
			}
			for(int i=numofValues-1; i>=0; i--) {
				Console.WriteLine("Getal " + (i+1) + ": " + values[i]);
			}
		}


		public static void Main (string[] args)
		{
			Oef2_862();
			//Oef1_10131();;
		}
	}
}
