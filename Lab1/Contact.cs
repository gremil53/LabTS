using System;

namespace ContactManager
{
    public class Contact
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public Contact(string name, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя не может быть пустым!");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Телефон не может быть пустым!");

            Name = name;
            PhoneNumber = phoneNumber;
        }
    }
}   