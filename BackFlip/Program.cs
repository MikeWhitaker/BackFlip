using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackFlip
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            var bf = new BackFlip();

            var firstArgument = args.FirstOrDefault();
            if (firstArgument == null)
                firstArgument = "aSafeDefault";
            
            firstArgument.ToLowerInvariant();

            switch (firstArgument)
            {
                case "-f":
                case "-file":
                    bf.ReplaceClipboardContensWithFoundFilename();
                    break;
                case "-t":
                case "-time":
                    bf.SetTimeOnClipboard();
                    break;
                case "-date":
                case "-d":
                    bf.SetDateStringOnClipboard();
                    break;
                case "-spellcheck":
                case "-s":
					bf.SpellCheck();
					break;
				default:
                    bf.Flip();
                    break;
            }
        }
    }
}
