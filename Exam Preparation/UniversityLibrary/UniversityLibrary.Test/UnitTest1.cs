namespace UniversityLibrary.Test
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using System;

    public class Tests
    {
        private UniversityLibrary library;
        [SetUp]
        public void Setup()
        {
             library = new UniversityLibrary();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(library);
            Type expectedType = typeof(List<TextBook>);
            Type actualType = library.Catalogue.GetType();

            Assert.AreEqual(expectedType, actualType);
        }

        [Test]
        public void AddTextBookToLibrary()
        {
            TextBook book = new TextBook("Crime", "John", "Horror");
            string expectedMessage = library.AddTextBookToLibrary(book);

            Assert.AreEqual(library.Catalogue.Count, 1);
            Assert.AreEqual(book.InventoryNumber, 1);
            Assert.AreEqual(expectedMessage,
                $"Book: {book.Title} - {book.InventoryNumber}" + Environment.NewLine
                + $"Category: {book.Category}" + Environment.NewLine
                + $"Author: {book.Author}");
        }

        [Test]
        public void LoanTextBookToLibraryMethodShouldWorkCorrectly()
        {
            TextBook bookOne = new TextBook("Pride and prejudice", "Jane Austin", "Classics");
            TextBook bookTwo = new TextBook("Crime and punishment", "Fyodor Dostoevsky", "Classics");
            bookOne.Holder = "Krisko";

            library.AddTextBookToLibrary(bookOne);
            library.AddTextBookToLibrary(bookTwo);

            string bookOneExpectedMessage = $"Krisko still hasn't returned {bookOne.Title}!";
            string bookTwoExpectedMessage = $"{bookTwo.Title} loaned to Krisko.";

            string bookOneActualMessage = library.LoanTextBook(1, "Krisko");
            string bookTwoActualMessage = library.LoanTextBook(2, "Krisko");

            Assert.AreEqual(bookOneExpectedMessage, bookOneActualMessage);
            Assert.AreEqual(bookTwoExpectedMessage, bookTwoActualMessage);
            Assert.That(bookTwo.Holder == "Krisko");
        }

        [Test]
        public void ReturnTextBookMethodShouldWorkCorrectly()
        {
            TextBook book = new TextBook("Pride and prejudice", "Jane Austin", "Classics");
            book.Holder = "Krisko";
            library.AddTextBookToLibrary(book);

            string expectedMessage = $"{book.Title} is returned to the library.";
            string actualMessage = library.ReturnTextBook(1);

            Assert.AreEqual(expectedMessage, actualMessage);
            Assert.AreEqual(book.Holder, String.Empty);
        }
    }
}