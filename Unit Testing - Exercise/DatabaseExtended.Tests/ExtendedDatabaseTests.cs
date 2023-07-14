using System;
using System.Collections.Generic;
using System.Linq;
using ExtendedDatabase;

namespace DatabaseExtended.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ExtendedDatabaseTests
    {
        private Person[] people;
        private Database myDatabase;

        [SetUp]
        public void SetUp()
        {
            this.people = new Person[16];
            this.myDatabase = new Database();
        }


        [TestCase(1234, "Gosho")]
        public void ConfirmPersonIsCorrectlyCreatedWithConstructor(long id, string userName)
        {
            Person person = new Person(1234, "Gosho");
            Person secondPerson = new Person(id, userName);

            Assert.AreEqual(person.UserName, secondPerson.UserName);
            Assert.AreEqual(person.Id, secondPerson.Id);
        }


        private static readonly object[] _sourceListsOneElement =
        {
            new object[] {new Person[] {new Person(123,"Koza")}},   //case 1 - 1 entry
        };
        private static readonly object[] _sourceListsTwoElements =
        {
            new object[] {new Person[] {new Person(123,"Ivan"), new Person(456, "Dragan") } },   //case - 2 entries
        };
        private static readonly object[] _sourceListsSixteenElements =
        {
            new object[] {new Person[] {
                new Person(123,"Petar"),
                new Person(3123, "Toni"),
                new Person(5,"Teodor"),
                new Person(54,"2"),
                new Person(77,"3"),
                new Person(8765,"ddd"),
                new Person(4242,"kkk"),
                new Person(3594312499,"Ivhhhhhan"),
                new Person(33,"zzzzz"),
                new Person(454,"llll"),
                new Person(555,"tutka"),
                new Person(666,"poli"),
                new Person(777,"vankata"),
                new Person(888,"mimi"),
                new Person(999,"Qqq"),
                new Person(10,"Alibaba")}},   //case - 16 entries
        };
        private static readonly object[] _sourceListsSeventeenElements =
        {
            new object[] {new Person[] {
                new Person(123,"Petar"),
                new Person(3123, "Toni"),
                new Person(5,"Teodor"),
                new Person(54,"2"),
                new Person(77,"3"),
                new Person(8765,"ddd"),
                new Person(4242,"kkk"),
                new Person(3594312499,"Ivhhhhhan"),
                new Person(33,"zzzzz"),
                new Person(454,"llll"),
                new Person(555,"tutka"),
                new Person(666,"poli"),
                new Person(777,"vankata"),
                new Person(888,"mimi"),
                new Person(999,"Qqq"),
                new Person(10,"Alibaba"),
                new Person(8731387,"Seventeen") }},   //case - 17 entries
        };
        private static readonly object[] _sourceListsNinenteenElements =
        {
            new object[] {new Person[] {
                new Person(123,"Petar"),
                new Person(3123, "Toni"),
                new Person(5,"Teodor"),
                new Person(54,"2"),
                new Person(77,"3"),
                new Person(8765,"ddd"),
                new Person(4242,"kkk"),
                new Person(3594312499,"Ivhhhhhan"),
                new Person(33,"zzzzz"),
                new Person(454,"llll"),
                new Person(555,"tutka"),
                new Person(666,"poli"),
                new Person(777,"vankata"),
                new Person(888,"mimi"),
                new Person(999,"Qqq"),
                new Person(10,"Alibaba"),
                new Person(31223333,"Seventeen"),
                new Person(10101,"Eighteen"),
                new Person(9996754471,"Nineteen") }},   //case - 19 entries
        };
        private static readonly object[] _sourceListsZeroElement =
        {
            new object[] {new Person[] {}}   //case 1 - 1 empty list
        };



        [TestCaseSource(nameof(_sourceListsOneElement))]
        [TestCaseSource(nameof(_sourceListsTwoElements))]
        [TestCaseSource(nameof(_sourceListsSixteenElements))]
        public void TestingIfAddRangeMethodWorksCorrectly(Person[] data)
        {
            List<Person> peopleList = new List<Person>();
            for (int i = 0; i < data.Length; i++)
            {
                peopleList.Add(data[i]);
            }
            Person[] finalPersonList = peopleList.ToArray();
            var count = data.Length;
            var addedCount = finalPersonList.Length;

            Assert.AreEqual(count, addedCount, "Not all elements added to the collections, count should match to data.Lenght");
        }

        [Test]
        public void TestingDatabaseConstructorWithZeroArgumentsCreates16PersonArrayDB()
        {
            Database db = new Database();
            int dbListCount = db.Count;
            int expectedListCount = 0;

            Assert.AreEqual(expectedListCount, dbListCount, "DataBase constructor should create 16 Person list if no entries");
        }

        [TestCaseSource(nameof(_sourceListsOneElement))]
        [TestCaseSource(nameof(_sourceListsTwoElements))]
        [TestCaseSource(nameof(_sourceListsSixteenElements))]
        [TestCaseSource(nameof(_sourceListsZeroElement))]
        public void CreateDatabaseWithDifferentCountOfPersonWithinRequiredRangeOfSixteen(Person[] data)
        {
            Database database = new Database(data);
            Person[] persons = data;

            int expectedCount = persons.Length;
            int actualCount = database.Count;

            Assert.AreEqual(expectedCount, actualCount, "Database should be created with the correct number of People, up to 16");
        }

        [TestCaseSource(nameof(_sourceListsSeventeenElements))]
        [TestCaseSource(nameof(_sourceListsNinenteenElements))]
        public void CreateDatabaseWithMoreThanSixteenPersonWithAddRangeMethod(Person[] data)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Database db = new Database(data);
            }, "Provided data length should be in range [0..16]!");
        }

        [TestCaseSource(nameof(_sourceListsTwoElements))]
        public void VerifyCountWorksCorrectly(Person[] data)
        {
            Database db = new Database(data);
            int count = db.Count;
            int expectedCount = data.Length;

            Assert.AreEqual(expectedCount, count, "Count should display correct value.");
        }

        [Test]
        public void TestAddingPersonWithSameNameAlreadyInTheListOfPersons()
        {
            Database db = new Database();
            db.Add(new Person(123, "Gosho"));

            Assert.Throws<InvalidOperationException>(() =>
            {
                db.Add(new Person(456, "Gosho"));
            }, "There is already user with this username!");
        }


        [Test]
        public void TestAddingPersonWithSameUserIDAlreadyInTheListOfPersons()
        {
            Database db = new Database();
            db.Add(new Person(123, "Gosho"));

            Assert.Throws<InvalidOperationException>(() =>
            {
                db.Add(new Person(123, "Ivan"));
            }, "There is already user with this Id!");
        }

        [TestCaseSource(nameof(_sourceListsSixteenElements))]
        public void TestAddingPersonWhenThePersonListAlreadySixteen(Person[] data)
        {
            Database db = new Database(data);

            Assert.Throws<InvalidOperationException>(() =>
            {
                db.Add(new Person(0, "UserSeventeenWhenAlreadyFull"));
            }, "Array's capacity must be exactly 16 integers!");
        }

        [Test]
        public void RemovingPersonFromListOfPersonReducesCount()
        {
            Database db = new Database();
            db.Add(new Person(123, "Gosho"));
            db.Add(new Person(456, "Pesho"));

            db.Remove();

            int actualCount = db.Count;
            int expectedCount = 1;

            Assert.AreEqual(expectedCount, actualCount, "Removing a person should reduce count and work successfully");
        }


        [Test]
        public void RemovingPersonFromEmptyListOfPerson()
        {
            Database db = new Database();

            Assert.Throws<InvalidOperationException>(() =>
            {
                db.Remove();
            });
        }

        [TestCase("")]
        [TestCase(null)]
        public void TryingToFindPersonWithNullOrEmptyStringName(string name)
        {
            Person[] persons = new Person[]
            {
                new Person(123,"Petar"),
                new Person(3123, "Toni")
            };

            Database db = new Database(persons);

            Assert.Throws<ArgumentNullException>(() =>
            {
                db.FindByUsername(name);
            }, "Username parameter is null!");
        }


        [TestCase("Mariya")]
        [TestCase("Petra")]
        public void TryingToFindPersonWithInvalidName(string name)
        {
            Person[] persons = new Person[]
            {
                new Person(123,"Petar"),
                new Person(3123, "Toni")
            };

            Database db = new Database(persons);

            Assert.Throws<InvalidOperationException>(() =>
            {
                db.FindByUsername(name);
            }, "No user is present by this username!");
        }


        [TestCase("Petar")]
        [TestCase("Toni")]
        public void FinbdingPersonByTheCorrectNameReturnsTheRightPerson(string name)
        {
            long userID = 123;
            Person[] persons = new Person[]
            {
                new Person(userID, name),
                new Person(3123, "Ivan"),
                new Person(724, "George")
            };

            Database db = new Database(persons);

            string correctPersonFound = db.FindByUsername(name).UserName;
            string expectedPersonFound = name;

            Assert.AreEqual(expectedPersonFound, correctPersonFound, "Correct person should be returned!");
        }

        [TestCase(-5)]
        [TestCase(-143)]
        public void FindPersonWithNegativeIDShouldThrowAnException(long id)
        {
            Person[] persons = new Person[]
            {
                new Person(3123, "Ivan"),
                new Person(724, "George")
            };

            Database db = new Database(persons);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                db.FindById(id);
            }, "Id should be a positive number!");
        }

        [TestCase(666)]
        [TestCase(0)]
        [TestCase(111)]
        public void FindPersonWithIncorrectIDShouldThrowAnException(long id)
        {

            Person[] persons = new Person[]
            {
                new Person(3123, "Ivan"),
                new Person(724, "George")
            };

            Database db = new Database(persons);

            Assert.Throws<InvalidOperationException>(() =>
            {
                db.FindById(id);
            }, "No user is present by this ID!");
        }


        [TestCase(456)]
        [TestCase(123)]
        [TestCase(0)]
        public void FindingTheCorrectUserWhenCorrectIdIsProvided(long id)
        {
            Person[] persons = new Person[]
            {
                new Person(id, "Ivan"),
                new Person(724, "George")
            };

            Database db = new Database(persons);

            long idFoundFromTestedFunction = (long)db.FindById(id).Id;
            long expectedId = id;

            Assert.AreEqual(expectedId, idFoundFromTestedFunction, "User IDs should be matching!");
        }


    }
}