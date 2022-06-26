using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;

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
            var clipboardText = Clipboard.GetText();

            if (!clipboardText.Contains("\\"))
                return false;

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
            var shotTime = DateTime.Now.ToShortTimeString();
            Clipboard.SetText(shotTime);
        }

		
	}
}
