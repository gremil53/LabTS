using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContactManager
{
    public class ContactManager1
    {
        private readonly string _contactsFilePath = "contacts.txt";
        private readonly string _groupsFilePath = "groups.txt";

        public List<Contact> Contacts { get; private set; }
        public List<ContactGroup> Groups { get; private set; }

        public ContactManager1()
        {
            Contacts = new List<Contact>();
            Groups = new List<ContactGroup>();
            LoadGroups();
            LoadContacts();

            if (!Groups.Any(g => g.Id == 0))
            {
                Groups.Add(new ContactGroup(0, "Без группы"));
                SaveGroups();
            }
        }

        // ===== МЕТОДЫ ДЛЯ ГРУПП =====

        public void AddGroup(string groupName)
        {
            // ПРОВЕРКА НА ПУСТОЕ ИМЯ (ТЕСТ-КЕЙС 2)
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Название группы не может быть пустым!");

            // ПРОВЕРКА НА ДУБЛИКАТ (ТЕСТ-КЕЙС 3)
            if (Groups.Any(g => g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Группа с таким именем уже существует!");

            int newId = Groups.Count > 0 ? Groups.Max(g => g.Id) + 1 : 1;
            Groups.Add(new ContactGroup(newId, groupName));
            SaveGroups();
        }

        public void RemoveGroup(int groupId)
        {
            if (groupId == 0) return;

            var group = Groups.FirstOrDefault(g => g.Id == groupId);
            if (group != null)
            {
                Groups.Remove(group);
                foreach (var contact in Contacts.Where(c => c.GroupId == groupId))
                {
                    contact.GroupId = 0;
                }
                SaveGroups();
                SaveContacts();
            }
        }

        public List<ContactGroup> GetAllGroups()
        {
            return Groups.ToList();
        }

        public void AssignContactToGroup(Contact contact, int groupId)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            if (!Groups.Any(g => g.Id == groupId))
                throw new ArgumentException("Группа не существует!");

            contact.GroupId = groupId;
            SaveContacts();
        }

        public List<Contact> GetContactsByGroup(int groupId)
        {
            return Contacts.Where(c => c.GroupId == groupId).ToList();
        }

        // ===== МЕТОДЫ ДЛЯ КОНТАКТОВ =====

        public void AddContact(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            if (string.IsNullOrWhiteSpace(contact.Name))
                throw new ArgumentException("Имя не может быть пустым!");

            if (string.IsNullOrWhiteSpace(contact.PhoneNumber))
                throw new ArgumentException("Телефон не может быть пустым!");

            if (!Groups.Any(g => g.Id == contact.GroupId))
                contact.GroupId = 0;

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

        // ===== СОХРАНЕНИЕ И ЗАГРУЗКА =====

        private void SaveContacts()
        {
            var lines = Contacts.Select(c => $"{c.Name}|{c.PhoneNumber}|{c.GroupId}");
            File.WriteAllLines(_contactsFilePath, lines);
        }

        private void LoadContacts()
        {
            if (!File.Exists(_contactsFilePath)) return;

            var lines = File.ReadAllLines(_contactsFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length >= 2)
                {
                    int groupId = parts.Length >= 3 ? int.Parse(parts[2]) : 0;
                    Contacts.Add(new Contact(parts[0], parts[1], groupId));
                }
            }
        }

        private void SaveGroups()
        {
            var lines = Groups.Select(g => $"{g.Id}|{g.Name}");
            File.WriteAllLines(_groupsFilePath, lines);
        }

        private void LoadGroups()
        {
            if (!File.Exists(_groupsFilePath)) return;

            var lines = File.ReadAllLines(_groupsFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    Groups.Add(new ContactGroup(int.Parse(parts[0]), parts[1]));
                }
            }
        }
    }
}