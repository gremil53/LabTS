using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

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

        // ===== ЭЛЕМЕНТЫ ДЛЯ ГРУПП =====
        private ComboBox groupComboBox;        // Выбор группы для нового контакта
        private ComboBox filterGroupComboBox;  // Фильтр по группам
        private Button addGroupButton;         // Кнопка "+" для создания группы
        private Button deleteGroupButton;      // Кнопка "Удалить группу"

        public ContactForm()
        {
            InitializeForm();
            _contactManager = new ContactManager1();
            LoadGroups();
            UpdateContactsList();
        }

        private void InitializeForm()
        {
            this.Text = "Управление контактами";
            this.Width = 650;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ===== Поле "Имя" =====
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

            // ===== Поле "Телефон" =====
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

            // ===== Кнопки =====
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

            // ===== ГРУППЫ =====
            Label groupLabel = new Label
            {
                Text = "Группа:",
                Location = new System.Drawing.Point(10, 102),
                Size = new System.Drawing.Size(50, 20)
            };

            groupComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(65, 100),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            addGroupButton = new Button
            {
                Location = new System.Drawing.Point(195, 98),
                Text = "+",
                Width = 30,
                Height = 23
            };
            addGroupButton.Click += AddGroupButton_Click;

            deleteGroupButton = new Button
            {
                Location = new System.Drawing.Point(230, 98),
                Text = "Удалить группу",
                Width = 100,
                Height = 23
            };
            deleteGroupButton.Click += DeleteGroupButton_Click;

            // ===== ФИЛЬТР ПО ГРУППАМ =====
            Label filterLabel = new Label
            {
                Text = "Фильтр:",
                Location = new System.Drawing.Point(340, 102),
                Size = new System.Drawing.Size(50, 20)
            };

            filterGroupComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(395, 100),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            filterGroupComboBox.SelectedIndexChanged += FilterGroupComboBox_SelectedIndexChanged;

            // ===== Поиск =====
            Label searchLabel = new Label
            {
                Text = "Поиск:",
                Location = new System.Drawing.Point(10, 142),
                Size = new System.Drawing.Size(50, 20)
            };

            searchTextBox = new TextBox
            {
                Location = new System.Drawing.Point(65, 140),
                Width = 200
            };

            searchButton = new Button
            {
                Location = new System.Drawing.Point(275, 138),
                Text = "Искать",
                Width = 80
            };
            searchButton.Click += SearchButton_Click;

            // ===== Список контактов =====
            contactsListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 175),
                Width = 610,
                Height = 270
            };

            // Добавляем всё на форму
            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(phoneLabel);
            this.Controls.Add(phoneNumberTextBox);
            this.Controls.Add(addContactButton);
            this.Controls.Add(removeContactButton);
            this.Controls.Add(groupLabel);
            this.Controls.Add(groupComboBox);
            this.Controls.Add(addGroupButton);
            this.Controls.Add(deleteGroupButton);
            this.Controls.Add(filterLabel);
            this.Controls.Add(filterGroupComboBox);
            this.Controls.Add(searchLabel);
            this.Controls.Add(searchTextBox);
            this.Controls.Add(searchButton);
            this.Controls.Add(contactsListBox);
        }

        // ===== ЗАГРУЗКА ГРУПП =====
        private void LoadGroups()
        {
            var groups = _contactManager.GetAllGroups();

            groupComboBox.DataSource = null;
            groupComboBox.DataSource = groups;
            groupComboBox.DisplayMember = "Name";
            groupComboBox.ValueMember = "Id";

            var allGroups = new List<ContactGroup>(groups);
            allGroups.Insert(0, new ContactGroup(-1, "Все группы"));

            filterGroupComboBox.DataSource = null;
            filterGroupComboBox.DataSource = allGroups;
            filterGroupComboBox.DisplayMember = "Name";
            filterGroupComboBox.ValueMember = "Id";
        }

        // ===== ДОБАВЛЕНИЕ ГРУППЫ =====
        private void AddGroupButton_Click(object sender, EventArgs e)
        {
            Form inputForm = new Form();
            inputForm.Text = "Новая группа";
            inputForm.Width = 400;
            inputForm.Height = 150;
            inputForm.StartPosition = FormStartPosition.CenterScreen;
            inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputForm.MaximizeBox = false;
            inputForm.MinimizeBox = false;

            Label lblPrompt = new Label();
            lblPrompt.Text = "Введите название группы:";
            lblPrompt.Location = new System.Drawing.Point(10, 10);
            lblPrompt.Size = new System.Drawing.Size(200, 20);

            TextBox txtGroupName = new TextBox();
            txtGroupName.Location = new System.Drawing.Point(10, 35);
            txtGroupName.Width = 360;

            Button btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Location = new System.Drawing.Point(200, 70);
            btnOk.Size = new System.Drawing.Size(80, 25);
            btnOk.DialogResult = DialogResult.OK;

            Button btnCancel = new Button();
            btnCancel.Text = "Отмена";
            btnCancel.Location = new System.Drawing.Point(290, 70);
            btnCancel.Size = new System.Drawing.Size(80, 25);
            btnCancel.DialogResult = DialogResult.Cancel;

            inputForm.Controls.Add(lblPrompt);
            inputForm.Controls.Add(txtGroupName);
            inputForm.Controls.Add(btnOk);
            inputForm.Controls.Add(btnCancel);

            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                string groupName = txtGroupName.Text;

                // ПРОВЕРКА НА ПУСТОЕ ИМЯ
                if (string.IsNullOrWhiteSpace(groupName))
                {
                    MessageBox.Show("Название группы не может быть пустым!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    _contactManager.AddGroup(groupName);
                    LoadGroups();
                    MessageBox.Show("Группа добавлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ===== УДАЛЕНИЕ ГРУППЫ =====
        private void DeleteGroupButton_Click(object sender, EventArgs e)
        {
            if (groupComboBox.SelectedItem is ContactGroup selectedGroup && selectedGroup.Id != 0)
            {
                if (MessageBox.Show($"Удалить группу \"{selectedGroup.Name}\"?", "Подтверждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _contactManager.RemoveGroup(selectedGroup.Id);
                    LoadGroups();
                    UpdateContactsList();
                    MessageBox.Show("Группа удалена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалить группу 'Без группы'!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ===== ФИЛЬТРАЦИЯ =====
        private void FilterGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateContactsList();
        }

        // ===== ОБНОВЛЕНИЕ СПИСКА КОНТАКТОВ =====
        private void UpdateContactsList()
        {
            contactsListBox.Items.Clear();

            int filterGroupId = -1;
            if (filterGroupComboBox.SelectedValue != null)
            {
                int.TryParse(filterGroupComboBox.SelectedValue.ToString(), out filterGroupId);
            }

            var contacts = filterGroupId == -1
                ? _contactManager.Contacts
                : _contactManager.GetContactsByGroup(filterGroupId);

            foreach (var contact in contacts)
            {
                var group = _contactManager.Groups.FirstOrDefault(g => g.Id == contact.GroupId);
                string groupName = group != null ? group.Name : "Без группы";
                contactsListBox.Items.Add($"{contact.Name} - {contact.PhoneNumber} [{groupName}]");
            }
        }

        private void ClearInputs()
        {
            nameTextBox.Clear();
            phoneNumberTextBox.Clear();
        }

        // ===== ДОБАВЛЕНИЕ КОНТАКТА =====
        private void AddContactButton_Click(object sender, EventArgs e)
        {
            try
            {
                string name = nameTextBox.Text;
                string phone = phoneNumberTextBox.Text;

                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Поле 'Имя' не может быть пустым!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(phone))
                {
                    MessageBox.Show("Поле 'Телефон' не может быть пустым!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedGroupId = groupComboBox.SelectedValue != null ? (int)groupComboBox.SelectedValue : 0;
                var newContact = new Contact(name.Trim(), phone.Trim(), selectedGroupId);
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

        // ===== УДАЛЕНИЕ КОНТАКТА =====
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
                    string phoneNumber = parts[1].Split('[')[0].Trim();

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

        // ===== ПОИСК =====
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
                    var group = _contactManager.Groups.FirstOrDefault(g => g.Id == contact.GroupId);
                    string groupName = group != null ? group.Name : "Без группы";
                    contactsListBox.Items.Add($"{contact.Name} - {contact.PhoneNumber} [{groupName}]");
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