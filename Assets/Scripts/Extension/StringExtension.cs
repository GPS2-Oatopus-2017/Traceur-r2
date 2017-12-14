using System.Collections;
using System.Collections.Generic;

public static class StringExtension
{
	private static string[] numberCaches;

	static StringExtension()
	{
		numberCaches = new string[100];

		// should go from 0 to 99.
		for (int i = 0; i < 100; i++)
		{
			numberCaches [i] = string.Format ("{0:00}", i);
		}
	}

	public static string ToStringLite(int number)
	{
		if(number >= 100) return number.ToString();
		return numberCaches[number];
	}
}
