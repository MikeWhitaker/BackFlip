using BackFlip.logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

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
	}
}
