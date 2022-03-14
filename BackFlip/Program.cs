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
            if(firstArgument != null && firstArgument.ToLowerInvariant() == "-f")
            {
                bf.ReplaceClipboardContensWithFoundFilename();
            }
            else
            {
                bf.Flip();
            }
        }
    }
}
