using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackFlip;
using System;
using System.Collections.Generic;
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
}