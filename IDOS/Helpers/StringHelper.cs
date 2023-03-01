namespace IDOS.Helpers;

using System.Globalization;
using System.Text;

public static class StringHelper {
	/// <summary>
	/// Calculates the Levenshtein distance between two given strings.
	/// </summary>
	/// <param name="a">The first string.</param>
	/// <param name="b">The second string.</param>
	/// <param name="caseSensitive">Whether the calculation should take into account the capitalization of the strings.</param>
	/// <returns>The integer representing the Levenshtein distance between the two strings.</returns>
	public static int LevenshteinDistance(string a, string b, bool caseSensitive = false) {
		// Preliminary optimization checks
		if (a == b) return 0;

		// Localize (sizable performance increase)
		int al = a.Length;
		int bl = b.Length;
		
		if (al == 0) return bl;
		if (bl == 0) return al;

		if (caseSensitive == false) {
			a = a.ToLower();
			b = b.ToLower();
		}

		int[,] matrix = new int[al + 1, bl + 1];

		// Initialization of matrix
		// ai = a character index
		for (int ai = -1; ai < al; matrix[++ai, 0] = ai) { }
		// bi = b character index
		for (int bi = -1; bi < bl; matrix[0, ++bi] = bi) { }

		for (int ai = 0; ai < al;) {
			int aip1 = ai + 1;
			
			for (int bi = 0; bi < bl;) {
				int bip1 = bi + 1;
				int cost = a[ai] == b[bi] ? 0 : 1;
		
				matrix[aip1, bip1] = Math.Min(
					Math.Min(matrix[ai, bip1] + 1, matrix[aip1, bi] + 1),
					matrix[ai, bi] + cost);
		
				bi = bip1;
			}
		
			ai = aip1;
		}
		
		return matrix[al, bl];
	}

	// <https://stackoverflow.com/a/249126/15683397> with some modifications.
	/// <summary>
	/// Removes diacritics from a string.
	/// </summary>
	/// <param name="str">The string to remove the diacritics from.</param>
	/// <returns>A string without any diacritics.</returns>
	public static string RemoveDiacritics(this string str) {     
		var normalizedString = str.Normalize(NormalizationForm.FormD);
		var stringBuilder = new StringBuilder(normalizedString.Length);

		foreach (char c in normalizedString) {
			UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
			
			if (unicodeCategory != UnicodeCategory.NonSpacingMark) stringBuilder.Append(c);
		}

		return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
	}
}