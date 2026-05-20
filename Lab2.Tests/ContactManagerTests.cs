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
            // Очищаем файл перед каждым тестом
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
            _contactManager = new ContactManager1();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Удаляем файл после каждого теста
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public void ContactManager_Constructor_CreatesEmptyList()
        {
            // Assert
            Assert.IsNotNull(_contactManager.Contacts);
            Assert.AreEqual(0, _contactManager.Contacts.Count);
        }

        [TestMethod]
        public void AddContact_ValidContact_IncreasesCount()
        {
            // Arrange
            var contact = new Contact("Иван", "1234567890");

            // Act
            _contactManager.AddContact(contact);

            // Assert
            Assert.AreEqual(1, _contactManager.Contacts.Count);
        }

        [TestMethod]
        public void AddContact_ValidContact_AddsCorrectContact()
        {
            // Arrange
            var contact = new Contact("Иван Петров", "+7 (999) 123-45-67");

            // Act
            _contactManager.AddContact(contact);

            // Assert
            Assert.AreEqual("Иван Петров", _contactManager.Contacts[0].Name);
            Assert.AreEqual("+7 (999) 123-45-67", _contactManager.Contacts[0].PhoneNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddContact_NullContact_ThrowsException()
        {
            // Act
            _contactManager.AddContact(null);
        }

        [TestMethod]
        public void RemoveContact_ExistingContact_DecreasesCount()
        {
            // Arrange
            var contact = new Contact("Иван", "123");
            _contactManager.AddContact(contact);
            Assert.AreEqual(1, _contactManager.Contacts.Count);

            // Act
            _contactManager.RemoveContact(contact);

            // Assert
            Assert.AreEqual(0, _contactManager.Contacts.Count);
        }

        [TestMethod]
        public void RemoveContact_ExistingContact_RemovesCorrectContact()
        {
            // Arrange
            var contact = new Contact("Иван", "123");
            _contactManager.AddContact(contact);

            // Act
            _contactManager.RemoveContact(contact);

            // Assert
            Assert.IsFalse(_contactManager.Contacts.Contains(contact));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveContact_NullContact_ThrowsException()
        {
            // Act
            _contactManager.RemoveContact(null);
        }

        [TestMethod]
        public void RemoveContact_NonExistentContact_DoesNothing()
        {
            // Arrange
            var contact = new Contact("Иван", "123");
            var otherContact = new Contact("Петр", "456");
            _contactManager.AddContact(contact);
            int countBefore = _contactManager.Contacts.Count;

            // Act
            _contactManager.RemoveContact(otherContact);

            // Assert
            Assert.AreEqual(countBefore, _contactManager.Contacts.Count);
        }

        [TestMethod]
        public void SearchContacts_EmptyQuery_ReturnsAllContacts()
        {
            // Arrange
            _contactManager.AddContact(new Contact("Иван", "111"));
            _contactManager.AddContact(new Contact("Петр", "222"));

            // Act
            var results = _contactManager.SearchContacts("");

            // Assert
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void SearchContacts_ByName_ReturnsMatchingContacts()
        {
            // Arrange
            _contactManager.AddContact(new Contact("Иван Петров", "111"));
            _contactManager.AddContact(new Contact("Петр Иванов", "222"));
            _contactManager.AddContact(new Contact("Сергей Сидоров", "333"));

            // Act
            var results = _contactManager.SearchContacts("Иван");

            // Assert
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void SearchContacts_ByName_FullMatch_ReturnsContact()
        {
            // Arrange
            _contactManager.AddContact(new Contact("Иван Петров", "111"));

            // Act
            var results = _contactManager.SearchContacts("Иван Петров");

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Иван Петров", results[0].Name);
        }

        [TestMethod]
        public void SearchContacts_ByPhone_ReturnsMatchingContacts()
        {
            // Arrange
            _contactManager.AddContact(new Contact("Иван", "111-222-333"));
            _contactManager.AddContact(new Contact("Петр", "444-555-666"));

            // Act
            var results = _contactManager.SearchContacts("222");

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("111-222-333", results[0].PhoneNumber);
        }

        [TestMethod]
        public void SearchContacts_ByPhone_FullMatch_ReturnsContact()
        {
            // Arrange
            _contactManager.AddContact(new Contact("Иван", "111-222-333"));

            // Act
            var results = _contactManager.SearchContacts("111-222-333");

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("111-222-333", results[0].PhoneNumber);
        }

        [TestMethod]
        public void SearchContacts_NotFound_ReturnsEmptyList()
        {
            // Arrange
            _contactManager.AddContact(new Contact("Иван", "111"));
            _contactManager.AddContact(new Contact("Петр", "222"));

            // Act
            var results = _contactManager.SearchContacts("Сергей");

            // Assert
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void SearchContacts_CaseInsensitive_ReturnsMatches()
        {
            // Arrange
            _contactManager.AddContact(new Contact("ИВАН ПЕТРОВ", "111"));
            _contactManager.AddContact(new Contact("иван петров", "222"));

            // Act
            var results = _contactManager.SearchContacts("иван");

            // Assert
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void AddMultipleContacts_AddsAllContacts()
        {
            // Arrange & Act
            _contactManager.AddContact(new Contact("Иван", "111"));
            _contactManager.AddContact(new Contact("Петр", "222"));
            _contactManager.AddContact(new Contact("Сергей", "333"));

            // Assert
            Assert.AreEqual(3, _contactManager.Contacts.Count);
        }

        [TestMethod]
        public void SaveContacts_ToFile_CreatesFile()
        {
            // Arrange
            _contactManager.AddContact(new Contact("Иван", "111"));
            _contactManager.AddContact(new Contact("Петр", "222"));

            // Assert
            Assert.IsTrue(File.Exists(_testFilePath));
        }

        [TestMethod]
        public void LoadContacts_FromFile_RestoresContacts()
        {
            // Arrange
            _contactManager.AddContact(new Contact("Иван", "111"));
            _contactManager.AddContact(new Contact("Петр", "222"));

            // Создаём новый менеджер, который загрузит данные из файла
            var newManager = new ContactManager1();

            // Assert
            Assert.AreEqual(2, newManager.Contacts.Count);
            Assert.AreEqual("Иван", newManager.Contacts[0].Name);
            Assert.AreEqual("Петр", newManager.Contacts[1].Name);
        }
    }
}