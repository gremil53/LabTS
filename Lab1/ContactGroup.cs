using System;

namespace ContactManager
{
    public class ContactGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ContactGroup(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название группы не может быть пустым!");

            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}