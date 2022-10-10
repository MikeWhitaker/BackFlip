using BackFlip.logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Windows.Forms;

namespace BackFlipTests.logic
{
	public abstract class SpellCheckTests
	{
		[TestClass]
		public class SpellCheck_constructor
		{
			[TestMethod]
			public void SpellCheck_Constructs()
			{
				// Arrange
				var mockFileSystem = Mock.Of<IFileSystem>();

				// Act & Assert
				var spellcheck = new SpellCheck(mockFileSystem); // Does not throw so the test succeeds.
			}
		}

		[TestClass]
		public class GetWordListFromFileMethod
		{
			[TestMethod]
			public void GetWordListFromFile_Default_ReturnsWordList()
			{
				// Arrange
				var file = Mock.Of<IFile>();
				var fileSystem = Mock.Of<IFileSystem>();

				Mock.Get(fileSystem).SetupGet(p => p.File).Returns(file);
				Mock.Get(file).Setup(f => f.ReadAllText(It.IsAny<string>())).Returns("word1\nword2\nword3");

				
				var spellcheck = new SpellCheck(fileSystem);

				// Act
				var result = spellcheck.GetWordListFromFile();

				// Assert
				Mock.Get(file).Verify(f => f.ReadAllText(It.IsAny<string>()), Times.Once);
				Assert.AreEqual(3, result.Count);
			}

			[TestMethod]
			public void GetWordListFromFile_ReadAllTextThrows_ErrorIsWrittenToConsole()
			{
				// Arrange
				var file = Mock.Of<IFile>();
				var fileSystem = Mock.Of<IFileSystem>();

				Mock.Get(fileSystem).SetupGet(p => p.File).Returns(file);
				Mock.Get(file).Setup(f => f.ReadAllText(It.IsAny<string>())).Throws<Exception>();
				var spellcheck = new SpellCheck(fileSystem);

				var output = new StringWriter();
				Console.SetOut(output); // Redirect console output to a string writer.

				// Act
				try
				{
					var result = spellcheck.GetWordListFromFile();	
				}
				catch (Exception e)
				{
					throw e;
				}

				// Assert
				var containsString = output.ToString().Contains("Exception");
				Assert.IsTrue(containsString);
			}
		}

		[TestClass]
		public class ExecuteSpellCheckTests
		{
			[TestMethod()]
			public void ExecuteSpellCheck_Default_CallsGetWordListFromFile()
			{
				// Arrange
				var spellChecker = new Mock<SpellCheck>();
				spellChecker.Setup(s => s.GetWordListFromFile()).Returns(new List<string>());
				spellChecker.CallBase = true;

				// Act
				spellChecker.Object.ExecuteSpellCheck();

				// Assert
				spellChecker.Verify(s => s.GetWordListFromFile(), Times.Once);
			}

			[TestMethod()]
			public void ExecuteSpellCheck_Default_CallsCalculateForEachItemInWordList()
			{
				// Arrange
				var spellChecker = new Mock<SpellCheck>();
				spellChecker.Setup(s => s.GetWordListFromFile()).Returns(new List<string>() { "word1", "word2", "word3" });
				spellChecker.Setup(s => s.Calculate(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
				spellChecker.CallBase = true;

				// Act
				spellChecker.Object.ExecuteSpellCheck();

				// Assert
				spellChecker.Verify(s => s.Calculate(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(3));
			}

			[TestMethod()]
			public void ExecuteSpellCheck_Default_CallsIsFirstResultExactMatch()
			{
				// Arrange
				var spellChecker = new Mock<SpellCheck>();
				spellChecker.Setup(s => s.GetWordListFromFile()).Returns(new List<string>() { "word1", "word2", "word3" });
				spellChecker.Setup(s => s.IsExactMatch(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
				spellChecker.CallBase = true;

				// Act
				spellChecker.Object.ExecuteSpellCheck();

				// Assert
				spellChecker.Verify(s => s.Calculate(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(3));
			}
		}


		[TestClass]
		public class GetFirstWordFromClipBoardTests
		{
			[TestMethod]
			public void MyTestMethod()
			{
				// Arrange
				Clipboard.SetText("word1 word2 word3");

				var sut = new SpellCheck(Mock.Of<IFileSystem>());

				//Act
				var result = sut.GetFirstWordFromClipBoard();


				//Assert
				Assert.AreEqual("word1", result);
			}
		}
	}
}
