namespace UniversityLibrary.Test
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using System;

    public class Practice
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
            Type expectedResult = typeof(List<TextBook>);
            Type actualResult = library.Catalogue.GetType();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void AddTextBookToLibrary()
        {
            TextBook book = new TextBook("Botev", "Ivan Vazov", "bulgarian");
            var actualResult = library.AddTextBookToLibrary(book);
            Assert.AreEqual(library.Catalogue.Count, 1);
            Assert.AreEqual(book.InventoryNumber, 1);

            var expectedResult = $"Book: {book.Title} - {book.InventoryNumber}" +
                Environment.NewLine + $"Category: {book.Category}" +
                Environment.NewLine + $"Author: {book.Author}";
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void LoanTextBook()
        {

        }
    }
}