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


            var firstArgument = args.FirstOrDefault().ToLowerInvariant();

            switch (firstArgument)
            {
                case "-f":
                    bf.ReplaceClipboardContensWithFoundFilename();
                    break;
                case "-file":
                    bf.ReplaceClipboardContensWithFoundFilename();
                    break;
                case "-t":
                    bf.SetTimeOnClipboard();
                    break;
                case "-time":
                    bf.SetTimeOnClipboard();
                    break;
                case "-date":
                    bf.SetDateStringOnClipboard();
                    break;
                case "-d":
                bf.SetDateStringOnClipboard();
                    break;
                default:
                    bf.Flip();
                    break;
            }
        }
    }
}
