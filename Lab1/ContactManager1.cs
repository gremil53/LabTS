using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContactManager
{
    public class ContactManager1
    {
        private readonly string _filePath = "contacts.txt";

        public List<Contact> Contacts { get; private set; }

        public ContactManager1()
        {
            Contacts = new List<Contact>();
            LoadContacts();
        }

        public void AddContact(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            if (string.IsNullOrWhiteSpace(contact.Name))
                throw new ArgumentException("Имя не может быть пустым!");

            if (string.IsNullOrWhiteSpace(contact.PhoneNumber))
                throw new ArgumentException("Телефон не может быть пустым!");

            Contacts.Add(contact);
            SaveContacts();
        }

        public void RemoveContact(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            var contactToRemove = Contacts.FirstOrDefault(c =>
                c.Name == contact.Name && c.PhoneNumber == contact.PhoneNumber);

            if (contactToRemove != null)
            {
                Contacts.Remove(contactToRemove);
                SaveContacts();
            }
        }

        public List<Contact> SearchContacts(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Contacts.ToList();

            return Contacts.Where(c =>
                c.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                c.PhoneNumber.Contains(query)).ToList();
        }

        public void SaveToFile(string filePath)
        {
            var lines = Contacts.Select(c => $"{c.Name}|{c.PhoneNumber}");
            File.WriteAllLines(filePath, lines);
        }

        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            Contacts.Clear();
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    Contacts.Add(new Contact(parts[0], parts[1]));
                }
            }
        }

        private void SaveContacts()
        {
            var lines = Contacts.Select(c => $"{c.Name}|{c.PhoneNumber}");
            File.WriteAllLines(_filePath, lines);
        }

        private void LoadContacts()
        {
            if (!File.Exists(_filePath)) return;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    Contacts.Add(new Contact(parts[0], parts[1]));
                }
            }
        }
    }
}