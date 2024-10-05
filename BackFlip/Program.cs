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
                case "-h":
                case "-help":
                    bf.ShowHelp();
                    break;  
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
                case "-datetime":
                case "-dt":
                    bf.SetDateTimeStringOnClipboard();
                    break;
                case "-spellcheck":
                case "-s":
                    bf.SpellCheck(1);
                    break;
                case "-s2":
                case "-se":
                    bf.SpellCheck(2);
                    break;
                case "-sig":
                    bf.SetSignatureOnClipboard();
                    break;
                case "-replace":
                case "-r":
                    bf.ReplaceClipboardContents();
                    break;
                default:
                    bf.Flip();
                    break;
            }
        }
    }
}