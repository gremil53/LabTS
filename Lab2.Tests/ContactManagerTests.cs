using ContactManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Lab2.Tests
{
    [TestClass]
    public class ContactManagerTests
    {
        private ContactManager1 _contactManager;
        private string _testFilePath = "contacts.txt";

        [TestInitialize]
        public void SetUp()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
            _contactManager = new ContactManager1();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

      

        [TestMethod]
        public void SearchContacts_EmptyQuery_ReturnsAllContacts()
        {
            _contactManager.AddContact(new Contact("Иван", "111"));
            _contactManager.AddContact(new Contact("Петр", "222"));

            var results = _contactManager.SearchContacts("");

            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void SearchContacts_ByName_ReturnsMatchingContacts()
        {
            _contactManager.AddContact(new Contact("Иван Петров", "111"));
            _contactManager.AddContact(new Contact("Петр Иванов", "222"));
            _contactManager.AddContact(new Contact("Сергей Сидоров", "333"));

            var results = _contactManager.SearchContacts("Иван");

            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void SearchContacts_ByName_FullMatch_ReturnsContact()
        {
            _contactManager.AddContact(new Contact("Иван Петров", "111"));

            var results = _contactManager.SearchContacts("Иван Петров");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Иван Петров", results[0].Name);
        }

        [TestMethod]
        public void SearchContacts_ByPhone_ReturnsMatchingContacts()
        {
            _contactManager.AddContact(new Contact("Иван", "111-222-333"));
            _contactManager.AddContact(new Contact("Петр", "444-555-666"));

            var results = _contactManager.SearchContacts("222");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("111-222-333", results[0].PhoneNumber);
        }

        [TestMethod]
        public void SearchContacts_ByPhone_FullMatch_ReturnsContact()
        {
            _contactManager.AddContact(new Contact("Иван", "111-222-333"));

            var results = _contactManager.SearchContacts("111-222-333");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("111-222-333", results[0].PhoneNumber);
        }

        [TestMethod]
        public void SearchContacts_NotFound_ReturnsEmptyList()
        {
            _contactManager.AddContact(new Contact("Иван", "111"));
            _contactManager.AddContact(new Contact("Петр", "222"));

            var results = _contactManager.SearchContacts("Сергей");

            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void SearchContacts_CaseInsensitive_ReturnsMatches()
        {
            _contactManager.AddContact(new Contact("ИВАН ПЕТРОВ", "111"));
            _contactManager.AddContact(new Contact("иван петров", "222"));

            var results = _contactManager.SearchContacts("иван");

            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void AddMultipleContacts_AddsAllContacts()
        {
            _contactManager.AddContact(new Contact("Иван", "111"));
            _contactManager.AddContact(new Contact("Петр", "222"));
            _contactManager.AddContact(new Contact("Сергей", "333"));

            Assert.AreEqual(3, _contactManager.Contacts.Count);
        }

        [TestMethod]
        public void SaveAndLoadContacts_RoundTrip_PreservesData()
        {
            // Arrange
            var originalManager = new ContactManager1();
            originalManager.AddContact(new Contact("Иван", "111"));
            originalManager.AddContact(new Contact("Петр", "222"));

            // Act
            originalManager.SaveToFile(_testFilePath);

            var loadedManager = new ContactManager1();
            loadedManager.LoadFromFile(_testFilePath);

            // Assert
            Assert.AreEqual(originalManager.Contacts.Count, loadedManager.Contacts.Count);
                    
        }


    }


}