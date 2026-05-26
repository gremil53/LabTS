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
        private TextBox searchTextBox;
        private Button addContactButton;
        private Button removeContactButton;
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
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ========== Поле "Имя" с подсказкой ==========
            nameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Width = 200,
                Text = "Имя",
                ForeColor = System.Drawing.Color.Gray
            };

            nameTextBox.Enter += (sender, e) =>
            {
                if (nameTextBox.Text == "Имя")
                {
                    nameTextBox.Text = "";
                    nameTextBox.ForeColor = System.Drawing.Color.Black;
                }
            };

            nameTextBox.Leave += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(nameTextBox.Text))
                {
                    nameTextBox.Text = "Имя";
                    nameTextBox.ForeColor = System.Drawing.Color.Gray;
                }
            };

            // ========== Поле "Телефон" с подсказкой ==========
            phoneNumberTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 40),
                Width = 200,
                Text = "Телефон",
                ForeColor = System.Drawing.Color.Gray
            };

            phoneNumberTextBox.Enter += (sender, e) =>
            {
                if (phoneNumberTextBox.Text == "Телефон")
                {
                    phoneNumberTextBox.Text = "";
                    phoneNumberTextBox.ForeColor = System.Drawing.Color.Black;
                }
            };

            phoneNumberTextBox.Leave += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(phoneNumberTextBox.Text))
                {
                    phoneNumberTextBox.Text = "Телефон";
                    phoneNumberTextBox.ForeColor = System.Drawing.Color.Gray;
                }
            };

            // ========== Кнопки ==========
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

            // ========== Поле "Поиск" с подсказкой ==========
            searchTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 110),
                Width = 200,
                Text = "Поиск",
                ForeColor = System.Drawing.Color.Gray
            };

            searchTextBox.Enter += (sender, e) =>
            {
                if (searchTextBox.Text == "Поиск")
                {
                    searchTextBox.Text = "";
                    searchTextBox.ForeColor = System.Drawing.Color.Black;
                }
            };

            searchTextBox.Leave += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchTextBox.Text))
                {
                    searchTextBox.Text = "Поиск";
                    searchTextBox.ForeColor = System.Drawing.Color.Gray;
                }
            };

            // ========== Кнопка "Искать" ==========
            searchButton = new Button
            {
                Location = new System.Drawing.Point(220, 108),
                Text = "Искать",
                Width = 80
            };
            searchButton.Click += SearchButton_Click;

            // ========== Список контактов ==========
            contactsListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 145),
                Width = 460,
                Height = 220
            };

            // Добавляем все элементы на форму
            this.Controls.Add(nameTextBox);
            this.Controls.Add(phoneNumberTextBox);
            this.Controls.Add(addContactButton);
            this.Controls.Add(removeContactButton);
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
            // Очищаем поля и возвращаем подсказки
            nameTextBox.Text = "Имя";
            nameTextBox.ForeColor = System.Drawing.Color.Gray;
            phoneNumberTextBox.Text = "Телефон";
            phoneNumberTextBox.ForeColor = System.Drawing.Color.Gray;
        }

        private void AddContactButton_Click(object sender, EventArgs e)
        {
            try
            {
                string name = nameTextBox.Text;
                string phone = phoneNumberTextBox.Text;

                // Проверка на подсказки
                if (name == "Имя") name = "";
                if (phone == "Телефон") phone = "";

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newContact = new Contact(name.Trim(), phone.Trim());
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

                if (query == "Поиск") query = "";

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
    }
}