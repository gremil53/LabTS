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
           
            string name = "Иван Петров";
            string phone = "+7 (999) 123-45-67";

            var contact = new Contact(name, phone);

            Assert.AreEqual(name, contact.Name);
            Assert.AreEqual(phone, contact.PhoneNumber);
        }

        [TestMethod]
        public void Contact_Name_CanBeChanged()
        {
            var contact = new Contact("Иван", "123");

            contact.Name = "Петр";

            Assert.AreEqual("Петр", contact.Name);
        }

        [TestMethod]
        public void Contact_Phone_CanBeChanged()
        {
            var contact = new Contact("Иван", "123");

            contact.PhoneNumber = "456";

            Assert.AreEqual("456", contact.PhoneNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_WhenNameIsNull_ThrowsException()
        {
            new Contact(null, "1234567890");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_WhenPhoneIsNull_ThrowsException()
        {
            new Contact("Иван Петров", null);
        }
    }
}