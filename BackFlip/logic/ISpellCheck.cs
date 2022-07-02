using System.Collections.Generic;

namespace BackFlip.logic
{
	public interface ISpellCheck
	{
		void ExecuteSpellCheck();
		List<string> GetWordListFromFile();
	}
}