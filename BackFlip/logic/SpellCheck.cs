using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackFlip.logic
{
	public class SpellCheck : ISpellCheck
	{
		private List<string> wordList = new List<String>();

		readonly IFileSystem fileSystem;

		public SpellCheck(IFileSystem fileSystem)
		{
			this.fileSystem = fileSystem;
		}

		public SpellCheck() : this(
			fileSystem: new FileSystem() //use default implementation which calls System.IO
		)
		{
		}

		public virtual List<string> GetWordListFromFile(int filenumber)
		{
			var filePath = ConfigurationManager.AppSettings["SpellingFile" + filenumber.ToString()];

			// if the filePath variable is null default to the first file
			if (filePath == null)
			{
				filePath = ConfigurationManager.AppSettings["SpellingFile1"];
			}

			try
			{
				var file = fileSystem.File.ReadAllText(filePath);
				var list = new List<string>();
				var lines = file.Split('\n');

				foreach (var line in lines)
				{
					// if line end with \r remove it
					var trimmedLine = line.TrimEnd('\r', '\n');

					wordList.Add(trimmedLine);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return wordList;
		}

		public void ExecuteSpellCheck(int filenumber)
		{
			// get the first five with the lowest count levenshtein routine 
			wordList = GetWordListFromFile(filenumber);
			var firstWordFromClipboardText = GetFirstWordFromClipBoard();

			var levenshteinDistanceDict = new Dictionary<string, int>();
			// iterate over the wordlist
			foreach (var word in wordList)
			{
				var dist = Calculate(firstWordFromClipboardText, word);
				levenshteinDistanceDict.Add(word, dist);
			}

			var sortedDictionary = SortLevenshteinDictionary(levenshteinDistanceDict);

			SetTop5ToClipboard(sortedDictionary, firstWordFromClipboardText);
		}

		public virtual Dictionary<string, int> SortLevenshteinDictionary(Dictionary<string, int> levenshteinDistanceDict)
		{
			var sortedDictionary = levenshteinDistanceDict.OrderBy(x => x.Value);
			var topFive = sortedDictionary.Take(5);
			return topFive.ToDictionary(x => x.Key, x => x.Value);
		}

		public virtual void SetTop5ToClipboard(Dictionary<string, int> sortedDict, string firstWordFromClipboardText)
		{
			var sb = new StringBuilder();
			foreach (var item in sortedDict)
			{
				if(item.Key == firstWordFromClipboardText) {
					sb.AppendLine(item.Key + " <3");
					continue;
				}
				
				sb.AppendLine(item.Key);
			}
			Clipboard.SetText(sb.ToString());
		}

		public virtual string GetFirstWordFromClipBoard()
		{
			var clipboardText = Clipboard.GetText();
			var words = clipboardText.Split(' ');
			var firstWord = words.First();
			return firstWord;
		}
		

		public virtual int Calculate(string source1, string source2) //O(n*m)
		{
			var source1Length = source1.Length;
			var source2Length = source2.Length;

			var matrix = new int[source1Length + 1, source2Length + 1];

			// First calculation, if one entry is empty return full length
			if (source1Length == 0)
				return source2Length;

			if (source2Length == 0)
				return source1Length;

			// Initialization of matrix with row size source1Length and columns size source2Length
			for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
			for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

			// Calculate rows and collumns distances
			for (var i = 1; i <= source1Length; i++)
			{
				for (var j = 1; j <= source2Length; j++)
				{
					var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

					matrix[i, j] = Math.Min(
						Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
						matrix[i - 1, j - 1] + cost);
				}
			}
			// return result
			return matrix[source1Length, source2Length];
		}

		public void IsExactMatch(string v1, string v2)
		{
			throw new NotImplementedException();
		}

		public void ExecuteSpellCheck()
		{
			throw new NotImplementedException();
		}

		public List<string> GetWordListFromFile()
		{
			throw new NotImplementedException();
		}
	}
}
