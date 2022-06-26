using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackFlip.logic
{
	public class SpellCheck
	{
		private List<string> wordList = new List<String>();

		public List<string> WordList { get => wordList; set => wordList = value; }

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

		public List<string> GetWordListFromFile()
		{
			var filePath = ConfigurationManager.AppSettings["SpellingFile"];
			try
			{
				var file = fileSystem.File.ReadAllText(filePath);
				var list = new List<string>();
				var lines = file.Split('\n');
				foreach (var line in lines)
				{
					wordList.Add(line);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return wordList;
		}


	}
}
