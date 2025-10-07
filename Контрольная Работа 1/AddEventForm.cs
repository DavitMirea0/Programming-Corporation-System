using CalendarApp.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalendarApp
{
    public partial class AddEventForm : Form
    {
        public string EventText { get; private set; }
        public string EventDescription { get; private set; }
        public DateTime EventDate { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool IsAllDay { get; private set; }
        public EventCategory EventCategory { get; private set; }
        public string EventColor { get; private set; }
        public ReminderType Reminder { get; private set; }

        private TextBox _titleTextBox;
        private TextBox _descriptionTextBox;
        private DateTimePicker _datePicker;
        private DateTimePicker _startTimePicker;
        private DateTimePicker _endTimePicker;
        private CheckBox _allDayCheckBox;
        private ComboBox _categoryComboBox;
        private ComboBox _reminderComboBox;
        private Button _colorButton;
        private Button _saveButton;
        private Button _cancelButton;

        public AddEventForm(DateTime selectedDate)
        {
            InitializeComponent(selectedDate);
        }

        private void InitializeComponent(DateTime selectedDate)
        {
            Text = "➕ Добавить событие";
            Size = new Size(500, 550);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Padding = new Padding(20);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                AutoScroll = true
            };

            // Заголовок
            var headerLabel = new Label
            {
                Text = "Новое событие",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 130, 180),
                Size = new Size(400, 40),
                Location = new Point(0, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Название события
            var titleLabel = new Label
            {
                Text = "Название события:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(200, 20),
                Location = new Point(0, 60)
            };

            _titleTextBox = new TextBox
            {
                Location = new Point(0, 85),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 9)
            };

            // Описание
            var descLabel = new Label
            {
                Text = "Описание:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(200, 20),
                Location = new Point(0, 120)
            };

            _descriptionTextBox = new TextBox
            {
                Location = new Point(0, 145),
                Size = new Size(400, 60),
                Font = new Font("Segoe UI", 9),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Дата
            var dateLabel = new Label
            {
                Text = "Дата:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(200, 20),
                Location = new Point(0, 220)
            };

            _datePicker = new DateTimePicker
            {
                Location = new Point(0, 245),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Short,
                Value = selectedDate
            };

            // Целодневное событие
            _allDayCheckBox = new CheckBox
            {
                Text = "Целодневное событие",
                Font = new Font("Segoe UI", 9),
                Size = new Size(200, 20),
                Location = new Point(220, 245),
                Checked = false
            };
            _allDayCheckBox.CheckedChanged += AllDayCheckBox_CheckedChanged;

            // Время начала
            var startTimeLabel = new Label
            {
                Text = "Время начала:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(200, 20),
                Location = new Point(0, 280)
            };

            _startTimePicker = new DateTimePicker
            {
                Location = new Point(0, 305),
                Size = new Size(150, 25),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                Value = DateTime.Today.AddHours(10) // 10:00 по умолчанию
            };

            // Время окончания
            var endTimeLabel = new Label
            {
                Text = "Время окончания:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(200, 20),
                Location = new Point(0, 340)
            };

            _endTimePicker = new DateTimePicker
            {
                Location = new Point(0, 365),
                Size = new Size(150, 25),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                Value = DateTime.Today.AddHours(11) // 11:00 по умолчанию
            };

            // Категория
            var categoryLabel = new Label
            {
                Text = "Категория:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(200, 20),
                Location = new Point(0, 400)
            };

            _categoryComboBox = new ComboBox
            {
                Location = new Point(0, 425),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9)
            };

            // Заполняем категории
            foreach (EventCategory category in Enum.GetValues(typeof(EventCategory)))
            {
                _categoryComboBox.Items.Add(new CategoryItem(category));
            }
            _categoryComboBox.SelectedIndex = 0;

            // Напоминание
            var reminderLabel = new Label
            {
                Text = "Напоминание:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(200, 20),
                Location = new Point(220, 400)
            };

            _reminderComboBox = new ComboBox
            {
                Location = new Point(220, 425),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9)
            };

            // Заполняем напоминания
            _reminderComboBox.Items.Add("Без напоминания");
            _reminderComboBox.Items.Add("За 5 минут");
            _reminderComboBox.Items.Add("За 15 минут");
            _reminderComboBox.Items.Add("За 30 минут");
            _reminderComboBox.Items.Add("За 1 час");
            _reminderComboBox.Items.Add("За 1 день");
            _reminderComboBox.SelectedIndex = 0;

            // Кнопки
            _saveButton = new Button
            {
                Text = "💾 Сохранить",
                Location = new Point(100, 470),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(34, 139, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            _saveButton.Click += SaveButton_Click;

            _cancelButton = new Button
            {
                Text = "❌ Отмена",
                Location = new Point(240, 470),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(220, 20, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            _cancelButton.Click += (s, e) => DialogResult = DialogResult.Cancel;

            mainPanel.Controls.AddRange(new Control[]
            {
                headerLabel,
                titleLabel,
                _titleTextBox,
                descLabel,
                _descriptionTextBox,
                dateLabel,
                _datePicker,
                _allDayCheckBox,
                startTimeLabel,
                _startTimePicker,
                endTimeLabel,
                _endTimePicker,
                categoryLabel,
                _categoryComboBox,
                reminderLabel,
                _reminderComboBox,
                _saveButton,
                _cancelButton
            });

            Controls.Add(mainPanel);

            // Обновляем состояние элементов при загрузке
            UpdateTimeControlsVisibility();
        }

        private void AllDayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTimeControlsVisibility();
        }

        private void UpdateTimeControlsVisibility()
        {
            bool enabled = !_allDayCheckBox.Checked;

            _startTimePicker.Enabled = enabled;
            _endTimePicker.Enabled = enabled;

            if (_allDayCheckBox.Checked)
            {
                _startTimePicker.Value = DateTime.Today;
                _endTimePicker.Value = DateTime.Today;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_titleTextBox.Text))
            {
                MessageBox.Show("Введите название события!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _titleTextBox.Focus();
                return;
            }

            if (_startTimePicker.Value >= _endTimePicker.Value && !_allDayCheckBox.Checked)
            {
                MessageBox.Show("Время окончания должно быть позже времени начала!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Сохраняем данные
            EventText = _titleTextBox.Text.Trim();
            EventDescription = _descriptionTextBox.Text.Trim();
            EventDate = _datePicker.Value.Date;
            StartTime = _allDayCheckBox.Checked ? EventDate : _startTimePicker.Value;
            EndTime = _allDayCheckBox.Checked ? EventDate : _endTimePicker.Value;
            IsAllDay = _allDayCheckBox.Checked;

            if (_categoryComboBox.SelectedItem is CategoryItem categoryItem)
            {
                EventCategory = categoryItem.Category;
            }
            else
            {
                EventCategory = EventCategory.Other;
            }

            // Устанавливаем цвет в зависимости от категории
            EventColor = GetCategoryColor(EventCategory);

            // Устанавливаем напоминание
            Reminder = _reminderComboBox.SelectedIndex switch
            {
                0 => ReminderType.None,
                1 => ReminderType.FiveMinutes,
                2 => ReminderType.FifteenMinutes,
                3 => ReminderType.ThirtyMinutes,
                4 => ReminderType.OneHour,
                5 => ReminderType.OneDay,
                _ => ReminderType.None
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private string GetCategoryColor(EventCategory category)
        {
            return category switch
            {
                EventCategory.Work => "#FF6B6B",      // Красный
                EventCategory.Personal => "#4ECDC4",   // Бирюзовый
                EventCategory.Holiday => "#FFD166",    // Желтый
                EventCategory.Birthday => "#FF9E6D",   // Оранжевый
                EventCategory.Meeting => "#06D6A0",    // Зеленый
                EventCategory.Appointment => "#118AB2", // Синий
                EventCategory.Other => "#9D4EDD",      // Фиолетовый
                _ => "#6C757D"                         // Серый
            };
        }

        // Вспомогательный класс для отображения категорий
        private class CategoryItem
        {
            public EventCategory Category { get; }
            public string DisplayName { get; }

            public CategoryItem(EventCategory category)
            {
                Category = category;
                DisplayName = GetCategoryDisplayName(category);
            }

            private string GetCategoryDisplayName(EventCategory category)
            {
                return category switch
                {
                    EventCategory.Work => "💼 Работа",
                    EventCategory.Personal => "👤 Личное",
                    EventCategory.Holiday => "🎉 Праздник",
                    EventCategory.Birthday => "🎂 День рождения",
                    EventCategory.Meeting => "🤝 Встреча",
                    EventCategory.Appointment => "📅 Встреча",
                    EventCategory.Other => "📌 Другое",
                    _ => category.ToString()
                };
            }

            public override string ToString() => DisplayName;
        }
    }
}