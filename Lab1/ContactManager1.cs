using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContactManager
{
    public class ContactManager
    {
        public List<Contact> Contacts { get; private set; }

        public ContactManager()
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

            if (contact.Name.Length < 2)
                throw new ArgumentException("Имя должно содержать минимум 2 символа!");

            Contacts.Add(contact);
            SaveContacts();
        }

        public void RemoveContact(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            Contacts.Remove(contact);
            SaveContacts();
        }

        public List<Contact> SearchContacts(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Contacts;

            // ИСПРАВЛЕНО: убрал StringComparison.OrdinalIgnoreCase
            return Contacts.Where(c =>
                c.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                c.PhoneNumber.Contains(query)).ToList();
        }

        private void SaveContacts()
        {
            var lines = Contacts.Select(c => $"{c.Name}|{c.PhoneNumber}");
            File.WriteAllLines("contacts.txt", lines);
        }

        private void LoadContacts()
        {
            if (!File.Exists("contacts.txt")) return;

            var lines = File.ReadAllLines("contacts.txt");
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