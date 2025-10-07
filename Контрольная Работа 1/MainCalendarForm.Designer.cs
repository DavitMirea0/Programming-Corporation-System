using CalendarApp.Models;
using CalendarApp.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CalendarApp
{
    public partial class MainCalendarForm : Form
    {
        private readonly CalendarService _calendarService;
        private MonthCalendar _monthCalendar;
        private Button _addEventButton;
        private Button _weekViewButton;
        private ListBox _eventsListBox;
        private Label _dateLabel;
        private TextBox _searchTextBox;
        private Button _searchButton;
        private Button _statsButton;
        private Button _dataManagerButton;
        private Button _leapYearButton;

        public MainCalendarForm()
        {
            _calendarService = new CalendarService();

            var reminderService = new ReminderService(_calendarService);

            InitializeComponent();
            InitializeSearchPanel();
            InitializeStatsButton();
            InitializeLeapYearButton();
        }

        private void InitializeComponent()
        {
            Text = "Календарь";
            Size = new Size(900, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Padding = new Padding(10);

            var headerLabel = new Label
            {
                Text = "📅 Вечный Календарь",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 130, 150),
                Size = new Size(290, 50),
                Location = new Point(20, 15),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _dateLabel = new Label
            {
                Text = $"Сегодня: {DateTime.Today:dd MMMM yyyy}",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Gray,
                Size = new Size(250, 25),
                Location = new Point(20, 60)
            };

            _monthCalendar = new MonthCalendar
            {
                Location = new Point(20, 100),
                Size = new Size(300, 200),
                ShowToday = true,
                ShowTodayCircle = true,
                MaxSelectionCount = 1
            };
            _monthCalendar.DateChanged += MonthCalendar_DateChanged;

            _addEventButton = new Button
            {
                Text = "➕ Добавить событие",
                Location = new Point(20, 320),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(100, 149, 237),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _addEventButton.Click += AddEventButton_Click;

            _weekViewButton = new Button
            {
                Text = "📆 Неделя",
                Location = new Point(170, 320),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(106, 90, 205),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _weekViewButton.Click += WeekViewButton_Click;

            var eventsLabel = new Label
            {
                Text = "События на выбранную дату:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(250, 25),
                Location = new Point(350, 100)
            };

            _eventsListBox = new ListBox
            {
                Location = new Point(350, 130),
                Size = new Size(500, 400),
                Font = new Font("Segoe UI", 9),
                BorderStyle = BorderStyle.FixedSingle
            };

            var statusLabel = new Label
            {
                Text = "Готов к работе",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Size = new Size(200, 20),
                Location = new Point(20, 650)
            };

            Controls.AddRange(new Control[]
            {
                headerLabel,
                _dateLabel,
                _monthCalendar,
                _addEventButton,
                _weekViewButton,
                eventsLabel,
                _eventsListBox,
                statusLabel
            });

            LoadEventsForDate(DateTime.Today);
        }

        private void InitializeLeapYearButton()
        {
            _leapYearButton = new Button
            {
                Text = "🗓️ Високосный год",
                Location = new Point(20, 420),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(255, 140, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _leapYearButton.Click += LeapYearButton_Click;

            this.Controls.Add(_leapYearButton);
        }

        private void LeapYearButton_Click(object sender, EventArgs e)
        {
            try
            {
                var leapYearForm = new LeapYearForm();
                leapYearForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия проверки високосного года: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeSearchPanel()
        {
            var searchPanel = new Panel
            {
                Location = new Point(350, 20),
                Size = new Size(500, 40),
                BackColor = Color.White
            };

            _searchTextBox = new TextBox
            {
                Location = new Point(0, 0),
                Size = new Size(400, 25),
                PlaceholderText = "Поиск событий...",
                Font = new Font("Segoe UI", 9)
            };

            _searchButton = new Button
            {
                Location = new Point(410, 0),
                Size = new Size(80, 25),
                Text = "Поиск",
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _searchButton.Click += SearchButton_Click;

            searchPanel.Controls.AddRange(new Control[] { _searchTextBox, _searchButton });
            this.Controls.Add(searchPanel);
        }

        private void InitializeStatsButton()
        {
            _statsButton = new Button
            {
                Text = "📊 Статистика",
                Location = new Point(20, 370),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(34, 139, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _statsButton.Click += StatsButton_Click;

            _dataManagerButton = new Button
            {
                Text = "💾 Управление данными",
                Location = new Point(170, 370),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(128, 0, 128),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _dataManagerButton.Click += DataManagerButton_Click;

            this.Controls.Add(_statsButton);
            this.Controls.Add(_dataManagerButton);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                var searchText = _searchTextBox.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    var results = _calendarService.SearchEvents(searchText);
                    ShowSearchResults(results);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StatsButton_Click(object sender, EventArgs e)
        {
            try
            {
                var statsForm = new StatsForm(_calendarService);
                statsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия статистики: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataManagerButton_Click(object sender, EventArgs e)
        {
            try
            {
                var dataManagerForm = new DataManagerForm(_calendarService);
                dataManagerForm.ShowDialog();

                LoadEventsForDate(_monthCalendar.SelectionStart);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия управления данными: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowSearchResults(System.Collections.Generic.List<CalendarEvent> events)
        {
            _eventsListBox.Items.Clear();

            if (events.Count == 0)
            {
                _eventsListBox.Items.Add("События не найдены");
            }
            else
            {
                foreach (var evt in events)
                {
                    var timeText = evt.IsAllDay ? "Весь день" : $"{evt.StartTime:HH:mm}";
                    var eventText = $"{GetCategoryIcon(evt.Category)} {evt.Title} | ⏰ {timeText} | {evt.Date:dd.MM.yyyy}";
                    _eventsListBox.Items.Add(eventText);
                }
            }
        }

        private void MonthCalendar_DateChanged(object? sender, DateRangeEventArgs e)
        {
            LoadEventsForDate(_monthCalendar.SelectionStart);
        }

        private void AddEventButton_Click(object? sender, EventArgs e)
        {
            try
            {
                var selectedDate = _monthCalendar.SelectionStart;
                using var addEventForm = new AddEventForm(selectedDate);

                if (addEventForm.ShowDialog() == DialogResult.OK)
                {
                    var newEvent = new CalendarEvent
                    {
                        Title = addEventForm.EventText,
                        Description = addEventForm.EventDescription,
                        Date = addEventForm.EventDate,
                        StartTime = addEventForm.StartTime,
                        EndTime = addEventForm.EndTime,
                        IsAllDay = addEventForm.IsAllDay,
                        Category = addEventForm.EventCategory,
                        Color = addEventForm.EventColor,
                        Reminder = addEventForm.Reminder
                    };

                    _calendarService.AddEvent(newEvent);
                    LoadEventsForDate(selectedDate);

                    MessageBox.Show("Событие успешно добавлено!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении события: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WeekViewButton_Click(object? sender, EventArgs e)
        {
            try
            {
                var selectedDate = _monthCalendar.SelectionStart;
                var weekViewForm = new WeekViewForm(selectedDate, _calendarService);
                weekViewForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии недельного просмотра: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEventsForDate(DateTime date)
        {
            _eventsListBox.Items.Clear();

            try
            {
                var events = _calendarService.GetEventsForDate(date);

                if (events.Count == 0)
                {
                    _eventsListBox.Items.Add("На эту дату событий нет");
                }
                else
                {
                    foreach (var evt in events.OrderBy(e => e.StartTime))
                    {
                        var timeText = evt.IsAllDay ? "Весь день" : $"{evt.StartTime:HH:mm}";
                        var eventText = $"{GetCategoryIcon(evt.Category)} {evt.Title} | ⏰ {timeText}";
                        _eventsListBox.Items.Add(eventText);
                    }
                }
            }
            catch (Exception ex)
            {
                _eventsListBox.Items.Add($"Ошибка загрузки событий: {ex.Message}");
            }
        }

        private static string GetCategoryIcon(EventCategory category) => category switch
        {
            EventCategory.Work => "💼",
            EventCategory.Personal => "👤",
            EventCategory.Holiday => "🎉",
            EventCategory.Birthday => "🎂",
            EventCategory.Meeting => "🤝",
            EventCategory.Appointment => "📅",
            EventCategory.Other => "📌",
            _ => "📌"
        };
    }
}