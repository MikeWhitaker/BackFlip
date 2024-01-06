using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackFlip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackFlip.Tests
{
    [TestClass()]
    public class FlipTests
    {
        [TestMethod()]
        public void BackFlip_WithSlashInClipboardMemory_FlipsTheSlash()
        {
            // Arrange
            var sut = new BackFlip(@"c:\test\test\");

            // Act
            sut.Flip();

            // Assert
            Assert.AreEqual(sut.GetClipboardText(), "c:/test/test/");
        }

        [TestMethod()]
        public void BackFlip_WithSlashInClipboardMemory_ReturnsTrue()
        {
            // Arrange
            var sut = new BackFlip(@"c:\test\test\");

            // Act
            var result = sut.Flip();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void BackFlip_WithoutSlashInClipboardMemory_RetrunFalse()
        {
            // Arrange
            var sut = new BackFlip("thereIsNoSlash");

            // Act
            var result = sut.Flip();

            // Assert
            Assert.IsFalse(result);
        }
    }

    [TestClass()]
    public class GetFilenameTest
    {
        [TestMethod]
        public void GetFilename_WithMatchingPattern_ReturnsTrue()
        {
            // Arrange
            var clipBoardContens = @"c:\test\test\filename.txt";
            var sut = new BackFlip(clipBoardContens);

            // Act
            var result = sut.ReplaceClipboardContensWithFoundFilename();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetFilename_WithMatchingPattern_SetsFilenameOntheClipboard()
        {
            // Arrange
            var clipBoardContens = @"c:\test\test\filename.txt";
            var sut = new BackFlip(clipBoardContens);

            // Act
            var result = sut.ReplaceClipboardContensWithFoundFilename();

            // Assert
            Assert.AreEqual(sut.GetClipboardText(), "filename.txt");
        }

        [TestMethod]
        public void GetFilename_WithoutAMatchingPattern_ReturnsFalse()
        {
            // Arrange
            var clipBoardContens = @"notMatching";
            var sut = new BackFlip(clipBoardContens);

            // Act
            var result = sut.ReplaceClipboardContensWithFoundFilename();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetFilename_WithoutMatchingPattern_DoesNotChangeTheContenseOfTheClipboard()
        {
            // Arrange
            var clipBoardContens = @"aString";
            var sut = new BackFlip(clipBoardContens);

            // Act
            var result = sut.ReplaceClipboardContensWithFoundFilename();

            // Assert
            Assert.AreEqual(sut.GetClipboardText(), "aString");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void GetFilename_WithMatchingLinuxPattern_ReturnsTrue()
        {
            // Arrange
            var clipBoardContens = @"c:/test/test/filename.txt";
            var sut = new BackFlip(clipBoardContens);

            // Act
            var result = sut.ReplaceClipboardContensWithFoundFilename();

            // Assert
            Assert.IsTrue(result);
        }
    }

    [TestClass]
    public class SetDateStringOnClipBoard
    {
        [TestMethod]
        public void SetDateStringOnClipboard_SetStringOnClipboard()
        {
            // Arrange
            var sut = new BackFlip();

            // Act
            sut.SetDateStringOnClipboard();

            // Assert
            var expected = DateTime.Now.ToShortDateString();
            var actual = sut.GetClipboardText();

            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class SetTimeOnClipboard
    {
        [TestMethod]
        public void SetTimeOnClipboard_SetStringOnClipboard()
        {
            // Arrange
            var sut = new BackFlip();

            // Act
            sut.SetTimeOnClipboard();
            var expected = DateTime.Now.ToShortTimeString();
            var actual = sut.GetClipboardText();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class SetDateTimeStringOnClipboard
    {
        [TestMethod]
        public void SetDateTimeStringOnClipboard_SetStringOnClipboard()
        {
            // Arrange
            var sut = new BackFlip();

            // Act
            sut.SetDateTimeStringOnClipboard();
            var expected = DateTime.Now.ToString();
            expected = expected.Substring(0, expected.LastIndexOf(':'));

            var actual = sut.GetClipboardText();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class SetSignatureOnClipboard
    {
        [TestMethod]
        public void SetSignatureOnClipboard_SetStringOnClipboard()
        {
            // Arrange
            var sut = new BackFlip();
            sut.SetDateTimeStringOnClipboard();
            var dateAndTime = sut.GetClipboardText();

            ConfigurationManager.AppSettings["Signature"] = "mySignature";
            var expected = string.Format("mySignature {0}", dateAndTime);

            // Act
            sut.SetSignatureOnClipboard();
            var actual = sut.GetClipboardText();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class ReplaceClipboardContents
    {
        [TestMethod]
        public void ReplaceClipboardContents_ReplacesDefaultFindStringWithDefaultReplaceString()
        {
            // Arrange
            var originalText = "This is a string containing the , defaultFindString";
            var sut = new BackFlip(originalText);
            ConfigurationManager.AppSettings["defaultFindSting"] = ",";
            ConfigurationManager.AppSettings["defaultReplaceString"] = ".";

            // Act
            sut.ReplaceClipboardContents();

            // Assert
            var expectedText = "This is a string containing the . defaultFindString";
            var actualText = sut.GetClipboardText();
            Assert.AreEqual(expectedText, actualText);
        }

        [TestMethod]
        public void ReplaceClipboardContents_ReplacesNumberContainingSearchtextWithDefaultReplaceString()
        {
            // Arrange
            var originalText = "19,98";
            var sut = new BackFlip(originalText);
            ConfigurationManager.AppSettings["defaultFindSting"] = ",";
            ConfigurationManager.AppSettings["defaultReplaceString"] = ".";

            // Act
            sut.ReplaceClipboardContents();

            // Assert
            var expectedText = "19.98";
            var actualText = sut.GetClipboardText();
            Assert.AreEqual(expectedText, actualText);
        }
        
        [TestMethod]
        public void ReplaceClipboardContents_WhenClipboardDoesNotContainDefaultFindString_ClipboardRemainsUnchanged()
        {
            // Arrange
            var originalText = "This is a string not containing the defaultFindString";
            var sut = new BackFlip(originalText);
            ConfigurationManager.AppSettings["defaultFindSting"] = ",";
            ConfigurationManager.AppSettings["defaultReplaceString"] = ".";

            // Act
            sut.ReplaceClipboardContents();

            // Assert
            var actualText = sut.GetClipboardText();
            Assert.AreEqual(originalText, actualText);
        }

        [TestMethod]
        public void ReplaceClipboardContents_WhenClipboardIsEmpty_ClipboardRemainsEmpty()
        {
            // Arrange
            var originalText = "";
            var sut = new BackFlip(originalText);
            ConfigurationManager.AppSettings["defaultFindSting"] = ",";
            ConfigurationManager.AppSettings["defaultReplaceString"] = ".";

            // Act
            sut.ReplaceClipboardContents();

            // Assert
            var actualText = sut.GetClipboardText();
            Assert.AreEqual(originalText, actualText);
        }
    }
    
}