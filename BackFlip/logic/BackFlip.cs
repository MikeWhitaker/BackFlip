using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;
using BackFlip.logic;

namespace BackFlip
{
    public class BackFlip
    {
        public BackFlip()
        {
           
        }
        public BackFlip(string clipboardText)
        {
            Clipboard.SetText(clipboardText);
        }

        public bool Flip()
        {
            // Check if the clipboard contains a backslash
            var clipboardText = Clipboard.GetText();

            if (!clipboardText.Contains("\\"))
            {
                // If not, run the help method
                ShowHelp();
                
                return false;
            }

            clipboardText = clipboardText.Replace('\\', '/');
            Clipboard.SetText(clipboardText); // This means that the test would have to access the clipboard.

            return true;
        }

        public string GetClipboardText() { // Mainly for the tests.
            return Clipboard.GetText();
        }

        public bool ReplaceClipboardContensWithFoundFilename()
        {
            var clipboardText = Clipboard.GetText();

            var windowsPattern = @"^.*\\(.+\\)*(.+)\.(.+)$";
            var linuxPattern = @"^.*\/(.+\/)*(.+)\.(.+)$";
            var matchWindows = Regex.Match(clipboardText, windowsPattern);
            var matchLinux = Regex.Match(clipboardText, linuxPattern);

            if (!matchWindows.Success && !matchLinux.Success) //should try to match based on linux file path
                return false;

            Match match;

            if (matchWindows.Success)
            {
                match = matchWindows;
            } else
            {
                match = matchLinux;
            }

            var filename = match.Groups[2].Value;
            var filenextention = match.Groups[3].Value;
            var filenameWithExtention = string.Format("{0}.{1}", filename, filenextention);
            Clipboard.SetText(filenameWithExtention);

            return true;
        }

        public void SetDateStringOnClipboard()
        {
            var shortDate = DateTime.Now.ToShortDateString();
            Clipboard.SetText(shortDate);
        }

        public void SetTimeOnClipboard()
        {
            var shortTime = DateTime.Now.ToShortTimeString();
            Clipboard.SetText(shortTime);
        }
        private static string ShortDateTime()
        {
            // get the date and time
            var shortDate = DateTime.Now.ToShortDateString();
            var shortTime = DateTime.Now.ToShortTimeString();

            // combine them
            var shortDateTime = string.Format("{0} {1}", shortDate, shortTime);
            return shortDateTime;
        }
        
        public void SetDateTimeStringOnClipboard()
        {
            var shortDateTime = ShortDateTime();

            // set the clipboard
            Clipboard.SetText(shortDateTime);
        }

        public void SetSignatureOnClipboard()
        {
            var signature = ConfigurationManager.AppSettings["Signature"];
            // get the short date time
            var shortDateTime = ShortDateTime();
            
            // combine them
            signature = string.Format("{0} {1}", signature, shortDateTime);
            
            Clipboard.SetText(signature);
        }
        
        public void SpellCheck(int filenumber)
		{
			var sc = new SpellCheck();
            sc.ExecuteSpellCheck(filenumber);
		}

        public void ShowHelp()
        {
            // Write the help text to the console
            Console.WriteLine("BackFlip:");
            Console.WriteLine("  -h, -help: Show this help text");
            Console.WriteLine("  -f, -file: Replace the clipboard contents with the filename found in the clipboard contents");
            Console.WriteLine("  -t, -time: Replace the clipboard contents with the current time");
            Console.WriteLine("  -d, -date: Replace the clipboard contents with the current date");
            Console.WriteLine("  -dt, -datetime: Replace the clipboard contents with the current date and time");
            Console.WriteLine("  -sig: Replace the clipboard contents with the signature found in the app.config file");
            Console.WriteLine("  -s, -spellcheck: Spellcheck the clipboard contents using the first spelling file");
            Console.WriteLine("  -s2: Spellcheck the clipboard contents using the second spelling file");
            Console.WriteLine("  If no arguments are given, the clipboard contents will be flipped from \\ to /");
        }
    }
}
