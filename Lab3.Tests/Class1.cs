using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContactManager;
using System;
using System.IO;
using System.Linq;

namespace Lab2.Tests
{
    [TestClass]
    public class ContactGroupTests
    {
        private ContactManager1 _contactManager;
        private string _contactsFile = "contacts.txt";
        private string _groupsFile = "groups.txt";

        [TestInitialize]
        public void SetUp()
        {
            if (File.Exists(_contactsFile)) File.Delete(_contactsFile);
            if (File.Exists(_groupsFile)) File.Delete(_groupsFile);
            _contactManager = new ContactManager1();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_contactsFile)) File.Delete(_contactsFile);
            if (File.Exists(_groupsFile)) File.Delete(_groupsFile);
        }

        // ===== ТЕСТ 1: Проверка обновления названия группы =====
        [TestMethod]
        public void EditGroupName_ShouldUpdateName()
        {
            // Arrange
            _contactManager.AddGroup("Семья");
            var group = _contactManager.GetAllGroups().First(g => g.Name == "Семья");

            // Act
            group.Name = "Друзья";

            // Assert
            Assert.AreEqual("Друзья", group.Name);
        }

        // ===== ТЕСТ 2: Проверка назначения контакта группе =====
        [TestMethod]
        public void AssignContactToGroup_ShouldUpdateContactGroupId()
        {
            // Arrange
            _contactManager.AddGroup("Работа");
            var group = _contactManager.GetAllGroups().First(g => g.Name == "Работа");
            var contact = new Contact("Иван", "1234567890");

            // Act
            _contactManager.AddContact(contact);
            _contactManager.AssignContactToGroup(contact, group.Id);

            // Assert
            Assert.AreEqual(group.Id, contact.GroupId);
        }

        // ===== ТЕСТ 3: Проверка получения контактов по группе =====
        [TestMethod]
        public void GetContactsByGroup_ShouldReturnCorrectContacts()
        {
            // Arrange
            _contactManager.AddGroup("Семья");
            var group = _contactManager.GetAllGroups().First(g => g.Name == "Семья");
            _contactManager.AddContact(new Contact("Мама", "111", group.Id));
            _contactManager.AddContact(new Contact("Папа", "222", group.Id));
            _contactManager.AddContact(new Contact("Друг", "333"));

            // Act
            var familyContacts = _contactManager.GetContactsByGroup(group.Id);

            // Assert
            Assert.AreEqual(2, familyContacts.Count);
        }

        // ===== ТЕСТ 4: Проверка удаления группы =====
        [TestMethod]
        public void RemoveGroup_ShouldRemoveGroupAndResetContacts()
        {
            // Arrange
            _contactManager.AddGroup("Коллеги");
            var group = _contactManager.GetAllGroups().First(g => g.Name == "Коллеги");
            _contactManager.AddContact(new Contact("Анна", "111", group.Id));

            // Act
            _contactManager.RemoveGroup(group.Id);

            // Assert
            Assert.IsFalse(_contactManager.GetAllGroups().Any(g => g.Name == "Коллеги"));
            Assert.AreEqual(0, _contactManager.GetContactsByGroup(group.Id).Count);
        }

        // ===== ТЕСТ 5: Проверка создания группы с пустым именем =====
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddGroup_EmptyName_ShouldThrowException()
        {
            // Act
            _contactManager.AddGroup("");
        }

        // ===== ТЕСТ 6: Проверка фильтрации контактов по группе =====
        [TestMethod]
        public void FilterContactsByGroup_ShouldReturnOnlyContactsInGroup()
        {
            // Arrange
            _contactManager.AddGroup("Семья");
            var familyGroup = _contactManager.GetAllGroups().First(g => g.Name == "Семья");
            _contactManager.AddContact(new Contact("Мама", "111", familyGroup.Id));
            _contactManager.AddContact(new Contact("Папа", "222", familyGroup.Id));
            _contactManager.AddContact(new Contact("Друг", "333"));

            // Act
            var filteredContacts = _contactManager.GetContactsByGroup(familyGroup.Id);

            // Assert
            Assert.AreEqual(2, filteredContacts.Count);
            Assert.IsTrue(filteredContacts.All(c => c.GroupId == familyGroup.Id));
        }

        // ===== ТЕСТ 7: Проверка формата отображения контакта =====
        [TestMethod]
        public void ContactToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            _contactManager.AddGroup("Семья");
            var group = _contactManager.GetAllGroups().First(g => g.Name == "Семья");
            var contact = new Contact("Иван Петров", "+7 999 123-45-67", group.Id);
            _contactManager.AddContact(contact);

            // Act
            var groupName = _contactManager.Groups.First(g => g.Id == contact.GroupId).Name;
            var displayString = $"{contact.Name} - {contact.PhoneNumber} [{groupName}]";

            // Assert
            Assert.AreEqual("Иван Петров - +7 999 123-45-67 [Семья]", displayString);
        }

        // ===== ТЕСТ 8: Проверка сохранения и загрузки групп =====
        [TestMethod]
        public void SaveAndLoadGroups_ShouldPersistGroups()
        {
            // Arrange
            _contactManager.AddGroup("Друзья");
            _contactManager.AddGroup("Спорт");

            // Создаём новый менеджер (должен загрузить данные из файлов)
            var newManager = new ContactManager1();

            // Assert
            Assert.IsTrue(newManager.GetAllGroups().Any(g => g.Name == "Друзья"));
            Assert.IsTrue(newManager.GetAllGroups().Any(g => g.Name == "Спорт"));
        }

        // ===== ТЕСТ 9: Проверка что контакт не может быть без группы =====
        [TestMethod]
        public void Contact_WithoutGroup_ShouldHaveGroupIdZero()
        {
            // Arrange & Act
            var contact = new Contact("Тест", "000");

            // Assert
            Assert.AreEqual(0, contact.GroupId);
        }

        // ===== ТЕСТ 10: Проверка удаления группы с контактами =====
        [TestMethod]
        public void RemoveGroup_WithContacts_ShouldMoveContactsToDefaultGroup()
        {
            // Arrange
            _contactManager.AddGroup("СтараяГруппа");
            var oldGroup = _contactManager.GetAllGroups().First(g => g.Name == "СтараяГруппа");
            _contactManager.AddContact(new Contact("Иван", "111", oldGroup.Id));
            _contactManager.AddContact(new Contact("Петр", "222", oldGroup.Id));

            // Act
            _contactManager.RemoveGroup(oldGroup.Id);

            // Assert
            var defaultGroupContacts = _contactManager.GetContactsByGroup(0);
            Assert.AreEqual(2, defaultGroupContacts.Count);
        }
    }
}