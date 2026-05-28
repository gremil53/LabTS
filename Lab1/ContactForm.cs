using System;
using System.Windows.Forms;
using System.Linq;

namespace ContactManager
{
    public class ContactForm : Form
    {
        private ContactManager1 _contactManager;
        private TextBox nameTextBox;
        private TextBox phoneNumberTextBox;
        private Button addContactButton;
        private Button removeContactButton;
        private TextBox searchTextBox;
        private Button searchButton;
        private ListBox contactsListBox;

        public ContactForm()
        {
            InitializeForm();
            _contactManager = new ContactManager1();
            UpdateContactsList();
        }

        private void InitializeForm()
        {
            this.Text = "Управление контактами";
            this.Width = 500;
            this.Height = 400;

            //Label для поле имя
            Label nameLabel = new Label
            {
                Text = "Имя:",
                Location = new System.Drawing.Point(10, 12),
                Size = new System.Drawing.Size(40, 20)
            };

            nameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(55, 10),
                Width = 200
            };

            //Label для поле телефон
            Label phoneLabel = new Label
            {
                Text = "Телефон:",
                Location = new System.Drawing.Point(10, 42),
                Size = new System.Drawing.Size(60, 20)
            };

            phoneNumberTextBox = new TextBox
            {
                Location = new System.Drawing.Point(75, 40),
                Width = 200
            };

            addContactButton = new Button
            {
                Location = new System.Drawing.Point(10, 70),
                Text = "Добавить",
                Width = 100
            };
            addContactButton.Click += AddContactButton_Click;

            removeContactButton = new Button
            {
                Location = new System.Drawing.Point(120, 70),
                Text = "Удалить",
                Width = 100
            };
            removeContactButton.Click += RemoveContactButton_Click;

            // Label для поле поиск
            Label searchLabel = new Label
            {
                Text = "Поиск:",
                Location = new System.Drawing.Point(10, 112),
                Size = new System.Drawing.Size(50, 20)
            };

            searchTextBox = new TextBox
            {
                Location = new System.Drawing.Point(65, 110),
                Width = 200
            };

            searchButton = new Button
            {
                Location = new System.Drawing.Point(275, 108),
                Text = "Искать",
                Width = 80
            };
            searchButton.Click += SearchButton_Click;

            contactsListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 145),
                Width = 460,
                Height = 220
            };

            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(phoneLabel);
            this.Controls.Add(phoneNumberTextBox);
            this.Controls.Add(addContactButton);
            this.Controls.Add(removeContactButton);
            this.Controls.Add(searchLabel);
            this.Controls.Add(searchTextBox);
            this.Controls.Add(searchButton);
            this.Controls.Add(contactsListBox);
        }


        private void UpdateContactsList()
        {
            contactsListBox.Items.Clear();
            foreach (var contact in _contactManager.Contacts)
            {
                contactsListBox.Items.Add($"{contact.Name} - {contact.PhoneNumber}");
            }
        }

        private void ClearInputs()
        {
            nameTextBox.Clear();
            phoneNumberTextBox.Clear();
        }

        private void AddContactButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(phoneNumberTextBox.Text))
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newContact = new Contact(nameTextBox.Text.Trim(), phoneNumberTextBox.Text.Trim());
                _contactManager.AddContact(newContact);
                UpdateContactsList();
                ClearInputs();

                MessageBox.Show("Контакт добавлен!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveContactButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (contactsListBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите контакт для удаления!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedItem = contactsListBox.SelectedItem.ToString();
                string[] parts = selectedItem.Split(new[] { " - " }, StringSplitOptions.None);

                if (parts.Length >= 2)
                {
                    string name = parts[0].Trim();
                    string phoneNumber = parts[1].Trim();

                    var contactToRemove = _contactManager.Contacts.FirstOrDefault(c =>
                        c.Name == name && c.PhoneNumber == phoneNumber);

                    if (contactToRemove != null)
                    {
                        _contactManager.RemoveContact(contactToRemove);
                        UpdateContactsList();
                        MessageBox.Show("Контакт удален!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                string query = searchTextBox.Text;

                if (string.IsNullOrWhiteSpace(query))
                {
                    UpdateContactsList();
                    return;
                }

                var searchResults = _contactManager.SearchContacts(query);
                contactsListBox.Items.Clear();

                foreach (var contact in searchResults)
                {
                    contactsListBox.Items.Add($"{contact.Name} - {contact.PhoneNumber}");
                }

                if (searchResults.Count == 0)
                {
                    MessageBox.Show("Контакты не найдены!", "Поиск",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.ClientSize = new System.Drawing.Size(347, 294);
            this.Name = "ContactForm";
            this.ResumeLayout(false);

        }
    }
}
