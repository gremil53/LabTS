using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ContactManager;

namespace Lab2.Tests
{
    [TestClass]
    public class ContactTests
    {
        [TestMethod]
        public void Contact_Constructor_ValidData_CreatesContact()
        {
            // Arrange
            string name = "Иван Петров";
            string phone = "+7 (999) 123-45-67";

            // Act
            var contact = new Contact(name, phone);

            // Assert
            Assert.AreEqual(name, contact.Name);
            Assert.AreEqual(phone, contact.PhoneNumber);
        }

        [TestMethod]
        public void Contact_Name_CanBeChanged()
        {
            // Arrange
            var contact = new Contact("Иван", "123");

            // Act
            contact.Name = "Петр";

            // Assert
            Assert.AreEqual("Петр", contact.Name);
        }

        [TestMethod]
        public void Contact_Phone_CanBeChanged()
        {
            // Arrange
            var contact = new Contact("Иван", "123");

            // Act
            contact.PhoneNumber = "456";

            // Assert
            Assert.AreEqual("456", contact.PhoneNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_WhenNameIsNull_ThrowsException()
        {
            // Act
            new Contact(null, "1234567890");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_WhenPhoneIsNull_ThrowsException()
        {
            // Act
            new Contact("Иван Петров", null);
        }
    }
}